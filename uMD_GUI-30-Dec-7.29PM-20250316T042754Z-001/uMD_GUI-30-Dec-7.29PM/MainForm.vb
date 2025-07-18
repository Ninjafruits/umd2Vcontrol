﻿' some important sources for techniques saverageused here:
'https://eshkay.wordpress.com/2013/03/25/vb-net-serial-port-communication-with-datareceived-event/
'https://msdn.microsoft.com/en-us/library/ms171728.aspxvelocityse

Imports System.Text
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Text.RegularExpressions
Imports System.IO.Ports
Imports System.ComponentModel
Imports System.Threading

Public Class MainForm
    ' Dim file As System.IO.StreamWriter
    Dim captureFile As System.IO.StreamWriter ' this is the log file to capture data
    Dim captureFileName As String = "" ' this is the name of the log file to capture data
    Dim currentcapturefile As String = "logfile1.txt"
    Public positionSeries As New Series
    Public velocitySeries As New Series
    Public angleseries As New Series
    Public fftSeries As New Series
    Public fftSeries2 As New Series
    Public chartcounter As UInt64
    Dim FFTdone As Boolean = True
    ' Creates and initializes a new Queue.
    Public displacementQueuex As New Queue()
    Public displacementQueuey As New Queue()
    Dim velocityQueuex As New Queue()
    Dim velocityQueuey As New Queue()
    Dim angleQueuex As New Queue()
    Dim angleQueuey As New Queue()
    'defining the menu items for the main menu bar
    Dim menuItems As New List(Of MenuItem)
    Dim myMenuItemfinish As New MenuItem("&Finish")
    Dim myMenuItemLogFile As New MenuItem("&Open Log File")
    Dim myMenuItemConfiguration As New MenuItem("&Interferometer Configuration")
    Dim myMenuItemCompensation As New MenuItem("&Environmental Compensation")
    Dim myMenuItemTestMode As New MenuItem("&Test Mode")
    Dim myMenuItemUSBPort As New MenuItem("&COM Port")
    Dim myMenuItemHelp As New MenuItem("&Help")
    Dim myMenuItemInformation As New MenuItem("&Information")
    Dim myMenuItemAbout As New MenuItem("&About")
    Dim myMenuItemNew As New MenuItem("&New")
    Dim myMenuItemUSBSubMenuCOMPorts As New MenuItem("&DummyText")

    'defining the main menu bar
    Dim mnuBar As New MainMenu()
    ' buffer for serial port object
    Dim spDrLine As String = ""
    Dim spBuffer As String = ""
    Public zeroAdjustment As Double = 0  ' this is what we need to set the data back to zero
    Public zeroAdjustment1 As Double = 0
    Public zeroAdjustment2 As Double = 0
    Public zeroAdjustment3 As Double = 0
    Public unitCorrectionFactor As Double = 1.0 ' 1.0 = nm 0.001 = um etc
    Public unitCorrectmm As Double = 1.0 ' 1.0 = mm 0.001 = um etc
    Public angleCorrectionFactor As Double = 3600.0 ' 3600 = arcsec 60 = arcmin 1 = degree
    Public angleCorrectdegree As Double = 1
    Public Angle_Reflector_Spacing As Double = 32.61
    Public multiplier As Double = 2    ' needed for interferometer type 1x 2x 4x
    Public multiplierCoefficient As Double = 2    ' needed for interferometer type 1x 2x 4x
    Public straightnessMultiplier As Double = 1 ' needed for straightness measurements
    Public SLCoefficient As Double = 360
    Public SSCoefficient As Double = 36
    Public currentValue As Double = 0
    Public currentValue1 As Double = 0
    Public currentValue2 As Double = 0
    Public currentValue3 As Double = 0
    Public previousValue As Double = 0
    Public previousValue1 As Double = 0
    Public previousValue2 As Double = 0
    Public previousValue3 As Double = 0
    Dim velocityValue As Double = 0
    Dim velocityValue1 As Double = 0
    Dim velocityValue2 As Double = 0
    Dim velocityValue3 As Double = 0
    Dim angleValue As Double = 0
    Dim angleValue1 As Double = 0
    Dim angleValue2 As Double = 0
    Dim angleValue3 As Double = 0
    Dim averagingValue As Double = 0
    Public displayValue As Double = 0
    Public displayValue1 As Double = 0
    Public displayValue2 As Double = 0
    Public displayValue3 As Double = 0
    Dim velocityValueList As New List(Of Double)
    Dim DFTValueList As New List(Of Double)
    Dim angleValueList As New List(Of Double)
    Dim averagingFromPrevious As Double = 0
    Dim velocityFromPrevious As Double = 0
    Dim angleFromPrevious As Double = 0
    Public average As Double = 0
    Public average1 As Double = 0
    Public average2 As Double = 0
    Public average3 As Double = 0
    Public PreviousAverage As Double = 0
    Public velocity As Double = 0
    Public velocity1 As Double = 0
    Public velocity2 As Double = 0
    Public velocity3 As Double = 0
    Public angle As Double = 0
    Public angle1 As Double = 0
    Public angle2 As Double = 0
    Public angle3 As Double = 0
    Public TestmodeFlag As Integer = 0
    Public Dimension As Integer = 1024     ' this value determines both the size of the plot graphs and the number of data points in the DFT
    Dim RealPartOfDFT(CInt(Dimension / 2)) As Double
    Dim averagingFromCurrent As Double = 0
    Dim velocityFromCurrent As Double = 0
    Dim angleFromCurrent As Double = 0
    Dim ImaginaryPartOfDFT(CInt(Dimension / 2)) As Double
    Dim DFTMax As Double = 0
    Public needsInitialZero As Integer = 0
    Dim outerLoopCounter, innerLoopCounter As Integer
    Public DifferenceValue As Double = 0
    Dim CurrentREFCount As Int64 = 0
    Dim PreviousREFCount As Int64 = 0
    Dim CurrentMEASCount As Int64 = 0
    Dim PreviousMEASCount As Int64 = 0
    Public ErrorFlag As Integer = 0
    Public SuspendFlag As Integer = 0
    Dim MeasCountCorrection As UInt64 = 0
    Public CurrentValueCorrection As Double = 0
    Public CurrentValueCorrection1 As Double = 0
    Public CurrentValueCorrection2 As Double = 0
    Public CurrentValueCorrection3 As Double = 0
    Dim SuspenREFCount As UInt64 = 0
    Dim SuspendMEASCount As UInt64 = 0
    Dim graphCount As UInt64 = CULng(Dimension)
    Dim plotCount As UInt64 = CULng(Dimension)
    Public SuspendCurrentValue As Double = 0
    Public SuspendCurrentValue1 As Double = 0
    Public SuspendCurrentValue2 As Double = 0
    Public SuspendCurrentValue3 As Double = 0
    Public REFFrequency As Double = 0
    Public MEASFrequency As Double = 0
    Public DIFFFrequency As Double = 0
    Dim previousREFFrequency As Double = 0
    Dim previousMEASFrequency As Double = 0
    Dim currentREFFrequency As Double = 0
    Dim currentMEASFrequency As Double = 0
    Dim previousDIFFFrequency As Double = 0
    Dim currentDIFFFrequency As Double = 0
    Public previousserialnumber As UInt64 = 0
    Dim serialnumberdifference As UInt64 = 0
    Public Wavelength As Double = 632.991372 ' Corrected wavelength
    Public previousSimulationDistance As Int64 = 0
    Public waveform As Double = 0
    Public previoussimulationVelocity As Int64 = 0
    Public previoussimREFCount As Int64 = 0
    Public previoussimMEASCount As Int64 = 0
    Public simrefcount As Int64 = 0
    Public simmeascount As Int64 = 0
    Public simulationDistance As Int64 = 0
    Public simulationVelocity As Int64 = 0
    Public simulationPhase As Int64 = 0
    Public simMEASFrequencyCount1 As Int64 = 0
    Public simulationDistance1 As Int64 = 0
    Public simulationVelocity1 As Int64 = 0
    Public simulationPhase1 As Int64 = 0
    Public simMEASFrequencyCount2 As Int64 = 0
    Public simulationDistance2 As Int64 = 0
    Public simulationVelocity2 As Int64 = 0
    Public simulationPhase2 As Int64 = 0
    Public simMEASFrequencyCount3 As Int64 = 0
    Public simulationDistance3 As Int64 = 0
    Public simulationVelocity3 As Int64 = 0
    Public simulationPhase3 As Int64 = 0
    Public simulationSerial As UInt64 = 0
    Public simulationLowSpeedCode As Int16 = 0
    Public simulationLowSpeedData As Int32 = 0
    Public bangbang As Double = 1
    Public counter As Double = 0
    Dim simulatedData As String
    Public simcount As Double = 0
    Public TMUnitsFactor As Double = 0
    Public TMFreqMult As Double = 1
    Public TMAmpMult As Double = 1
    Public TMOfsMult As Double = 1
    Public TMFreqValue As Double = 1
    Public TMAmpValue As Double = 1
    Public TMOfsValue As Double = 1
    Public EDEnabled As Integer = 1
    'Dim DFTThread As New Thread(AddressOf DFT)
    Private DFTThread As New System.ComponentModel.BackgroundWorker 'set new backgroundworker
    Dim resetEvent As ManualResetEvent = New ManualResetEvent(False)
    Public IgnoreCount As Integer = 0
    Public phase As Double = 0
    Dim incomingData As String
    Dim Capture_Flag As Integer = 0
    Dim Capture_Enable As Integer = 0
    Dim range As Double = 0
    Dim ScrollRate As Integer = 1
    Public MFLoaded As Integer = 0
    Dim TMWaveform As Integer = 0
    Dim TMREFFrequency As Double = 1
    Dim temp1 As Integer = 0
    Public TMWaveformFlag As Integer = 4
    Dim DFT_Skip_Factor As Integer = 1
    Dim zero As Double = 0
    Dim ClearCounter As Integer = 0
    Dim CurrentValuePhase As Double = 0
    Dim CurrentValuePhase1 As Double = 0
    Dim CurrentValuePhase2 As Double = 0
    Dim CurrentValuePhase3 As Double = 0
    Dim PreviousCurrentValuePhase As Double = 0
    Dim PhaseValue As Integer = 0
    Dim PhaseValue1 As Integer = 0
    Dim PhaseValue2 As Integer = 0
    Dim PhaseValue3 As Integer = 0
    Dim Diagnostic2Value As Integer = 0
    Dim Diagnostic3Value As Integer = 0
    Dim Diagnostic4Value As Integer = 0
    Dim Diagnostic4Count As Integer = 0
    Dim Diagnostic4Save As Integer = 0
    Dim Diagnostic5Value As Integer = 0
    Dim DP32COMCounts As Integer = 0
    Dim DP32PRCCounts As Integer = 0
    Dim LowSpeedCode As Integer = 0
    Dim LowSpeedData As Integer = 0
    Dim TemperatureAutoValue As Integer = -1
    Dim PressureAutoValue As Integer = -1
    Dim HumidityAutoValue As Integer = -1
    Dim AxisData(4, 5) As Int64 ' First index selects Axis Primary,1,2,3; second parameter selects REFFreqCount,MEASFreqCount, Displacement, Velocity, Phase. 
    Public PrimaryAxisSelect As Integer = 1
    Public PrimaryAxisFlip As Integer = 1 ' 1 -> no flip, -1 -> flip
    Public Axis1Flip As Integer = 1
    Public Axis2Flip As Integer = 1
    Public Axis3Flip As Integer = 1
    Public MultipleAxesFlag As Integer = 1 ' Bits 0,1,2 = Axis 1,2,3, bit 4 is multiaxis enable
    Public AxesPolarityFlag As Integer = 0 ' Bits 0,1,2 = Axis 1,2,3, flip sign
    Public TMMultipleAxesFlag As Integer = 1
    Dim FirmwareVersion As Integer = 0
    Dim FirmwareVersionSet As Integer = 0
    ' Dim uMDVersion As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() ' + ", " +
    Dim SampleFrequency As Double = 610.35
    Dim SamplePeriod As Double = 1638.4
    Dim SimulationSamples As Integer = 15
    Dim buildRevision As String
    Dim parts As String()
    Dim build As Integer = 0
    Dim revision As Integer = 0
    Public HomodyneAxes As Integer = 0 ' Set to 1, 2, or 3 via low speed data for homodyne operation - suppresses error checking
    Public HomodyneMultiplier As Double = 1 ' Number of counts/cycle for homodyne mode
    Public MultiplierCombined As Double = 1 ' Number of counts/cycle for homodyne mode
    Dim HomodyneSet As Integer = 0  ' Trigger to turn off REF,MEAS,DIFF labels
    Dim MonitorCount As Integer = 0 ' Send only 1/16th of ScrollRate values
    Dim average_nm As Long = 0 ' Displacement in nm for primary axis
    Dim average1_nm As Long = 0 ' Displacement in nm for axis 1
    Dim average2_nm As Long = 0 ' Displacement in nm for axis 2
    Dim average3_nm As Long = 0 ' Displacement in nm for axis 3
    Dim dateTimeOfBuild As String

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'SerialPort1.Close() ' this hangs the program. known MS bug https://social.msdn.microsoft.com/Forums/en-US/ce8ce1a3-64ed-4f26-b9ad-e2ff1d3be0a5/serial-port-hangs-whilst-closing?forum=Vsexpressvcs
        'End
        'file.Close()
        If Not (captureFile Is Nothing) Then
            captureFile.Close()
        End If

        My.Settings.AveragingValue = TrackBar1.Value
        My.Settings.ScrollFactor = CInt(NumericUpDown_Scale.Value)
        My.Settings.TMUnitsFactor = TMUnitsFactor
        My.Settings.DFT_Skip_Factor = DFT_Skip_Factor
        My.Settings.LogFile = currentcapturefile

        If (Configuration.Interpolation_CheckBox.Checked = True) Then
            My.Settings.InterpolationFlag = 1
        Else
            My.Settings.InterpolationFlag = 0
        End If

        If (Configuration.Encoder_Checkbox.Checked = True) Then
            My.Settings.EncoderFlag = 1
        Else
            My.Settings.EncoderFlag = 0
        End If

        If (TestMode.Diagnostic_Enable_CheckBox.Checked = True) Then
            My.Settings.DiagnosticReadoutFlag = 1
        Else
            My.Settings.DiagnosticReadoutFlag = 0
        End If

        If (TestMode.Error_Detection_CheckBox.Checked = True) Then
            My.Settings.ErrorDetectionFlag = 1
        Else
            My.Settings.ErrorDetectionFlag = 0
        End If

        My.Settings.Temperature = Compensation.NumericUpDown_Temperature.Value
        My.Settings.Pressure = Compensation.NumericUpDown_Pressure.Value
        My.Settings.Humidity = Compensation.NumericUpDown_Humidity.Value
        My.Settings.TempFactor = Compensation.TextBox_TempFactor.Text
        My.Settings.PresFactor = Compensation.TextBox_PresFactor.Text
        My.Settings.HumiFactor = Compensation.TextBox_HumiFactor.Text
        My.Settings.TFactor = Compensation.TCorrection
        My.Settings.PFactor = Compensation.PCorrection
        My.Settings.HFactor = Compensation.HCorrection
        My.Settings.VacuumWavelength = CType(Compensation.NumericUpDown_Wavelength.Value, String)

        If (Compensation.Temperature_Auto_CheckBox.Checked = True) Then
            My.Settings.TemperatureCompAutoFlag = 1
        Else
            My.Settings.TemperatureCompAutoFlag = 0
        End If

        If (Compensation.Pressure_Auto_CheckBox.Checked = True) Then
            My.Settings.PressureCompAutoFlag = 1
        Else
            My.Settings.PressureCompAutoFlag = 0
        End If

        If (Compensation.Humidity_Auto_Checkbox.Checked = True) Then
            My.Settings.HumidityCompAutoFlag = 1
        Else
            My.Settings.HumidityCompAutoFlag = 0
        End If

        My.Settings.Save()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        buildRevision = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        parts = buildRevision.Split("."c)
        build = Integer.Parse(parts(2))
        revision = Integer.Parse(parts(3))
        dateTimeOfBuild = (New DateTime(2000, 1, 1) + New TimeSpan(build, 0, 0, 0) + TimeSpan.FromSeconds(revision * 2)).ToString()
        About.uMD_Version.Text = buildRevision + ", " + dateTimeOfBuild

        AddHandler DFTThread.DoWork, AddressOf DFT
        DisplacementButton_Click(sender, e)
        Chart1.Series.Clear()
        Chart1.ChartAreas(0).AxisX.LabelStyle.Enabled = False
        Chart1.ChartAreas(0).AxisX.MajorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisX.MinorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisY.MinorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisY.LabelStyle.Format = "e2" 'https://msdn.microsoft.com/en-us/library/dwhawy9k.aspx
        Chart1.ChartAreas(0).AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None
        Chart1.ChartAreas(0).AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 2.25F, System.Drawing.FontStyle.Bold)

        positionSeries.ChartType = SeriesChartType.FastLine
        For chartcounter = 0 To CULng(Dimension - 1)
            positionSeries.Points.AddXY(chartcounter, 0.5 * Math.Sin(chartcounter / 40))
        Next
        Chart1.Series.Add(positionSeries)

        velocitySeries.ChartType = SeriesChartType.FastLine
        For chartcounter = 0 To CULng(Dimension - 1)
            velocitySeries.Points.AddXY(chartcounter, 0.5 * Math.Cos(chartcounter / 40))
            velocityValueList.Add(0.0)  ' make sure the list is not empty
            DFTValueList.Add(0.0)  ' make sure the list is not empty
        Next

        angleseries.ChartType = SeriesChartType.FastLine
        For chartcounter = 0 To CULng(Dimension - 1)
            angleseries.Points.AddXY(chartcounter, 0.25 * Math.Asin(Math.Sin(chartcounter / 40)))
            angleValueList.Add(0.0)  ' make sure the list is not empty
        Next

        DFTThread.RunWorkerAsync()
        fftSeries.ChartType = SeriesChartType.FastLine

        ' Menu Stuff
        ' first lets create an empty submenu for the com port list under the USB top menu
        menuItems.Add(myMenuItemUSBSubMenuCOMPorts)    ' need an empty list to be able to delete/change them at runtime

        ' Next, attach that list to the USB top menu
        myMenuItemUSBPort.MenuItems.Add(myMenuItemUSBSubMenuCOMPorts)

        ' Next attach all the top menus to the menu bar.
        mnuBar.MenuItems.Add(myMenuItemfinish)
        mnuBar.MenuItems.Add(myMenuItemLogFile)
        mnuBar.MenuItems.Add(myMenuItemConfiguration)
        mnuBar.MenuItems.Add(myMenuItemCompensation)
        mnuBar.MenuItems.Add(myMenuItemTestMode)
        '        mnuBar.MenuItems.Add(myMenuItemHelp)
        mnuBar.MenuItems.Add(myMenuItemHelp)
        mnuBar.MenuItems.Add(myMenuItemUSBPort)
        ' mnuBar.MenuItems.Add(myMenuItemOptions)

        '      AddHandler myMenuItemConfiguration.Click, AddressOf Me.myMenuItemhelp_Click
        '     myMenuItemHelp.MenuItems.Add(myMenuItemConfiguration)



        myMenuItemHelp.MenuItems.Add(myMenuItemInformation)
        AddHandler myMenuItemInformation.Click, AddressOf Me.myMenuIteminformation_Click
        myMenuItemHelp.MenuItems.Add(myMenuItemAbout)
        AddHandler myMenuItemAbout.Click, AddressOf Me.myMenuItemabout_Click

        ' Next replace the application with the menu bar we just crafted
        Me.Menu = mnuBar

        ' Finally, add the handlers to the menu items so that they can respond to clicks
        AddHandler myMenuItemfinish.Click, AddressOf Me.myMenuItemfinish_Click
        AddHandler myMenuItemLogFile.Click, AddressOf Me.myMenuItemLogFile_Click
        AddHandler myMenuItemConfiguration.Click, AddressOf Me.myMenuItemConfiguration_Click
        AddHandler myMenuItemCompensation.Click, AddressOf Me.myMenuItemCompensation_Click
        AddHandler myMenuItemTestMode.Click, AddressOf Me.myMenuItemTestMode_Click
        AddHandler myMenuItemUSBPort.Popup, AddressOf Me.myMenuItemUSBPort_Click
        '   AddHandler myMenuItemHelp.Popup, AddressOf Me.myMenuItemhelp_Click

        ' load user settings
        multiplier = My.Settings.Multiplier
        If multiplier = 1 Then
            Configuration.Button1x.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Button1x.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Button2x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button2x.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Button4x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button4x.ForeColor = Color.FromKnownColor(KnownColor.Black)
        ElseIf multiplier = 2 Then
            Configuration.Button1x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button1x.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Button2x.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Button2x.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Button4x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button4x.ForeColor = Color.FromKnownColor(KnownColor.Black)
        ElseIf multiplier = 4 Then
            Configuration.Button1x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button1x.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Button2x.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Button2x.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Button4x.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Button4x.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        End If

        multiplierCoefficient = multiplier

        unitCorrectionFactor = My.Settings.UnitCorrectionFactor
        If unitCorrectionFactor = 0.000001 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "nm"
        ElseIf unitCorrectionFactor = 0.001 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "μm"
        ElseIf unitCorrectionFactor = 1 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "mm"
        ElseIf unitCorrectionFactor = 10 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "cm"
        ElseIf unitCorrectionFactor = 1000 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "m"
        ElseIf unitCorrectionFactor = 25.4 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.Black)
            UnitLabel.Text = "in"
        ElseIf unitCorrectionFactor = 304.8 Then
            Configuration.Buttonnm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonnm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonum.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonum.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonmm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonmm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttoncm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttoncm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonm.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonm.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonft.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonft.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            UnitLabel.Text = "ft"
        End If

        Axis1_Units_Label.Text = UnitLabel.Text
        Axis2_Units_Label.Text = UnitLabel.Text
        Axis3_Units_Label.Text = UnitLabel.Text

        angleCorrectionFactor = My.Settings.AngleCorrectionFactor

        If angleCorrectionFactor = 3600.0 Then
            Configuration.Buttonarcsec.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonarcsec.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonarcmin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonarcmin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttondegree.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttondegree.ForeColor = Color.FromKnownColor(KnownColor.Black)
            AngleLabel.Text = "arcsec"
        ElseIf angleCorrectionFactor = 60.0 Then
            Configuration.Buttonarcsec.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonarcsec.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Configuration.Buttonarcmin.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttonarcmin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttondegree.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttondegree.ForeColor = Color.FromKnownColor(KnownColor.Black)
            AngleLabel.Text = "arcmin"
        ElseIf angleCorrectionFactor = 1.0 Then
            Configuration.Buttonarcsec.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonarcsec.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttonarcmin.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Configuration.Buttonarcmin.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Configuration.Buttondegree.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Configuration.Buttondegree.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            AngleLabel.Text = "degree"
        End If

        Axis1_Angle_Label.Text = AngleLabel.Text
        Axis2_Angle_Label.Text = AngleLabel.Text
        Axis3_Angle_Label.Text = AngleLabel.Text

        Axis_UnitsA.Text = AngleLabel.Text
        Axis_UnitsD.Text = UnitLabel.Text
        RangeUnits.Text = UnitLabel.Text
        Label_RangeTime.Visible = False
        RangeUnits.Visible = False

        Axis1Flip = My.Settings.Axis1Flip
        If (Axis1Flip = 1) Then
            Configuration.Axis1_Polarity_CheckBox.Checked = False
        Else
            Configuration.Axis1_Polarity_CheckBox.Checked = True
        End If

        PrimaryAxisFlip = Axis1Flip

        Axis2Flip = My.Settings.Axis2Flip
        If (Axis2Flip = 1) Then
            Configuration.Axis2_Polarity_CheckBox.Checked = False
        Else
            Configuration.Axis2_Polarity_CheckBox.Checked = True
        End If

        Axis3Flip = My.Settings.Axis3Flip
        If (Axis3Flip = 1) Then
            Configuration.Axis3_Polarity_CheckBox.Checked = False
        Else
            Configuration.Axis3_Polarity_CheckBox.Checked = True
        End If

        Configuration.NumericUpDown_SL_Coefficient.Value = CDec(My.Settings.SLCoefficient)
        Configuration.NumericUpDown_SS_Coefficient.Value = CDec(My.Settings.SSCoefficient)
        Configuration.NumericUpDown_ARS.Value = CDec(My.Settings.Angle_Reflector_Spacing)

        Compensation.ComboBox_TempUnits.Text = My.Settings.TempUnits
        Compensation.NumericUpDown_Temperature.Minimum = 32
        Compensation.NumericUpDown_Temperature.Maximum = 158
        If Compensation.ComboBox_TempUnits.Text = "Degrees C" Then
            Compensation.NumericUpDown_Temperature.Minimum = 0
            Compensation.NumericUpDown_Temperature.Maximum = 70
        ElseIf Compensation.ComboBox_TempUnits.Text = "Degrees K" Then
            Compensation.NumericUpDown_Temperature.Minimum = 273
            Compensation.NumericUpDown_Temperature.Maximum = 343
        End If
        Compensation.NumericUpDown_Temperature.Value = CDec(My.Settings.Temperature)

        Compensation.ComboBox_Pressure_Units.Text = My.Settings.PressureUnits
        Compensation.NumericUpDown_Pressure.Minimum = 500
        Compensation.NumericUpDown_Pressure.Maximum = 2000
        If Compensation.ComboBox_Pressure_Units.Text = "mm/Hg" Then
            Compensation.NumericUpDown_Pressure.Minimum = 380
            Compensation.NumericUpDown_Pressure.Maximum = 1520
        End If
        Compensation.NumericUpDown_Pressure.Value = CDec(My.Settings.Pressure)

        Compensation.NumericUpDown_Humidity.Value = CDec(My.Settings.Humidity)

        Compensation.TextBox_TempFactor.Text = My.Settings.TempFactor
        Compensation.TextBox_PresFactor.Text = My.Settings.PresFactor
        Compensation.TextBox_HumiFactor.Text = My.Settings.HumiFactor
        Compensation.TCorrection = My.Settings.TFactor
        Compensation.PCorrection = My.Settings.PFactor
        Compensation.HCorrection = My.Settings.HFactor

        Compensation.ECFactor = My.Settings.ECFactor

        If Compensation.ECFactor = 1 Then
            Compensation.ECOff_Button.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Compensation.ECOff_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            Compensation.ECOn_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Compensation.ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
        Else
            Compensation.ECOff_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Compensation.ECOff_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Compensation.ECOn_Button.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            Compensation.ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        End If

        Compensation.NumericUpDown_Wavelength.Value = CDec(My.Settings.VacuumWavelength)
        Wavelength = Compensation.ECFactor * CDbl(My.Settings.VacuumWavelength)
        WLText.Text = Wavelength.ToString("000.000000")

        If My.Settings.TemperatureCompAutoFlag = 1 Then
            Compensation.Temperature_Auto_CheckBox.Checked = True
        Else
            Compensation.Temperature_Auto_CheckBox.Checked = False
        End If

        If My.Settings.PressureCompAutoFlag = 1 Then
            Compensation.Pressure_Auto_CheckBox.Checked = True
        Else
            Compensation.Pressure_Auto_CheckBox.Checked = False
        End If

        If My.Settings.HumidityCompAutoFlag = 1 Then
            Compensation.Humidity_Auto_Checkbox.Checked = True
        Else
            Compensation.Humidity_Auto_Checkbox.Checked = False
        End If

        TMWaveformFlag = My.Settings.TMWaveformFlag

        TestMode.Button_Constant.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        TestMode.Button_Constant.ForeColor = Color.FromKnownColor(KnownColor.Black)
        TestMode.Button_Ramp.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        TestMode.Button_Ramp.ForeColor = Color.FromKnownColor(KnownColor.Black)
        TestMode.Button_Triangle.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        TestMode.Button_Triangle.ForeColor = Color.FromKnownColor(KnownColor.Black)
        TestMode.Button_Sine.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        TestMode.Button_Sine.ForeColor = Color.FromKnownColor(KnownColor.Black)

        If TMWaveformFlag = 1 Then
            TestMode.Button_Constant.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            TestMode.Button_Constant.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ElseIf TMWaveformFlag = 2 Then
            TestMode.Button_Ramp.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            TestMode.Button_Ramp.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ElseIf TMWaveformFlag = 4 Then
            TestMode.Button_Triangle.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            TestMode.Button_Triangle.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ElseIf TMWaveformFlag = 8 Then
            TestMode.Button_Sine.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
            TestMode.Button_Sine.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        End If

        TMUnitsFactor = My.Settings.TMUnitsFactor

        If TMUnitsFactor = 0.001 Then
            TestMode.ComboBox_Units.Text = "μm"
        ElseIf TMUnitsFactor = 1 Then
            TestMode.ComboBox_Units.Text = "mm"
        ElseIf TMUnitsFactor = 10 Then
            TestMode.ComboBox_Units.Text = "cm"
        ElseIf TMUnitsFactor = 1000 Then
            TestMode.ComboBox_Units.Text = "m"
        ElseIf TMUnitsFactor = 25.4 Then
            TestMode.ComboBox_Units.Text = "in"
        ElseIf TMUnitsFactor = 304.8 Then
            TestMode.ComboBox_Units.Text = "ft"
        End If

        TMFreqValue = TestMode.Trackbar_Frequency.Value * 10 * TMFreqMult
        TestMode.Textbox_Frequency.Text = (TMFreqValue / 10).ToString("0.000")
        TMAmpValue = TestMode.TrackBar_Amplitude.Value * TMAmpMult / 2
        TestMode.TextBox_Amplitude.Text = (TMAmpValue / 50).ToString("0.0000")
        TMOfsValue = TestMode.TrackBar_Offset.Value * TMOfsMult
        TestMode.TextBox_Offset.Text = (TMOfsValue / 10).ToString("0.00")

        TestMode.NumericUpDown_FGREF_Value.Value = CDec(My.Settings.TMREFFrequency)
        TMREFFrequency = CDec(My.Settings.TMREFFrequency)

        TrackBar1.Value = CInt(My.Settings.AveragingValue)
        averagingValue = 1000 * (1 - (1 / (Math.Pow(10, CDbl(TrackBar1.Value / 333)))))
        AverageLabel.Text = (averagingValue / 1000).ToString("F3")

        If ((TrackBar1.Value >= 0) And (TrackBar1.Value < 100)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ElseIf ((TrackBar1.Value >= 100) And (TrackBar1.Value < 300)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.Brown)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.Brown)
        ElseIf ((TrackBar1.Value >= 300) And (TrackBar1.Value < 500)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkRed)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkRed)
        ElseIf ((TrackBar1.Value >= 500) And (TrackBar1.Value < 600)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkOrange)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkOrange)
        ElseIf ((TrackBar1.Value >= 600) And (TrackBar1.Value < 700)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkGoldenrod)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkGoldenrod)
        ElseIf ((TrackBar1.Value >= 700) And (TrackBar1.Value < 800)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkGreen)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkGreen)
        ElseIf ((TrackBar1.Value >= 800) And (TrackBar1.Value < 900)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkBlue)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkBlue)
        Else : Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkViolet)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkViolet)
        End If

        Timer1.Start()
        ' file = My.Computer.FileSystem.OpenTextFileWriter("data.txt", True)
        ' Initialize graph

        TestModeLabel.Visible = False
        EDOff_Label.Visible = False
        HomodyneMult_Label.Visible = False
        straightnessMultiplier = 1
        NumericUpDown_Scale.Value = My.Settings.ScrollFactor
        ScrollRate = CInt(NumericUpDown_Scale.Value)

        DFT_Skip_Factor = My.Settings.DFT_Skip_Factor

        If DFT_Skip_Factor = 30 Then
            Frequency_Axis.Text = "  0.3         1                2                3                4                5                6               7                8                9              10"
            ComboBox_DFT_Range.Text = "10"
        ElseIf DFT_Skip_Factor = 10 Then
            Frequency_Axis.Text = "    1          3                6                9               12             15              18              21             24              27             30"
            DFT_Skip_Factor = 10
            ComboBox_DFT_Range.Text = "30"
        ElseIf DFT_Skip_Factor = 3 Then
            Frequency_Axis.Text = "1  3   6    10              20              30              40              50              60             70              80              90            100"
            ComboBox_DFT_Range.Text = "100"
        ElseIf DFT_Skip_Factor = 1 Then
            Frequency_Axis.Text = "   10        30              60              90            120            150            180            210            240          270             300"
            ComboBox_DFT_Range.Text = "300"
        End If

        Graph_Averaging_CheckBox.Visible = True

        If My.Settings.InterpolationFlag = 1 Then
            Configuration.Interpolation_CheckBox.Checked = True
        Else
            Configuration.Interpolation_CheckBox.Checked = False
        End If

        If My.Settings.EncoderFlag = 1 Then
            Configuration.Encoder_Checkbox.Checked = True
        Else
            Configuration.Encoder_Checkbox.Checked = False
        End If

        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If

        If My.Settings.DiagnosticReadoutFlag = 1 Then
            TestMode.Diagnostic_Enable_CheckBox.Checked = True
        Else
            TestMode.Diagnostic_Enable_CheckBox.Checked = False
        End If

        If My.Settings.ErrorDetectionFlag = 1 Then
            TestMode.Error_Detection_CheckBox.Checked = True
        Else
            TestMode.Error_Detection_CheckBox.Checked = False
        End If

        currentcapturefile = My.Settings.LogFile
        Logfile_Text.Visible = False
        Logfile_Label.Visible = False
        DFT_Hz.Visible = False
        Phase_Value.Visible = False
        Phase_Label.Visible = False
        PBA_RM_Value.Visible = False
        PORTB_Label.Visible = False
        RMA_RM_Value.Visible = False
        REFMEAS_Label.Visible = False
        Phase_Error_Value.Visible = False
        Phase_Error_Label.Visible = False
        Sample_Frequency_Value.Visible = False
        Sample_Frequency_Label.Visible = False
        DP32_PRC_Percent_Value.Visible = False
        DP32_PRC_Percent_Label.Visible = False
        DP32_COM_Percent_Label.Visible = False

        Monitor_Tick.Visible = False

        MFLoaded = 1
    End Sub

    Private Sub Comport_Click(sender As Object, e As EventArgs)
        ' extract COM port name
        Dim comportstrting As String = sender.ToString
        Dim a() As String = Regex.Split(comportstrting, "Text: ")
        'MsgBox(a(1))
        ' open COM port
        If SerialPort1.IsOpen = False Then
            Try ' Interferometer Port Open
                SerialPort1.Close()
            Catch ex As Exception
                ' nothing
            End Try
            SerialPort1.PortName = a(1)
            SerialPort1.BaudRate = 115200
            SerialPort1.Parity = Parity.None
            SerialPort1.StopBits = StopBits.One
            SerialPort1.DataBits = 8
            SerialPort1.Handshake = Handshake.None
            SerialPort1.NewLine = vbCr
            SerialPort1.DtrEnable = True 'important
            SerialPort1.RtsEnable = True 'important
            Try
                SerialPort1.Open()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

            If TestmodeFlag = 0 Then
                needsInitialZero = 1 ' make sure to zero out the reference system
            End If
        Else
            Try ' Monitor Port Open
                SerialPort2.Close()
            Catch ex As Exception
                ' nothing
            End Try
            SerialPort2.PortName = a(1)
            SerialPort2.BaudRate = 115200
            SerialPort2.Parity = Parity.None
            SerialPort2.StopBits = StopBits.One
            SerialPort2.DataBits = 8
            SerialPort2.Handshake = Handshake.None
            SerialPort2.NewLine = vbCr
            SerialPort2.DtrEnable = True 'important
            SerialPort2.RtsEnable = True 'important
            Try
                SerialPort2.Open()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Monitor_Tick.Visible = True
        End If
    End Sub

    Private Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Try
            If SerialPort1.IsOpen Then
                'Dim incomingData As String
                incomingData = SerialPort1.ReadExisting()

                If Not (captureFile Is Nothing) And Capture_Flag = 1 And IgnoreCount = 0 And TestmodeFlag = 0 And TMWaveformFlag = 1 Then
                    captureFile.Write(incomingData) ' Back door to capture all incoming data: Test Mode off but set to Constant
                End If

                spDrLine = spDrLine & incomingData ' important
                If InStr(1, spDrLine, vbLf) > 0 Then
                    spBuffer = spDrLine.Substring(0, spDrLine.LastIndexOf(vbLf)) ' pull in the buffer up to the last line feed
                    spDrLine = spDrLine.Substring(spDrLine.LastIndexOf(vbLf) + 1) ' stuff the rest back into the incoming buffer

                    Try
                        If False = SimulationTimer.Enabled Then
                            Me.SetText(spBuffer)
                        End If
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                Else
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    ''' <summary>
    ''' '''''''''''''''''''''''''''''''''''''''''''''''''''''''' Initalizes bridge to call mqtt and send value2 as well as safeguard
    ''' </summary>
    Private ReadOnly _bridge As New MqttBridge()
    Private _mqttStarted As Boolean
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not _mqttStarted Then
            ' replace with your real broker address/port
            Await _bridge.ConnectAsync("localhost", 1883)
            _mqttStarted = True
            Console.WriteLine("✅ MQTT bridge connected")

        End If
    End Sub
    ''' <summary>
    ''' ''''''''''''''''''''''''''''''''''''''''''''''''''''''' end of initialize bridge
    ''' </summary>
    Private Async Sub SetText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the 
        ' calling thread to the thread ID of the creating thread. 
        ' If these threads are different, it returns true. 
        Dim k As Integer
        If Me.Chart1.InvokeRequired Then    'what is good for chart1 is also good for chart2
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Try
                ' first split data into sets
                Dim sets() As String = [text].Split(vbLf.ToCharArray)
                For k = 0 To sets.Length - 1
                    ' any line beginning with a '!' is debug info.
                    If sets(k).StartsWith("!") Then
                        Console.Write(sets(k) + vbCrLf)
                    End If
                    Dim values() As String = sets(k).Split(" ".ToCharArray)
                    'make sure the current set has exactly 8 fields or 16 fields
                    If values.Length.Equals(8) Or values.Length.Equals(16) Then

                        AxisData(1, 0) = Convert.ToInt64(values(0)) ' REFFreqCount (same for all 3 axes)
                        AxisData(1, 1) = Convert.ToInt64(values(1)) ' MEAS1FreqCount
                        AxisData(1, 2) = Convert.ToInt64(values(2)) ' TotalDistance1
                        AxisData(1, 3) = Convert.ToInt64(values(3)) ' Velocity1Count
                        AxisData(1, 4) = Convert.ToInt32(values(4)) ' Phase1Value

                        If values.Length.Equals(16) Then
                            AxisData(2, 1) = Convert.ToInt64(values(8)) ' MEAS2FreqCount
                            If AxisData(2, 1) > 0 Or HomodyneAxes > 1 Then ' Test for MEAS2FreqCounts or homodyne axis 2
                                MultipleAxesFlag = MultipleAxesFlag Or &HA ' Set Mutiple-Axis Mode with axis 2 enabled
                            End If
                            AxisData(3, 1) = Convert.ToInt64(values(12)) ' MEAS3FreqCount
                            If AxisData(3, 1) > 0 Or HomodyneAxes > 2 Then ' Test for MEAS3FreqCounts or homodyne axis 3
                                MultipleAxesFlag = MultipleAxesFlag Or &HC ' Set Mutiple-Axis Mode with axis 3 enabled
                            End If
                            If MultipleAxesFlag > 1 Then
                                AxisData(2, 0) = Convert.ToInt64(values(0)) ' REFFreqCount (same for all 3 axes)
                                AxisData(2, 2) = Convert.ToInt64(values(9)) ' TotalDistance2
                                AxisData(2, 3) = Convert.ToInt64(values(10)) ' Velocity2Count
                                AxisData(2, 4) = Convert.ToInt32(values(11)) ' Phase2Value

                                AxisData(3, 0) = Convert.ToInt64(values(0)) ' REFFreqCount (same for all 3 axes)
                                AxisData(3, 2) = Convert.ToInt64(values(13)) ' TotalDistance3
                                AxisData(3, 3) = Convert.ToInt64(values(14)) ' Velocity3Count
                                AxisData(3, 4) = Convert.ToInt32(values(15)) ' Phase3Value

                                If Configuration.Interpolation_CheckBox.Checked = True Then
                                    ' PhaseValue = Convert.ToInt32(values(4))
                                    PhaseValue1 = CInt(AxisData(1, 4))
                                    PhaseValue2 = CInt(AxisData(2, 4))
                                    PhaseValue3 = CInt(AxisData(3, 4))

                                    If (PhaseValue1 And &H7E00) = 0 Then
                                        CurrentValuePhase1 = (CDbl(PhaseValue1)) / 256
                                    ElseIf (PhaseValue1 < 0) Then ' REF and MEAS both valid
                                        CurrentValuePhase1 = (CDbl(PhaseValue1)) / 256
                                    Else
                                        Diagnostic4Count = 10
                                        Diagnostic4Save = PhaseValue1
                                        CurrentValuePhase1 = 0
                                    End If

                                    If (PhaseValue2 And &H7E00) = 0 Then
                                        CurrentValuePhase2 = (CDbl(PhaseValue2)) / 256
                                    ElseIf (PhaseValue2 < 0) Then ' REF and MEAS both valid
                                        CurrentValuePhase2 = (CDbl(PhaseValue2)) / 256
                                    Else
                                        Diagnostic4Count = 10
                                        Diagnostic4Save = PhaseValue2
                                        CurrentValuePhase2 = 0
                                    End If

                                    If (PhaseValue3 And &H7E00) = 0 Then
                                        CurrentValuePhase3 = (CDbl(PhaseValue3)) / 256
                                    ElseIf (PhaseValue3 < 0) Then ' REF and MEAS both valid
                                        CurrentValuePhase3 = (CDbl(PhaseValue3)) / 256
                                    Else
                                        Diagnostic4Count = 10
                                        Diagnostic4Save = PhaseValue1
                                        CurrentValuePhase3 = 0
                                    End If
                                Else
                                    CurrentValuePhase1 = 0
                                    CurrentValuePhase2 = 0
                                    CurrentValuePhase3 = 0
                                End If

                                ' currentValue is displacement - total suspend count but NOT zeroadjusted
                                currentValue1 = (Convert.ToDouble(AxisData(1, 2)) - CurrentValuePhase1) * Wavelength / 2.0 - CurrentValueCorrection1 ' Difference in nm; 1/2 wavelength, because path traveled at least twice
                                currentValue2 = (Convert.ToDouble(AxisData(2, 2)) - CurrentValuePhase2) * Wavelength / 2.0 - CurrentValueCorrection2 ' Difference in nm; 1/2 wavelength, because path traveled at least twice
                                currentValue3 = (Convert.ToDouble(AxisData(3, 2)) - CurrentValuePhase3) * Wavelength / 2.0 - CurrentValueCorrection3 ' Difference in nm; 1/2 wavelength, because path traveled at least twice

                                velocityValue1 = (currentValue1 - previousValue1) * SampleFrequency
                                velocityValue2 = (currentValue2 - previousValue2) * SampleFrequency
                                velocityValue3 = (currentValue3 - previousValue3) * SampleFrequency

                                previousValue1 = currentValue1
                                previousValue2 = currentValue2
                                previousValue3 = currentValue3

                                If (Configuration.Encoder_Checkbox.Checked = False) Then
                                    angleValue1 = Math.Asin((currentValue1 - zeroAdjustment1) / Configuration.NumericUpDown_ARS.Value / 1000000) * angleCorrectionFactor * 57.296
                                    angleValue2 = Math.Asin((currentValue2 - zeroAdjustment2) / Configuration.NumericUpDown_ARS.Value / 1000000) * angleCorrectionFactor * 57.296
                                    angleValue3 = Math.Asin((currentValue3 - zeroAdjustment3) / Configuration.NumericUpDown_ARS.Value / 1000000) * angleCorrectionFactor * 57.296
                                Else
                                    angleValue1 = (currentValue1 - zeroAdjustment1) / Configuration.NumericUpDown_ARS.Value / 1000000 * angleCorrectionFactor * 57.296
                                    angleValue2 = (currentValue2 - zeroAdjustment2) / Configuration.NumericUpDown_ARS.Value / 1000000 * angleCorrectionFactor * 57.296
                                    angleValue3 = (currentValue3 - zeroAdjustment3) / Configuration.NumericUpDown_ARS.Value / 1000000 * angleCorrectionFactor * 57.296
                                End If
                            End If
                        End If

                        AxisData(0, 0) = AxisData(PrimaryAxisSelect, 0) ' REFFreqCount (same for all 3 axes)
                        AxisData(0, 1) = AxisData(PrimaryAxisSelect, 1) ' Primary MEASFreqCount
                        AxisData(0, 2) = AxisData(PrimaryAxisSelect, 2) ' Primary TotalDistance
                        AxisData(0, 3) = AxisData(PrimaryAxisSelect, 3) ' Primary VelocityCount
                        AxisData(0, 4) = AxisData(PrimaryAxisSelect, 4) ' Primary PhaseValue
                        '''''''''''''''''''''''''''''''''''''' Add program here to call VBbridge and send data value2 ''''''''''''''''''''''''''''''''''''''
                        Dim v2 As String = values(2).ToString()

                        ' call your newly-parameterized function
                        Await _bridge.PublishAsync(v2)
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        If Not (captureFile Is Nothing) And Capture_Flag = 1 And IgnoreCount = 0 Then
                            If TestmodeFlag = 1 Then  ' Capture all in Test Mode 
                                captureFile.Write("R:" + values(0) + " M:" + values(1) + " D:" + values(2) + " V:" + values(3) + " P:" + values(4) + " N:" + values(5) + "T" + vbCrLf)
                            ElseIf TestmodeFlag = 0 And Not TMWaveformFlag = 1 Then  ' Capture the only Displacement and SN if normal mode
                                captureFile.Write("D:" + values(2) + " N:" + values(5) + vbCrLf)

                            End If
                        End If

                        LowSpeedCode = Convert.ToInt32(values(6)) ' Low Speed Code
                        LowSpeedData = Convert.ToInt32(values(7)) ' Low Speed Data

                        If (LowSpeedCode = 3) And (Compensation.Temperature_Auto_CheckBox.Checked = True) Then
                            TemperatureAutoValue = LowSpeedData
                        ElseIf (LowSpeedCode = 5) And (Compensation.Pressure_Auto_CheckBox.Checked = True) Then
                            PressureAutoValue = LowSpeedData
                        ElseIf (LowSpeedCode = 6) And (Compensation.Humidity_Auto_Checkbox.Checked = True) Then
                            HumidityAutoValue = LowSpeedData
                        ElseIf (LowSpeedCode = 8) Then
                            SampleFrequency = LowSpeedData / 100
                            SamplePeriod = 1000000 / SampleFrequency
                            SimulationSamples = CInt(SampleFrequency / 40)
                        ElseIf (LowSpeedCode = 10) Then
                            FirmwareVersion = LowSpeedData
                        ElseIf (LowSpeedCode = 20) Then
                            HomodyneAxes = LowSpeedData And 255
                            HomodyneMultiplier = LowSpeedData >> 8
                        ElseIf (LowSpeedCode = 111) Then
                            Diagnostic2Value = LowSpeedData
                        ElseIf (LowSpeedCode = 101) Then
                            Diagnostic3Value = LowSpeedData
                        ElseIf (LowSpeedCode = 121) Then
                            DP32PRCCounts = LowSpeedData
                        ElseIf (LowSpeedCode = 122) Then
                            DP32COMCounts = LowSpeedData
                        End If

                        ' Data computation for primary axis
                        If Configuration.Interpolation_CheckBox.Checked = True Then
                            ' PhaseValue = Convert.ToInt32(values(4))
                            PhaseValue = CInt(AxisData(0, 4))
                            If (PhaseValue And &H7E00) = 0 Then
                                CurrentValuePhase = (CDbl(PhaseValue)) / 256
                            ElseIf (PhaseValue < 0) Then ' REF and MEAS both valid
                                CurrentValuePhase = (CDbl(PhaseValue)) / 256
                            Else
                                Diagnostic4Count = 10
                                Diagnostic4Save = PhaseValue
                                CurrentValuePhase = 0
                                ' CurrentValuePhase = PreviousCurrentValuePhase ' Use the previous value
                            End If
                        Else
                            CurrentValuePhase = 0
                        End If

                        ' currentValue = (Convert.ToDouble(values(2)) - CurrentValuePhase) * Wavelength / 2.0 - CurrentValueCorrection ' Difference in nm; 1/2 wavelength, because path traveled at least twice
                        currentValue = (Convert.ToDouble(AxisData(0, 2)) - CurrentValuePhase) * Wavelength / 2.0 - CurrentValueCorrection ' Difference in nm; 1/2 wavelength, because path traveled at least twice
                        ' velocityValue = Convert.ToDouble(values(3)) * Wavelength / 2.0 * SampleFrequency ' Velocity = displacement difference in 1/SampleFrequencys * SampleFrequency
                        velocityValue = (currentValue - previousValue) * SampleFrequency
                        If (Configuration.Encoder_Checkbox.Checked = False) Then
                            angleValue = Math.Asin((currentValue - zeroAdjustment) / Configuration.NumericUpDown_ARS.Value / 1000000) * angleCorrectionFactor * 57.296
                        Else
                            angleValue = (currentValue - zeroAdjustment) / Configuration.NumericUpDown_ARS.Value / 1000000 * angleCorrectionFactor * 57.296
                        End If
                        ' displayValue = Math.Asin(average / Configuration.NumericUpDown_ARS.Value / 1000000) * angleCorrectionFactor * 57.296
                        previousValue = currentValue

                        If IgnoreCount = 0 Then
                            If needsInitialZero = 1 Then
                                zeroAdjustment = currentValue
                                zeroAdjustment1 = currentValue1
                                zeroAdjustment2 = currentValue2
                                zeroAdjustment3 = currentValue3
                                average = 0
                                average1 = 0
                                average2 = 0
                                average3 = 0
                                velocity = 0
                                velocity1 = 0
                                velocity2 = 0
                                velocity3 = 0
                                angle = 0
                                angle1 = 0
                                angle2 = 0
                                angle3 = 0
                                velocityValue = 0
                                velocityValue1 = 0
                                velocityValue2 = 0
                                velocityValue3 = 0
                                angleValue = 0
                                angleValue1 = 0
                                angleValue2 = 0
                                angleValue3 = 0
                                averagingFromCurrent = 0
                                averagingFromPrevious = 0
                                velocityFromCurrent = 0
                                velocityFromPrevious = 0
                                angleFromCurrent = 0
                                angleFromPrevious = 0
                                SuspendCurrentValue = 0
                                SuspendCurrentValue1 = 0
                                SuspendCurrentValue2 = 0
                                SuspendCurrentValue3 = 0
                                previousValue = currentValue
                                previousValue1 = currentValue1
                                previousValue2 = currentValue2
                                previousValue3 = currentValue3
                                Phase_Error_Value.Visible = False

                                If Suspend.Text.Equals("Resume") Then  ' force exit from suspend mode
                                    Suspend.Text = "Suspend"
                                    Suspend.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
                                    Suspend.ForeColor = Color.FromKnownColor(KnownColor.Black)
                                    '   Suspend_Label.Visible = False
                                    SuspendFlag = 0
                                    ErrorFlag = 0
                                End If
                                needsInitialZero = 0 ' make sure to zero out the reference system only once
                            End If
                        End If
                        Try
                            serialnumberdifference = Convert.ToUInt64(values(5)) - previousserialnumber
                        Catch
                        End Try

                        If serialnumberdifference = 1 Then
                            REFFrequency = AxisData(0, 0) / SamplePeriod
                            MEASFrequency = AxisData(0, 1) / SamplePeriod
                            DIFFFrequency = MEASFrequency - REFFrequency
                            If SuspendFlag = 0 Then
                                If IgnoreCount = 0 Then
                                    ' If ErrorFlag = 0 Then
                                    If (EDEnabled = 1) Then
                                        If REFFrequency = 0 Then
                                            ErrorFlag = ErrorFlag Or 1 ' REF is dead => REF (Head) Error
                                        End If
                                        If MEASFrequency = 0 Then  ' MEAS is dead => MEAS (Path) Error
                                            ErrorFlag = ErrorFlag Or 2 ' MEAS is dead = > MEAS (Path) Error
                                        End If
                                        If MEASFrequency > ((2 * REFFrequency) - 0.1) Then
                                            ErrorFlag = ErrorFlag Or 4 ' Excessive stage speed positive => Slew (Rate+) error
                                        End If
                                        If MEASFrequency < 0.1 Then
                                            ErrorFlag = ErrorFlag Or 8 ' Excessive stage speed negative => Slew (Rate-) error
                                        End If
                                    End If
                                End If
                            End If

                            unitCorrectmm = 1 / unitCorrectionFactor / 1000000
                            angleCorrectdegree = 1 / angleCorrectionFactor

                            MultiplierCombined = multiplierCoefficient * HomodyneMultiplier

                            If SuspendFlag = 0 Then

                                PreviousAverage = average
                                averagingFromPrevious = (0 + averagingValue / 1000) * average ' nm
                                averagingFromCurrent = (1.0 - averagingValue / 1000) * straightnessMultiplier * (currentValue - zeroAdjustment) / MultiplierCombined
                                average = averagingFromPrevious + averagingFromCurrent

                                velocityFromPrevious = (0 + averagingValue / 1000) * velocity ' nm
                                velocityFromCurrent = velocityValue * (1.0 - averagingValue / 1000) / MultiplierCombined
                                velocity = velocityFromPrevious + velocityFromCurrent

                                angleFromPrevious = (0 + averagingValue / 1000) * angle
                                angleFromCurrent = angleValue * (1.0 - averagingValue / 1000)
                                angle = angleFromPrevious + angleFromCurrent

                                If MultipleAxesFlag > 1 Then

                                    average1 = straightnessMultiplier * (currentValue1 - zeroAdjustment1) / MultiplierCombined
                                    average2 = straightnessMultiplier * (currentValue2 - zeroAdjustment2) / MultiplierCombined
                                    average3 = straightnessMultiplier * (currentValue3 - zeroAdjustment3) / MultiplierCombined

                                    velocity1 = velocityValue1 / MultiplierCombined
                                    velocity2 = velocityValue2 / MultiplierCombined
                                    velocity3 = velocityValue3 / MultiplierCombined

                                    angle1 = angleValue1
                                    angle2 = angleValue2
                                    angle3 = angleValue3

                                    If AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                                        displayValue1 = Axis1Flip * angle1
                                        displayValue2 = Axis2Flip * angle2
                                        displayValue3 = Axis3Flip * angle3
                                    ElseIf VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' velocity mode
                                        displayValue1 = Axis1Flip * velocity1 * unitCorrectmm
                                        displayValue2 = Axis2Flip * velocity2 * unitCorrectmm
                                        displayValue3 = Axis3Flip * velocity3 * unitCorrectmm
                                    Else
                                        displayValue1 = Axis1Flip * average1 * unitCorrectmm
                                        displayValue2 = Axis2Flip * average2 * unitCorrectmm
                                        displayValue3 = Axis3Flip * average3 * unitCorrectmm
                                    End If
                                End If

                                If AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                                    displayValue = PrimaryAxisFlip * angle
                                ElseIf VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' velocity mode
                                    displayValue = PrimaryAxisFlip * velocity * unitCorrectmm
                                Else : displayValue = PrimaryAxisFlip * average * unitCorrectmm
                                End If

                                If GraphControl.Text.Equals("Disable Graph") Then
                                    If IgnoreCount = 0 Then
                                        displacementQueuex.Enqueue(chartcounter)
                                        If Graph_Averaging_CheckBox.Checked = False Then
                                            displacementQueuey.Enqueue(PrimaryAxisFlip * straightnessMultiplier * unitCorrectmm * (currentValue - zeroAdjustment) / MultiplierCombined)
                                        Else
                                            displacementQueuey.Enqueue(PrimaryAxisFlip * average * unitCorrectmm)
                                        End If

                                        velocityQueuex.Enqueue(chartcounter)
                                        If Graph_Averaging_CheckBox.Checked = False Then
                                            velocityQueuey.Enqueue(PrimaryAxisFlip * unitCorrectmm * velocityValue / MultiplierCombined)
                                        Else
                                            velocityQueuey.Enqueue(PrimaryAxisFlip * velocity * unitCorrectmm)
                                        End If

                                        'velocityQueuey.Enqueue(unitCorrectmm * velocityValue / multiplierCombined)
                                        angleQueuex.Enqueue(chartcounter)
                                        If Graph_Averaging_CheckBox.Checked = False Then
                                            angleQueuey.Enqueue(PrimaryAxisFlip * angleValue)
                                        Else
                                            angleQueuey.Enqueue(PrimaryAxisFlip * angle)
                                        End If

                                        ' angleQueuey.Enqueue(Math.Asin(average / 32.61 / 1000000) * angleCorrectdegree * 57.296)
                                        chartcounter = CULng(chartcounter + 1)
                                    End If
                                End If
                            End If
                        ElseIf 0 = serialnumberdifference Then
                            Console.Write(" sample duplicate" + vbCrLf)
                        Else
                            Console.Write((Convert.ToUInt64(values(5)) - previousserialnumber - 1).ToString + " sample(s) number skipped" + vbCrLf)
                        End If
                    Else 'values.length incorrect
                        Console.Write("values.length incorrect " + values.Length.ToString + vbCrLf)
                    End If
                    Try
                        previousserialnumber = Convert.ToUInt64(values(5))
                    Catch
                    End Try
                Next
            Catch ex As Exception
                'MsgBox(ex.ToString)
            End Try
        End If
        If IgnoreCount > 0 Then
            IgnoreCount = IgnoreCount - 1
        Else
            IgnoreCount = 0
        End If
    End Sub

    Delegate Sub SetTextCallback([text] As String)

    Private Sub myMenuItemUSBPort_Click(sender As Object, e As EventArgs)
        ' Add functionality to the menu items using the Click event.  
        ' clear menu first
        Dim currentMenu As MenuItem
        Dim i As Integer
        For i = menuItems.Count - 1 To 0 Step -1
            currentMenu = menuItems(i)
            myMenuItemUSBPort.MenuItems.Remove(currentMenu)
        Next
        menuItems.Clear()
        ' populate new items
        For Each sp As String In My.Computer.Ports.SerialPortNames
            Dim myMenuItemOpen As New MenuItem(sp)
            myMenuItemUSBPort.MenuItems.Add(myMenuItemOpen)
            menuItems.Add(myMenuItemOpen)
            AddHandler myMenuItemOpen.Click, AddressOf Me.Comport_Click
            mnuBar.MenuItems.Add(myMenuItemUSBPort)
        Next
    End Sub

    Private Sub myMenuIteminformation_Click(sender As Object, e As EventArgs)
        information.ShowDialog()
    End Sub

    Private Sub myMenuItemabout_Click(sender As Object, e As EventArgs)
        About.ShowDialog()
    End Sub

    Private Sub myMenuItemfinish_Click(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub myMenuItemLogFile_Click(sender As Object, e As EventArgs)
        ' If Not (captureFile Is Nothing) Then
        Dim saveFileDialog1 As New SaveFileDialog()
        If myMenuItemLogFile.Text = ("&Close Log File") Then ' Capture file is open
            Capture_Flag = 0
            Capture_Enable = 0
            Capture_Button.Text = "Enable Capture"
            Capture_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Capture_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
            captureFile.Close()
            Logfile_Text.Visible = False
            Logfile_Label.Visible = False
            myMenuItemLogFile.Text = ("& Open Log File")
        Else ' Capture file is null or closed
            saveFileDialog1.FileName = currentcapturefile
            saveFileDialog1.Filter = "Log Files |*.txt|All Files | *.*"
            saveFileDialog1.Title = "Select a Log File for Capture of Measurement Data"
            If (saveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                captureFileName = saveFileDialog1.FileName.ToString()
                captureFile = My.Computer.FileSystem.OpenTextFileWriter(captureFileName, False)
                Capture_Enable = 1
                myMenuItemLogFile.Text = ("&Close Log File")
                currentcapturefile = System.IO.Path.GetFileName(saveFileDialog1.FileName)
                Logfile_Text.Visible = True
                Logfile_Label.Visible = True
                Logfile_Text.Text = currentcapturefile.ToString
            End If
        End If
    End Sub

    Private Sub Capture_Button_Click(sender As Object, e As EventArgs) Handles Capture_Button.Click
        If Capture_Button.Text.Equals("Enable Capture") Then
            '    If Not (captureFile Is Nothing) Then
            If (Capture_Enable = 1) Then ' Turn capture on
                Capture_Button.Text = "Disable Capture"
                Capture_Button.BackgroundImage = uMDGUI.My.Resources.Resources.GreenButton1
                Capture_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
                captureFile.Write(vbCrLf + "Sample Frequency = " + SampleFrequency.ToString + " Hz" + vbCrLf + vbCrLf)
                Capture_Flag = 1
            End If
        Else
            Capture_Button.Text = "Enable Capture"
            Capture_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Capture_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
            Capture_Flag = 0
        End If
    End Sub

    Private Sub myMenuItemConfiguration_Click(sender As Object, e As EventArgs)
        ' pop up configuration window
        Configuration.ShowDialog()
    End Sub

    Private Sub myMenuItemCompensation_Click(sender As Object, e As EventArgs)
        ' pop up compensation window
        Compensation.ShowDialog()
    End Sub

    Private Sub myMenuItemTestMode_Click(sender As Object, e As EventArgs)
        ' pop up Test Mode window
        TestMode.ShowDialog()
    End Sub

    Private Sub ZeroButton_Click(sender As Object, e As EventArgs) Handles ZeroButton.Click
        ErrorFlag = 0
        needsInitialZero = 1
        IgnoreCount = 1
        '  Phase_Label.Visible = False
        Phase_Error_Label.Visible = False
        If Suspend.Text.Equals("Resume") Then
            Axis1_Value.Text = "0.000"
            Axis2_Value.Text = "0.000"
            Axis3_Value.Text = "0.000"
            ValueDisplay.Text = "0.000"
        End If
    End Sub

    Private Sub DFT(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ' not the fastest, but easy to implement
        ' https://en.wikipedia.org/wiki/Discrete_Fourier_transform
        ' a sample buffer of N samples produces N/2 frequency bins
        Dim progresscount As Integer = 0
        Thread.CurrentThread.Priority = ThreadPriority.BelowNormal
        Do
            Try
                resetEvent.WaitOne()
                resetEvent.Reset()
                For outerLoopCounter = 0 To CInt(((Dimension / 2) - 1))
                    RealPartOfDFT(outerLoopCounter) = 0
                    ImaginaryPartOfDFT(outerLoopCounter) = 0
                Next outerLoopCounter
                For outerLoopCounter = 0 To CInt(((Dimension / 2) - 1))
                    For innerLoopCounter = 0 To (Dimension - 1)
                        RealPartOfDFT(outerLoopCounter) = RealPartOfDFT(outerLoopCounter) + DFTValueList(innerLoopCounter) * Math.Cos(2 * Math.PI * outerLoopCounter * innerLoopCounter / Dimension)
                        ImaginaryPartOfDFT(outerLoopCounter) = ImaginaryPartOfDFT(outerLoopCounter) - DFTValueList(innerLoopCounter) * Math.Sin(2 * Math.PI * outerLoopCounter * innerLoopCounter / Dimension)
                    Next innerLoopCounter
                Next outerLoopCounter
                FFTdone = True
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Loop
    End Sub

    Private Sub DisplacementButton_Click(sender As Object, e As EventArgs) Handles DisplacementButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        AngleButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        straightnessMultiplier = 1
        multiplierCoefficient = multiplier

        UnitLabel.Visible = True
        TimeLabel.Visible = False
        AngleLabel.Visible = False

        Axis1_Units_Label.Visible = Axis1_Label.Visible
        Axis2_Units_Label.Visible = Axis2_Label.Visible
        Axis3_Units_Label.Visible = Axis3_Label.Visible

        Axis1_Time_Label.Visible = False
        Axis2_Time_Label.Visible = False
        Axis3_Time_Label.Visible = False

        Axis1_Angle_Label.Visible = False
        Axis2_Angle_Label.Visible = False
        Axis3_Angle_Label.Visible = False

        Chart1.Series.Clear()
        Chart1.Series.Add(positionSeries)

        Compression_Label.Text = "Time Compression"
        Label_Range.Text = "Displacement Range"
        Graph_Label.Text = "Displacement"
        RangeUnits.Text = UnitLabel.Text

        ScrollRate = CInt(NumericUpDown_Scale.Value)

        If GraphControl.Text.Equals("Enable Graph") Then
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            NumericUpDown_Scale.Visible = False
            Label_Range.Visible = False
            ComboBox_Range.Visible = False
            RangeUnits.Visible = False
            Axis_UnitsD.Visible = False
        Else
            Graph_Averaging_CheckBox.Visible = True
            Compression_Label.Visible = True
            NumericUpDown_Scale.Visible = True
            Label_Range.Visible = True
            ComboBox_Range.Visible = True

            If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
                Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                RangeUnits.Visible = False
            Else
                RangeUnits.Visible = True
            End If

            Axis_UnitsD.Visible = True
        End If

        Axis_S.Visible = False
        AngleLabel.Visible = False
        Axis_UnitsA.Visible = False
        Label_RangeTime.Visible = False
        Frequency_Axis.Visible = False
        ComboBox_DFT_Range.Visible = False
        DFT_Hz.Visible = False
    End Sub

    Private Sub VelocityButton_Click(sender As Object, e As EventArgs) Handles VelocityButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        AngleButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        multiplierCoefficient = multiplier

        UnitLabel.Visible = True
        TimeLabel.Visible = True
        AngleLabel.Visible = False

        Axis1_Units_Label.Visible = Axis1_Label.Visible
        Axis2_Units_Label.Visible = Axis2_Label.Visible
        Axis3_Units_Label.Visible = Axis3_Label.Visible

        Axis1_Time_Label.Visible = Axis1_Label.Visible
        Axis2_Time_Label.Visible = Axis2_Label.Visible
        Axis3_Time_Label.Visible = Axis3_Label.Visible

        Axis1_Angle_Label.Visible = False
        Axis2_Angle_Label.Visible = False
        Axis3_Angle_Label.Visible = False

        Chart1.Series.Clear()
        Chart1.Series.Add(velocitySeries)

        Compression_Label.Text = "Time Compression"
        Label_Range.Text = "Velociy Range"
        Graph_Label.Text = "Velocity"
        RangeUnits.Text = UnitLabel.Text

        ScrollRate = CInt(NumericUpDown_Scale.Value)

        If GraphControl.Text.Equals("Enable Graph") Then
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            RangeUnits.Visible = False
            Label_Range.Visible = False
            Label_RangeTime.Visible = False
            Axis_UnitsD.Visible = False
            Axis_S.Visible = False
            NumericUpDown_Scale.Visible = False
            ComboBox_Range.Visible = False
        Else
            Graph_Averaging_CheckBox.Visible = True
            Compression_Label.Visible = True
            NumericUpDown_Scale.Visible = True
            Label_Range.Visible = True
            ComboBox_Range.Visible = True

            If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
                Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                RangeUnits.Visible = False
                Label_RangeTime.Visible = False
            Else
                RangeUnits.Visible = True
                Label_RangeTime.Visible = True
            End If

            Axis_UnitsD.Visible = True
            Axis_S.Visible = True
        End If

        Axis_UnitsA.Visible = False
        Frequency_Axis.Visible = False
        ComboBox_DFT_Range.Visible = False
        DFT_Hz.Visible = False
    End Sub

    Private Sub AngleButton_Click(sender As Object, e As EventArgs) Handles AngleButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        AngleButton.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
            Graph_Label.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
            Graph_Label.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        straightnessMultiplier = 1
        multiplierCoefficient = 1

        UnitLabel.Visible = False
        TimeLabel.Visible = False
        AngleLabel.Visible = True

        Axis1_Units_Label.Visible = False
        Axis2_Units_Label.Visible = False
        Axis3_Units_Label.Visible = False

        Axis1_Time_Label.Visible = False
        Axis2_Time_Label.Visible = False
        Axis3_Time_Label.Visible = False

        Axis1_Angle_Label.Visible = Axis1_Label.Visible
        Axis2_Angle_Label.Visible = Axis2_Label.Visible
        Axis3_Angle_Label.Visible = Axis3_Label.Visible

        Chart1.Series.Clear()
        Chart1.Series.Add(angleseries)

        Compression_Label.Text = "Time Compression"
        Label_Range.Text = "Angle Range"
        RangeUnits.Text = AngleLabel.Text

        ScrollRate = CInt(NumericUpDown_Scale.Value)

        If GraphControl.Text.Equals("Enable Graph") Then
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            NumericUpDown_Scale.Visible = False
            ComboBox_Range.Visible = False
            Label_Range.Visible = False
            Label_RangeTime.Visible = False
            Axis_UnitsA.Visible = False
        Else
            Graph_Averaging_CheckBox.Visible = True
            Compression_Label.Visible = True
            NumericUpDown_Scale.Visible = True
            Label_Range.Visible = True
            ComboBox_Range.Visible = True

            If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
                Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                RangeUnits.Visible = False
                Label_RangeTime.Visible = False
            Else
                Label_RangeTime.Visible = False
                RangeUnits.Visible = True
            End If
            Axis_UnitsA.Visible = True
        End If

        Axis_UnitsD.Visible = False
        Axis_S.Visible = False
        Frequency_Axis.Visible = False
        ComboBox_DFT_Range.Visible = False
        NumericUpDown_Scale.Visible = True
        DFT_Hz.Visible = False
    End Sub

    Private Sub StraightnessLongButton_Click(sender As Object, e As EventArgs) Handles StraightnessLongButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        AngleButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        straightnessMultiplier = Configuration.NumericUpDown_SL_Coefficient.Value
        multiplierCoefficient = 1

        UnitLabel.Visible = True
        TimeLabel.Visible = False
        AngleLabel.Visible = False

        Axis1_Units_Label.Visible = Axis1_Label.Visible
        Axis2_Units_Label.Visible = Axis2_Label.Visible
        Axis3_Units_Label.Visible = Axis3_Label.Visible

        Axis1_Time_Label.Visible = False
        Axis2_Time_Label.Visible = False
        Axis3_Time_Label.Visible = False

        Axis1_Angle_Label.Visible = False
        Axis2_Angle_Label.Visible = False
        Axis3_Angle_Label.Visible = False

        Chart1.Series.Clear()
        Chart1.Series.Add(positionSeries)

        Compression_Label.Text = "Time Compression"
        Label_Range.Text = "Straightness Long Range"
        Graph_Label.Text = "Straightness Long"
        RangeUnits.Text = UnitLabel.Text

        ScrollRate = CInt(NumericUpDown_Scale.Value)

        If GraphControl.Text.Equals("Enable Graph") Then
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            NumericUpDown_Scale.Visible = False
            Label_Range.Visible = True
            ComboBox_Range.Visible = False
            RangeUnits.Visible = False
            Axis_UnitsD.Visible = False
        Else
            Graph_Averaging_CheckBox.Visible = True
            Compression_Label.Visible = True
            NumericUpDown_Scale.Visible = True
            ComboBox_Range.Visible = True

            If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
                Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                RangeUnits.Visible = False
            Else

                RangeUnits.Visible = True
            End If

            Axis_UnitsD.Visible = True
        End If

        Label_RangeTime.Visible = False
        Axis_S.Visible = False
        Axis_UnitsA.Visible = False
        Frequency_Axis.Visible = False
        ComboBox_DFT_Range.Visible = False
        DFT_Hz.Visible = False
    End Sub


    Private Sub StraightnessShortButton_Click(sender As Object, e As EventArgs) Handles StraightnessShortButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        AngleButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        straightnessMultiplier = Configuration.NumericUpDown_SS_Coefficient.Value
        multiplierCoefficient = 1

        UnitLabel.Visible = True
        TimeLabel.Visible = False
        AngleLabel.Visible = False

        Axis1_Units_Label.Visible = Axis1_Label.Visible
        Axis2_Units_Label.Visible = Axis2_Label.Visible
        Axis3_Units_Label.Visible = Axis3_Label.Visible

        Axis1_Time_Label.Visible = False
        Axis2_Time_Label.Visible = False
        Axis3_Time_Label.Visible = False

        Axis1_Angle_Label.Visible = False
        Axis2_Angle_Label.Visible = False
        Axis3_Angle_Label.Visible = False

        Chart1.Series.Clear()
        Chart1.Series.Add(positionSeries)

        Compression_Label.Text = "Time Compression"
        Label_Range.Text = "Straightness Short Range"
        Graph_Label.Text = "Straightness Short"
        RangeUnits.Text = UnitLabel.Text

        ScrollRate = CInt(NumericUpDown_Scale.Value)

        If GraphControl.Text.Equals("Enable Graph") Then
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            NumericUpDown_Scale.Visible = False
            Label_Range.Visible = False
            RangeUnits.Visible = False
            ComboBox_Range.Visible = False
            Axis_UnitsD.Visible = False
        Else
            Graph_Averaging_CheckBox.Visible = True
            Compression_Label.Visible = True
            NumericUpDown_Scale.Visible = True
            Label_Range.Visible = True
            ComboBox_Range.Visible = True

            If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
                Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
                Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
                RangeUnits.Visible = False
            Else
                RangeUnits.Visible = True
            End If

            Axis_UnitsD.Visible = True
        End If

        Label_RangeTime.Visible = False
        Axis_S.Visible = False
        Axis_UnitsA.Visible = False
        Frequency_Axis.Visible = False
        ComboBox_DFT_Range.Visible = False
        DFT_Hz.Visible = False
    End Sub

    Private Sub FrequencyButton_BackgroundImageChanged(sender As Object, e As EventArgs) Handles FrequencyButton.BackgroundImageChanged
        Me.ComboBox_Range.Text = "Auto"
    End Sub

    Private Sub FrequencyButton_Click(sender As Object, e As EventArgs) Handles FrequencyButton.Click
        DisplacementButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        DisplacementButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        VelocityButton.BackgroundImage = uMDGUI.My.Resources.Resources.Orange1
        AngleButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        If Configuration.Encoder_Checkbox.Checked = True Then
            AngleButton.Text = "Encoder Angle"
        Else
            AngleButton.Text = "Angle"
        End If
        StraightnessLongButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessLongButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        StraightnessShortButton.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        StraightnessShortButton.ForeColor = Color.FromKnownColor(KnownColor.Black)
        FrequencyButton.BackgroundImage = uMDGUI.My.Resources.Resources.Orange1
        FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        straightnessMultiplier = 1

        UnitLabel.Visible = True
        TimeLabel.Visible = False
        AngleLabel.Visible = False

        Axis1_Units_Label.Visible = Axis1_Label.Visible
        Axis2_Units_Label.Visible = Axis2_Label.Visible
        Axis3_Units_Label.Visible = Axis3_Label.Visible

        Axis1_Time_Label.Visible = False
        Axis2_Time_Label.Visible = False
        Axis3_Time_Label.Visible = False

        Axis1_Angle_Label.Visible = False
        Axis2_Angle_Label.Visible = False
        Axis3_Angle_Label.Visible = False

        Chart1.Series.Clear()
        Chart1.Series.Add(fftSeries)

        Graph_Label.Text = "Frequency (Hz)"

        ScrollRate = 1

        Graph_Averaging_CheckBox.Visible = False
        Compression_Label.Text = "DFT Frequency Range"
        Compression_Label.Visible = True
        NumericUpDown_Scale.Visible = False
        NumericUpDown_Scale.Value = 1
        ComboBox_DFT_Range.Visible = True
        Label_Range.Text = "DFT Amplitude Range"
        Label_Range.Visible = True
        ComboBox_Range.Visible = True
        RangeUnits.Visible = False
        Label_RangeTime.Visible = False

        Axis_UnitsD.Visible = False
        Axis_S.Visible = False
        Axis_UnitsA.Visible = False

        DFTMax = 0
        DFT_Hz.Visible = True
        Frequency_Axis.Visible = True
    End Sub

    Private Sub TrackBar1_selectionchangecommitted(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        averagingValue = 1000 * (1 - (1 / (Math.Pow(10, CDbl(TrackBar1.Value / 333)))))
        AverageLabel.Text = (averagingValue / 1000).ToString("F3")
        If ((TrackBar1.Value >= 0) And (TrackBar1.Value < 100)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ElseIf ((TrackBar1.Value >= 100) And (TrackBar1.Value < 300)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.Brown)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.Brown)
        ElseIf ((TrackBar1.Value >= 300) And (TrackBar1.Value < 500)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkRed)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkRed)
        ElseIf ((TrackBar1.Value >= 500) And (TrackBar1.Value < 600)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkOrange)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkOrange)
        ElseIf ((TrackBar1.Value >= 600) And (TrackBar1.Value < 700)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkGoldenrod)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkGoldenrod)
        ElseIf ((TrackBar1.Value >= 700) And (TrackBar1.Value < 800)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkGreen)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkGreen)
        ElseIf ((TrackBar1.Value >= 800) And (TrackBar1.Value < 900)) Then
            Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkBlue)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkBlue)
        Else : Averaging_Label.ForeColor = Color.FromKnownColor(KnownColor.DarkViolet)
            AverageLabel.ForeColor = Color.FromKnownColor(KnownColor.DarkViolet)
        End If
    End Sub

    Private Sub GraphControl_Click(sender As Object, e As EventArgs) Handles GraphControl.Click
        If GraphControl.Text.Equals("Disable Graph") Then ' Turn graph off
            GraphControl.Text = "Enable Graph"
            Chart1.Hide()
            Me.Height = 298
            Graph_Label.Visible = False
            Graph_Averaging_CheckBox.Visible = False
            Compression_Label.Visible = False
            NumericUpDown_Scale.Visible = False
            ComboBox_Range.Visible = False
            Label_Range.Visible = False
            Axis_UnitsD.Visible = False
            Axis_S.Visible = False
        Else
            GraphControl.Text = "Disable Graph" ' Turn graph on
            Chart1.Show()
            Me.Height = 600
            Graph_Label.Visible = True

            If FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                Graph_Averaging_CheckBox.Visible = False
                Compression_Label.Visible = True
                NumericUpDown_Scale.Visible = False
                ComboBox_Range.Visible = False
                Label_Range.Visible = False
                Axis_UnitsD.Visible = False
                Axis_S.Visible = False
                Axis_UnitsA.Visible = False
            ElseIf VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                Graph_Averaging_CheckBox.Visible = False
                Compression_Label.Visible = True
                NumericUpDown_Scale.Visible = True
                ComboBox_Range.Visible = True
                Label_Range.Visible = True
                Axis_UnitsD.Visible = True
                Axis_S.Visible = True
                Axis_UnitsA.Visible = False
            ElseIf AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                Graph_Averaging_CheckBox.Visible = False
                Compression_Label.Visible = True
                NumericUpDown_Scale.Visible = True
                ComboBox_Range.Visible = True
                Label_Range.Visible = True
                Axis_UnitsD.Visible = False
                Axis_S.Visible = False
                Axis_UnitsA.Visible = True
            Else
                Graph_Averaging_CheckBox.Visible = True
                Compression_Label.Visible = True
                NumericUpDown_Scale.Visible = True
                ComboBox_Range.Visible = True
                Label_Range.Visible = True
                Axis_UnitsD.Visible = True
                Axis_S.Visible = False
                Axis_UnitsA.Visible = False
            End If
        End If
    End Sub

    '   Private Sub NumericUpDown_Scale_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown_Scale.ValueChanged
    '       TimeScale = CInt(NumericUpDown_Scale.Value)
    '       ScrollRate = CInt(NumericUpDown_Scale.Value)
    '   End Sub

    Private Sub Suspend_Click(sender As Object, e As EventArgs) Handles Suspend.Click
        If Suspend.Text.Equals("Suspend") Then  ' Enter Suspend mode
            Suspend.Text = "Resume"
            Suspend.BackgroundImage = uMDGUI.My.Resources.Resources.YellowButton1
            Suspend.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
            SuspendCurrentValue = currentValue  ' CurrentValue when Suspend entered
            SuspendCurrentValue1 = currentValue1
            SuspendCurrentValue2 = currentValue2
            SuspendCurrentValue3 = currentValue3
            SuspendFlag = 1
        Else                                    ' Exit Suspend mode - Incrmeent CurrentValueCorrection by difference during Suspend
            Suspend.Text = "Suspend"
            Suspend.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
            Suspend.ForeColor = Color.FromKnownColor(KnownColor.Black)
            CurrentValueCorrection = CurrentValueCorrection + currentValue - SuspendCurrentValue
            CurrentValueCorrection1 = CurrentValueCorrection1 + currentValue1 - SuspendCurrentValue1
            CurrentValueCorrection2 = CurrentValueCorrection2 + currentValue2 - SuspendCurrentValue2
            CurrentValueCorrection3 = CurrentValueCorrection3 + currentValue3 - SuspendCurrentValue3
            SuspendFlag = 0
            ErrorFlag = 0
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' limit the update rate of the value to about 60 Hz

        If (FirmwareVersionSet = 0) Then
            If Not (FirmwareVersion = 0) Then
                About.Firmware_Version.Text = CDec(FirmwareVersion / 100).ToString("00.00")
                FirmwareVersionSet = 1
            End If
        End If

        If HomodyneAxes > HomodyneSet Then ' Turn off frequency readouts, error detection, show "Homodyne Mode""
            REFLabel.Visible = False
            MEASLabel.Visible = False
            DIFFLabel.Visible = False
            REF.Visible = False
            MEAS.Visible = False
            DIFF.Visible = False
            REFMHzLabel.Visible = False
            MEASMHzLabel.Visible = False
            DIFFKHzLabel.Visible = False
            TestMode.Error_Detection_CheckBox.Checked = False
            TestMode.Error_Detection_CheckBox.Visible = False
            EDEnabled = 0
            EDOff_Label.Text = "Homodyne Mode  X"
            EDOff_Label.ForeColor = Color.Brown
            EDOff_Label.Visible = True
            HomodyneMult_Label.Text = HomodyneMultiplier.ToString("0")
            HomodyneMult_Label.ForeColor = Color.Brown
            HomodyneMult_Label.Visible = True
            HomodyneSet = HomodyneAxes
            Configuration.Interpolation_CheckBox.Visible = False
        End If

        If ((MultipleAxesFlag And &H9) = &H9) Then
            Axis1_Label.Visible = True
        End If
        If ((MultipleAxesFlag And &H1) = 0) Then
            Axis1_Label.Visible = False
            Axis1_Value.Visible = False
            Axis1_Units_Label.Visible = False
            Axis1_Time_Label.Visible = False
            Axis1_Angle_Label.Visible = False
        End If
        If ((MultipleAxesFlag And &HA) = &HA) Then
            Axis2_Label.Visible = True
        End If
        If ((MultipleAxesFlag And &H2) = 0) Then
            Axis2_Label.Visible = False
            Axis2_Value.Visible = False
            Axis2_Units_Label.Visible = False
            Axis2_Time_Label.Visible = False
            Axis2_Angle_Label.Visible = False
        End If
        If ((MultipleAxesFlag And &HC) = &HC) Then
            Axis3_Label.Visible = True
        End If
        If ((MultipleAxesFlag And &H4) = 0) Then
            Axis3_Label.Visible = False
            Axis3_Value.Visible = False
            Axis3_Units_Label.Visible = False
            Axis3_Time_Label.Visible = False
            Axis3_Angle_Label.Visible = False
        End If

        If ((Compensation.Temperature_Auto_CheckBox.Checked = True) And (TemperatureAutoValue >= 0) And (TemperatureAutoValue <= 7000)) Then
            If (Compensation.ComboBox_TempUnits.Text = "Degrees F") Then
                Compensation.NumericUpDown_Temperature.Value = CDec((TemperatureAutoValue * 9 / 500) + 32)
            ElseIf (Compensation.ComboBox_TempUnits.Text = "Degrees K") Then
                Compensation.NumericUpDown_Temperature.Value = CDec((TemperatureAutoValue / 100) + 273)
            Else
                Compensation.NumericUpDown_Temperature.Value = CDec(TemperatureAutoValue / 100)
            End If
        End If

        If ((Compensation.Pressure_Auto_CheckBox.Checked = True) And (PressureAutoValue >= 50000) And (PressureAutoValue <= 200000)) Then
            If (Compensation.ComboBox_Pressure_Units.Text = "mm/Hg") Then
                Compensation.NumericUpDown_Pressure.Value = CDec(PressureAutoValue * 0.76 / 100)
            Else
                Compensation.NumericUpDown_Pressure.Value = CDec(PressureAutoValue / 100)
            End If
        End If

        If ((Compensation.Humidity_Auto_Checkbox.Checked = True) And (HumidityAutoValue >= 0) And (HumidityAutoValue <= 1000)) Then
            Compensation.NumericUpDown_Humidity.Value = CDec(HumidityAutoValue / 10)
        End If

        If TestMode.Diagnostic_Enable_CheckBox.Checked = True Then
            If HomodyneSet = 0 Then
                Phase_Label.Visible = True
                PORTB_Label.Visible = True
                REFMEAS_Label.Visible = True
                Phase_Error_Label.Visible = True
            Else
                Phase_Value.Visible = False
                PBA_RM_Value.Visible = False
                PBA_RP_Value.Visible = False
                RMA_RM_Value.Visible = False
                RMA_RP__Value.Visible = False
                Phase_Error_Label.Visible = False
                Phase_Error_Value.Visible = False
                Phase_Label.Visible = False
                PORTB_Label.Visible = False
                REFMEAS_Label.Visible = False
            End If

            Sample_Frequency_Label.Visible = True
            DP32_PRC_Percent_Label.Visible = True
            DP32_COM_Percent_Label.Visible = True

            If HomodyneSet = 0 Then
                If (PhaseValue And &H7E00) = 0 Or PhaseValue < 0 Then ' REF or MEAS valid
                    '   If Not PhaseValue = 0 Then
                    Phase_Value.Text = PhaseValue.ToString("###0") ' Phase
                    Phase_Value.Visible = True
                Else
                    Phase_Value.Visible = False
                End If

                If Not (Diagnostic2Value And &HFF00) = &H8000 Then
                    PBA_RM_Value.Text = (((((Diagnostic2Value >> 8) And &HFF) + 128) And &HFF) - 128).ToString("d0") ' REFToMEAS
                    PBA_RM_Value.Visible = True
                    PORTB_Label.Visible = True
                Else
                    PBA_RM_Value.Visible = False
                End If

                If Not (Diagnostic2Value And &HFF) = &H80 Then
                    PBA_RP_Value.Text = ((((Diagnostic2Value And &HFF) + 128) And &HFF) - 128).ToString("d0") ' REFPeriod
                    PBA_RP_Value.Visible = True
                Else
                    PBA_RP_Value.Visible = False
                End If

                If Not (Diagnostic3Value And &HFF00) = &H8000 Then
                    RMA_RM_Value.Text = (((((Diagnostic3Value >> 8) And &HFF) + 128) And &HFF) - 128).ToString("d0") ' Second REF change or calculated REF delay
                    RMA_RM_Value.Visible = True
                Else
                    RMA_RM_Value.Visible = False
                End If

                If Not (Diagnostic3Value And &HFF) = &H80 Then
                    RMA_RP__Value.Text = ((((Diagnostic3Value And &HFF) + 128) And &HFF) - 128).ToString("#0") ' First REF change
                    RMA_RP__Value.Visible = True
                Else
                    RMA_RP__Value.Visible = False
                End If

                If (Diagnostic4Count > 0) Then
                    Phase_Error_Value.Text = Diagnostic4Save.ToString("x0000") ' NoChange Flag Word
                    Diagnostic4Count -= 1
                    Phase_Error_Value.Visible = True
                ElseIf (Diagnostic4Count <= 0) Then
                    Phase_Error_Value.Visible = False
                End If
            End If

            If Not (DP32PRCCounts) = 0 Or (HomodyneAxes > 0) Then
                Sample_Frequency_Value.Visible = True
                Sample_Frequency_Value.Text = (SampleFrequency).ToString("##0.00")
                DP32_PRC_Percent_Value.Text = (CLng(DP32PRCCounts) / 655.36).ToString("##0.00")
                DP32_PRC_Percent_Value.Visible = True
            Else
                Sample_Frequency_Value.Visible = False
                DP32_PRC_Percent_Value.Visible = False
            End If

            If Not (DP32COMCounts) = 0 Or (HomodyneAxes > 0) Then
                DP32_COM_Percent_Value.Text = (CLng(DP32COMCounts) / 655.36).ToString("##0.00")
                DP32_COM_Percent_Value.Visible = True
            Else
                DP32_COM_Percent_Value.Visible = False
            End If
        Else
            Phase_Value.Visible = False
            PBA_RM_Value.Visible = False
            PBA_RP_Value.Visible = False
            RMA_RM_Value.Visible = False
            RMA_RP__Value.Visible = False
            Phase_Error_Value.Visible = False
            Sample_Frequency_Value.Visible = False
            DP32_PRC_Percent_Value.Visible = False
            DP32_COM_Percent_Value.Visible = False
            Phase_Label.Visible = False
            PORTB_Label.Visible = False
            REFMEAS_Label.Visible = False
            Phase_Error_Label.Visible = False
            Sample_Frequency_Label.Visible = False
            DP32_PRC_Percent_Label.Visible = False
            DP32_COM_Percent_Label.Visible = False
        End If

        If IgnoreCount = 0 Then
            If REFFrequency > 0 And HomodyneAxes = 0 Then
                REF.Text = REFFrequency.ToString("0.000")
                REF.Visible = True
            Else
                REF.Visible = False
            End If

            If MEASFrequency > 0 And HomodyneAxes = 0 Then
                MEAS.Text = MEASFrequency.ToString("0.000")
                MEAS.Visible = True
            Else
                MEAS.Visible = False
            End If

            If REFFrequency > 0 And MEASFrequency > 0 And HomodyneAxes = 0 Then
                DIFF.Text = (DIFFFrequency * 1000).ToString("#,##0.00")
                DIFF.Visible = True
            Else
                DIFF.Visible = False
            End If

            'Kludge to prevent momentary flashes of large value when turning on Test Mode FG
            If IgnoreCount = 0 Then ' And Math.Abs(velocityValue) < 100000000 Then

                '     If ((Configuration.Axis1_Polarity_CheckBox.Checked = True) And (PrimaryAxisSelect = 1)) Then
                ' displayValue = -displayValue
                ' End If
                ' If ((Configuration.Axis2_Polarity_CheckBox.Checked = True) And (PrimaryAxisSelect = 2)) Then
                ' displayValue = -displayValue
                ' End If
                ' If ((Configuration.Axis3_Polarity_CheckBox.Checked = True) And (PrimaryAxisSelect = 3)) Then
                'displayValue = -displayValue
                'End If

                If SuspendFlag = 1 Then
                    ' Do not update display

                ElseIf EDEnabled = 1 And ErrorFlag > 0 Then
                    UnitLabel.Visible = False
                    TimeLabel.Visible = False
                    AngleLabel.Visible = False

                    If (ErrorFlag And 3) = 3 Then
                        ValueDisplay.Text = "  No Signals Error"
                    ElseIf (ErrorFlag And 3) = 1 Then
                        ValueDisplay.Text = "  REF (Head) Error"
                    ElseIf (ErrorFlag And 3) = 2 Then
                        ValueDisplay.Text = " MEAS (Path) Error"
                    ElseIf (ErrorFlag And 4) = 4 Then
                        ValueDisplay.Text = "SLEW (Rate+) Error"
                    ElseIf (ErrorFlag And 8) = 8 Then
                        ValueDisplay.Text = "SLEW (Rate-) Error"
                    End If

                ElseIf AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                    AngleLabel.Visible = True
                    If angleCorrectionFactor = 3600 Then
                        ValueDisplay.Text = displayValue.ToString("###,###,###,##0.000") 'arcsec
                    ElseIf angleCorrectionFactor = 60 Then
                        ValueDisplay.Text = displayValue.ToString("#,###,###,##0.000.00") 'arcmin
                    ElseIf angleCorrectionFactor = 1.0 Then
                        ValueDisplay.Text = displayValue.ToString("###,###,##0.000,000") 'degree
                    End If
                Else
                    UnitLabel.Visible = True
                    If unitCorrectionFactor = 0.000001 Then
                        ValueDisplay.Text = displayValue.ToString("###,###,###,##0.00") 'nm
                    ElseIf unitCorrectionFactor = 0.001 Then
                        ValueDisplay.Text = displayValue.ToString("##,###,###,##0.000") 'um
                    ElseIf unitCorrectionFactor = 1 Then
                        ValueDisplay.Text = displayValue.ToString("##,###,##0.000,000") 'mm
                    ElseIf unitCorrectionFactor = 10 Then
                        ValueDisplay.Text = displayValue.ToString("#,###,##0.000,000,0") 'cm
                    ElseIf unitCorrectionFactor = 1000 Then
                        ValueDisplay.Text = displayValue.ToString("#,###,##0.000,000,00") 'm
                    ElseIf unitCorrectionFactor = 25.4 Then
                        ValueDisplay.Text = displayValue.ToString("#,###,##0.000,000,0") 'in
                    ElseIf unitCorrectionFactor = 304.8 Then
                        ValueDisplay.Text = displayValue.ToString("#,###,##0.000,000.00") 'ft
                    End If

                    If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                        TimeLabel.Visible = True
                    End If
                End If

                If SuspendFlag = 0 Then
                    If ((MultipleAxesFlag And &H9) = &H9) Then
                        ' If (Configuration.Axis1_Polarity_CheckBox.Checked = True) Then
                        ' displayValue1 = -displayValue1
                        'End If
                        Axis1_Value.Visible = True
                        If AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                            If angleCorrectionFactor = 3600 Then
                                Axis1_Value.Text = displayValue1.ToString("###,###,###,##0.000") 'arcsec
                            ElseIf angleCorrectionFactor = 60 Then
                                Axis1_Value.Text = displayValue1.ToString("#,###,###,##0.000.00") 'arcmin
                            ElseIf angleCorrectionFactor = 1.0 Then
                                Axis1_Value.Text = displayValue1.ToString("###,###,##0.000,000") 'degree
                            End If
                        Else
                            Axis1_Units_Label.Visible = True
                            If unitCorrectionFactor = 0.000001 Then
                                Axis1_Value.Text = displayValue1.ToString("###,###,###,##0.00") 'nm
                            ElseIf unitCorrectionFactor = 0.001 Then
                                Axis1_Value.Text = displayValue1.ToString("##,###,###,##0.000") 'um
                            ElseIf unitCorrectionFactor = 1 Then
                                Axis1_Value.Text = displayValue1.ToString("##,###,##0.000,000") 'mm
                            ElseIf unitCorrectionFactor = 10 Then
                                Axis1_Value.Text = displayValue1.ToString("#,###,##0.000,000,0") 'cm
                            ElseIf unitCorrectionFactor = 1000 Then
                                Axis1_Value.Text = displayValue1.ToString("#,###,##0.000,000,00") 'm
                            ElseIf unitCorrectionFactor = 25.4 Then
                                Axis1_Value.Text = displayValue1.ToString("#,###,##0.000,000,0") 'in
                            ElseIf unitCorrectionFactor = 304.8 Then
                                Axis1_Value.Text = displayValue1.ToString("#,###,##0.000,000.00") 'ft
                            End If

                            If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                                Axis1_Time_Label.Visible = True
                            End If
                        End If
                    End If

                    If ((MultipleAxesFlag And &HA) = &HA) Then
                        'If (Configuration.Axis2_Polarity_CheckBox.Checked = True) Then
                        ' displayValue2 = -displayValue2
                        'End If

                        Axis2_Value.Visible = True
                        If AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                            If angleCorrectionFactor = 3600 Then
                                Axis2_Value.Text = displayValue2.ToString("###,###,###,##0.000") 'arcsec
                            ElseIf angleCorrectionFactor = 60 Then
                                Axis2_Value.Text = displayValue2.ToString("#,###,###,##0.000.00") 'arcmin
                            ElseIf angleCorrectionFactor = 1.0 Then
                                Axis2_Value.Text = displayValue2.ToString("###,###,##0.000,000") 'degree
                            End If
                        Else
                            Axis2_Units_Label.Visible = True
                            If unitCorrectionFactor = 0.000001 Then
                                Axis2_Value.Text = displayValue2.ToString("###,###,###,##0.00") 'nm
                            ElseIf unitCorrectionFactor = 0.001 Then
                                Axis2_Value.Text = displayValue2.ToString("##,###,###,##0.000") 'um
                            ElseIf unitCorrectionFactor = 1 Then
                                Axis2_Value.Text = displayValue2.ToString("##,###,##0.000,000") 'mm
                            ElseIf unitCorrectionFactor = 10 Then
                                Axis2_Value.Text = displayValue2.ToString("#,###,##0.000,000,0") 'cm
                            ElseIf unitCorrectionFactor = 1000 Then
                                Axis2_Value.Text = displayValue2.ToString("#,###,##0.000,000,00") 'm
                            ElseIf unitCorrectionFactor = 25.4 Then
                                Axis2_Value.Text = displayValue2.ToString("#,###,##0.000,000,0") 'in
                            ElseIf unitCorrectionFactor = 304.8 Then
                                Axis2_Value.Text = displayValue2.ToString("#,###,##0.000,000.00") 'ft
                            End If

                            If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                                Axis2_Time_Label.Visible = True
                            End If
                        End If
                    End If

                    If ((MultipleAxesFlag And &HC) = &HC) Then
                        '    If (Configuration.Axis3_Polarity_CheckBox.Checked = True) Then
                        'displayValue3 = -displayValue3
                        ' End If

                        Axis3_Value.Visible = True
                        If AngleButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' angle mode
                            If angleCorrectionFactor = 3600 Then
                                Axis3_Value.Text = displayValue3.ToString("###,###,###,##0.000") 'arcsec
                            ElseIf angleCorrectionFactor = 60 Then
                                Axis3_Value.Text = displayValue3.ToString("#,###,###,##0.000.00") 'arcmin
                            ElseIf angleCorrectionFactor = 1.0 Then
                                Axis3_Value.Text = displayValue3.ToString("###,###,##0.000,000") 'degree
                            End If
                        Else
                            Axis3_Units_Label.Visible = True
                            If unitCorrectionFactor = 0.000001 Then
                                Axis3_Value.Text = displayValue3.ToString("###,###,###,##0.00") 'nm
                            ElseIf unitCorrectionFactor = 0.001 Then
                                Axis3_Value.Text = displayValue3.ToString("##,###,###,##0.000") 'um
                            ElseIf unitCorrectionFactor = 1 Then
                                Axis3_Value.Text = displayValue3.ToString("##,###,##0.000,000") 'mm
                            ElseIf unitCorrectionFactor = 10 Then
                                Axis3_Value.Text = displayValue3.ToString("#,###,##0.000,000,0") 'cm
                            ElseIf unitCorrectionFactor = 1000 Then
                                Axis3_Value.Text = displayValue3.ToString("#,###,##0.000,000,00") 'm
                            ElseIf unitCorrectionFactor = 25.4 Then
                                Axis3_Value.Text = displayValue3.ToString("#,###,##0.000,000,0") 'in
                            ElseIf unitCorrectionFactor = 304.8 Then
                                Axis3_Value.Text = displayValue3.ToString("#,###,##0.000,000.00") 'ft
                            End If
                            If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                                Axis3_Time_Label.Visible = True
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If GraphControl.Text.Equals("Disable Graph") Then   ' are we graphing?
            Dim x1 As Double
            Dim y1 As Double
            Dim x2 As Double
            Dim y2 As Double
            Dim x3 As Double
            Dim y3 As Double
            While displacementQueuex.Count > 0
                x1 = CDbl(displacementQueuex.Dequeue())
                y1 = CDbl(displacementQueuey.Dequeue())
                x2 = CDbl(velocityQueuex.Dequeue())
                y2 = CDbl(velocityQueuey.Dequeue())
                x3 = CDbl(angleQueuex.Dequeue())
                y3 = CDbl(angleQueuey.Dequeue())
                graphCount = graphCount + CULng(1)

                ScrollRate = CInt(NumericUpDown_Scale.Value)

                If 0 = (graphCount Mod ScrollRate) Then
                    plotCount = plotCount + CULng(1)
                    positionSeries.Points.AddXY(plotCount, y1)
                    positionSeries.Points.RemoveAt(0)
                    velocitySeries.Points.AddXY(plotCount, y2)
                    velocitySeries.Points.RemoveAt(0)
                    angleseries.Points.AddXY(plotCount, y3)
                    angleseries.Points.RemoveAt(0)
                    velocityValueList.Add(y2)
                    velocityValueList.RemoveAt(0)

                    ' Send every 16th plotted current average value to monitor COM port if open
                    If SerialPort2.IsOpen Then
                        If 0 = (MonitorCount Mod 8) Then
                            Monitor_Tick.Visible = Not Monitor_Tick.Visible
                        End If
                        If 0 = (MonitorCount Mod 16) Then
                            If (MultipleAxesFlag > 15) Then
                                average1_nm = CLng(average1) * Axis1Flip
                                average2_nm = CLng(average2) * Axis2Flip
                                average3_nm = CLng(average3) * Axis3Flip

                                SerialPort2.WriteLine(average1_nm.ToString("0") + ":" +
                                                      average2_nm.ToString("0") + ":" +
                                                      average3_nm.ToString("0") + ":*")
                            Else
                                average_nm = CLng(average) * PrimaryAxisFlip

                                SerialPort2.WriteLine(average_nm.ToString("0") + ":*")
                            End If
                        End If
                        MonitorCount = MonitorCount + 1
                    End If

                    If 0 = (graphCount Mod DFT_Skip_Factor) Then
                        DFTValueList.Add(y2)
                        DFTValueList.RemoveAt(0)
                    End If
                End If

            End While
            ' DFT related

            Dim counter As Integer

            If FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then  ' are we doing dft?

                If True = FFTdone Then   ' make sure we are not still busy with the previous calculation
                    FFTdone = False
                    fftSeries.Points.Clear()
                    For counter = 0 To (CInt(((Dimension / 2) - 1)))
                        '                        fftSeries.Points.AddXY(counter, (ImaginaryPartOfDFT(counter) * ImaginaryPartOfDFT(counter)) / 1048576 + (RealPartOfDFT(counter) * RealPartOfDFT(counter)) / 1048576)
                        'fftSeries.Points.AddXY(counter, (Math.Sqrt((ImaginaryPartOfDFT(counter) * ImaginaryPartOfDFT(counter)))) / 1048576 + (Math.Sqrt((RealPartOfDFT(counter) * RealPartOfDFT(counter))) / 1048576))
                        'fftSeries.Points.AddXY(counter, (Math.Sqrt((ImaginaryPartOfDFT(counter) * ImaginaryPartOfDFT(counter)) + (RealPartOfDFT(counter) * RealPartOfDFT(counter)) / 1024)))
                        '                        fftSeries.Points.AddXY(counter, Math.Sqrt((ImaginaryPartOfDFT(counter) * ImaginaryPartOfDFT(counter)) / 1048576 + (RealPartOfDFT(counter) * RealPartOfDFT(counter)) / 1048576))
                        fftSeries.Points.AddXY(counter, Math.Sqrt((ImaginaryPartOfDFT(counter) * ImaginaryPartOfDFT(counter)) + (RealPartOfDFT(counter) * RealPartOfDFT(counter))) / 1024)
                    Next
                    resetEvent.Set()
                End If
                'now update graphDIFF
            End If
            Chart1.ResetAutoValues()
        End If
    End Sub

    Private Sub SimulationTimer_Tick(sender As Object, e As EventArgs) Handles SimulationTimer.Tick

        ' USB Communications Format:

        ' Standard (Single Axis) Data:

        '  0: REF Frequency Count
        '  1: MEAS Frequency Count 1
        '  2: Distance 1
        '  3: Velocity Count 1
        '  4: Phase 1 - Fractional offset * 256 XOR 0x4000=no PORTB MEAS 1; 0x2000=no PORTB 2nd REF; 0x1000=no PORTB 1st REF
        '                                        0x800=no Timer3/5 MEAS 1; 0x400=no Timer3/5 2nd REF; 0x200=no Timer3/5 1st REF
        '  5: Sequence Number
        '  6: LowSpeedCode
        '  7: LowSpeedData

        ' Add these when Multiple Axes are Enabled:

        '  8: MEAS Frequency Count 2
        '  9: Distance 2
        ' 10: Velocity Count 2
        ' 11: Phase 2
        ' 12: MEAS Frequency Count 3
        ' 13: Distance 3
        ' 14: Velocity Count 3
        ' 15: Phase 3

        ' LowSpeedCode (specifies contents of LowSpeedData):

        ' 0-99 GUI Data/Control:

        '  0: No Data
        '  1: Laser Power
        '  2: Signal Strength
        '  3: Temperature 1 (XXX.YY, 0 to 70.00 degC)
        '  4: Temperature 2 (XXX.YY, 0 to 70.00 degC)
        '  5: Pressure (XXX.YY mBar, 500.00 to 2000.00)
        '  6: Humidity (XXX.Y%, 0 to 100.0)

        '  8: Sample Frequency (XXX.YY Hz, default is 610.35 Hz)

        ' 10: Firmware Version # XXX.YY

        ' 20: Homodyne Interferometer if non-zero = (# of axes + 256 * counts/cycle)

        ' 100-199 Diagnostics:

        ' 101: Phase FirstREFChange (LB) and SecondREFChange (HB) (Averaging 1)
        ' 102: Phase REFPeriod (LB) and REFtoMeas (HB) (Averaging 1)
        ' 111: Phase FirstREFChange (LB) and SecondREFChange (HB) (Averaging 2)
        ' 112: Phase REFPeriod (LB) and REFtoMeas (HB) (Averaging 2)

        ' 121: Number of DP32 CPU clocks spent in capture and analysis
        ' 122: Number of DP32 CPU clocks spent in USB communications including text conversion

        ' 200-255: Reserved

        Dim counter As Integer
        Dim simulationLowSpeedCodeSelect As Integer

        For counter = 0 To SimulationSamples ' With simulation timer at 25 ms, this produces 600 samples/second, close to 610.35
            simrefcount = simrefcount + CLng(TestMode.NumericUpDown_FGREF_Value.Value * SamplePeriod)
            simmeascount = simrefcount + CLng(simulationDistance)

            If TestMode.Button_Constant.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                waveform = 0
                simulationDistance = CLng(12638 * (TMUnitsFactor * (TestMode.TrackBar_Offset.Value * 0.01 * TMAmpValue) * MultiplierCombined / 2))

            ElseIf TestMode.Button_Ramp.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                waveform = waveform + ((0.000655 * TMFreqValue) * bangbang)
                simulationDistance = CLng(CDbl(12638) * TMUnitsFactor * ((waveform + TestMode.TrackBar_Offset.Value) * 0.01 * TMAmpValue * MultiplierCombined / 2))

            ElseIf TestMode.Button_Triangle.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' Triangle implemented as Asin(sin()) to be consistent with Sin
                waveform = 2 / Math.PI * Math.Asin(Math.Sin(simcount * (TMFreqValue * 0.1) * (Math.PI * 2) / SampleFrequency + phase))
                simulationDistance = CLng(12638 * TMUnitsFactor * ((waveform + TestMode.TrackBar_Offset.Value) * 0.01 * TMAmpValue * MultiplierCombined / 2))
                simcount = simcount + 1

            ElseIf TestMode.Button_Sine.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then ' At 1 Hz will do a cycle in SampleFrequency samples
                waveform = Math.Sin(simcount * (TMFreqValue * 0.1) * (Math.PI * 2) / SampleFrequency + phase)
                simulationDistance = CLng(12638.0 * TMUnitsFactor * ((waveform + TestMode.TrackBar_Offset.Value) * 0.01 * TMAmpValue * MultiplierCombined / 2.0))
                simcount = simcount + 1
            End If

            If simulationDistance < 0 Then
                bangbang = -1
            Else
                bangbang = 1
            End If

            simulationVelocity = (simulationDistance - previousSimulationDistance)

            simulationPhase = &H80 ' simluation Phase = 128

            simulationLowSpeedCodeSelect = CInt(simulationSerial) And &HF

            If (simulationLowSpeedCodeSelect) = 3 Then ' simulation Temperature = 34.00
                simulationLowSpeedCode = 3
                simulationLowSpeedData = 3400
            End If

            If (simulationLowSpeedCodeSelect) = 5 Then ' simulation Pressure = 746.00
                simulationLowSpeedCode = 5
                simulationLowSpeedData = 74600
            End If

            If (simulationLowSpeedCodeSelect) = 6 Then ' simulation Humidity = 89.0
                simulationLowSpeedCode = 6
                simulationLowSpeedData = 890
            End If

            If (simulationLowSpeedCodeSelect) = 8 Then ' simulation Sample Frequency = 610.35
                simulationLowSpeedCode = 8
                simulationLowSpeedData = 61035
            End If

            If (simulationLowSpeedCodeSelect) = 9 Then ' simulation DP32 PRC % = 10.00 + #Axes * 10.00
                simulationLowSpeedCode = 121
                simulationLowSpeedData = 6553 + (TMMultipleAxesFlag And &H1) * 6553 + (TMMultipleAxesFlag And &H2) * 3276 + (TMMultipleAxesFlag And &H4) * 1639
            End If

            If (simulationLowSpeedCodeSelect) = 10 Then ' simulation DP32 COM = 15.00 (1 axis) or 25.00 (2 or 3 axis)
                simulationLowSpeedCode = 122
                If (TMMultipleAxesFlag And &H6) > 0 Then
                    simulationLowSpeedData = 16384
                Else
                    simulationLowSpeedData = 9830
                End If
            End If

            If (simulationLowSpeedCodeSelect) = 11 Then ' simulation RMA RM RP = 8 25
                simulationLowSpeedCode = 101
                simulationLowSpeedData = 25 + (12 << 8)
            End If

            If (simulationLowSpeedCodeSelect) = 12 Then ' simulation PBA RM RP = 8 25
                simulationLowSpeedCode = 111
                simulationLowSpeedData = 25 + (12 << 8)
            End If

            If Not (TMMultipleAxesFlag And &H1) = 0 Then
                simMEASFrequencyCount1 = simmeascount - previoussimMEASCount
                simulationDistance1 = simulationDistance
                simulationVelocity1 = simulationVelocity
                simulationPhase1 = simulationPhase
            Else
                simMEASFrequencyCount1 = 0
                simulationDistance1 = 0
                simulationVelocity1 = 0
                simulationPhase1 = 0
            End If

            If Not (TMMultipleAxesFlag And &H2) = 0 Then
                simMEASFrequencyCount2 = simmeascount - previoussimMEASCount
                simulationDistance2 = simulationDistance
                simulationVelocity2 = simulationVelocity
                simulationPhase2 = simulationPhase
            Else
                simMEASFrequencyCount2 = 0
                simulationDistance2 = 0
                simulationVelocity2 = 0
                simulationPhase2 = 0
            End If

            If Not (TMMultipleAxesFlag And &H4) = 0 Then
                simMEASFrequencyCount3 = simmeascount - previoussimMEASCount
                simulationDistance3 = simulationDistance
                simulationVelocity3 = simulationVelocity
                simulationPhase3 = simulationPhase
            Else
                simMEASFrequencyCount3 = 0
                simulationDistance3 = 0
                simulationVelocity3 = 0
                simulationPhase3 = 0
            End If

            If Not (TMMultipleAxesFlag And &H8) = 0 Then ' Muliple axes mode
                simulatedData = (simrefcount - previoussimREFCount).ToString("D") + " " +
                 simMEASFrequencyCount1.ToString("D") + " " +
                 simulationDistance1.ToString("D") + " " +
                 simulationVelocity1.ToString("D") + " " +
                 simulationPhase1.ToString("D") + " " +
                 simulationSerial.ToString("D") + " " +
                 simulationLowSpeedCode.ToString("D") + " " +
                 simulationLowSpeedData.ToString("D") + " " +
                 simMEASFrequencyCount2.ToString("D") + " " +
                 simulationDistance2.ToString("D") + " " +
                 simulationVelocity2.ToString("D") + " " +
                 simulationPhase2.ToString("D") + " " +
                 simMEASFrequencyCount3.ToString("D") + " " +
                 simulationDistance3.ToString("D") + " " +
                 simulationVelocity3.ToString("D") + " " +
                 simulationPhase3.ToString("D")
            Else ' Single axis mode
                simulatedData = (simrefcount - previoussimREFCount).ToString("D") + " " +
                 (simmeascount - previoussimMEASCount).ToString("D") + " " +
                 simulationDistance.ToString("D") + " " +
                 simulationVelocity.ToString("D") + " " +
                 simulationPhase.ToString("D") + " " +
                 simulationSerial.ToString("D") + " " +
                 simulationLowSpeedCode.ToString("D") + " " +
                 simulationLowSpeedData.ToString("D")
            End If

            simulationSerial = simulationSerial + CULng(1)
            previousSimulationDistance = simulationDistance
            previoussimulationVelocity = simulationVelocity
            previoussimREFCount = simrefcount
            previoussimMEASCount = simmeascount

            Me.SetText(simulatedData)
        Next

    End Sub

    Private Sub REF_Click(sender As Object, e As EventArgs) Handles REF.Click
        ErrorFlag = ErrorFlag Or 1  ' Set the REF bit of the error flag word
    End Sub

    Private Sub MEAS_Click(sender As Object, e As EventArgs) Handles MEAS.Click
        ErrorFlag = ErrorFlag Or 2 ' Set the MEAS bit of the error flag word
    End Sub

    Private Sub DIFF_Click(sender As Object, e As EventArgs) Handles DIFF.Click
        If velocityValue > 0 Then ErrorFlag = 4 Else ErrorFlag = 8 ' Set the Slew+ or Slew- bit of the error flag word
    End Sub

    Private Sub ComboBox_Range_click(sender As Object, e As EventArgs) Handles ComboBox_Range.Click

        If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
            Label_RangeTime.Visible = True
            Label_RangeTime.Text = "/s"
        Else
            Label_RangeTime.Visible = False
        End If
        If FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
            RangeUnits.Visible = False
        End If
        RangeUnits.Visible = True
    End Sub

    Private Sub ComboBox_DFT_Range_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_DFT_Range.SelectedIndexChanged
        If ComboBox_DFT_Range.Text = "10" Then
            Frequency_Axis.Text = "  0.3         1                2                3                4                5                6               7                8                9              10"
            DFT_Skip_Factor = 30
        ElseIf ComboBox_DFT_Range.Text = "30" Then
            Frequency_Axis.Text = "    1          3                6                9               12             15              18              21              24              27              30"
            DFT_Skip_Factor = 10
        ElseIf ComboBox_DFT_Range.Text = "100" Then
            Frequency_Axis.Text = "1  3   6    10              20              30              40              50              60             70              80              90            100"
            DFT_Skip_Factor = 3
        ElseIf ComboBox_DFT_Range.Text = "300" Then
            Frequency_Axis.Text = "   10        30              60              90            120            150            180            210            240          270             300"
            DFT_Skip_Factor = 1
        End If

        If MFLoaded = 1 Then ' And FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
            For ClearCounter = 0 To Dimension - 1
                DFTValueList.Add(zero)
                DFTValueList.RemoveAt(0)
            Next ClearCounter
        End If
    End Sub

    Private Sub ComboBox_Range_TextChanged(sender As Object, e As EventArgs) Handles ComboBox_Range.TextChanged
        If Not Double.TryParse(ComboBox_Range.Text, range) Then     ' Boolean true if Auto
            Chart1.ChartAreas(0).AxisY.Minimum = Double.NaN
            Chart1.ChartAreas(0).AxisY.Maximum = Double.NaN
            Label_RangeTime.Visible = False
            RangeUnits.Visible = False

        Else
            Chart1.ChartAreas(0).AxisY.Maximum = range
            If FrequencyButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                Chart1.ChartAreas(0).AxisY.Minimum = 0
            Else
                Chart1.ChartAreas(0).AxisY.Minimum = -range
            End If
            If VelocityButton.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                Label_RangeTime.Visible = True
            Else
                Label_RangeTime.Visible = False
            End If
            If UnitLabel.Text = "Degree" Then
                RangeUnits.Text = AngleLabel.Text
                RangeUnits.Visible = True
            Else
                RangeUnits.Text = UnitLabel.Text
                RangeUnits.Visible = True
            End If
        End If
        Chart1.ResetAutoValues()
        Chart1.ChartAreas(0).RecalculateAxesScale()
    End Sub

    Private Sub Axis1_Label_Click(sender As Object, e As EventArgs) Handles Axis1_Label.Click
        PrimaryAxisSelect = 1
        Axis1_Label.ForeColor = Color.FromKnownColor(KnownColor.BlueViolet)
        Axis2_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        Axis3_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        zeroAdjustment = zeroAdjustment1
        CurrentValueCorrection = CurrentValueCorrection1
        ErrorFlag = 0
        '        needsInitialZero = 1
        IgnoreCount = 0
    End Sub

    Private Sub Axis2_Label_Click(sender As Object, e As EventArgs) Handles Axis2_Label.Click
        PrimaryAxisSelect = 2
        Axis1_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        Axis2_Label.ForeColor = Color.FromKnownColor(KnownColor.BlueViolet)
        Axis3_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        zeroAdjustment = zeroAdjustment2
        CurrentValueCorrection = CurrentValueCorrection2
        ErrorFlag = 0
        '       needsInitialZero = 1
        IgnoreCount = 0
    End Sub

    Private Sub ValueDisplay_Click(sender As Object, e As EventArgs) Handles ValueDisplay.Click

    End Sub

    Private Sub Axis3_Label_Click(sender As Object, e As EventArgs) Handles Axis3_Label.Click
        PrimaryAxisSelect = 3
        Axis1_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        Axis2_Label.ForeColor = Color.FromKnownColor(KnownColor.Black)
        Axis3_Label.ForeColor = Color.FromKnownColor(KnownColor.BlueViolet)
        zeroAdjustment = zeroAdjustment3
        CurrentValueCorrection = CurrentValueCorrection3
        ErrorFlag = 0
        '      needsInitialZero = 1
        IgnoreCount = 0
    End Sub
End Class

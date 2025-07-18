﻿Imports System.Windows.Forms

Public Class Compensation

    Public Temperature As Double
    Public Pressure As Double
    Public Humidity As Double
    Public TemperatureC As Double = 20
    Public PressureATM As Double = 1000
    Public HumidityRel As Double = 50
    Public TCorrection As Double = 1
    Public PCorrection As Double = 1
    Public HCorrection As Double = 1
    Public ECFactor As Double = 1

    Private Sub CMPDone_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CMPDone_Button.Click
        My.Settings.Temperature = NumericUpDown_Temperature.Value
        My.Settings.TempUnits = ComboBox_TempUnits.Text
        My.Settings.Pressure = NumericUpDown_Pressure.Value
        My.Settings.PressureUnits = ComboBox_Pressure_Units.Text
        My.Settings.Humidity = NumericUpDown_Humidity.Value
        My.Settings.TempFactor = TextBox_TempFactor.Text
        My.Settings.PresFactor = TextBox_PresFactor.Text
        My.Settings.HumiFactor = TextBox_HumiFactor.Text
        My.Settings.TFactor = TCorrection
        My.Settings.PFactor = PCorrection
        My.Settings.HFactor = HCorrection
        My.Settings.ECFactor = ECFactor
        My.Settings.Save()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Compensation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Temperature = My.Settings.Temperature
        Pressure = My.Settings.Pressure
        HumidityRel = My.Settings.Humidity
    End Sub

    Private Sub NumericUpDown_Temperature_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown_Temperature.ValueChanged
        If MainForm.MFLoaded = 1 Then
            Temperature = NumericUpDown_Temperature.Value
            If ComboBox_TempUnits.Text.Equals("Degrees C") Then
                TemperatureC = Temperature
            ElseIf ComboBox_TempUnits.Text.Equals("Degrees F") Then
                TemperatureC = (5 / 9) * (Temperature - 32)
            Else
                TemperatureC = Temperature - 273
                'TextBox_TempFactor.Text = MainForm.Temperature.ToString("#0000.000000")
            End If

            TCorrection = 1 / (1 + (0.000271375 * 293 / (273 + TemperatureC)))
            TextBox_TempFactor.Text = TCorrection.ToString("#0.000000000")

            If ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                ECFactor = TCorrection * PCorrection * HCorrection
                ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
                MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
                MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
            End If
        End If
    End Sub

    Private Sub ComboBox_TempUnits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_TempUnits.SelectedIndexChanged

        If MainForm.MFLoaded = 1 Then
            Temperature = NumericUpDown_Temperature.Value
            If ComboBox_TempUnits.Text.Equals("Degrees C") Then
                NumericUpDown_Temperature.Minimum = 0
                NumericUpDown_Temperature.Maximum = 70
                NumericUpDown_Temperature.Value = 20
                TemperatureC = 20
            ElseIf ComboBox_TempUnits.Text.Equals("Degrees F") Then
                NumericUpDown_Temperature.Minimum = 32
                NumericUpDown_Temperature.Maximum = 158
                NumericUpDown_Temperature.Value = 68
                TemperatureC = (5 / 9) * (Temperature - 32)
            Else
                NumericUpDown_Temperature.Minimum = 273
                NumericUpDown_Temperature.Maximum = 343
                NumericUpDown_Temperature.Value = 293
                TemperatureC = Temperature - 273
            End If

            TCorrection = 1 / (1 + (0.000271375 * 293 / (273 + TemperatureC)))
            TextBox_TempFactor.Text = TCorrection.ToString("#0.000000000")

            If ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                ECFactor = TCorrection * PCorrection * HCorrection
                ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
                MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
                MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
            End If
        End If
    End Sub

    Private Sub NumericUpDown_Pressure_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown_Pressure.ValueChanged
        If MainForm.MFLoaded = 1 Then
            Pressure = NumericUpDown_Pressure.Value
            If ComboBox_Pressure_Units.Text.Equals("mm/Hg") Then
                PressureATM = Pressure / 0.76
            Else
                PressureATM = (Pressure)
            End If

            PCorrection = 1 / (1 + (0.000271375 * PressureATM / 1013))
            TextBox_PresFactor.Text = PCorrection.ToString("#0.000000000")

            If ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                ECFactor = TCorrection * PCorrection * HCorrection
                ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
                MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
                MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
            End If
        End If
    End Sub

    Private Sub ComboBox_Pressure_Units_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox_Pressure_Units.SelectedIndexChanged
        If MainForm.MFLoaded = 1 Then
            Pressure = NumericUpDown_Pressure.Value
            If ComboBox_Pressure_Units.Text.Equals("mm/Hg") Then
                NumericUpDown_Pressure.Minimum = 380
                NumericUpDown_Pressure.Maximum = 1520
                NumericUpDown_Pressure.Value = 760
                Pressure = 760
                PressureATM = 1000
            Else
                ComboBox_Pressure_Units.Text.Equals("mBar")
                NumericUpDown_Pressure.Value = 1000
                NumericUpDown_Pressure.Minimum = 500
                NumericUpDown_Pressure.Maximum = 2000
                PressureATM = Pressure
            End If

            PCorrection = 1 / (1 + (0.000271375 * PressureATM / 1013))
            TextBox_PresFactor.Text = PCorrection.ToString("#0.000000000")

            If ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                '   ECFactor = TCorrection * PCorrection * HCorrection
                ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
                MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
                MainForm.WLText.Text = (MainForm.Wavelength * ECFactor).ToString("000.000000")
            End If
        End If
    End Sub

    Private Sub NumericUpDown_Humidity_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown_Humidity.ValueChanged
        If MainForm.MFLoaded = 1 Then
            Dim HumidityReference As Double = 1.000271375 * 10 ' 10 is for the 10 percent bins
            HumidityRel = NumericUpDown_Humidity.Value
            If HumidityRel < 0 Then
                HCorrection = 1 ' Out of range, assume 50 percent
            ElseIf HumidityRel < 10 Then  ' 0 to 10%
                HCorrection = HumidityReference / (((10 - HumidityRel) * 1.000271799) + (HumidityRel * 1.000271714))
            ElseIf HumidityRel < 20 Then                   ' 10 to 20%
                HCorrection = HumidityReference / (((20 - HumidityRel) * 1.000271714) + ((HumidityRel - 10) * 1.000271629))
            ElseIf HumidityRel < 30 Then                   ' 20 to 30%
                HCorrection = HumidityReference / (((30 - HumidityRel) * 1.000271629) + ((HumidityRel - 20) * 1.000271544))
            ElseIf HumidityRel < 40 Then                   ' 30 to 40%
                HCorrection = HumidityReference / (((40 - HumidityRel) * 1.000271544) + ((HumidityRel - 30) * 1.000271459))
            ElseIf HumidityRel < 50 Then                   ' 40 to 50%
                HCorrection = HumidityReference / (((50 - HumidityRel) * 1.000271459) + ((HumidityRel - 40) * 1.000271375))
            ElseIf HumidityRel < 60 Then                   ' 50 to 60%
                HCorrection = HumidityReference / (((60 - HumidityRel) * 1.000271375) + ((HumidityRel - 50) * 1.00027129))
            ElseIf HumidityRel < 70 Then                   ' 60 to 70%
                HCorrection = HumidityReference / (((70 - HumidityRel) * 1.00027129) + ((HumidityRel - 60) * 1.000271205))
            ElseIf HumidityRel < 80 Then                   ' 70 to 80%
                HCorrection = HumidityReference / (((80 - HumidityRel) * 1.000271205) + ((HumidityRel - 70) * 1.00027112))
            ElseIf HumidityRel < 90 Then                   ' 80 to 90%
                HCorrection = HumidityReference / (((90 - HumidityRel) * 1.00027112) + ((HumidityRel - 80) * 1.000271035))
            ElseIf HumidityRel <= 100 Then                 ' 90 to 100%
                HCorrection = HumidityReference / (((100 - HumidityRel) * 1.000271035) + ((HumidityRel - 90) * 1.00027095))
            Else
                HCorrection = 1 ' Out of range, assume 50 percent
            End If

            TextBox_HumiFactor.Text = HCorrection.ToString("#0.000000000")

            If ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText) Then
                ' ECFactor = TCorrection * PCorrection * HCorrection
                ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
                MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
                MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
            End If
        End If
    End Sub

    Private Sub ComboBox_Humidity_Units_SelectedIndexChanged(sender As Object, e As EventArgs)
        NumericUpDown_Humidity.Value = 50
    End Sub

    Private Sub ECOn_Button_Click(sender As Object, e As EventArgs) Handles ECOn_Button.Click
        ECOff_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        ECOff_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
        ECOn_Button.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ECFactor = TCorrection * PCorrection * HCorrection
        ECFactor = 1 - ((0.000786 * Pressure * 0.133322) / (273 + Temperature)) + (0.000000000015 * HumidityRel * (Temperature * Temperature + 160))
        MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
        MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
    End Sub

    Private Sub ECOff_Button_Click(sender As Object, e As EventArgs) Handles ECOff_Button.Click
        ECOff_Button.BackgroundImage = uMDGUI.My.Resources.Resources.ActiveButton6
        ECOff_Button.ForeColor = Color.FromKnownColor(KnownColor.ActiveCaptionText)
        ECOn_Button.BackgroundImage = uMDGUI.My.Resources.Resources.InActiveButton4
        ECOn_Button.ForeColor = Color.FromKnownColor(KnownColor.Black)
        MainForm.Wavelength = NumericUpDown_Wavelength.Value    ' should this not be a default value when we press off??
        ECFactor = 1.0
        MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
    End Sub

    Private Sub NumericUpDown_Wavelength_valueChangeCommitted(sender As Object, e As EventArgs) Handles NumericUpDown_Wavelength.ValueChanged
        If MainForm.MFLoaded = 1 Then
            MainForm.Wavelength = NumericUpDown_Wavelength.Value * ECFactor
            MainForm.WLText.Text = MainForm.Wavelength.ToString("000.000000")
        End If
    End Sub

End Class
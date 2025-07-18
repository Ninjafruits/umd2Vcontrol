﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class Settings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As Settings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New Settings()),Settings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As Settings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property Multiplier() As Integer
        Get
            Return CType(Me("Multiplier"),Integer)
        End Get
        Set
            Me("Multiplier") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property UnitCorrectionFactor() As Double
        Get
            Return CType(Me("UnitCorrectionFactor"),Double)
        End Get
        Set
            Me("UnitCorrectionFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property AveragingValue() As Double
        Get
            Return CType(Me("AveragingValue"),Double)
        End Get
        Set
            Me("AveragingValue") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property AngleCorrectionFactor() As Double
        Get
            Return CType(Me("AngleCorrectionFactor"),Double)
        End Get
        Set
            Me("AngleCorrectionFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("32.61")>  _
    Public Property Angle_Reflector_Spacing() As Double
        Get
            Return CType(Me("Angle_Reflector_Spacing"),Double)
        End Get
        Set
            Me("Angle_Reflector_Spacing") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("360")>  _
    Public Property SLCoefficient() As Double
        Get
            Return CType(Me("SLCoefficient"),Double)
        End Get
        Set
            Me("SLCoefficient") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("36")>  _
    Public Property SSCoefficient() As Double
        Get
            Return CType(Me("SSCoefficient"),Double)
        End Get
        Set
            Me("SSCoefficient") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("20")>  _
    Public Property Temperature() As Double
        Get
            Return CType(Me("Temperature"),Double)
        End Get
        Set
            Me("Temperature") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("760")>  _
    Public Property Pressure() As Double
        Get
            Return CType(Me("Pressure"),Double)
        End Get
        Set
            Me("Pressure") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("50")>  _
    Public Property Humidity() As Double
        Get
            Return CType(Me("Humidity"),Double)
        End Get
        Set
            Me("Humidity") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Degrees C")>  _
    Public Property TempUnits() As String
        Get
            Return CType(Me("TempUnits"),String)
        End Get
        Set
            Me("TempUnits") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("mm/Hg")>  _
    Public Property PressureUnits() As String
        Get
            Return CType(Me("PressureUnits"),String)
        End Get
        Set
            Me("PressureUnits") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0.999728699")>  _
    Public Property TempFactor() As String
        Get
            Return CType(Me("TempFactor"),String)
        End Get
        Set
            Me("TempFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0.999732179")>  _
    Public Property PresFactor() As String
        Get
            Return CType(Me("PresFactor"),String)
        End Get
        Set
            Me("PresFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1.000000000")>  _
    Public Property HumiFactor() As String
        Get
            Return CType(Me("HumiFactor"),String)
        End Get
        Set
            Me("HumiFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("632.991372")>  _
    Public Property VacuumWavelength() As String
        Get
            Return CType(Me("VacuumWavelength"),String)
        End Get
        Set
            Me("VacuumWavelength") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property TFactor() As Double
        Get
            Return CType(Me("TFactor"),Double)
        End Get
        Set
            Me("TFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property PFactor() As Double
        Get
            Return CType(Me("PFactor"),Double)
        End Get
        Set
            Me("PFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property HFactor() As Double
        Get
            Return CType(Me("HFactor"),Double)
        End Get
        Set
            Me("HFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property ECFactor() As Double
        Get
            Return CType(Me("ECFactor"),Double)
        End Get
        Set
            Me("ECFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("3")>  _
    Public Property ScrollFactor() As Integer
        Get
            Return CType(Me("ScrollFactor"),Integer)
        End Get
        Set
            Me("ScrollFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("3.75")>  _
    Public Property TMREFFrequency() As Double
        Get
            Return CType(Me("TMREFFrequency"),Double)
        End Get
        Set
            Me("TMREFFrequency") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("8")>  _
    Public Property TMWaveformFlag() As Integer
        Get
            Return CType(Me("TMWaveformFlag"),Integer)
        End Get
        Set
            Me("TMWaveformFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property TMUnitsFactor() As Double
        Get
            Return CType(Me("TMUnitsFactor"),Double)
        End Get
        Set
            Me("TMUnitsFactor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("LogFile1.txt")>  _
    Public Property LogFile() As String
        Get
            Return CType(Me("LogFile"),String)
        End Get
        Set
            Me("LogFile") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property DFT_Skip_Factor() As Integer
        Get
            Return CType(Me("DFT_Skip_Factor"),Integer)
        End Get
        Set
            Me("DFT_Skip_Factor") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property InterpolationFlag() As Integer
        Get
            Return CType(Me("InterpolationFlag"),Integer)
        End Get
        Set
            Me("InterpolationFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property DiagnosticReadoutFlag() As Integer
        Get
            Return CType(Me("DiagnosticReadoutFlag"),Integer)
        End Get
        Set
            Me("DiagnosticReadoutFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property ErrorDetectionFlag() As Integer
        Get
            Return CType(Me("ErrorDetectionFlag"),Integer)
        End Get
        Set
            Me("ErrorDetectionFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property TemperatureCompAutoFlag() As Integer
        Get
            Return CType(Me("TemperatureCompAutoFlag"),Integer)
        End Get
        Set
            Me("TemperatureCompAutoFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property PressureCompAutoFlag() As Integer
        Get
            Return CType(Me("PressureCompAutoFlag"),Integer)
        End Get
        Set
            Me("PressureCompAutoFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property HumidityCompAutoFlag() As Integer
        Get
            Return CType(Me("HumidityCompAutoFlag"),Integer)
        End Get
        Set
            Me("HumidityCompAutoFlag") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property Axis1Flip() As Integer
        Get
            Return CType(Me("Axis1Flip"),Integer)
        End Get
        Set
            Me("Axis1Flip") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property Axis2Flip() As Integer
        Get
            Return CType(Me("Axis2Flip"),Integer)
        End Get
        Set
            Me("Axis2Flip") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("1")>  _
    Public Property Axis3Flip() As Integer
        Get
            Return CType(Me("Axis3Flip"),Integer)
        End Get
        Set
            Me("Axis3Flip") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property EncoderFlag() As Integer
        Get
            Return CType(Me("EncoderFlag"),Integer)
        End Get
        Set
            Me("EncoderFlag") = value
        End Set
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.uMDGUI.Settings
            Get
                Return Global.uMDGUI.Settings.Default
            End Get
        End Property
    End Module
End Namespace

﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class About
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
        Me.uMD_Label = New System.Windows.Forms.Label()
        Me.Firmware_Label = New System.Windows.Forms.Label()
        Me.Developed_Label = New System.Windows.Forms.Label()
        Me.Jan_Label = New System.Windows.Forms.Label()
        Me.Sam_Label = New System.Windows.Forms.Label()
        Me.About_uMD1 = New System.Windows.Forms.Label()
        Me.Copyright1 = New System.Windows.Forms.Label()
        Me.TMClose_Button = New System.Windows.Forms.Button()
        Me.Sam_Link = New System.Windows.Forms.LinkLabel()
        Me.Jan_Link = New System.Windows.Forms.LinkLabel()
        Me.uMD_Version = New System.Windows.Forms.TextBox()
        Me.Firmware_Version = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'uMD_Label
        '
        Me.uMD_Label.AutoSize = True
        Me.uMD_Label.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uMD_Label.ForeColor = System.Drawing.Color.Black
        Me.uMD_Label.Location = New System.Drawing.Point(40, 81)
        Me.uMD_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.uMD_Label.Name = "uMD_Label"
        Me.uMD_Label.Size = New System.Drawing.Size(103, 22)
        Me.uMD_Label.TabIndex = 103
        Me.uMD_Label.Text = "GUI Build:"
        '
        'Firmware_Label
        '
        Me.Firmware_Label.AutoSize = True
        Me.Firmware_Label.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Firmware_Label.ForeColor = System.Drawing.Color.Black
        Me.Firmware_Label.Location = New System.Drawing.Point(40, 117)
        Me.Firmware_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Firmware_Label.Name = "Firmware_Label"
        Me.Firmware_Label.Size = New System.Drawing.Size(180, 22)
        Me.Firmware_Label.TabIndex = 104
        Me.Firmware_Label.Text = "Firmware Version:"
        '
        'Developed_Label
        '
        Me.Developed_Label.AutoSize = True
        Me.Developed_Label.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Developed_Label.ForeColor = System.Drawing.Color.Black
        Me.Developed_Label.Location = New System.Drawing.Point(40, 154)
        Me.Developed_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Developed_Label.Name = "Developed_Label"
        Me.Developed_Label.Size = New System.Drawing.Size(144, 22)
        Me.Developed_Label.TabIndex = 105
        Me.Developed_Label.Text = "Developed by:"
        '
        'Jan_Label
        '
        Me.Jan_Label.AutoSize = True
        Me.Jan_Label.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Jan_Label.ForeColor = System.Drawing.Color.Black
        Me.Jan_Label.Location = New System.Drawing.Point(79, 187)
        Me.Jan_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Jan_Label.Name = "Jan_Label"
        Me.Jan_Label.Size = New System.Drawing.Size(96, 22)
        Me.Jan_Label.TabIndex = 106
        Me.Jan_Label.Text = "Jan Beck"
        '
        'Sam_Label
        '
        Me.Sam_Label.AutoSize = True
        Me.Sam_Label.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Sam_Label.ForeColor = System.Drawing.Color.Black
        Me.Sam_Label.Location = New System.Drawing.Point(79, 214)
        Me.Sam_Label.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Sam_Label.Name = "Sam_Label"
        Me.Sam_Label.Size = New System.Drawing.Size(167, 22)
        Me.Sam_Label.TabIndex = 107
        Me.Sam_Label.Text = "Sam Goldwasser"
        '
        'About_uMD1
        '
        Me.About_uMD1.AutoSize = True
        Me.About_uMD1.Font = New System.Drawing.Font("Arial", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.About_uMD1.ForeColor = System.Drawing.Color.Black
        Me.About_uMD1.Location = New System.Drawing.Point(113, 34)
        Me.About_uMD1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.About_uMD1.Name = "About_uMD1"
        Me.About_uMD1.Size = New System.Drawing.Size(331, 29)
        Me.About_uMD1.TabIndex = 110
        Me.About_uMD1.Text = "Micro Measurement Display"
        '
        'Copyright1
        '
        Me.Copyright1.AutoSize = True
        Me.Copyright1.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Copyright1.ForeColor = System.Drawing.Color.Black
        Me.Copyright1.Location = New System.Drawing.Point(40, 254)
        Me.Copyright1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Copyright1.Name = "Copyright1"
        Me.Copyright1.Size = New System.Drawing.Size(225, 24)
        Me.Copyright1.TabIndex = 111
        Me.Copyright1.Text = "Copyright © 1994-2019"
        '
        'TMClose_Button
        '
        Me.TMClose_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TMClose_Button.BackgroundImage = Global.uMDGUI.My.Resources.Resources.InActiveButton4
        Me.TMClose_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.TMClose_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TMClose_Button.Location = New System.Drawing.Point(248, 318)
        Me.TMClose_Button.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TMClose_Button.Name = "TMClose_Button"
        Me.TMClose_Button.Size = New System.Drawing.Size(89, 28)
        Me.TMClose_Button.TabIndex = 112
        Me.TMClose_Button.Text = "CLOSE"
        '
        'Sam_Link
        '
        Me.Sam_Link.AutoSize = True
        Me.Sam_Link.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Sam_Link.LinkArea = New System.Windows.Forms.LinkArea(1, 29)
        Me.Sam_Link.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.Sam_Link.Location = New System.Drawing.Point(244, 214)
        Me.Sam_Link.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Sam_Link.Name = "Sam_Link"
        Me.Sam_Link.Size = New System.Drawing.Size(291, 27)
        Me.Sam_Link.TabIndex = 113
        Me.Sam_Link.TabStop = True
        Me.Sam_Link.Text = "(http://www.repairfaq.org/sam/)"
        Me.Sam_Link.UseCompatibleTextRendering = True
        '
        'Jan_Link
        '
        Me.Jan_Link.AutoSize = True
        Me.Jan_Link.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Jan_Link.LinkArea = New System.Windows.Forms.LinkArea(1, 38)
        Me.Jan_Link.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.Jan_Link.Location = New System.Drawing.Point(172, 187)
        Me.Jan_Link.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Jan_Link.Name = "Jan_Link"
        Me.Jan_Link.Size = New System.Drawing.Size(363, 27)
        Me.Jan_Link.TabIndex = 114
        Me.Jan_Link.TabStop = True
        Me.Jan_Link.Text = "(https://sites.google.com/site/janbeck/)"
        Me.Jan_Link.UseCompatibleTextRendering = True
        '
        'uMD_Version
        '
        Me.uMD_Version.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.uMD_Version.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.uMD_Version.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uMD_Version.Location = New System.Drawing.Point(145, 81)
        Me.uMD_Version.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.uMD_Version.Name = "uMD_Version"
        Me.uMD_Version.ReadOnly = True
        Me.uMD_Version.Size = New System.Drawing.Size(373, 22)
        Me.uMD_Version.TabIndex = 115
        Me.uMD_Version.Text = "Unknown"
        '
        'Firmware_Version
        '
        Me.Firmware_Version.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Firmware_Version.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Firmware_Version.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Firmware_Version.Location = New System.Drawing.Point(220, 117)
        Me.Firmware_Version.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Firmware_Version.Name = "Firmware_Version"
        Me.Firmware_Version.ReadOnly = True
        Me.Firmware_Version.Size = New System.Drawing.Size(171, 22)
        Me.Firmware_Version.TabIndex = 116
        Me.Firmware_Version.Text = "Unknown"
        '
        'About
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 384)
        Me.Controls.Add(Me.Firmware_Version)
        Me.Controls.Add(Me.uMD_Version)
        Me.Controls.Add(Me.Jan_Link)
        Me.Controls.Add(Me.Sam_Link)
        Me.Controls.Add(Me.TMClose_Button)
        Me.Controls.Add(Me.Copyright1)
        Me.Controls.Add(Me.About_uMD1)
        Me.Controls.Add(Me.Sam_Label)
        Me.Controls.Add(Me.Jan_Label)
        Me.Controls.Add(Me.Developed_Label)
        Me.Controls.Add(Me.Firmware_Label)
        Me.Controls.Add(Me.uMD_Label)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About Micro Measurement Display"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uMD_Label As System.Windows.Forms.Label
    Friend WithEvents Firmware_Label As System.Windows.Forms.Label
    Friend WithEvents Developed_Label As System.Windows.Forms.Label
    Friend WithEvents Jan_Label As System.Windows.Forms.Label
    Friend WithEvents Sam_Label As System.Windows.Forms.Label
    Friend WithEvents About_uMD1 As System.Windows.Forms.Label
    Friend WithEvents Copyright1 As System.Windows.Forms.Label
    Friend WithEvents TMClose_Button As System.Windows.Forms.Button
    Friend WithEvents Sam_Link As System.Windows.Forms.LinkLabel
    Friend WithEvents Jan_Link As System.Windows.Forms.LinkLabel
    Friend WithEvents uMD_Version As System.Windows.Forms.TextBox
    Friend WithEvents Firmware_Version As System.Windows.Forms.TextBox
End Class

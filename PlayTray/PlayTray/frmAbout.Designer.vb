<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.lnkDocumentation = New System.Windows.Forms.LinkLabel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.btnCheckUpdates = New System.Windows.Forms.Button()
        Me.lnkLicense = New System.Windows.Forms.LinkLabel()
        Me.btnCredits = New System.Windows.Forms.Button()
        Me.btnCopyInfo = New System.Windows.Forms.Button()
        Me.lblCredits1 = New System.Windows.Forms.Label()
        Me.lblCredits2 = New System.Windows.Forms.Label()
        Me.lnkBass = New System.Windows.Forms.LinkLabel()
        Me.lblCredits0 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblCredits3 = New System.Windows.Forms.Label()
        Me.lnkDiscogs = New System.Windows.Forms.LinkLabel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtShortcuts = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtSystemInfo = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'picIcon
        '
        Me.picIcon.Location = New System.Drawing.Point(19, -1)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(88, 92)
        Me.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picIcon.TabIndex = 0
        Me.picIcon.TabStop = False
        '
        'lblAppName
        '
        Me.lblAppName.AutoSize = True
        Me.lblAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblAppName.Location = New System.Drawing.Point(130, 14)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(81, 20)
        Me.lblAppName.TabIndex = 1
        Me.lblAppName.Text = "Play Tray"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(132, 37)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(69, 13)
        Me.lblVersion.TabIndex = 2
        Me.lblVersion.Text = "Version 1.0.0"
        '
        'lblCopyright
        '
        Me.lblCopyright.AutoSize = True
        Me.lblCopyright.Location = New System.Drawing.Point(132, 55)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(43, 13)
        Me.lblCopyright.TabIndex = 3
        Me.lblCopyright.Text = "© 2025"
        '
        'lnkDocumentation
        '
        Me.lnkDocumentation.AutoSize = True
        Me.lnkDocumentation.Location = New System.Drawing.Point(131, 550)
        Me.lnkDocumentation.Name = "lnkDocumentation"
        Me.lnkDocumentation.Size = New System.Drawing.Size(89, 13)
        Me.lnkDocumentation.TabIndex = 5
        Me.lnkDocumentation.TabStop = True
        Me.lnkDocumentation.Text = "Visit Github Repo"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(293, 540)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 33)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "OK"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblDescription
        '
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblDescription.Location = New System.Drawing.Point(6, 12)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(337, 45)
        Me.lblDescription.TabIndex = 7
        Me.lblDescription.Text = "Network player"
        '
        'btnCheckUpdates
        '
        Me.btnCheckUpdates.Enabled = False
        Me.btnCheckUpdates.Location = New System.Drawing.Point(15, 540)
        Me.btnCheckUpdates.Name = "btnCheckUpdates"
        Me.btnCheckUpdates.Size = New System.Drawing.Size(110, 33)
        Me.btnCheckUpdates.TabIndex = 9
        Me.btnCheckUpdates.Text = "Check Updates"
        Me.btnCheckUpdates.UseVisualStyleBackColor = True
        '
        'lnkLicense
        '
        Me.lnkLicense.AutoSize = True
        Me.lnkLicense.Location = New System.Drawing.Point(226, 550)
        Me.lnkLicense.Name = "lnkLicense"
        Me.lnkLicense.Size = New System.Drawing.Size(44, 13)
        Me.lnkLicense.TabIndex = 10
        Me.lnkLicense.TabStop = True
        Me.lnkLicense.Text = "License"
        '
        'btnCredits
        '
        Me.btnCredits.Location = New System.Drawing.Point(291, 43)
        Me.btnCredits.Name = "btnCredits"
        Me.btnCredits.Size = New System.Drawing.Size(75, 33)
        Me.btnCredits.TabIndex = 11
        Me.btnCredits.Text = "Credits"
        Me.btnCredits.UseVisualStyleBackColor = True
        Me.btnCredits.Visible = False
        '
        'btnCopyInfo
        '
        Me.btnCopyInfo.Location = New System.Drawing.Point(291, 12)
        Me.btnCopyInfo.Name = "btnCopyInfo"
        Me.btnCopyInfo.Size = New System.Drawing.Size(75, 33)
        Me.btnCopyInfo.TabIndex = 12
        Me.btnCopyInfo.Text = "Copy Info"
        Me.btnCopyInfo.UseVisualStyleBackColor = True
        Me.btnCopyInfo.Visible = False
        '
        'lblCredits1
        '
        Me.lblCredits1.AutoSize = True
        Me.lblCredits1.Location = New System.Drawing.Point(18, 14)
        Me.lblCredits1.Name = "lblCredits1"
        Me.lblCredits1.Size = New System.Drawing.Size(70, 13)
        Me.lblCredits1.TabIndex = 13
        Me.lblCredits1.Text = "Development"
        '
        'lblCredits2
        '
        Me.lblCredits2.AutoSize = True
        Me.lblCredits2.Location = New System.Drawing.Point(18, 118)
        Me.lblCredits2.Name = "lblCredits2"
        Me.lblCredits2.Size = New System.Drawing.Size(100, 13)
        Me.lblCredits2.TabIndex = 14
        Me.lblCredits2.Text = "Third-Party Libraries"
        '
        'lnkBass
        '
        Me.lnkBass.AutoSize = True
        Me.lnkBass.Location = New System.Drawing.Point(31, 144)
        Me.lnkBass.Name = "lnkBass"
        Me.lnkBass.Size = New System.Drawing.Size(44, 13)
        Me.lnkBass.TabIndex = 15
        Me.lnkBass.TabStop = True
        Me.lnkBass.Text = "Unseen"
        '
        'lblCredits0
        '
        Me.lblCredits0.Location = New System.Drawing.Point(6, 51)
        Me.lblCredits0.Name = "lblCredits0"
        Me.lblCredits0.Size = New System.Drawing.Size(337, 22)
        Me.lblCredits0.TabIndex = 16
        Me.lblCredits0.Text = "Remake"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblCredits3)
        Me.GroupBox1.Controls.Add(Me.lnkDiscogs)
        Me.GroupBox1.Controls.Add(Me.lblCredits2)
        Me.GroupBox1.Controls.Add(Me.lblCredits1)
        Me.GroupBox1.Controls.Add(Me.lnkBass)
        Me.GroupBox1.Location = New System.Drawing.Point(17, 158)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(349, 217)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        '
        'lblCredits3
        '
        Me.lblCredits3.AutoSize = True
        Me.lblCredits3.Location = New System.Drawing.Point(18, 166)
        Me.lblCredits3.Name = "lblCredits3"
        Me.lblCredits3.Size = New System.Drawing.Size(100, 13)
        Me.lblCredits3.TabIndex = 16
        Me.lblCredits3.Text = "Third-Party Libraries"
        '
        'lnkDiscogs
        '
        Me.lnkDiscogs.AutoSize = True
        Me.lnkDiscogs.Location = New System.Drawing.Point(31, 181)
        Me.lnkDiscogs.Name = "lnkDiscogs"
        Me.lnkDiscogs.Size = New System.Drawing.Size(45, 13)
        Me.lnkDiscogs.TabIndex = 17
        Me.lnkDiscogs.TabStop = True
        Me.lnkDiscogs.Text = "Discogs"
        '
        'TabControl1
        '
        Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(17, 381)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(349, 153)
        Me.TabControl1.TabIndex = 18
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtShortcuts)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(341, 124)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Shortcuts"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'txtShortcuts
        '
        Me.txtShortcuts.Location = New System.Drawing.Point(-2, -3)
        Me.txtShortcuts.Multiline = True
        Me.txtShortcuts.Name = "txtShortcuts"
        Me.txtShortcuts.ReadOnly = True
        Me.txtShortcuts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtShortcuts.Size = New System.Drawing.Size(343, 126)
        Me.txtShortcuts.TabIndex = 9
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.txtSystemInfo)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(341, 124)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "System Info"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'txtSystemInfo
        '
        Me.txtSystemInfo.Location = New System.Drawing.Point(0, 0)
        Me.txtSystemInfo.Multiline = True
        Me.txtSystemInfo.Name = "txtSystemInfo"
        Me.txtSystemInfo.ReadOnly = True
        Me.txtSystemInfo.Size = New System.Drawing.Size(341, 123)
        Me.txtSystemInfo.TabIndex = 5
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblCredits0)
        Me.GroupBox2.Controls.Add(Me.lblDescription)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 82)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(349, 78)
        Me.GroupBox2.TabIndex = 19
        Me.GroupBox2.TabStop = False
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(385, 595)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCopyInfo)
        Me.Controls.Add(Me.btnCredits)
        Me.Controls.Add(Me.lnkLicense)
        Me.Controls.Add(Me.btnCheckUpdates)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lnkDocumentation)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblAppName)
        Me.Controls.Add(Me.picIcon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About"
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents picIcon As PictureBox
    Friend WithEvents lblAppName As Label
    Friend WithEvents lblVersion As Label
    Friend WithEvents lblCopyright As Label
    Friend WithEvents lnkDocumentation As LinkLabel
    Friend WithEvents btnClose As Button
    Friend WithEvents lblDescription As Label
    Friend WithEvents btnCheckUpdates As Button
    Friend WithEvents lnkLicense As LinkLabel
    Friend WithEvents btnCredits As Button
    Friend WithEvents btnCopyInfo As Button
    Friend WithEvents lblCredits1 As Label
    Friend WithEvents lblCredits2 As Label
    Friend WithEvents lnkBass As LinkLabel
    Friend WithEvents lblCredits0 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents txtShortcuts As TextBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents txtSystemInfo As TextBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblCredits3 As Label
    Friend WithEvents lnkDiscogs As LinkLabel
End Class

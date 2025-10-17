<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.tcSettings = New System.Windows.Forms.TabControl()
        Me.Tab1 = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.chkCheckUpdates = New System.Windows.Forms.CheckBox()
        Me.chkSaveHistory = New System.Windows.Forms.CheckBox()
        Me.chkExtractTitle = New System.Windows.Forms.CheckBox()
        Me.chkShowTips = New System.Windows.Forms.CheckBox()
        Me.chkShowSplash = New System.Windows.Forms.CheckBox()
        Me.chkAutoPlay = New System.Windows.Forms.CheckBox()
        Me.Tab2 = New System.Windows.Forms.TabPage()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboModifier = New System.Windows.Forms.ComboBox()
        Me.chkMultimediaKeys = New System.Windows.Forms.CheckBox()
        Me.Tab3 = New System.Windows.Forms.TabPage()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.btnTroubleshooting = New System.Windows.Forms.Button()
        Me.btnTestFirewall = New System.Windows.Forms.Button()
        Me.btnSetupFirewall = New System.Windows.Forms.Button()
        Me.btnSetupNetwork = New System.Windows.Forms.Button()
        Me.lblNote2 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.btnRefreshIPs = New System.Windows.Forms.Button()
        Me.lblBindAddress = New System.Windows.Forms.Label()
        Me.cboBindAddress = New System.Windows.Forms.ComboBox()
        Me.lblServerStatus = New System.Windows.Forms.Label()
        Me.numServerPort = New System.Windows.Forms.NumericUpDown()
        Me.chkEnableServer = New System.Windows.Forms.CheckBox()
        Me.Tab4 = New System.Windows.Forms.TabPage()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.lnkGetMusixMatchToken = New System.Windows.Forms.LinkLabel()
        Me.txtMusixMatchToken = New System.Windows.Forms.TextBox()
        Me.lblMusixMatch = New System.Windows.Forms.Label()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.btnValidate = New System.Windows.Forms.Button()
        Me.lnkGetToken = New System.Windows.Forms.LinkLabel()
        Me.txtDiscogsToken = New System.Windows.Forms.TextBox()
        Me.lblDiscogsToken = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblInactivityMinutes = New System.Windows.Forms.Label()
        Me.numAutoCloseMinutes = New System.Windows.Forms.NumericUpDown()
        Me.chkAutoClose = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblMuteSeconds = New System.Windows.Forms.Label()
        Me.numMuteLevel = New System.Windows.Forms.NumericUpDown()
        Me.numTimedMuteSeconds = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblLogLevel = New System.Windows.Forms.Label()
        Me.cboLogLevel = New System.Windows.Forms.ComboBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.tcSettings.SuspendLayout()
        Me.Tab1.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.Tab2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Tab3.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        CType(Me.numServerPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tab4.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.numAutoCloseMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.numMuteLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTimedMuteSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcSettings
        '
        Me.tcSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tcSettings.Controls.Add(Me.Tab1)
        Me.tcSettings.Controls.Add(Me.Tab2)
        Me.tcSettings.Controls.Add(Me.Tab3)
        Me.tcSettings.Controls.Add(Me.Tab4)
        Me.tcSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.tcSettings.Location = New System.Drawing.Point(0, 0)
        Me.tcSettings.Name = "tcSettings"
        Me.tcSettings.SelectedIndex = 0
        Me.tcSettings.Size = New System.Drawing.Size(634, 345)
        Me.tcSettings.TabIndex = 0
        '
        'Tab1
        '
        Me.Tab1.Controls.Add(Me.GroupBox5)
        Me.Tab1.Location = New System.Drawing.Point(4, 25)
        Me.Tab1.Name = "Tab1"
        Me.Tab1.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab1.Size = New System.Drawing.Size(626, 316)
        Me.Tab1.TabIndex = 0
        Me.Tab1.Text = "General"
        Me.Tab1.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.LinkLabel2)
        Me.GroupBox5.Controls.Add(Me.LinkLabel1)
        Me.GroupBox5.Controls.Add(Me.chkCheckUpdates)
        Me.GroupBox5.Controls.Add(Me.chkSaveHistory)
        Me.GroupBox5.Controls.Add(Me.chkExtractTitle)
        Me.GroupBox5.Controls.Add(Me.chkShowTips)
        Me.GroupBox5.Controls.Add(Me.chkShowSplash)
        Me.GroupBox5.Controls.Add(Me.chkAutoPlay)
        Me.GroupBox5.Location = New System.Drawing.Point(20, 10)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(598, 290)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Options"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(500, 197)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(74, 13)
        Me.LinkLabel2.TabIndex = 11
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Open beloved"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(383, 197)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(93, 13)
        Me.LinkLabel1.TabIndex = 10
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Open track history"
        '
        'chkCheckUpdates
        '
        Me.chkCheckUpdates.AutoSize = True
        Me.chkCheckUpdates.Location = New System.Drawing.Point(234, 236)
        Me.chkCheckUpdates.Name = "chkCheckUpdates"
        Me.chkCheckUpdates.Size = New System.Drawing.Size(113, 17)
        Me.chkCheckUpdates.TabIndex = 8
        Me.chkCheckUpdates.Text = "Check for updates"
        Me.chkCheckUpdates.UseVisualStyleBackColor = True
        '
        'chkSaveHistory
        '
        Me.chkSaveHistory.AutoSize = True
        Me.chkSaveHistory.Location = New System.Drawing.Point(234, 197)
        Me.chkSaveHistory.Name = "chkSaveHistory"
        Me.chkSaveHistory.Size = New System.Drawing.Size(111, 17)
        Me.chkSaveHistory.TabIndex = 7
        Me.chkSaveHistory.Text = "Save track history"
        Me.chkSaveHistory.UseVisualStyleBackColor = True
        '
        'chkExtractTitle
        '
        Me.chkExtractTitle.AutoSize = True
        Me.chkExtractTitle.Location = New System.Drawing.Point(234, 158)
        Me.chkExtractTitle.Name = "chkExtractTitle"
        Me.chkExtractTitle.Size = New System.Drawing.Size(173, 17)
        Me.chkExtractTitle.TabIndex = 6
        Me.chkExtractTitle.Text = "Enable LCD Smartie integration"
        Me.chkExtractTitle.UseVisualStyleBackColor = True
        '
        'chkShowTips
        '
        Me.chkShowTips.AutoSize = True
        Me.chkShowTips.Location = New System.Drawing.Point(234, 119)
        Me.chkShowTips.Name = "chkShowTips"
        Me.chkShowTips.Size = New System.Drawing.Size(153, 17)
        Me.chkShowTips.TabIndex = 5
        Me.chkShowTips.Text = "Show tips on track change"
        Me.chkShowTips.UseVisualStyleBackColor = True
        '
        'chkShowSplash
        '
        Me.chkShowSplash.AutoSize = True
        Me.chkShowSplash.Location = New System.Drawing.Point(234, 80)
        Me.chkShowSplash.Name = "chkShowSplash"
        Me.chkShowSplash.Size = New System.Drawing.Size(121, 17)
        Me.chkShowSplash.TabIndex = 4
        Me.chkShowSplash.Text = "Show splash screen"
        Me.chkShowSplash.UseVisualStyleBackColor = True
        '
        'chkAutoPlay
        '
        Me.chkAutoPlay.AutoSize = True
        Me.chkAutoPlay.Location = New System.Drawing.Point(234, 41)
        Me.chkAutoPlay.Name = "chkAutoPlay"
        Me.chkAutoPlay.Size = New System.Drawing.Size(120, 17)
        Me.chkAutoPlay.TabIndex = 3
        Me.chkAutoPlay.Text = "Auto play on startup"
        Me.chkAutoPlay.UseVisualStyleBackColor = True
        '
        'Tab2
        '
        Me.Tab2.Controls.Add(Me.lblNote)
        Me.Tab2.Controls.Add(Me.GroupBox1)
        Me.Tab2.Location = New System.Drawing.Point(4, 25)
        Me.Tab2.Name = "Tab2"
        Me.Tab2.Padding = New System.Windows.Forms.Padding(3)
        Me.Tab2.Size = New System.Drawing.Size(626, 316)
        Me.Tab2.TabIndex = 1
        Me.Tab2.Text = "Hotkeys"
        Me.Tab2.UseVisualStyleBackColor = True
        '
        'lblNote
        '
        Me.lblNote.AutoSize = True
        Me.lblNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblNote.ForeColor = System.Drawing.Color.RosyBrown
        Me.lblNote.Location = New System.Drawing.Point(245, 118)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(319, 13)
        Me.lblNote.TabIndex = 4
        Me.lblNote.Text = "NOTICE: Changes on this tab require program restart to be applied"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cboModifier)
        Me.GroupBox1.Controls.Add(Me.chkMultimediaKeys)
        Me.GroupBox1.Location = New System.Drawing.Point(20, 10)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(591, 104)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Hotkeys Multimedia Keys"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(225, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select hotkey combination"
        '
        'cboModifier
        '
        Me.cboModifier.AutoCompleteCustomSource.AddRange(New String() {"NONE", "CTRL+SHIFT", "CTRL+ALT", "CTRL+ALT+SHIFT", ""})
        Me.cboModifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModifier.FormattingEnabled = True
        Me.cboModifier.Location = New System.Drawing.Point(375, 31)
        Me.cboModifier.Name = "cboModifier"
        Me.cboModifier.Size = New System.Drawing.Size(146, 21)
        Me.cboModifier.TabIndex = 0
        '
        'chkMultimediaKeys
        '
        Me.chkMultimediaKeys.AutoSize = True
        Me.chkMultimediaKeys.Location = New System.Drawing.Point(228, 68)
        Me.chkMultimediaKeys.Name = "chkMultimediaKeys"
        Me.chkMultimediaKeys.Size = New System.Drawing.Size(136, 17)
        Me.chkMultimediaKeys.TabIndex = 1
        Me.chkMultimediaKeys.Text = "Enable multimedia keys"
        Me.chkMultimediaKeys.UseVisualStyleBackColor = True
        '
        'Tab3
        '
        Me.Tab3.Controls.Add(Me.GroupBox8)
        Me.Tab3.Controls.Add(Me.lblNote2)
        Me.Tab3.Controls.Add(Me.GroupBox6)
        Me.Tab3.Location = New System.Drawing.Point(4, 25)
        Me.Tab3.Name = "Tab3"
        Me.Tab3.Size = New System.Drawing.Size(626, 316)
        Me.Tab3.TabIndex = 2
        Me.Tab3.Text = "Network"
        Me.Tab3.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.btnTroubleshooting)
        Me.GroupBox8.Controls.Add(Me.btnTestFirewall)
        Me.GroupBox8.Controls.Add(Me.btnSetupFirewall)
        Me.GroupBox8.Controls.Add(Me.btnSetupNetwork)
        Me.GroupBox8.Location = New System.Drawing.Point(20, 196)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(597, 107)
        Me.GroupBox8.TabIndex = 11
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Tools"
        '
        'btnTroubleshooting
        '
        Me.btnTroubleshooting.Location = New System.Drawing.Point(385, 57)
        Me.btnTroubleshooting.Name = "btnTroubleshooting"
        Me.btnTroubleshooting.Size = New System.Drawing.Size(140, 32)
        Me.btnTroubleshooting.TabIndex = 9
        Me.btnTroubleshooting.Text = "❓ Help"
        Me.btnTroubleshooting.UseVisualStyleBackColor = True
        '
        'btnTestFirewall
        '
        Me.btnTestFirewall.Location = New System.Drawing.Point(216, 57)
        Me.btnTestFirewall.Name = "btnTestFirewall"
        Me.btnTestFirewall.Size = New System.Drawing.Size(140, 32)
        Me.btnTestFirewall.TabIndex = 8
        Me.btnTestFirewall.Text = "✓ Test Connection"
        Me.btnTestFirewall.UseVisualStyleBackColor = True
        '
        'btnSetupFirewall
        '
        Me.btnSetupFirewall.Location = New System.Drawing.Point(385, 19)
        Me.btnSetupFirewall.Name = "btnSetupFirewall"
        Me.btnSetupFirewall.Size = New System.Drawing.Size(140, 32)
        Me.btnSetupFirewall.TabIndex = 7
        Me.btnSetupFirewall.Text = " 2 - Setup Firewall Rule"
        Me.btnSetupFirewall.UseVisualStyleBackColor = True
        '
        'btnSetupNetwork
        '
        Me.btnSetupNetwork.Location = New System.Drawing.Point(214, 19)
        Me.btnSetupNetwork.Name = "btnSetupNetwork"
        Me.btnSetupNetwork.Size = New System.Drawing.Size(140, 32)
        Me.btnSetupNetwork.TabIndex = 6
        Me.btnSetupNetwork.Text = "1️ - Setup Network Access"
        Me.btnSetupNetwork.UseVisualStyleBackColor = True
        '
        'lblNote2
        '
        Me.lblNote2.AutoSize = True
        Me.lblNote2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblNote2.ForeColor = System.Drawing.Color.RosyBrown
        Me.lblNote2.Location = New System.Drawing.Point(235, 177)
        Me.lblNote2.Name = "lblNote2"
        Me.lblNote2.Size = New System.Drawing.Size(319, 13)
        Me.lblNote2.TabIndex = 10
        Me.lblNote2.Text = "NOTICE: Changes on this tab require program restart to be applied"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.btnRefreshIPs)
        Me.GroupBox6.Controls.Add(Me.lblBindAddress)
        Me.GroupBox6.Controls.Add(Me.cboBindAddress)
        Me.GroupBox6.Controls.Add(Me.lblServerStatus)
        Me.GroupBox6.Controls.Add(Me.numServerPort)
        Me.GroupBox6.Controls.Add(Me.chkEnableServer)
        Me.GroupBox6.Location = New System.Drawing.Point(20, 10)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(598, 164)
        Me.GroupBox6.TabIndex = 3
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Server"
        '
        'btnRefreshIPs
        '
        Me.btnRefreshIPs.Location = New System.Drawing.Point(467, 85)
        Me.btnRefreshIPs.Name = "btnRefreshIPs"
        Me.btnRefreshIPs.Size = New System.Drawing.Size(75, 32)
        Me.btnRefreshIPs.TabIndex = 5
        Me.btnRefreshIPs.Text = "🔄 Refresh"
        Me.btnRefreshIPs.UseVisualStyleBackColor = True
        '
        'lblBindAddress
        '
        Me.lblBindAddress.AutoSize = True
        Me.lblBindAddress.Location = New System.Drawing.Point(215, 61)
        Me.lblBindAddress.Name = "lblBindAddress"
        Me.lblBindAddress.Size = New System.Drawing.Size(80, 13)
        Me.lblBindAddress.TabIndex = 4
        Me.lblBindAddress.Text = "Bind to address"
        '
        'cboBindAddress
        '
        Me.cboBindAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBindAddress.FormattingEnabled = True
        Me.cboBindAddress.Location = New System.Drawing.Point(301, 58)
        Me.cboBindAddress.Name = "cboBindAddress"
        Me.cboBindAddress.Size = New System.Drawing.Size(241, 21)
        Me.cboBindAddress.TabIndex = 3
        '
        'lblServerStatus
        '
        Me.lblServerStatus.AutoSize = True
        Me.lblServerStatus.Location = New System.Drawing.Point(215, 88)
        Me.lblServerStatus.Name = "lblServerStatus"
        Me.lblServerStatus.Size = New System.Drawing.Size(104, 13)
        Me.lblServerStatus.TabIndex = 2
        Me.lblServerStatus.Text = "Server status display"
        '
        'numServerPort
        '
        Me.numServerPort.Location = New System.Drawing.Point(385, 20)
        Me.numServerPort.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.numServerPort.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numServerPort.Name = "numServerPort"
        Me.numServerPort.Size = New System.Drawing.Size(58, 20)
        Me.numServerPort.TabIndex = 1
        Me.numServerPort.Value = New Decimal(New Integer() {8999, 0, 0, 0})
        '
        'chkEnableServer
        '
        Me.chkEnableServer.AutoSize = True
        Me.chkEnableServer.Location = New System.Drawing.Point(218, 21)
        Me.chkEnableServer.Name = "chkEnableServer"
        Me.chkEnableServer.Size = New System.Drawing.Size(161, 17)
        Me.chkEnableServer.TabIndex = 0
        Me.chkEnableServer.Text = "Enable remote control server"
        Me.chkEnableServer.UseVisualStyleBackColor = True
        '
        'Tab4
        '
        Me.Tab4.Controls.Add(Me.GroupBox9)
        Me.Tab4.Controls.Add(Me.GroupBox7)
        Me.Tab4.Controls.Add(Me.GroupBox4)
        Me.Tab4.Controls.Add(Me.GroupBox3)
        Me.Tab4.Controls.Add(Me.GroupBox2)
        Me.Tab4.Location = New System.Drawing.Point(4, 25)
        Me.Tab4.Name = "Tab4"
        Me.Tab4.Size = New System.Drawing.Size(626, 316)
        Me.Tab4.TabIndex = 3
        Me.Tab4.Text = "Advanced"
        Me.Tab4.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.lnkGetMusixMatchToken)
        Me.GroupBox9.Controls.Add(Me.txtMusixMatchToken)
        Me.GroupBox9.Controls.Add(Me.lblMusixMatch)
        Me.GroupBox9.Location = New System.Drawing.Point(22, 244)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(588, 69)
        Me.GroupBox9.TabIndex = 13
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Lyrics"
        Me.GroupBox9.Visible = False
        '
        'lnkGetMusixMatchToken
        '
        Me.lnkGetMusixMatchToken.AutoSize = True
        Me.lnkGetMusixMatchToken.Location = New System.Drawing.Point(465, 42)
        Me.lnkGetMusixMatchToken.Name = "lnkGetMusixMatchToken"
        Me.lnkGetMusixMatchToken.Size = New System.Drawing.Size(109, 13)
        Me.lnkGetMusixMatchToken.TabIndex = 6
        Me.lnkGetMusixMatchToken.TabStop = True
        Me.lnkGetMusixMatchToken.Text = "Get your free API Key"
        '
        'txtMusixMatchToken
        '
        Me.txtMusixMatchToken.Location = New System.Drawing.Point(333, 17)
        Me.txtMusixMatchToken.Name = "txtMusixMatchToken"
        Me.txtMusixMatchToken.Size = New System.Drawing.Size(241, 20)
        Me.txtMusixMatchToken.TabIndex = 5
        '
        'lblMusixMatch
        '
        Me.lblMusixMatch.AutoSize = True
        Me.lblMusixMatch.Location = New System.Drawing.Point(182, 20)
        Me.lblMusixMatch.Name = "lblMusixMatch"
        Me.lblMusixMatch.Size = New System.Drawing.Size(145, 13)
        Me.lblMusixMatch.TabIndex = 4
        Me.lblMusixMatch.Text = "Enter your MusixMatch token"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.btnValidate)
        Me.GroupBox7.Controls.Add(Me.lnkGetToken)
        Me.GroupBox7.Controls.Add(Me.txtDiscogsToken)
        Me.GroupBox7.Controls.Add(Me.lblDiscogsToken)
        Me.GroupBox7.Location = New System.Drawing.Point(20, 144)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(591, 91)
        Me.GroupBox7.TabIndex = 12
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Cover art support"
        '
        'btnValidate
        '
        Me.btnValidate.Location = New System.Drawing.Point(337, 50)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(75, 32)
        Me.btnValidate.TabIndex = 3
        Me.btnValidate.Text = "Validate"
        Me.btnValidate.UseVisualStyleBackColor = True
        '
        'lnkGetToken
        '
        Me.lnkGetToken.AutoSize = True
        Me.lnkGetToken.Location = New System.Drawing.Point(480, 60)
        Me.lnkGetToken.Name = "lnkGetToken"
        Me.lnkGetToken.Size = New System.Drawing.Size(98, 13)
        Me.lnkGetToken.TabIndex = 2
        Me.lnkGetToken.TabStop = True
        Me.lnkGetToken.Text = "Get your free token"
        '
        'txtDiscogsToken
        '
        Me.txtDiscogsToken.Location = New System.Drawing.Point(337, 20)
        Me.txtDiscogsToken.Name = "txtDiscogsToken"
        Me.txtDiscogsToken.Size = New System.Drawing.Size(241, 20)
        Me.txtDiscogsToken.TabIndex = 1
        '
        'lblDiscogsToken
        '
        Me.lblDiscogsToken.AutoSize = True
        Me.lblDiscogsToken.Location = New System.Drawing.Point(207, 23)
        Me.lblDiscogsToken.Name = "lblDiscogsToken"
        Me.lblDiscogsToken.Size = New System.Drawing.Size(124, 13)
        Me.lblDiscogsToken.TabIndex = 0
        Me.lblDiscogsToken.Text = "Enter your discogs token"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblInactivityMinutes)
        Me.GroupBox4.Controls.Add(Me.numAutoCloseMinutes)
        Me.GroupBox4.Controls.Add(Me.chkAutoClose)
        Me.GroupBox4.Location = New System.Drawing.Point(20, 10)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(591, 61)
        Me.GroupBox4.TabIndex = 11
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Autoclose"
        '
        'lblInactivityMinutes
        '
        Me.lblInactivityMinutes.AutoSize = True
        Me.lblInactivityMinutes.Location = New System.Drawing.Point(402, 30)
        Me.lblInactivityMinutes.Name = "lblInactivityMinutes"
        Me.lblInactivityMinutes.Size = New System.Drawing.Size(43, 13)
        Me.lblInactivityMinutes.TabIndex = 5
        Me.lblInactivityMinutes.Text = "minutes"
        '
        'numAutoCloseMinutes
        '
        Me.numAutoCloseMinutes.Location = New System.Drawing.Point(356, 26)
        Me.numAutoCloseMinutes.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.numAutoCloseMinutes.Name = "numAutoCloseMinutes"
        Me.numAutoCloseMinutes.Size = New System.Drawing.Size(43, 20)
        Me.numAutoCloseMinutes.TabIndex = 2
        '
        'chkAutoClose
        '
        Me.chkAutoClose.AutoSize = True
        Me.chkAutoClose.Location = New System.Drawing.Point(210, 28)
        Me.chkAutoClose.Name = "chkAutoClose"
        Me.chkAutoClose.Size = New System.Drawing.Size(150, 17)
        Me.chkAutoClose.TabIndex = 0
        Me.chkAutoClose.Text = "Auto close on inactivity of "
        Me.chkAutoClose.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.lblMuteSeconds)
        Me.GroupBox3.Controls.Add(Me.numMuteLevel)
        Me.GroupBox3.Controls.Add(Me.numTimedMuteSeconds)
        Me.GroupBox3.Location = New System.Drawing.Point(20, 77)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(591, 61)
        Me.GroupBox3.TabIndex = 10
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Mute"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(208, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Temp mute for"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(441, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "percent of volume level"
        '
        'lblMuteSeconds
        '
        Me.lblMuteSeconds.AutoSize = True
        Me.lblMuteSeconds.Location = New System.Drawing.Point(332, 27)
        Me.lblMuteSeconds.Name = "lblMuteSeconds"
        Me.lblMuteSeconds.Size = New System.Drawing.Size(61, 13)
        Me.lblMuteSeconds.TabIndex = 6
        Me.lblMuteSeconds.Text = "seconds @"
        '
        'numMuteLevel
        '
        Me.numMuteLevel.Location = New System.Drawing.Point(397, 25)
        Me.numMuteLevel.Name = "numMuteLevel"
        Me.numMuteLevel.Size = New System.Drawing.Size(43, 20)
        Me.numMuteLevel.TabIndex = 4
        '
        'numTimedMuteSeconds
        '
        Me.numTimedMuteSeconds.Location = New System.Drawing.Point(286, 25)
        Me.numTimedMuteSeconds.Maximum = New Decimal(New Integer() {720, 0, 0, 0})
        Me.numTimedMuteSeconds.Name = "numTimedMuteSeconds"
        Me.numTimedMuteSeconds.Size = New System.Drawing.Size(43, 20)
        Me.numTimedMuteSeconds.TabIndex = 3
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblLogLevel)
        Me.GroupBox2.Controls.Add(Me.cboLogLevel)
        Me.GroupBox2.Enabled = False
        Me.GroupBox2.Location = New System.Drawing.Point(643, 137)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(591, 61)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Log"
        '
        'lblLogLevel
        '
        Me.lblLogLevel.AutoSize = True
        Me.lblLogLevel.Location = New System.Drawing.Point(209, 22)
        Me.lblLogLevel.Name = "lblLogLevel"
        Me.lblLogLevel.Size = New System.Drawing.Size(54, 13)
        Me.lblLogLevel.TabIndex = 8
        Me.lblLogLevel.Text = "Log Level"
        '
        'cboLogLevel
        '
        Me.cboLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLogLevel.FormattingEnabled = True
        Me.cboLogLevel.Items.AddRange(New Object() {"None", "Errors", "Info", "Debug"})
        Me.cboLogLevel.Location = New System.Drawing.Point(284, 19)
        Me.cboLogLevel.Name = "cboLogLevel"
        Me.cboLogLevel.Size = New System.Drawing.Size(128, 21)
        Me.cboLogLevel.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(378, 346)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 32)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(540, 346)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(459, 346)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 32)
        Me.btnApply.TabIndex = 2
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 390)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.tcSettings)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.tcSettings.ResumeLayout(False)
        Me.Tab1.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.Tab2.ResumeLayout(False)
        Me.Tab2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Tab3.ResumeLayout(False)
        Me.Tab3.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.numServerPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tab4.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.numAutoCloseMinutes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.numMuteLevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTimedMuteSeconds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tcSettings As TabControl
    Friend WithEvents Tab1 As TabPage
    Friend WithEvents Tab2 As TabPage
    Friend WithEvents chkShowTips As CheckBox
    Friend WithEvents chkShowSplash As CheckBox
    Friend WithEvents chkAutoPlay As CheckBox
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents chkCheckUpdates As CheckBox
    Friend WithEvents chkSaveHistory As CheckBox
    Friend WithEvents chkExtractTitle As CheckBox
    Friend WithEvents chkMultimediaKeys As CheckBox
    Friend WithEvents cboModifier As ComboBox
    Friend WithEvents Tab3 As TabPage
    Friend WithEvents chkEnableServer As CheckBox
    Friend WithEvents Tab4 As TabPage
    Friend WithEvents lblServerStatus As Label
    Friend WithEvents numServerPort As NumericUpDown
    Friend WithEvents numMuteLevel As NumericUpDown
    Friend WithEvents numTimedMuteSeconds As NumericUpDown
    Friend WithEvents numAutoCloseMinutes As NumericUpDown
    Friend WithEvents cboLogLevel As ComboBox
    Friend WithEvents chkAutoClose As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblInactivityMinutes As Label
    Friend WithEvents lblLogLevel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblMuteSeconds As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents lblNote As Label
    Friend WithEvents btnRefreshIPs As Button
    Friend WithEvents lblBindAddress As Label
    Friend WithEvents cboBindAddress As ComboBox
    Friend WithEvents btnSetupNetwork As Button
    Friend WithEvents btnSetupFirewall As Button
    Friend WithEvents btnTestFirewall As Button
    Friend WithEvents btnTroubleshooting As Button
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents lblDiscogsToken As Label
    Friend WithEvents txtDiscogsToken As TextBox
    Friend WithEvents lnkGetToken As LinkLabel
    Friend WithEvents LinkLabel2 As LinkLabel
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents btnValidate As Button
    Friend WithEvents lblNote2 As Label
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents lnkGetMusixMatchToken As LinkLabel
    Friend WithEvents txtMusixMatchToken As TextBox
    Friend WithEvents lblMusixMatch As Label
End Class

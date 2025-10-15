<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tmrTips = New System.Windows.Forms.Timer(Me.components)
        Me.tmrAutoClose = New System.Windows.Forms.Timer(Me.components)
        Me.tmrMute = New System.Windows.Forms.Timer(Me.components)
        Me.cmsMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmiStationInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiPlay = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiStop = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenLink = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenPlaylist = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOpenYouTube = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiFavorites = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiLevel = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.niTray = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.tmrFlashIcon = New System.Windows.Forms.Timer(Me.components)
        Me.tsmiShowLyrics = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tmrAutoClose
        '
        '
        'cmsMain
        '
        Me.cmsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiStationInfo, Me.tsmiSeparator1, Me.tsmiPlay, Me.tsmiStop, Me.tsmiSeparator2, Me.tsmiOpen, Me.tsmiSeparator3, Me.tsmiFavorites, Me.tsmiSettings, Me.tsmiLevel, Me.tsmiShowLyrics, Me.tsmiSeparator4, Me.tsmiAbout, Me.tsmiExit})
        Me.cmsMain.Name = "cmsMain"
        Me.cmsMain.Size = New System.Drawing.Size(181, 270)
        '
        'tsmiStationInfo
        '
        Me.tsmiStationInfo.Name = "tsmiStationInfo"
        Me.tsmiStationInfo.Size = New System.Drawing.Size(180, 22)
        Me.tsmiStationInfo.Text = "Station name"
        '
        'tsmiSeparator1
        '
        Me.tsmiSeparator1.Name = "tsmiSeparator1"
        Me.tsmiSeparator1.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiPlay
        '
        Me.tsmiPlay.Name = "tsmiPlay"
        Me.tsmiPlay.Size = New System.Drawing.Size(180, 22)
        Me.tsmiPlay.Text = "Play"
        '
        'tsmiStop
        '
        Me.tsmiStop.Name = "tsmiStop"
        Me.tsmiStop.Size = New System.Drawing.Size(180, 22)
        Me.tsmiStop.Text = "Stop"
        '
        'tsmiSeparator2
        '
        Me.tsmiSeparator2.Name = "tsmiSeparator2"
        Me.tsmiSeparator2.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiOpen
        '
        Me.tsmiOpen.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOpenLink, Me.tsmiOpenPlaylist, Me.tsmiOpenYouTube})
        Me.tsmiOpen.Name = "tsmiOpen"
        Me.tsmiOpen.Size = New System.Drawing.Size(180, 22)
        Me.tsmiOpen.Text = "Open"
        '
        'tsmiOpenLink
        '
        Me.tsmiOpenLink.Name = "tsmiOpenLink"
        Me.tsmiOpenLink.Size = New System.Drawing.Size(162, 22)
        Me.tsmiOpenLink.Text = "Open Link..."
        '
        'tsmiOpenPlaylist
        '
        Me.tsmiOpenPlaylist.Name = "tsmiOpenPlaylist"
        Me.tsmiOpenPlaylist.Size = New System.Drawing.Size(162, 22)
        Me.tsmiOpenPlaylist.Text = "Open Playlist..."
        '
        'tsmiOpenYouTube
        '
        Me.tsmiOpenYouTube.Name = "tsmiOpenYouTube"
        Me.tsmiOpenYouTube.Size = New System.Drawing.Size(162, 22)
        Me.tsmiOpenYouTube.Text = "Open YouTube..."
        '
        'tsmiSeparator3
        '
        Me.tsmiSeparator3.Name = "tsmiSeparator3"
        Me.tsmiSeparator3.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiFavorites
        '
        Me.tsmiFavorites.Name = "tsmiFavorites"
        Me.tsmiFavorites.Size = New System.Drawing.Size(180, 22)
        Me.tsmiFavorites.Text = "Favorites"
        '
        'tsmiSettings
        '
        Me.tsmiSettings.Name = "tsmiSettings"
        Me.tsmiSettings.Size = New System.Drawing.Size(180, 22)
        Me.tsmiSettings.Text = "Settings..."
        '
        'tsmiLevel
        '
        Me.tsmiLevel.Name = "tsmiLevel"
        Me.tsmiLevel.Size = New System.Drawing.Size(180, 22)
        Me.tsmiLevel.Text = "Level..."
        '
        'tsmiSeparator4
        '
        Me.tsmiSeparator4.Name = "tsmiSeparator4"
        Me.tsmiSeparator4.Size = New System.Drawing.Size(177, 6)
        '
        'tsmiAbout
        '
        Me.tsmiAbout.Name = "tsmiAbout"
        Me.tsmiAbout.Size = New System.Drawing.Size(180, 22)
        Me.tsmiAbout.Text = "About..."
        '
        'tsmiExit
        '
        Me.tsmiExit.Name = "tsmiExit"
        Me.tsmiExit.Size = New System.Drawing.Size(180, 22)
        Me.tsmiExit.Text = "Exit"
        '
        'niTray
        '
        Me.niTray.ContextMenuStrip = Me.cmsMain
        Me.niTray.Text = "PlayTray"
        Me.niTray.Visible = True
        '
        'tmrFlashIcon
        '
        '
        'tsmiShowLyrics
        '
        Me.tsmiShowLyrics.Name = "tsmiShowLyrics"
        Me.tsmiShowLyrics.Size = New System.Drawing.Size(180, 22)
        Me.tsmiShowLyrics.Text = "🎤 Show Lyrics..."
        Me.tsmiShowLyrics.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Maroon
        Me.ClientSize = New System.Drawing.Size(286, 151)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.ShowInTaskbar = False
        Me.Text = "PlayTray"
        Me.cmsMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tmrTips As Timer
    Friend WithEvents tmrAutoClose As Timer
    Friend WithEvents tmrMute As Timer
    Friend WithEvents cmsMain As ContextMenuStrip
    Friend WithEvents niTray As NotifyIcon
    Friend WithEvents tsmiStationInfo As ToolStripMenuItem
    Friend WithEvents tsmiSeparator1 As ToolStripSeparator
    Friend WithEvents tsmiPlay As ToolStripMenuItem
    Friend WithEvents tsmiStop As ToolStripMenuItem
    Friend WithEvents tsmiSeparator2 As ToolStripSeparator
    Friend WithEvents tsmiOpen As ToolStripMenuItem
    Friend WithEvents tsmiOpenLink As ToolStripMenuItem
    Friend WithEvents tsmiOpenPlaylist As ToolStripMenuItem
    Friend WithEvents tsmiOpenYouTube As ToolStripMenuItem
    Friend WithEvents tsmiSeparator3 As ToolStripSeparator
    Friend WithEvents tsmiSettings As ToolStripMenuItem
    Friend WithEvents tsmiLevel As ToolStripMenuItem
    Friend WithEvents tsmiSeparator4 As ToolStripSeparator
    Friend WithEvents tsmiAbout As ToolStripMenuItem
    Friend WithEvents tsmiExit As ToolStripMenuItem
    Friend WithEvents tsmiFavorites As ToolStripMenuItem
    Friend WithEvents tmrFlashIcon As Timer
    Friend WithEvents tsmiShowLyrics As ToolStripMenuItem
End Class

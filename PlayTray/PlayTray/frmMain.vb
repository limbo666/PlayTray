Imports Un4seen.Bass
Imports System.Runtime.InteropServices
Imports System.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports System.Timers
Public Class frmMain
    ' For rounded corners on forms if needed later


    Private metadataSyncProc As SYNCPROC

    ' Windows message constant for hotkeys
    Private Const WM_HOTKEY As Integer = &H312


    ' Tips window
    Private tipsForm As frmTips

    ' Stats window
    Private statsForm As frmStats

    ' Volume form
    Private levelForm As frmLevel

    ' Flag to prevent registry update during shutdown
    Private isShuttingDown As Boolean = False



    ' Timed mute tracking
    Private isMuted As Boolean = False
    Private volumeBeforeMute As Integer = 0
    Private WithEvents tmrMuteRestore As New Timer()


    'Openplaylistdirectly
    Private playListpath As String



    ' --- Soft Timer Section ---
    Private flashTimer As System.Timers.Timer
    Private seqCount As Integer = 0



    Public Sub New()
        InitializeComponent()

        ' Hide form before it shows
        Me.Visible = False
        Me.ShowInTaskbar = False
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Hide()
    End Sub




    ' ============================================
    ' TIMED MUTE (Phase 15)
    ' ============================================

    Private Sub InitializeMuteTimer()
        ' Timer is created in variable declaration with WithEvents
        tmrMuteRestore.Interval = 1000 ' Will be set dynamically
        tmrMuteRestore.Enabled = False
    End Sub

    Private Sub ToggleTimedMute()
        Try
            If isMuted Then
                ' Already muted, restore immediately
                RestoreVolume()
            Else
                ' Start mute
                StartTimedMute()
            End If

        Catch ex As Exception
            MessageBox.Show("Error toggling mute: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub StartTimedMute()
        Try
            If g_SettingsManager Is Nothing Then Return

            ' Store current volume
            volumeBeforeMute = g_CurrentVolume

            ' Get mute level and duration from settings
            Dim muteLevel As Integer = g_SettingsManager.MuteLevel
            Dim muteDuration As Integer = g_SettingsManager.TimedMuteSeconds * 1000 ' Convert to milliseconds

            ' Apply mute level
            g_CurrentVolume = muteLevel
            If g_StreamHandle <> 0 Then
                Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
            End If

            ' Set muted flag
            isMuted = True

            ' Start timer
            tmrMuteRestore.Interval = muteDuration
            tmrMuteRestore.Enabled = True

            ' Show notification
            Dim message As String = $"Volume muted to {muteLevel}% for {g_SettingsManager.TimedMuteSeconds} seconds"
            ShowTipsWindow("Timed Mute", message, "Muted", True)

        Catch ex As Exception
            MessageBox.Show("Error starting mute: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub RestoreVolume()
        Try
            ' Stop timer
            tmrMuteRestore.Enabled = False

            If Not isMuted Then Return

            ' Restore original volume
            g_CurrentVolume = volumeBeforeMute
            If g_StreamHandle <> 0 Then
                Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
            End If

            ' Clear muted flag
            isMuted = False

            ' Show notification
            ShowTipsWindow("Timed Mute", $"Volume restored to {g_CurrentVolume}%", "Unmuted", True)

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub tmrMuteRestore_Tick(sender As Object, e As EventArgs) Handles tmrMuteRestore.Elapsed
        ' Timer expired, restore volume
        RestoreVolume()
    End Sub

    ' Cancel mute if user manually changes volume
    Private Sub CancelMuteIfActive()
        If isMuted Then
            tmrMuteRestore.Enabled = False
            isMuted = False
            ' Don't restore volume since user is changing it manually
        End If
    End Sub



    ' ============================================
    ' HTTP SERVER PUBLIC METHODS (Phase 17)
    ' ============================================

    Public Sub TriggerPlay()
        If Not g_IsPlaying Then
            ResumePlayback()
        End If
    End Sub

    Public Sub TriggerStop()
        If g_IsPlaying Then
            StopPlayback()
        End If
    End Sub

    Public Sub ApplyVolume(volume As Integer)
        g_CurrentVolume = volume

        If g_StreamHandle <> 0 Then
            Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
        End If

        If g_SettingsManager IsNot Nothing Then
            g_SettingsManager.LastVolume = g_CurrentVolume
        End If
    End Sub

    Public Sub PlayStationByPosition(position As Integer)
        If g_StationManager IsNot Nothing Then
            Dim station = g_StationManager.GetStation(position)
            If station IsNot Nothing Then
                PlayStationFromObject(station)
            End If
        End If
    End Sub

    Public Sub TriggerSaveBeloved()
        SaveBelovedTrack()
    End Sub


    ' ============================================
    ' SERVER ELEVATION HELPERS
    ' ============================================

    Private Function IsRunningAsAdmin() As Boolean
        Try
            Dim identity = System.Security.Principal.WindowsIdentity.GetCurrent()
            Dim principal = New System.Security.Principal.WindowsPrincipal(identity)
            Return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
        Catch
            Return False
        End Try
    End Function

    Private Function NeedsElevationForServer() As Boolean
        ' Check if we need elevation
        If g_SettingsManager Is Nothing OrElse Not g_SettingsManager.EnableServer Then
            Return False
        End If

        ' Already admin
        If IsRunningAsAdmin() Then
            Return False
        End If

        ' Check if binding to network (not just localhost)
        Dim bindAddress As String = g_SettingsManager.ServerBindAddress
        If String.IsNullOrWhiteSpace(bindAddress) OrElse
       bindAddress = "localhost" OrElse
       bindAddress = "127.0.0.1" Then
            Return False ' Localhost doesn't need elevation
        End If

        ' Check if URL is reserved
        If IsUrlReserved(bindAddress, g_SettingsManager.ServerPort) Then
            Return False ' URL is reserved, no elevation needed
        End If

        ' Need elevation
        Return True
    End Function

    Private Function IsUrlReserved(bindAddress As String, port As Integer) As Boolean
        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = "netsh"
            psi.Arguments = "http show urlacl"
            psi.RedirectStandardOutput = True
            psi.UseShellExecute = False
            psi.CreateNoWindow = True

            Dim process As Process = Process.Start(psi)
            Dim output As String = process.StandardOutput.ReadToEnd()
            process.WaitForExit()

            ' Check if our URL is in the output
            Dim urlPattern As String = $"http://{bindAddress}:{port}/"
            If output.Contains(urlPattern) Then
                Return True
            End If

            ' Also check wildcard
            If output.Contains($"http://*:{port}/") OrElse output.Contains($"http://+:{port}/") Then
                Return True
            End If

            Return False

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub RestartAsAdmin()
        Try
            Dim psi As New ProcessStartInfo()
            psi.FileName = Application.ExecutablePath
            psi.Verb = "runas" ' Request elevation
            psi.UseShellExecute = True

            ' Start elevated instance
            Process.Start(psi)

            ' Exit this instance
            Application.Exit()

        Catch ex As Exception
            ' User cancelled UAC or error
            MessageBox.Show("Could not restart with Administrator rights." & vbCrLf & vbCrLf &
                       "The server will be disabled.",
                       "Elevation Cancelled",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information)
        End Try
    End Sub


    ' ============================================
    ' VOLUME CONTROL (Phase 10)
    ' ============================================

    Private Sub ShowLevelForm()
        Try
            ' Close existing form if open
            If levelForm IsNot Nothing AndAlso Not levelForm.IsDisposed Then
                levelForm.Close()
            End If

            ' Create new form
            levelForm = New frmLevel()
            levelForm.Show()

        Catch ex As Exception
            MessageBox.Show("Error showing volume control: " & ex.Message, "Error")
        End Try
    End Sub


    Public Sub TriggerTimedMute()
        ToggleTimedMute()
    End Sub

    Public Sub TriggerUnmute()
        If isMuted Then
            RestoreVolume()
        End If
    End Sub

    ' ============================================
    ' TIPS WINDOW (Phase 11)
    ' ============================================

    Private Sub ShowTipsWindow(stationName As String, trackInfo As String, status As String, Optional forceShow As Boolean = False)
        Try
            ' Check settings unless forced (from hotkey)
            If Not forceShow Then
                If g_SettingsManager IsNot Nothing AndAlso Not g_SettingsManager.ShowTips Then
                    Return
                End If
            End If

            ' Create tips form if needed
            If tipsForm Is Nothing OrElse tipsForm.IsDisposed Then
                tipsForm = New frmTips()
            End If

            '
            ' with album art (if available)
            tipsForm.ShowTips(stationName, trackInfo, status, currentAlbumArtPath)  ' CHANGED: Added currentAlbumArtPath

        Catch ex As Exception
            'MsgBox("Error showing tips: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Public Sub ShowTipsFromHotkey()
        ' Called from hotkey - ALWAYS show regardless of settings
        Try
            If g_CurrentStation IsNot Nothing Then
                Dim trackInfo As String = tsmiStationInfo.Text.Replace("🎵 ", "").Replace(g_CurrentStation.Name & " - ", "")
                Dim status As String = If(g_IsPlaying, "Playing", "Stopped")
                ShowTipsWindow(g_CurrentStation.Name, trackInfo, status, True)  ' forceShow = True
            Else
                ShowTipsWindow("Play Tray", "No station playing", "Stopped", True)  ' forceShow = True
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub





    ' ============================================
    ' STATION MANAGEMENT (Phase 4)
    ' ============================================

    Private Sub LoadStations()
        Try
            g_StationManager = New StationManager(StationsFile)

            If g_StationManager.LoadStations() Then
                ' Populate favorites menu
                PopulateFavoritesMenu()

                ' Get station count
                Dim stationCount As Integer = g_StationManager.Stations.Count

                ' Debug message
                '    MessageBox.Show("Loaded " & stationCount.ToString() & " stations", "Debug")

                If stationCount > 0 Then
                    tsmiPlay.Enabled = True
                    tsmiFavorites.Enabled = True
                Else
                    tsmiFavorites.Enabled = False
                End If
            Else
                MessageBox.Show("Failed to load stations.", "Warning")
                tsmiFavorites.Enabled = False
            End If

        Catch ex As Exception
            MessageBox.Show("Error initializing stations: " & ex.Message, "Error")
            tsmiFavorites.Enabled = False
        End Try
    End Sub



    Private Sub PopulateFavoritesMenu()
        Try
            Dim favMenuItem = DirectCast(tsmiFavorites, ToolStripMenuItem)
            favMenuItem.DropDownItems.Clear()

            Dim stationsList As List(Of Station) = g_StationManager.Stations
            Dim stationCount As Integer = stationsList.Count

            ' Get the configured hotkey modifier string directly from settings
            Dim modifierString As String = GetModifierDisplayString()
            Dim showHotkeys As Boolean = (g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.HotkeyModifier > 0)


            For i As Integer = 0 To stationCount - 1

                Try
                    Dim station As Station = stationsList.Item(i)

                    Dim menuItem As New ToolStripMenuItem()
                    Dim keyNum As Integer = If(station.Position = 10, 0, station.Position)

                    ' Show hotkey only if not disabled
                    If showHotkeys Then
                        menuItem.Text = "📻 " & station.Name & " (" & modifierString & "+" & keyNum.ToString() & ")"
                    Else
                        menuItem.Text = "📻 " & station.Name
                    End If
                    Select Case i
                        Case 0
                            menuItem.Image = My.Resources.mnu1
                        Case 1
                            menuItem.Image = My.Resources.mnu2
                        Case 2
                            menuItem.Image = My.Resources.mnu3
                        Case 3
                            menuItem.Image = My.Resources.mnu4
                        Case 4
                            menuItem.Image = My.Resources.mnu5
                        Case 5
                            menuItem.Image = My.Resources.mnu6

                        Case 6
                            menuItem.Image = My.Resources.mnu7
                        Case 7
                            menuItem.Image = My.Resources.mnu8
                        Case 8
                            menuItem.Image = My.Resources.mnu9

                        Case 9
                            menuItem.Image = My.Resources.mnu10




                    End Select
                    menuItem.Tag = station

                    AddHandler menuItem.Click, AddressOf FavoriteStation_Click

                    favMenuItem.DropDownItems.Add(menuItem)
                Catch ex As Exception

                End Try

            Next



            If stationCount = 0 Then
                Dim emptyItem As New ToolStripMenuItem("No stations configured")
                emptyItem.Enabled = False
                favMenuItem.DropDownItems.Add(emptyItem)
            End If

            Dim menuItemSteady As New ToolStripMenuItem()
            menuItemSteady.Text = "Edit favorites"
            menuItemSteady.Image = My.Resources.mnueditfavorites
            favMenuItem.DropDownItems.Add(menuItemSteady)
            AddHandler menuItemSteady.Click, AddressOf EditFavoritesMenu

        Catch ex As Exception
            MessageBox.Show("Error populating favorites: " & ex.Message & vbCrLf & ex.StackTrace, "Error")
        End Try
    End Sub

    Private Function GetModifierDisplayString() As String
        If g_SettingsManager Is Nothing Then
            Return "Ctrl+Shift"
        End If

        Select Case g_SettingsManager.HotkeyModifier
            Case 0
                Return "None"
            Case 1
                Return "Ctrl+Shift"
            Case 2
                Return "Ctrl+Alt"
            Case 3
                Return "Ctrl+Alt+Shift"
            Case Else
                Return "Ctrl+Shift"
        End Select
    End Function
    Private Sub FavoriteStation_Click(sender As Object, e As EventArgs)
        Try
            ResetStopTime() ' Reset timer
            Dim menuItem = TryCast(sender, ToolStripMenuItem)
            If menuItem IsNot Nothing Then
                Dim station = TryCast(menuItem.Tag, Station)
                If station IsNot Nothing Then
                    PlayStationFromObject(station)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error playing favorite: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub EditFavoritesMenu(sender As Object, e As EventArgs)
        frmEditJson.Show()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        If NeedsAdminElevation() AndAlso Not IsRunningAsAdmin() Then
            Try
                ' Restart as admin silently
                Dim psi As New ProcessStartInfo()
                psi.FileName = Application.ExecutablePath
                psi.Verb = "runas"
                psi.UseShellExecute = True

                Process.Start(psi)
                Application.Exit()
                Return

            Catch ex As Exception
                ' User cancelled UAC, disable server and continue
                DisableServerAndContinue()
            End Try
        End If

        ' Create software timer (300 ms interval)
        flashTimer = New System.Timers.Timer(300)
        AddHandler flashTimer.Elapsed, AddressOf FlashIcon_Elapsed

        ' Hide the form
        Me.Visible = False
        Me.WindowState = FormWindowState.Minimized
        Me.ShowInTaskbar = False
        Me.Hide()

        ' Ensure Playback folder structure exists
        EnsurePlaybackFoldersExist()

        ' Initialize BASS
        If Not InitializeBASS() Then
            MessageBox.Show("Failed to initialize BASS audio library." & vbCrLf &
                          "Error: " & Bass.BASS_ErrorGetCode().ToString(),
                          "Initialization Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
            Application.Exit()
            Return
        End If


        ' Set up tray icon
        niTray.Visible = True
        niTray.Icon = My.Resources.stop_icon ' You'll need to add icons to resources
        niTray.Text = APP_NAME & " - Stopped"

        ' Set up context menu
        BuildContextMenu()

        ' Initialize metadata callback
        metadataSyncProc = New SYNCPROC(AddressOf MetadataSync)

        ' Load settings
        LoadSettings()
        ' Load stations
        LoadStations()

        ' Initialize track history
        InitializeTrackHistory()

        ' Apply settings
        ApplySettings(True)  ' isStartup = True
        ' Register hotkeys
        RegisterHotkeys()

        UpdateTrayIcon()

        ' Initialize auto-close timer
        InitializeAutoCloseTimer()


        ' Start HTTP server if enabled
        StartHTTPServer()
        If g_SettingsManager.ShowSplash = True Then
            frmSplash.Show()
        End If

    End Sub


    Private Sub FlashIcon_Elapsed(sender As Object, e As ElapsedEventArgs)
        If seqCount < 6 Then
            seqCount += 1
            ' Update tray icon safely on the UI thread
            Me.Invoke(Sub()
                          niTray.Icon = If(seqCount Mod 2 = 0, My.Resources.Seq5, My.Resources.Seq6)
                      End Sub)
        Else
            seqCount = 0
            flashTimer.Stop()
            Me.Invoke(Sub() UpdateTrayIcon())
        End If
    End Sub




    ' ============================================
    ' STARTUP ELEVATION CHECK
    ' ============================================

    Private Function NeedsAdminElevation() As Boolean
        Try
            ' Load settings first to check if server is enabled
            If g_SettingsManager Is Nothing Then
                Dim settingsPath As String = IO.Path.Combine(AppPath, "PlayTray_settings.ini")
                g_SettingsManager = New SettingsManager(settingsPath)
                g_SettingsManager.LoadSettings()
            End If

            ' Only need elevation if server is enabled
            Return g_SettingsManager.EnableServer

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub DisableServerAndContinue()
        Try
            If g_SettingsManager IsNot Nothing Then
                ' Disable server for this session
                g_SettingsManager.EnableServer = False

                MessageBox.Show(
                "The HTTP server has been disabled for this session." & vbCrLf & vbCrLf &
                "PlayTray will continue without server functionality." & vbCrLf &
                "You can enable it from Settings → Network.",
                "Server Disabled",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Function RestartAsAdminWithConfirmation() As Boolean
        Try
            Dim result = MessageBox.Show(
            "PlayTray HTTP Server requires Administrator privileges to start." & vbCrLf & vbCrLf &
            "Would you like to restart PlayTray as Administrator?" & vbCrLf & vbCrLf &
            "Click YES to restart with admin rights (recommended)" & vbCrLf &
            "Click NO to continue without the server",
            "Administrator Rights Required",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1)

            If result = DialogResult.Yes Then
                ' User wants to elevate
                Try
                    Dim psi As New ProcessStartInfo()
                    psi.FileName = Application.ExecutablePath
                    psi.Verb = "runas" ' Request elevation
                    psi.UseShellExecute = True

                    ' Start elevated instance
                    Process.Start(psi)

                    ' Return true to signal we should exit this instance
                    Return True

                Catch ex As Exception
                    ' User cancelled UAC or error occurred
                    MessageBox.Show(
                    "Could not restart with Administrator rights." & vbCrLf & vbCrLf &
                    "The HTTP server will be disabled." & vbCrLf &
                    "You can enable it later from Settings → Network.",
                    "Elevation Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)

                    Return False
                End Try
            Else
                ' User declined elevation
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Sub TriggerNextStation()
        PlayNextFavorite()
    End Sub

    Public Sub TriggerPreviousStation()
        PlayPreviousFavorite()
    End Sub
    Private Sub StartHTTPServer()
        Try
            If g_SettingsManager Is Nothing OrElse Not g_SettingsManager.EnableServer Then
                Return
            End If

            Dim port As Integer = g_SettingsManager.ServerPort
            Dim bindAddress As String = g_SettingsManager.ServerBindAddress

            ' Validate and clean bind address
            bindAddress = ValidateBindAddress(bindAddress)

            g_HTTPServer = New HTTPServer(port)

            Try
                If g_HTTPServer.StartServer(bindAddress) Then
                    ' Success!

                    ' Start broadcast service
                    StartBroadcastService()

                    ' Show bound addresses (optional, for debugging)
                    ' Dim boundAddresses = g_HTTPServer.GetBoundAddresses()
                    ' Console.WriteLine("Server bound to: " & String.Join(", ", boundAddresses))
                End If

            Catch ex As HttpListenerException
                ' Access denied error
                If ex.ErrorCode = 5 OrElse ex.Message.Contains("Access is denied") Then
                    HandleServerAccessDenied(bindAddress, port)
                Else
                    ' Other error - show details
                    MessageBox.Show($"Failed to start HTTP server: {ex.Message}" & vbCrLf & vbCrLf &
                              $"Attempted to bind to: {bindAddress}:{port}" & vbCrLf & vbCrLf &
                              "The server will be disabled. Check Settings → Network.",
                              "Server Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error)
                End If

            Catch ex As Exception
                ' General error
                MessageBox.Show($"Failed to start HTTP server: {ex.Message}" & vbCrLf & vbCrLf &
                          $"Attempted to bind to: {bindAddress}:{port}",
                          "Server Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error)
            End Try

        Catch ex As Exception
            MessageBox.Show("Error starting HTTP server: " & ex.Message, "Error")
        End Try
    End Sub

    Private Function ValidateBindAddress(bindAddress As String) As String
        Try
            ' Clean up the address
            If String.IsNullOrWhiteSpace(bindAddress) Then
                bindAddress = GetFirstNetworkIP()
            End If

            ' If still empty or localhost, use localhost
            If String.IsNullOrWhiteSpace(bindAddress) OrElse
           bindAddress = "localhost" OrElse
           bindAddress = "127.0.0.1" Then
                Return "localhost"
            End If

            ' If wildcard, keep it
            If bindAddress = "*" Then
                Return "*"
            End If

            ' Try to parse as IP address to validate format
            Dim testIP As IPAddress = Nothing
            If IPAddress.TryParse(bindAddress, testIP) Then
                ' Valid IP format
                If testIP.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                    ' IPv4 - good
                    Return bindAddress
                Else
                    ' IPv6 - not supported, fall back
                    Return GetFirstNetworkIP()
                End If
            Else
                ' Invalid format, get first available
                Return GetFirstNetworkIP()
            End If

        Catch ex As Exception
            ' On any error, fall back to localhost
            Return "localhost"
        End Try
    End Function

    Private Function GetFirstNetworkIP() As String
        Try
            Dim hostName As String = System.Net.Dns.GetHostName()
            Dim addresses = System.Net.Dns.GetHostAddresses(hostName)

            ' Find first IPv4 non-loopback address
            For Each addr In addresses
                If addr.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork AndAlso
               Not addr.ToString().StartsWith("127.") Then
                    Return addr.ToString()
                End If
            Next

            ' No network IP found, use localhost
            Return "localhost"

        Catch ex As Exception
            Return "localhost"
        End Try
    End Function
    Private Sub HandleServerAccessDenied(bindAddress As String, port As Integer)
        ' Show user-friendly dialog with options
        Dim result = MessageBox.Show(
        "HTTP Server requires elevated permissions to bind to network interfaces." & vbCrLf & vbCrLf &
        "You have three options:" & vbCrLf & vbCrLf &
        "1. Click YES to restart PlayTray as Administrator (recommended)" & vbCrLf &
        "2. Click NO to setup URL reservation (one-time, no admin needed after)" & vbCrLf &
        "3. Click CANCEL to continue without the server" & vbCrLf & vbCrLf &
        "What would you like to do?",
        "Server Permissions Required",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1)

        Select Case result
            Case DialogResult.Yes
                ' Restart as admin
                RestartAsAdmin()

            Case DialogResult.No
                ' Setup URL reservation
                SetupUrlReservationFromMain(bindAddress, port)

            Case DialogResult.Cancel
                ' Continue without server
                MessageBox.Show(
                "The HTTP server will be disabled." & vbCrLf & vbCrLf &
                "You can enable it later from Settings → Network → Setup Network Access",
                "Server Disabled",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        End Select
    End Sub

    Private Sub SetupUrlReservationFromMain(bindAddress As String, port As Integer)
        Try
            Dim url As String = If(bindAddress = "*", $"http://*:{port}/", $"http://{bindAddress}:{port}/")
            Dim arguments As String = $"http add urlacl url={url} user=Everyone"

            ' Create elevated process
            Dim psi As New ProcessStartInfo()
            psi.FileName = "netsh"
            psi.Arguments = arguments
            psi.Verb = "runas" ' Request elevation
            psi.UseShellExecute = True
            psi.CreateNoWindow = False

            Dim process As Process = Process.Start(psi)
            process.WaitForExit()

            If process.ExitCode = 0 Then
                MessageBox.Show(
                "Network access configured successfully!" & vbCrLf & vbCrLf &
                "Please restart PlayTray to start the server.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

                ' Ask if user wants to restart now
                Dim restartResult = MessageBox.Show(
                "Would you like to restart PlayTray now?",
                "Restart?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If restartResult = DialogResult.Yes Then
                    ' Restart without elevation (URL is now reserved)
                    Application.Restart()
                    Application.Exit()
                End If
            Else
                MessageBox.Show(
                "Setup failed. You can try again from Settings → Network → Setup Network Access",
                "Setup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show(
            "Setup cancelled or failed. The server will be disabled.",
            "Setup Cancelled",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub StopHTTPServer()
        Try
            ' Stop broadcast first
            StopBroadcastService()

            If g_HTTPServer IsNot Nothing Then
                g_HTTPServer.StopServer()
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    ' ============================================
    ' AUTO CLOSE TIMER (Phase 16)
    ' ============================================

    Private Sub InitializeAutoCloseTimer()
        Try
            ' Configure timer
            tmrAutoClose.Interval = 60000 ' Check every 1 minute

            ' Start timer if enabled
            If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.AutoCloseEnabled Then
                tmrAutoClose.Enabled = True
            Else
                tmrAutoClose.Enabled = False
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub tmrAutoClose_Tick(sender As Object, e As EventArgs) Handles tmrAutoClose.Tick
        Try
            ' Only count down when stopped
            If Not g_IsPlaying Then
                CheckAutoClose()
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub CheckAutoClose()
        Try
            If g_SettingsManager Is Nothing OrElse Not g_SettingsManager.AutoCloseEnabled Then
                Return
            End If

            ' Get inactivity time in minutes
            Dim inactivityMinutes As Integer = g_SettingsManager.InactivityMinutes

            ' Check how long we've been stopped
            Dim minutesStopped As Integer = GetMinutesSinceStopped()

            If minutesStopped >= inactivityMinutes Then
                ' Show warning before closing
                'Dim result = MessageBox.Show(
                '$"PlayTray has been inactive for {inactivityMinutes} minutes." & vbCrLf & vbCrLf &
                '"The application will close automatically." & vbCrLf & vbCrLf &
                '"Click OK to close now, or Cancel to keep running.",
                '"Auto Close",
                'MessageBoxButtons.OKCancel,
                'MessageBoxIcon.Information)

                ' If result = DialogResult.OK Then
                ' User confirmed, close app



                ' ToDo add splashscreen on exit
                ' frmSplash.show()
                CleanupAndExit()
                '    Else
                ' User cancelled, reset timer
                '   ResetStopTime()
                'End If
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private lastStopTime As DateTime = DateTime.MinValue

    ' Album art tracking
    Private currentAlbumArtPath As String = ""

    Private Sub ResetStopTime()
        lastStopTime = DateTime.Now
    End Sub

    Private Function GetMinutesSinceStopped() As Integer
        If lastStopTime = DateTime.MinValue Then
            ' First time or never stopped, reset
            lastStopTime = DateTime.Now
            Return 0
        End If

        Dim elapsed As TimeSpan = DateTime.Now - lastStopTime
        Return CInt(elapsed.TotalMinutes)
    End Function


    ' ============================================
    ' TRACK HISTORY (Phase 18)
    ' ============================================

    Private Sub InitializeTrackHistory()
        Try
            ' Ensure Playback folders exist
            EnsurePlaybackFoldersExist()

            ' Use paths from modGlobals
            g_TrackHistory = New TrackHistory(TrackHistoryFile, BelovedFile)

        Catch ex As Exception
            MessageBox.Show("Error initializing track history: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SaveBelovedTrack()
        Try
            If g_TrackHistory Is Nothing Then Return

            Dim stationName As String = If(g_CurrentStation IsNot Nothing, g_CurrentStation.Name, "Unknown")
            Dim result As BelovedSaveResult = g_TrackHistory.SaveBeloved(stationName, g_CurrentTrackInfo)

            ' Show result via tips window (like timed mute)
            If result.Success Then
                ShowTipsWindow("Beloved Track", result.Message, "✓ Saved", True)
            Else
                ShowTipsWindow("Beloved Track", result.Message, "Not Saved", True)
            End If

        Catch ex As Exception
            ShowTipsWindow("Beloved Track", "Error saving track", "Failed", True)
        End Try
    End Sub


    ' ============================================
    ' SETTINGS MANAGEMENT (Phase 5)
    ' ============================================

    Private Sub LoadSettings()
        Try
            g_SettingsManager = New SettingsManager(SettingsFile)
            g_SettingsManager.LoadSettings()

        Catch ex As Exception
            MessageBox.Show("Error loading settings: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub ApplySettings(Optional isStartup As Boolean = False)
        Try
            If g_SettingsManager Is Nothing Then Return

            ' Apply volume
            g_CurrentVolume = g_SettingsManager.LastVolume

            ' If currently playing, apply volume to stream
            If g_StreamHandle <> 0 Then
                Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
            End If
            ' Apply auto-close timer setting
            If g_SettingsManager.AutoCloseEnabled Then
                tmrAutoClose.Enabled = True
                ResetStopTime()
            Else
                tmrAutoClose.Enabled = False
            End If



            ' Apply auto-play ONLY on startup
            If isStartup AndAlso g_SettingsManager.AutoPlay Then
                ' Try to play last station
                If Not String.IsNullOrEmpty(g_SettingsManager.LastStation) Then
                    Dim station = g_StationManager.GetStationByName(g_SettingsManager.LastStation)
                    If station IsNot Nothing Then
                        PlayStationFromObject(station)
                    Else
                        ' Station not found, play first available
                        PlayTestStream()
                    End If
                Else
                    ' No last station, play first
                    PlayTestStream()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error applying settings: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SaveCurrentSettings()
        Try
            If g_SettingsManager Is Nothing Then Return

            ' Update current values
            g_SettingsManager.LastVolume = g_CurrentVolume

            If g_CurrentStation IsNot Nothing Then
                g_SettingsManager.LastStation = g_CurrentStation.Name
            End If

            ' Save to file
            g_SettingsManager.SaveSettings()

        Catch ex As Exception
            ' Silent fail on settings save
        End Try
    End Sub
    Private Function InitializeBASS() As Boolean
        Try
            ' Initialize BASS
            If Not Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Me.Handle) Then
                Return False
            End If

            ' Load plugins
            Dim aacPlugin = Bass.BASS_PluginLoad(IO.Path.Combine(AppPath, "bass_aac.dll"))
            Dim flacPlugin = Bass.BASS_PluginLoad(IO.Path.Combine(AppPath, "bassflac.dll"))

            ' Set default volume
            g_CurrentVolume = 75

            Return True
        Catch ex As Exception
            MessageBox.Show("BASS initialization error: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub BuildContextMenu()
        ' Top item - Station info (will be updated dynamically)
        tsmiStationInfo.Text = "No station selected"
        tsmiStationInfo.Enabled = False

        ' Play/Stop - will be enabled after loading stations
        tsmiPlay.Enabled = False
        tsmiStop.Enabled = False

        ' Favorites - will be enabled after loading stations
        tsmiFavorites.Enabled = True  ' CHANGED: Enable by default

        ' Open submenu items
        tsmiOpenLink.Enabled = True
        tsmiOpenPlaylist.Enabled = True
        tsmiOpenYouTube.Enabled = False ' Future feature
    End Sub

    Private Sub UpdateTrayIcon()
        If g_IsPlaying Then
            niTray.Icon = My.Resources.play_icon
            niTray.Text = APP_NAME & " - Playing"
            tsmiPlay.Enabled = False
            tsmiStop.Enabled = True
        Else
            niTray.Icon = My.Resources.stop_icon
            niTray.Text = APP_NAME & " - Stopped"
            tsmiPlay.Enabled = True
            tsmiStop.Enabled = False
        End If
    End Sub

    Private Sub UpdateStationDisplay()
        If g_CurrentStation IsNot Nothing Then
            Dim status As String = If(g_IsPlaying, "Playing", "Stopped")
            tsmiStationInfo.Text = $"🎵 {g_CurrentStation.Name} - {status}"
        Else
            tsmiStationInfo.Text = "No station selected"
        End If
    End Sub

    ' ============================================
    ' MENU HANDLERS
    ' ============================================

    Private Sub tsmiPlay_Click(sender As Object, e As EventArgs) Handles tsmiPlay.Click
        If Not g_IsPlaying Then
            ResumePlayback()  ' CHANGED from PlayTestStream()
        End If
    End Sub

    Private Sub tsmiStop_Click(sender As Object, e As EventArgs) Handles tsmiStop.Click
        StopPlayback()
    End Sub

    Private Sub tsmiSettings_Click(sender As Object, e As EventArgs) Handles tsmiSettings.Click
        Try
            ' Reset auto-close timer on user interaction
            ResetStopTime()

            Dim settingsForm As New frmSettings()

            If settingsForm.ShowDialog() = DialogResult.OK Then
                ' Settings were saved, apply any immediate changes
                ApplySettings()
            End If

        Catch ex As Exception
            MessageBox.Show("Error opening settings: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub tsmiLevel_Click(sender As Object, e As EventArgs) Handles tsmiLevel.Click
        ShowLevelForm()
    End Sub
    Private Sub tsmiAbout_Click(sender As Object, e As EventArgs) Handles tsmiAbout.Click
        Try
            Dim aboutForm As New frmAbout()
            aboutForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error opening about dialog: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub tsmiExit_Click(sender As Object, e As EventArgs) Handles tsmiExit.Click
        CleanupAndExit()
    End Sub

    Private Sub tsmiOpenLink_Click(sender As Object, e As EventArgs) Handles tsmiOpenLink.Click
        Try
            Dim openLinkForm As New frmOpenLink()
            openLinkForm.ShowDialog()

        Catch ex As Exception
            MessageBox.Show("Error opening link form: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub tsmiOpenPlaylist_Click(sender As Object, e As EventArgs) Handles tsmiOpenPlaylist.Click
        'Try
        '    ResetStopTime() ' Reset timer
        '    Dim openPlaylistForm As New frmOpenPlaylist()
        '    openPlaylistForm.ShowDialog()

        'Catch ex As Exception
        '    MessageBox.Show("Error opening playlist form: " & ex.Message, "Error")
        'End Try

        ' Open directly from menu item
        Try
            Using ofd As New OpenFileDialog()
                ' Set up filters
                ofd.Filter = "Playlist Files|*.pls;*.m3u;*.m3u8|PLS Files (*.pls)|*.pls|M3U Files (*.m3u;*.m3u8)|*.m3u;*.m3u8|All Files (*.*)|*.*"
                ofd.FilterIndex = 1
                ofd.Title = "Open Playlist File"

                ' Set initial directory from settings or last selected
                If Not String.IsNullOrEmpty(playListpath) AndAlso Directory.Exists(Path.GetDirectoryName(playListpath)) Then
                    ofd.InitialDirectory = Path.GetDirectoryName(playListpath)
                ElseIf g_SettingsManager IsNot Nothing AndAlso Not String.IsNullOrEmpty(g_SettingsManager.LastPlaylistFolder) Then
                    If Directory.Exists(g_SettingsManager.LastPlaylistFolder) Then
                        ofd.InitialDirectory = g_SettingsManager.LastPlaylistFolder
                    End If
                End If

                If ofd.ShowDialog() = DialogResult.OK Then
                    playListpath = ofd.FileName

                    ' Save folder location to settings
                    If g_SettingsManager IsNot Nothing Then
                        g_SettingsManager.LastPlaylistFolder = Path.GetDirectoryName(ofd.FileName)
                        g_SettingsManager.SaveSettings()
                    End If
                End If
            End Using


            PlayplayslistItem()

        Catch ex As Exception
            MessageBox.Show("Error browsing for file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub


    Private Sub PlayplayslistItem()


        Try
            ' Validate file path
            If String.IsNullOrWhiteSpace(playListpath) Then
                '    MessageBox.Show("Please select a playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not File.Exists(playListpath) Then
                MessageBox.Show("File not found: " & playListpath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Parse playlist and get first URL
            Dim url As String = PlaylistParser.GetFirstUrl(playListpath)

            If String.IsNullOrEmpty(url) Then
                MessageBox.Show("Could not find any valid stream URL in the playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Show the extracted URL
            '   lblExtractedURL.Text = url
            '    lblExtractedURL.Visible = True

            ' Save to recent URLs
            AddToRecentUrls(url)

            ' Create temporary station
            Dim fileName As String = Path.GetFileNameWithoutExtension(playListpath)
            Dim tempStation As New Station()
            tempStation.Name = fileName
            tempStation.Link = url
            tempStation.Position = 0

            ' Play the stream
            Dim mainForm As frmMain = DirectCast(Application.OpenForms("frmMain"), frmMain)
            If mainForm IsNot Nothing Then
                g_CurrentStation = tempStation
                mainForm.PlayStreamFromLink(url, fileName)
            End If

            ' Show success message
            '  MessageBox.Show("Playing first stream from playlist:" & vbCrLf & url, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Close form
            Me.Close()

        Catch ex As Exception
            'MessageBox.Show("Error playing playlist: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub AddToRecentUrls(url As String)
        Try
            Dim recentFilePath As String = IO.Path.Combine(AppPath, "recent_urls.txt")

            ' Read existing URLs
            Dim urls As New List(Of String)()
            If File.Exists(recentFilePath) Then
                urls.AddRange(File.ReadAllLines(recentFilePath))
            End If

            ' Remove if already exists
            If urls.Contains(url) Then
                urls.Remove(url)
            End If

            ' Add to end (most recent)
            urls.Add(url)

            ' Keep only last 10
            If urls.Count > 10 Then
                urls = urls.Skip(urls.Count - 10).ToList()
            End If

            ' Save
            File.WriteAllLines(recentFilePath, urls.ToArray())

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub



    Private Sub tsmiStationInfo_Click(sender As Object, e As EventArgs) Handles tsmiStationInfo.Click
        ' Open station website - will implement in Phase 21
        If g_CurrentStation IsNot Nothing AndAlso Not String.IsNullOrEmpty(g_CurrentStation.StationWebAddress) Then
            Try
                Process.Start(g_CurrentStation.StationWebAddress)
            Catch ex As Exception
                MessageBox.Show("Could not open website: " & ex.Message, "Error")
            End Try
        End If
    End Sub

    ' ============================================
    ' TRAY ICON EVENTS
    ' ============================================

    Private Sub niTray_MouseClick(sender As Object, e As MouseEventArgs) Handles niTray.MouseClick
        If e.Button = MouseButtons.Left Then
            ' Check for Ctrl+LeftClick
            If Control.ModifierKeys = Keys.Control Then
                ' Show stats
                ShowStatsWindow()
            Else
                ' Normal left click - toggle playback
                ResetStopTime() ' Reset timer
                If g_IsPlaying Then
                    StopPlayback()
                Else
                    ResumePlayback()
                End If
            End If
        End If
    End Sub

    Public Sub ShowStatsWindow()
        Try
            ' Get current stream stats
            Dim stationName As String = "No Station"
            Dim bitrate As Integer = 0
            Dim codec As String = "Unknown"

            If g_CurrentStation IsNot Nothing Then
                stationName = g_CurrentStation.Name
            End If

            If g_StreamHandle <> 0 AndAlso g_IsPlaying Then
                ' Get bitrate and codec
                GetStreamStats(bitrate, codec)
            Else
                codec = "Not Playing"
            End If

            ' Create stats form if needed
            If statsForm Is Nothing OrElse statsForm.IsDisposed Then
                statsForm = New frmStats()
            End If

            ' Show stats
            statsForm.ShowStats(stationName, bitrate, codec)

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub
    Private Sub GetStreamStats(ByRef bitrate As Integer, ByRef codec As String)
        Try
            If g_StreamHandle = 0 Then
                bitrate = 0
                codec = "Unknown"
                Return
            End If

            ' Get channel info
            Dim info As BASS_CHANNELINFO = New BASS_CHANNELINFO()
            If Bass.BASS_ChannelGetInfo(g_StreamHandle, info) Then
                ' Decode codec from ctype
                codec = GetCodecName(info.ctype)

                ' Get bitrate (in kbps)
                Dim bitrateValue As Single = 0
                If Bass.BASS_ChannelGetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_BITRATE, bitrateValue) Then
                    bitrate = CInt(Math.Round(bitrateValue))
                Else
                    ' Fallback: estimate from channel info
                    bitrate = 128 ' Default estimate
                End If
            Else
                bitrate = 0
                codec = "Unknown"
            End If

        Catch ex As Exception
            bitrate = 0
            codec = "Error"
        End Try
    End Sub

    Private Function GetCodecName(channelType As BASSChannelType) As String
        Try
            ' Get string representation
            Dim typeString As String = channelType.ToString()

            ' Parse common types
            If typeString.Contains("MP3") Then
                Return "MP3"
            ElseIf typeString.Contains("AAC") Then
                Return "AAC"
            ElseIf typeString.Contains("FLAC") Then
                Return "FLAC"
            ElseIf typeString.Contains("OGG") Then
                Return "OGG"
            ElseIf typeString.Contains("WAV") Then
                Return "WAV"
            Else
                Return "Stream"
            End If

        Catch ex As Exception
            Return "Unknown"
        End Try
    End Function
    ' ============================================
    ' PLAYBACK METHODS (Basic - Phase 2)
    ' ============================================

    Private Sub PlayTestStream()
        If g_StationManager IsNot Nothing AndAlso g_StationManager.Stations.Count > 0 Then
            Dim firstStation As Station = g_StationManager.Stations(0)
            PlayStationFromObject(firstStation)
        Else
            MessageBox.Show("No stations available", "Error")
        End If
    End Sub



    Private Sub ResumePlayback()
        ' Try to resume last played station
        If g_CurrentStation IsNot Nothing Then
            ' Resume the station that was playing
            PlayStationFromObject(g_CurrentStation)
        ElseIf g_SettingsManager IsNot Nothing AndAlso Not String.IsNullOrEmpty(g_SettingsManager.LastStation) Then
            ' Try to play last station from settings
            Dim station = g_StationManager.GetStationByName(g_SettingsManager.LastStation)
            If station IsNot Nothing Then
                PlayStationFromObject(station)
            Else
                ' Last station not found, play first available
                PlayFirstAvailableStation()
            End If
        Else
            ' No history, play first available station
            PlayFirstAvailableStation()
        End If
    End Sub

    Private Sub PlayFirstAvailableStation()
        ' Play the first station in the list
        If g_StationManager IsNot Nothing AndAlso g_StationManager.Stations.Count > 0 Then
            Dim firstStation As Station = g_StationManager.Stations.Item(0)
            PlayStationFromObject(firstStation)
        Else
            MessageBox.Show("No stations available", "Error")
        End If
    End Sub




    Private Sub PlayStationFromObject(station As Station)
        If station Is Nothing Then Return

        g_CurrentStation = station
        ' Reset station tracking for history
        If g_TrackHistory IsNot Nothing Then
            g_TrackHistory.ResetStationTracking()
        End If

        PlayStream(station.Link, station.Name)
    End Sub


    Private Sub PlayStream(url As String, stationName As String)
        Try
            ' Stop any existing stream
            StopPlayback()

            ' Create stream
            g_StreamHandle = Bass.BASS_StreamCreateURL(
            url,
            0,
            BASSFlag.BASS_DEFAULT Or BASSFlag.BASS_STREAM_STATUS,
            Nothing,
            IntPtr.Zero
        )

            If g_StreamHandle = 0 Then
                Dim errorCode = Bass.BASS_ErrorGetCode()
                MessageBox.Show("Failed to open stream: " & errorCode.ToString(), "Error")

                ' Update registry - Error status
                If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                    RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.ErrorState, stationName, "Failed to open stream", g_CurrentVolume)
                End If

                Return
            End If

            ' Set up metadata sync
            Bass.BASS_ChannelSetSync(g_StreamHandle, BASSSync.BASS_SYNC_META, 0, metadataSyncProc, IntPtr.Zero)
            Bass.BASS_ChannelSetSync(g_StreamHandle, BASSSync.BASS_SYNC_OGG_CHANGE, 0, metadataSyncProc, IntPtr.Zero)

            ' Set volume
            Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)

            ' Start playback
            If Bass.BASS_ChannelPlay(g_StreamHandle, False) Then
                g_IsPlaying = True
                ' Clear auto-close timer when playing
                lastStopTime = DateTime.MinValue
                g_CurrentTrackInfo = "Getting stream..."  ' Initialize track info
                UpdateTrayIcon()
                tsmiStationInfo.Text = "🎵 " & stationName & " - Getting stream..."
                tsmiStationInfo.Enabled = True

                ' Show tips when starting playback
                ShowTipsWindow(stationName, "Getting stream...", "Playing")

                ' Update registry - Connecting status
                If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                    RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.Connecting, stationName, "Connecting...", g_CurrentVolume)
                End If
            Else
                Dim errorCode = Bass.BASS_ErrorGetCode()
                MessageBox.Show("Failed to play stream: " & errorCode.ToString(), "Error")
                Bass.BASS_StreamFree(g_StreamHandle)
                g_StreamHandle = 0

                ' Update registry - Error status
                If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                    RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.ErrorState, stationName, "Failed to play", g_CurrentVolume)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error playing stream: " & ex.Message, "Error")

            ' Update registry - Error status
            If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.ErrorState, stationName, "Exception: " & ex.Message, g_CurrentVolume)
            End If
        End Try
    End Sub




    ' Public method for external forms to play streams
    Public Sub PlayStreamFromLink(url As String, stationName As String)
        PlayStream(url, stationName)
    End Sub

    ' Public method to refresh favorites menu
    Public Sub RefreshFavoritesMenu()
        PopulateFavoritesMenu()
    End Sub


    Private Sub StopPlayback()
        Try
            If g_StreamHandle <> 0 Then
                Bass.BASS_ChannelStop(g_StreamHandle)
                Bass.BASS_StreamFree(g_StreamHandle)
                g_StreamHandle = 0
            End If

            g_IsPlaying = False
            ' Reset auto-close timer
            ResetStopTime()

            ' Cancel timed mute if active
            If isMuted Then
                RestoreVolume()
            End If

            UpdateTrayIcon()
            UpdateStationDisplay()

            ' Update registry - Stopped status (only if not shutting down)
            If Not isShuttingDown AndAlso g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                Dim stationName As String = If(g_CurrentStation IsNot Nothing, g_CurrentStation.Name, "")
                RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.Stopped, stationName, "", g_CurrentVolume)
            End If

        Catch ex As Exception
            MessageBox.Show($"Error stopping playback: {ex.Message}", "Error")
        End Try
    End Sub

    ' ============================================
    ' METADATA HANDLING (Phase 3)
    ' ============================================

    Private Sub MetadataSync(handle As Integer, channel As Integer, data As Integer, user As IntPtr)
        ' Called when metadata changes
        UpdateMetadata()
    End Sub

    Private Sub UpdateMetadata()
        If g_StreamHandle = 0 Then Return

        Try
            ' Get metadata
            Dim metaPtr As IntPtr = Bass.BASS_ChannelGetTags(g_StreamHandle, BASSTag.BASS_TAG_META)

            If metaPtr <> IntPtr.Zero Then
                Dim meta As String = Marshal.PtrToStringAnsi(metaPtr)
                If Not String.IsNullOrEmpty(meta) Then
                    Dim title As String = ParseMetadata(meta, "StreamTitle")
                    If Not String.IsNullOrEmpty(title) Then
                        UpdateTrackDisplay(title)
                    End If
                End If
            End If

        Catch ex As Exception
            ' Silently continue on metadata errors
        End Try
    End Sub

    Private Function ParseMetadata(metadata As String, key As String) As String
        Try
            Dim searchKey As String = key & "='"
            Dim startIndex As Integer = metadata.IndexOf(searchKey)

            If startIndex >= 0 Then
                startIndex += searchKey.Length
                Dim endIndex As Integer = metadata.IndexOf("';", startIndex)

                If endIndex > startIndex Then
                    Return metadata.Substring(startIndex, endIndex - startIndex)
                End If
            End If
        Catch
        End Try

        Return ""
    End Function
    Private Sub UpdateTrackDisplay(trackInfo As String)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() UpdateTrackDisplay(trackInfo))
        Else
            Dim stationName As String = "Unknown Station"
            If g_CurrentStation IsNot Nothing Then
                stationName = g_CurrentStation.Name

                If g_CurrentStation.FiltersList IsNot Nothing Then
                    Dim filterCount As Integer = g_CurrentStation.FiltersList.Count
                    For i As Integer = 0 To filterCount - 1
                        Dim filterItem = g_CurrentStation.FiltersList.Item(i)
                        If Not String.IsNullOrEmpty(filterItem.Find) Then
                            trackInfo = trackInfo.Replace(filterItem.Find, filterItem.Replace)
                        End If
                    Next
                End If
            End If

            tsmiStationInfo.Text = "🎵 " & stationName & " - " & trackInfo

            Dim trayIconText As String = APP_NAME & vbCrLf & stationName & vbCrLf & trackInfo
            If trayIconText.Length > 63 Then
                trayIconText = stationName & vbCrLf & trackInfo
                If trayIconText.Length > 63 Then
                    trayIconText = trayIconText.Substring(0, 60) & "..."
                End If
            End If
            niTray.Text = trayIconText

            ' Store track info globally
            g_CurrentTrackInfo = trackInfo

            ' Fetch album art (async, non-blocking)
            FetchAlbumArt(trackInfo)  ' ADD THIS LINE

            ' Show tips on track change
            If g_IsPlaying Then
                ShowTipsWindow(stationName, trackInfo, "Playing")
            End If

            ' Update registry - Playing with track info (only if we have real track info)
            If g_IsPlaying AndAlso g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
                If Not trackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                    RegistryExporter.UpdateRegistry(RegistryExporter.PlayerStatus.Playing, stationName, trackInfo, g_CurrentVolume)
                End If
            End If

            ' Log track to history file (if enabled)
            If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.SaveTrackHistory Then
                If g_TrackHistory IsNot Nothing AndAlso g_IsPlaying Then
                    g_TrackHistory.LogTrack(stationName, trackInfo)
                End If
            End If
        End If
    End Sub


    ' ============================================
    ' CLEANUP
    ' ============================================

    Private Sub CleanupAndExit()

        ' Set shutdown flag
        isShuttingDown = True

        ' Save settings
        SaveCurrentSettings()

        ' Unregister hotkeys
        If g_HotkeyManager IsNot Nothing Then
            g_HotkeyManager.UnregisterAllHotkeys()
        End If

        ' Stop playback (but don't update registry yet)
        If g_StreamHandle <> 0 Then
            Bass.BASS_ChannelStop(g_StreamHandle)
            Bass.BASS_StreamFree(g_StreamHandle)
            g_StreamHandle = 0
        End If

        g_IsPlaying = False

        ' Free BASS
        Bass.BASS_Free()

        ' Hide tray icon
        niTray.Visible = False

        ' Close tips window
        If tipsForm IsNot Nothing AndAlso Not tipsForm.IsDisposed Then
            tipsForm.Close()
        End If

        ' Clear registry LAST (sets NotRunning = 0)
        RegistryExporter.ClearRegistry()

        ' Stop HTTP server
        StopHTTPServer()


        ' Exit
        Application.Exit()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Prevent closing, minimize to tray instead
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Hide()
        End If
    End Sub

    ' ============================================
    ' HOTKEY HANDLING (Phase 7)
    ' ============================================

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If m.Msg = WM_HOTKEY Then
            Dim hotkeyId As Integer = CInt(m.WParam)
            HandleHotkey(hotkeyId)
        End If
    End Sub

    Private Sub HandleHotkey(hotkeyId As Integer)
        Try
            Select Case hotkeyId
                Case HotkeyManager.HOTKEY_PLAY
                    tmrFlashIcon.Enabled = True
                    If Not g_IsPlaying Then
                        ResumePlayback()  ' CHANGED
                    End If

                Case HotkeyManager.HOTKEY_STOP
                    tmrFlashIcon.Enabled = True
                    If g_IsPlaying Then
                        StopPlayback()
                    End If


                Case HotkeyManager.HOTKEY_SHOWTIPS
                    ShowTipsFromHotkey()
                    tmrFlashIcon.Enabled = True

                Case HotkeyManager.HOTKEY_EXIT
                    CleanupAndExit()
                    'tmrFlashIcon.Enabled = True
                Case HotkeyManager.HOTKEY_OPENLINK
                    tmrFlashIcon.Enabled = True
                    tsmiOpenLink.PerformClick()

                Case HotkeyManager.HOTKEY_VOLUMEUP
                    tmrFlashIcon.Enabled = True
                    AdjustVolume(10)

                Case HotkeyManager.HOTKEY_VOLUMEDOWN
                    tmrFlashIcon.Enabled = True
                    AdjustVolume(-10)

                Case HotkeyManager.HOTKEY_TIMEDMUTE
                    tmrFlashIcon.Enabled = True
                    ToggleTimedMute()


                Case HotkeyManager.HOTKEY_SAVEBELOVED
                    tmrFlashIcon.Enabled = True
                    SaveBelovedTrack()

                Case HotkeyManager.HOTKEY_SHOWCOVER
                    tmrFlashIcon.Enabled = True
                    ShowAlbumArtFromHotkey()


                Case HotkeyManager.HOTKEY_SHOWLYRICS
                    tmrFlashIcon.Enabled = True
                    tsmiShowLyrics.PerformClick()

                Case 200 ' Media Play/Pause
                    tmrFlashIcon.Enabled = True
                    If g_IsPlaying Then
                        StopPlayback()
                    Else
                        PlayTestStream()
                    End If

                Case 201 ' Media Stop
                    tmrFlashIcon.Enabled = True
                    If g_IsPlaying Then
                        StopPlayback()
                    End If

                Case 202 ' Media Next
                    tmrFlashIcon.Enabled = True
                    PlayNextFavorite()

                Case 203 ' Media Previous
                    tmrFlashIcon.Enabled = True
                    PlayPreviousFavorite()

                Case Else
                    ' Check if it's a favorite hotkey (100-109)
                    If hotkeyId >= HotkeyManager.HOTKEY_FAVORITE_BASE AndAlso hotkeyId < HotkeyManager.HOTKEY_FAVORITE_BASE + 10 Then
                        Dim favoriteIndex As Integer = hotkeyId - HotkeyManager.HOTKEY_FAVORITE_BASE
                        Dim favoritePosition As Integer = If(favoriteIndex = 0, 10, favoriteIndex)
                        PlayFavoriteByPosition(favoritePosition)
                    End If


            End Select

        Catch ex As Exception
            ' Silent fail for hotkey errors
        End Try
    End Sub

    Private Sub AdjustVolume(change As Integer)
        g_CurrentVolume += change

        ' Clamp to 0-100
        If g_CurrentVolume < 0 Then g_CurrentVolume = 0
        If g_CurrentVolume > 100 Then g_CurrentVolume = 100

        ' Cancel timed mute if active (user is manually adjusting)
        CancelMuteIfActive()

        ' Apply to BASS
        If g_StreamHandle <> 0 Then
            Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
        End If

        ' Save to settings
        If g_SettingsManager IsNot Nothing Then
            g_SettingsManager.LastVolume = g_CurrentVolume
        End If
        ' Update registry with new volume
        If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.ExtractTitle Then
            Dim stationName As String = If(g_CurrentStation IsNot Nothing, g_CurrentStation.Name, "")
            Dim status As RegistryExporter.PlayerStatus = If(g_IsPlaying, RegistryExporter.PlayerStatus.Playing, RegistryExporter.PlayerStatus.Stopped)
            RegistryExporter.UpdateRegistry(status, stationName, g_CurrentTrackInfo, g_CurrentVolume)
        End If
        ' Show level form briefly to show current volume
        ShowLevelForm()
    End Sub

    Public Sub ShowAlbumArtFromHotkey()
        Try
            ' Check if we have album art (use frmMain's stored path, not frmTips)
            If String.IsNullOrEmpty(currentAlbumArtPath) OrElse Not IO.File.Exists(currentAlbumArtPath) Then
                ' No album art available
                MessageBox.Show("No album art available for current track.",
                          "Album Art",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)
                Return
            End If

            ' Build track title from frmMain globals (not from frmTips)
            Dim trackTitle As String = "Unknown Track"

            If g_CurrentStation IsNot Nothing Then
                trackTitle = g_CurrentStation.Name

                If Not String.IsNullOrWhiteSpace(g_CurrentTrackInfo) AndAlso
               Not g_CurrentTrackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                    trackTitle &= " - " & g_CurrentTrackInfo
                End If
            End If

            ' Show album art viewer
            Dim coverForm As New frmCover()
            frmCover.ShowCover(currentAlbumArtPath, trackTitle)

        Catch ex As Exception
            MessageBox.Show("Error showing album art: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






    Private Sub PlayFavoriteByPosition(position As Integer)
        Try
            If g_StationManager IsNot Nothing Then
                Dim station = g_StationManager.GetStation(position)
                If station IsNot Nothing Then
                    PlayStationFromObject(station)
                End If
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub PlayNextFavorite()
        Try
            If g_StationManager Is Nothing OrElse g_CurrentStation Is Nothing Then
                PlayTestStream()
                Return
            End If

            Dim currentPos As Integer = g_CurrentStation.Position
            Dim stationsList = g_StationManager.Stations.OrderBy(Function(s) s.Position).ToList()

            ' Find next station
            Dim nextStation As Station = Nothing
            For Each station In stationsList
                If station.Position > currentPos Then
                    nextStation = station
                    Exit For
                End If
            Next

            ' If no next, wrap to first
            If nextStation Is Nothing AndAlso stationsList.Count > 0 Then
                nextStation = stationsList(0)
            End If

            If nextStation IsNot Nothing Then
                PlayStationFromObject(nextStation)
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub PlayPreviousFavorite()
        Try
            If g_StationManager Is Nothing OrElse g_CurrentStation Is Nothing Then
                PlayTestStream()
                Return
            End If

            Dim currentPos As Integer = g_CurrentStation.Position
            Dim stationsList = g_StationManager.Stations.OrderByDescending(Function(s) s.Position).ToList()

            ' Find previous station
            Dim prevStation As Station = Nothing
            For Each station In stationsList
                If station.Position < currentPos Then
                    prevStation = station
                    Exit For
                End If
            Next

            ' If no previous, wrap to last
            If prevStation Is Nothing AndAlso stationsList.Count > 0 Then
                prevStation = stationsList(0)
            End If

            If prevStation IsNot Nothing Then
                PlayStationFromObject(prevStation)
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub
    Private Sub RegisterHotkeys()
        Try
            If g_SettingsManager Is Nothing Then Return

            ' Create hotkey manager
            g_HotkeyManager = New HotkeyManager(Me.Handle)

            ' Check if hotkeys are disabled
            If g_SettingsManager.HotkeyModifier = 0 Then
                ' Hotkeys disabled, only register media keys if enabled
                If g_SettingsManager.MultimediaKeys Then
                    g_HotkeyManager.RegisterMediaKeys()
                End If
            Else
                ' Register all hotkeys with configured modifier
                g_HotkeyManager.RegisterAllHotkeys(g_SettingsManager.HotkeyModifier)

                ' Register media keys if enabled
                If g_SettingsManager.MultimediaKeys Then
                    g_HotkeyManager.RegisterMediaKeys()
                End If
            End If

            ' Write active hotkeys file
            Dim hotkeyFilePath As String = IO.Path.Combine(AppPath, "active_hotkeys.txt")
            g_HotkeyManager.WriteActiveHotkeysFile(hotkeyFilePath)

        Catch ex As Exception
            MessageBox.Show("Error registering hotkeys: " & ex.Message, "Warning")
        End Try
    End Sub

    ' ============================================
    ' LAN DISCOVERY BROADCAST (UDP)
    ' ============================================

    Private Sub StartBroadcastService()
        Try
            ' Only start if enabled in settings
            If g_SettingsManager Is Nothing OrElse Not g_SettingsManager.BroadcastEnabled Then
                Return
            End If

            ' Only broadcast when HTTP server is running
            If g_HTTPServer Is Nothing OrElse Not g_HTTPServer.IsRunning Then
                Return
            End If

            ' Create broadcast service
            Dim logPath As String = IO.Path.Combine(AppPath, "broadcast_debug.log")
            Dim debugEnabled As Boolean = If(g_SettingsManager IsNot Nothing, g_SettingsManager.BroadcastDebug, False)

            g_BroadcastService = New BroadcastService(debugEnabled, logPath)

            ' Start broadcasting
            g_BroadcastService.StartBroadcast()

        Catch ex As Exception
            ' Silent fail - don't interrupt app
        End Try
    End Sub

    Private Sub StopBroadcastService()
        Try
            If g_BroadcastService IsNot Nothing Then
                g_BroadcastService.StopBroadcast()
                g_BroadcastService = Nothing
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    ' ============================================
    ' ALBUM ART FETCHER (Native VB.NET)
    ' ============================================

    Private Sub FetchAlbumArt(trackInfo As String)
        Try
            ' Skip if no track info
            If String.IsNullOrWhiteSpace(trackInfo) OrElse trackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                currentAlbumArtPath = ""
                Return
            End If

            ' Ensure Covers folder exists
            EnsurePlaybackFoldersExist()

            ' Generate sanitized track name for filename
            Dim sanitizedTrackName As String = SanitizeFilename(trackInfo, 50)

            ' Generate unique filename based on track hash
            Dim hash As String = GetTrackHash(trackInfo)
            Dim coverFilename As String = $"{sanitizedTrackName}_{hash}.jpg"
            Dim coverPath As String = IO.Path.Combine(CoversFolder, coverFilename)

            ' Check if already exists
            If IO.File.Exists(coverPath) Then
                currentAlbumArtPath = coverPath
                Return
            End If

            ' Reset current path
            currentAlbumArtPath = ""

            ' Fetch asynchronously (don't block UI)
            System.Threading.Tasks.Task.Run(Sub()
                                                Try
                                                    Dim success As Boolean = DownloadDiscogsImage(trackInfo, coverPath)

                                                    If success Then
                                                        ' Update current path
                                                        currentAlbumArtPath = coverPath

                                                        ' Update tips if showing
                                                        If tipsForm IsNot Nothing AndAlso Not tipsForm.IsDisposed AndAlso tipsForm.Visible Then
                                                            Me.Invoke(Sub() tipsForm.UpdateAlbumArt(coverPath))
                                                        End If

                                                        ' Cleanup old images
                                                        CleanupOldAlbumArt()
                                                    End If
                                                Catch ex As Exception
                                                    ' Silent fail - don't interrupt playback
                                                End Try
                                            End Sub)

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Function DownloadDiscogsImage(query As String, outputPath As String) As Boolean
        Try
            ' Get token from settings
            Dim token As String = ""
            If g_SettingsManager IsNot Nothing Then
                token = g_SettingsManager.DiscogsToken
            End If

            ' Skip if no token
            If String.IsNullOrWhiteSpace(token) Then
                Return False
            End If

            Using client As New System.Net.WebClient()
                client.Headers.Add("User-Agent", "PlayTray/1.0 +https://github.com/playtray")

                ' Build search URL with token from settings
                Dim searchUrl As String = "https://api.discogs.com/database/search?" &
                                     $"q={Uri.EscapeDataString(query)}" &
                                     "&type=release" &
                                     "&per_page=1" &
                                     $"&token={token}"

                ' Download search results (with timeout handling)
                Dim jsonResponse As String = ""

                Try
                    jsonResponse = client.DownloadString(searchUrl)
                Catch webEx As System.Net.WebException
                    ' Timeout or network error
                    Return False
                End Try

                ' Parse JSON to find cover_image URL (simple regex parsing)
                Dim coverUrlMatch = System.Text.RegularExpressions.Regex.Match(jsonResponse, """cover_image""\s*:\s*""([^""]+)""")

                If Not coverUrlMatch.Success Then
                    Return False
                End If

                Dim imageUrl As String = coverUrlMatch.Groups(1).Value

                ' Skip placeholder images
                If imageUrl.Contains("spacer.gif") OrElse imageUrl.Contains("1x1.gif") Then
                    Return False
                End If

                ' Download image
                client.DownloadFile(imageUrl, outputPath)

                ' Verify file was created and has content
                If IO.File.Exists(outputPath) Then
                    Dim fileInfo As New IO.FileInfo(outputPath)
                    If fileInfo.Length > 1000 Then ' At least 1KB
                        Return True
                    Else
                        ' Too small, probably error image
                        IO.File.Delete(outputPath)
                        Return False
                    End If
                End If

                Return False

            End Using

        Catch ex As Exception
            ' Delete partial file on error
            Try
                If IO.File.Exists(outputPath) Then
                    IO.File.Delete(outputPath)
                End If
            Catch
            End Try

            Return False
        End Try
    End Function

    Private Function GetTrackHash(trackInfo As String) As String
        Try
            ' Create hash from track info for unique filename
            Using md5 = System.Security.Cryptography.MD5.Create()
                Dim inputBytes = System.Text.Encoding.UTF8.GetBytes(trackInfo.ToLower().Trim())
                Dim hashBytes = md5.ComputeHash(inputBytes)

                ' Convert to hex string (first 8 chars)
                Dim sb As New Text.StringBuilder()
                For i As Integer = 0 To 3 ' First 4 bytes = 8 hex chars
                    sb.Append(hashBytes(i).ToString("x2"))
                Next

                Return sb.ToString()
            End Using
        Catch ex As Exception
            ' Fallback to simple hash
            Return Math.Abs(trackInfo.GetHashCode()).ToString("x8")
        End Try
    End Function


    ''' <summary>
    ''' Sanitizes track info string for use as filename
    ''' Removes invalid characters and truncates to specified length
    ''' </summary>
    Private Function SanitizeFilename(trackInfo As String, maxLength As Integer) As String
        Try
            ' Start with trimmed track info
            Dim sanitized As String = trackInfo.Trim()

            ' Replace invalid filename characters with underscore
            Dim invalidChars() As Char = IO.Path.GetInvalidFileNameChars()
            For Each c As Char In invalidChars
                sanitized = sanitized.Replace(c, "_"c)
            Next

            ' Also replace some additional problematic characters
            sanitized = sanitized.Replace(" ", "_")
            sanitized = sanitized.Replace(".", "_")
            sanitized = sanitized.Replace(",", "_")
            sanitized = sanitized.Replace("'", "")
            sanitized = sanitized.Replace("""", "")

            ' Remove multiple consecutive underscores
            While sanitized.Contains("__")
                sanitized = sanitized.Replace("__", "_")
            End While

            ' Truncate to max length
            If sanitized.Length > maxLength Then
                sanitized = sanitized.Substring(0, maxLength)
            End If

            ' Remove trailing underscore if present
            sanitized = sanitized.TrimEnd("_"c)

            ' Ensure not empty
            If String.IsNullOrEmpty(sanitized) Then
                sanitized = "track"
            End If

            Return sanitized

        Catch ex As Exception
            ' Fallback to generic name
            Return "track"
        End Try
    End Function

    Private Sub CleanupOldAlbumArt()
        Try
            ' Ensure Covers folder exists
            If Not IO.Directory.Exists(CoversFolder) Then
                Return
            End If

            ' Keep only last 500 album art images to save disk space
            ' Changed from 5000 to 500 since we now use descriptive names
            Dim coverFiles = IO.Directory.GetFiles(CoversFolder, "*_*.jpg")

            If coverFiles.Length > 500 Then
                ' Sort by last write time (oldest first)
                Dim sorted = coverFiles.OrderBy(Function(f) IO.File.GetLastWriteTime(f)).ToArray()

                ' Delete oldest files, keep 500 newest
                For i As Integer = 0 To sorted.Length - 501
                    Try
                        IO.File.Delete(sorted(i))
                    Catch
                        ' Skip if file is locked
                    End Try
                Next
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub


    Private seqCount2 As Integer = 0
    Private Sub tmrFlashIcon_Tick(sender As Object, e As EventArgs) Handles tmrFlashIcon.Tick
        If seqCount2 < 6 Then
            niTray.Icon = If(seqCount2 Mod 2 = 0, My.Resources.Seq5, My.Resources.Seq6)
            seqCount2 += 1

        Else
            ' Stop flashing after 3 cycles (6 ticks)
            seqCount2 = 0
            tmrFlashIcon.Stop()
            UpdateTrayIcon()
        End If

    End Sub

    Public Sub StartSoftFlash()
        If Me.InvokeRequired Then
            Me.Invoke(Sub() StartSoftFlash())
        Else
            seqCount2 = 0
            tmrFlashIcon.Start()
        End If
    End Sub

    Private Sub tsmiShowLyrics_Click(sender As Object, e As EventArgs) Handles tsmiShowLyrics.Click
        Try
            ' Check if we have track info
            If String.IsNullOrWhiteSpace(g_CurrentTrackInfo) OrElse
               g_CurrentTrackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("No track information available.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Show lyrics form
            Dim lyricsForm As New frmLyrics()
            lyricsForm.ShowLyrics(g_CurrentTrackInfo)

        Catch ex As Exception
            MessageBox.Show("Error showing lyrics: " & ex.Message, "Error")
        End Try
    End Sub
End Class
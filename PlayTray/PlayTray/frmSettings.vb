
Imports System.Diagnostics
Imports System.Net.Sockets
Imports System.IO

Public Class frmSettings

    Private settingsLoaded As Boolean = False

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCurrentSettings()
        settingsLoaded = True

        ' Add tooltips in frmSettings_Load
        SetupServerButtonTooltips()
    End Sub


    Private Sub SetupServerButtonTooltips()
        Try
            Dim tooltip As New ToolTip()
            tooltip.AutoPopDelay = 10000 ' Show for 10 seconds
            tooltip.InitialDelay = 500
            tooltip.ReshowDelay = 200

            ' Setup Network Access
            tooltip.SetToolTip(btnSetupNetwork,
            "Reserve URL for network access (one-time setup)." & vbCrLf &
            "After this, the server can run without Administrator rights." & vbCrLf &
            "Requires Administrator privileges to run.")

            ' Setup Firewall Rule
            tooltip.SetToolTip(btnSetupFirewall,
            "Create Windows Firewall rule to allow incoming connections." & vbCrLf &
            "Required for network access from other devices." & vbCrLf &
            "Requires Administrator privileges to run.")

            ' Test Connection
            tooltip.SetToolTip(btnTestFirewall,
            "Display the server URL and copy it to clipboard." & vbCrLf &
            "Use this to test if the server is accessible from the network.")

            ' Troubleshooting
            tooltip.SetToolTip(btnTroubleshooting,
            "View troubleshooting guide with manual setup instructions." & vbCrLf &
            "Includes PowerShell commands and common issues.")

            ' Refresh IPs button
            tooltip.SetToolTip(btnRefreshIPs,
            "Refresh the list of available network interfaces.")

        Catch ex As Exception
            ' Silent fail - tooltips are not critical
        End Try
    End Sub

    Private Sub LoadCurrentSettings()
        If g_SettingsManager Is Nothing Then Return

        Try
            ' === GENERAL TAB ===
            chkAutoPlay.Checked = g_SettingsManager.AutoPlay
            chkShowSplash.Checked = g_SettingsManager.ShowSplash
            chkShowTips.Checked = g_SettingsManager.ShowTips
            chkExtractTitle.Checked = g_SettingsManager.ExtractTitle
            chkSaveHistory.Checked = g_SettingsManager.SaveTrackHistory
            chkCheckUpdates.Checked = g_SettingsManager.CheckForUpdates
            chkCheckUpdates.Enabled = False ' Future feature



            ' === HOTKEYS TAB ===
            cboModifier.Items.Clear()
            cboModifier.Items.Add("NONE (Disabled)")
            cboModifier.Items.Add("CTRL + SHIFT")
            cboModifier.Items.Add("CTRL + ALT")
            cboModifier.Items.Add("CTRL + ALT + SHIFT")
            cboModifier.SelectedIndex = g_SettingsManager.HotkeyModifier

            chkMultimediaKeys.Checked = g_SettingsManager.MultimediaKeys

            ' === NETWORK TAB ===
            chkEnableServer.Checked = g_SettingsManager.EnableServer
            numServerPort.Value = g_SettingsManager.ServerPort

            ' Populate IP addresses
            PopulateBindAddresses()

            ' Select saved address
            SelectBindAddress(g_SettingsManager.ServerBindAddress)

            UpdateServerStatus()

            ' === ADVANCED TAB ===
            chkAutoClose.Checked = g_SettingsManager.AutoCloseEnabled
            numAutoCloseMinutes.Value = g_SettingsManager.InactivityMinutes
            numTimedMuteSeconds.Value = g_SettingsManager.TimedMuteSeconds
            numMuteLevel.Value = g_SettingsManager.MuteLevel

            cboLogLevel.Items.Clear()
            cboLogLevel.Items.Add("None")
            cboLogLevel.Items.Add("Errors Only")
            cboLogLevel.Items.Add("Info")
            cboLogLevel.Items.Add("Debug")
            cboLogLevel.SelectedIndex = g_SettingsManager.LogLevel


            ' Discogs token
            txtDiscogsToken.Text = g_SettingsManager.DiscogsToken


            ' MusixMatch token
            txtMusixMatchToken.Text = g_SettingsManager.MusixMatchToken

        Catch ex As Exception
            MessageBox.Show("Error loading settings: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SaveSettingsToManager()
        If g_SettingsManager Is Nothing Then Return

        Try
            ' === GENERAL TAB ===
            g_SettingsManager.AutoPlay = chkAutoPlay.Checked
            g_SettingsManager.ShowSplash = chkShowSplash.Checked
            g_SettingsManager.ShowTips = chkShowTips.Checked
            g_SettingsManager.ExtractTitle = chkExtractTitle.Checked
            g_SettingsManager.SaveTrackHistory = chkSaveHistory.Checked
            g_SettingsManager.CheckForUpdates = chkCheckUpdates.Checked



            ' === HOTKEYS TAB ===
            g_SettingsManager.HotkeyModifier = cboModifier.SelectedIndex
            g_SettingsManager.MultimediaKeys = chkMultimediaKeys.Checked

            ' === NETWORK TAB ===
            g_SettingsManager.EnableServer = chkEnableServer.Checked
            g_SettingsManager.ServerPort = CInt(numServerPort.Value)
            g_SettingsManager.ServerBindAddress = GetSelectedBindAddress()

            ' === ADVANCED TAB ===
            g_SettingsManager.AutoCloseEnabled = chkAutoClose.Checked
            g_SettingsManager.InactivityMinutes = CInt(numAutoCloseMinutes.Value)
            g_SettingsManager.TimedMuteSeconds = CInt(numTimedMuteSeconds.Value)
            g_SettingsManager.MuteLevel = CInt(numMuteLevel.Value)
            g_SettingsManager.LogLevel = cboLogLevel.SelectedIndex

            ' Discogs token
            g_SettingsManager.DiscogsToken = txtDiscogsToken.Text.Trim()

            ' MusixMatch token
            g_SettingsManager.MusixMatchToken = txtMusixMatchToken.Text.Trim()

        Catch ex As Exception
            MessageBox.Show("Error saving settings: " & ex.Message, "Error")
        End Try
    End Sub


    Private Sub lnkGetMusixMatchToken_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkGetMusixMatchToken.LinkClicked
        Try
            ' Open MusixMatch developer page
            Process.Start("https://developer.musixmatch.com")

            MessageBox.Show(
                "To get your FREE MusixMatch API key:" & vbCrLf & vbCrLf &
                "1. Create a free account" & vbCrLf &
                "2. Go to Applications → Create new application" & vbCrLf &
                "3. Copy the API key" & vbCrLf &
                "4. Paste it here" & vbCrLf & vbCrLf &
                "FREE TIER: 2000 API calls/day, 30% of lyrics",
                "Get MusixMatch API Key",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Could not open browser. Please visit:" & vbCrLf & vbCrLf &
                       "https://developer.musixmatch.com",
                       "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub lnkGetToken_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkGetToken.LinkClicked
        Try
            ' Open Discogs developer page
            Process.Start("https://www.discogs.com/settings/developers")

            MessageBox.Show("To get your free Discogs API token:" & vbCrLf & vbCrLf &
                       "1. Log in or create a Discogs account" & vbCrLf &
                       "2. Go to Settings > Developers" & vbCrLf &
                       "3. Click 'Generate new token'" & vbCrLf &
                       "4. Copy the token and paste it here",
                       "Get Discogs Token",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Could not open browser. Please visit:" & vbCrLf & vbCrLf &
                       "https://www.discogs.com/settings/developers",
                       "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub UpdateServerStatus()
        If chkEnableServer.Checked Then
            Dim bindAddr As String = GetSelectedBindAddress()
            Dim port As Integer = CInt(numServerPort.Value)

            Dim statusText As String = ""

            If bindAddr = "*" Then
                statusText = $"Server will bind to all interfaces on port {port}"
            Else
                statusText = $"Server will bind to:" & vbCrLf &
                        $"• localhost:{port}" & vbCrLf &
                        $"• 127.0.0.1:{port}" & vbCrLf &
                        $"• {bindAddr}:{port}"
            End If

            lblServerStatus.Text = statusText
            lblServerStatus.ForeColor = Color.Green
        Else
            lblServerStatus.Text = "Server disabled"
            lblServerStatus.ForeColor = Color.Gray
        End If
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SaveSettingsToManager()
        g_SettingsManager.SaveSettings()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveSettingsToManager()
        g_SettingsManager.SaveSettings()
        '  MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub chkEnableServer_CheckedChanged(sender As Object, e As EventArgs) Handles chkEnableServer.CheckedChanged
        If settingsLoaded Then
            UpdateServerStatus()
        End If
    End Sub

    Private Sub numServerPort_ValueChanged(sender As Object, e As EventArgs) Handles numServerPort.ValueChanged
        If settingsLoaded Then
            UpdateServerStatus()
        End If
    End Sub



    Private Sub chkAutoClose_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoClose.CheckedChanged
        If settingsLoaded Then
            numAutoCloseMinutes.Enabled = chkAutoClose.Checked
        End If
    End Sub

    Private Sub PopulateBindAddresses()
        Try
            cboBindAddress.Items.Clear()

            ' Get all local IP addresses (NO localhost options)
            Dim hostName As String = System.Net.Dns.GetHostName()
            Dim addresses = System.Net.Dns.GetHostAddresses(hostName)

            Dim hasNetworkIP As Boolean = False

            For Each addr In addresses
                ' Only IPv4 addresses
                If addr.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                    ' Skip loopback addresses (127.x.x.x)
                    If Not addr.ToString().StartsWith("127.") Then
                        Dim displayName As String = $"{addr} (Network Interface)"
                        cboBindAddress.Items.Add(New BindAddressItem(displayName, addr.ToString()))
                        hasNetworkIP = True
                    End If
                End If
            Next

            ' Add separator if we have network IPs
            If hasNetworkIP Then
                cboBindAddress.Items.Add(New BindAddressItem("--- Advanced ---", ""))
            End If

            ' Add "All Interfaces" option
            cboBindAddress.Items.Add(New BindAddressItem("All Network Interfaces (*)", "*"))

            ' Select first item if nothing selected
            If cboBindAddress.SelectedIndex < 0 AndAlso cboBindAddress.Items.Count > 0 Then
                cboBindAddress.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading network addresses: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SelectBindAddress(address As String)
        Try
            ' If address is localhost or 127.0.0.1, select first network IP
            If address = "localhost" OrElse address = "127.0.0.1" Then
                If cboBindAddress.Items.Count > 0 Then
                    cboBindAddress.SelectedIndex = 0
                End If
                Return
            End If

            ' Find matching address
            For i As Integer = 0 To cboBindAddress.Items.Count - 1
                Dim item = TryCast(cboBindAddress.Items(i), BindAddressItem)
                If item IsNot Nothing AndAlso item.Address.Equals(address, StringComparison.OrdinalIgnoreCase) Then
                    cboBindAddress.SelectedIndex = i
                    Return
                End If
            Next

            ' Not found, default to first
            If cboBindAddress.Items.Count > 0 Then
                cboBindAddress.SelectedIndex = 0
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub btnRefreshIPs_Click(sender As Object, e As EventArgs) Handles btnRefreshIPs.Click
        Dim currentSelection As String = GetSelectedBindAddress()
        PopulateBindAddresses()
        SelectBindAddress(currentSelection)
    End Sub

    Private Function GetSelectedBindAddress() As String
        Try
            Dim item = TryCast(cboBindAddress.SelectedItem, BindAddressItem)
            If item IsNot Nothing Then
                Return item.Address
            End If
        Catch
            ' Fall through
        End Try
        Return "localhost"
    End Function

    Private Sub cboBindAddress_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBindAddress.SelectedIndexChanged
        If settingsLoaded Then
            UpdateServerStatus()
        End If
    End Sub

    ' Helper class for combobox items
    Private Class BindAddressItem
        Public Property DisplayName As String
        Public Property Address As String

        Public Sub New(display As String, addr As String)
            DisplayName = display
            Address = addr
        End Sub

        Public Overrides Function ToString() As String
            Return DisplayName
        End Function
    End Class

    Private Sub btnSetupNetwork_Click(sender As Object, e As EventArgs) Handles btnSetupNetwork.Click
        Try
            Dim bindAddress As String = GetSelectedBindAddress()
            Dim port As Integer = CInt(numServerPort.Value)

            ' Check if already reserved
            If IsUrlReserved(bindAddress, port) Then
                MessageBox.Show("Network access is already configured for this address and port.", "Already Setup", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Show explanation
            Dim result = MessageBox.Show(
            "This will configure Windows to allow network access without requiring Administrator rights." & vbCrLf & vbCrLf &
            "This is a one-time setup that requires Administrator privileges." & vbCrLf & vbCrLf &
            "After this setup, PlayTray can run normally without admin rights." & vbCrLf & vbCrLf &
            "Continue?",
            "Setup Network Access",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                SetupUrlReservation(bindAddress, port)
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function IsUrlReserved(bindAddress As String, port As Integer) As Boolean
        Try
            ' Run netsh to check reservations
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
            Dim urlPattern As String = If(bindAddress = "*", $"http://*:{port}/", $"http://{bindAddress}:{port}/")
            Return output.Contains(urlPattern)

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub SetupUrlReservation(bindAddress As String, port As Integer)
        Try
            ' Prepare the netsh command
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
                "PlayTray can now access the network without Administrator rights." & vbCrLf & vbCrLf &
                "You may need to restart the application.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Else
                MessageBox.Show(
                "Setup failed. Please run this command manually as Administrator:" & vbCrLf & vbCrLf &
                $"netsh {arguments}",
                "Setup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error during setup: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSetupFirewall_Click(sender As Object, e As EventArgs) Handles btnSetupFirewall.Click
        Try
            Dim port As Integer = CInt(numServerPort.Value)

            ' Check if rule already exists
            If IsFirewallRuleExists(port) Then
                Dim result = MessageBox.Show(
                "A firewall rule already exists for this port." & vbCrLf & vbCrLf &
                "Do you want to recreate it?",
                "Rule Exists",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    RemoveFirewallRule(port)
                Else
                    Return
                End If
            End If

            ' Show explanation
            Dim setupResult = MessageBox.Show(
            "This will create a Windows Firewall rule to allow incoming connections on the server port." & vbCrLf & vbCrLf &
            $"Port: {port}" & vbCrLf & vbCrLf &
            "This requires Administrator privileges." & vbCrLf & vbCrLf &
            "Continue?",
            "Setup Firewall Rule",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If setupResult = DialogResult.Yes Then
                SetupFirewallRule(port)
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function IsFirewallRuleExists(port As Integer) As Boolean
        Try
            ' Use PowerShell to check for rule
            Dim psi As New ProcessStartInfo()
            psi.FileName = "powershell.exe"
            psi.Arguments = $"-Command ""Get-NetFirewallPortFilter | Where-Object {{$_.LocalPort -eq {port}}} | Get-NetFirewallRule | Where-Object {{$_.Direction -eq 'Inbound' -and $_.Action -eq 'Allow'}}"""
            psi.RedirectStandardOutput = True
            psi.UseShellExecute = False
            psi.CreateNoWindow = True

            Dim process As Process = Process.Start(psi)
            Dim output As String = process.StandardOutput.ReadToEnd()
            process.WaitForExit()

            ' If output has content, rule exists
            Return Not String.IsNullOrWhiteSpace(output)

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub RemoveFirewallRule(port As Integer)
        Try
            ' Remove existing rule using PowerShell
            Dim psi As New ProcessStartInfo()
            psi.FileName = "powershell.exe"
            psi.Arguments = $"-Command ""Remove-NetFirewallRule -DisplayName 'PlayTray HTTP Server (Port {port})' -ErrorAction SilentlyContinue"""
            psi.Verb = "runas"
            psi.UseShellExecute = True
            psi.CreateNoWindow = False

            Dim process As Process = Process.Start(psi)
            process.WaitForExit()

        Catch ex As Exception
            ' Silent fail - rule might not exist
        End Try
    End Sub

    Private Sub SetupFirewallRule(port As Integer)
        Try
            ' Method 1: Try PowerShell (Windows 8+)
            ' TCP rule for HTTP server
            Dim psCommandTCP As String = $"New-NetFirewallRule -DisplayName 'PlayTray HTTP Server (Port {port})' -Direction Inbound -Protocol TCP -LocalPort {port} -Action Allow -Profile Any"

            ' UDP rule for broadcast discovery
            Dim psCommandUDP As String = $"New-NetFirewallRule -DisplayName 'PlayTray Discovery Broadcast (Port 18999)' -Direction Inbound -Protocol UDP -LocalPort 18999 -Action Allow -Profile Any"

            Dim psi As New ProcessStartInfo()
            psi.FileName = "powershell.exe"
            psi.Arguments = $"-Command ""{psCommandTCP}; {psCommandUDP}"""  ' CHANGED: Execute both commands
            psi.Verb = "runas" ' Request elevation
            psi.UseShellExecute = True
            psi.CreateNoWindow = False

            Dim process As Process = Process.Start(psi)
            process.WaitForExit()

            If process.ExitCode = 0 Then
                MessageBox.Show(
                "Firewall rules created successfully!" & vbCrLf & vbCrLf &
                $"• TCP Port {port} (HTTP Server)" & vbCrLf &
                "• UDP Port 18999 (LAN Discovery)" & vbCrLf & vbCrLf &
                "Network access is now enabled.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Else
                ' PowerShell failed, try netsh
                SetupFirewallRuleNetsh(port)
            End If

        Catch ex As Exception
            ' Try fallback method
            Try
                SetupFirewallRuleNetsh(port)
            Catch ex2 As Exception
                MessageBox.Show(
                "Setup failed. Please create the rules manually:" & vbCrLf & vbCrLf &
                "1. Open Windows Defender Firewall" & vbCrLf &
                "2. Advanced Settings > Inbound Rules > New Rule" & vbCrLf &
                "3. Create TWO rules:" & vbCrLf &
                $"   - TCP Port {port} (HTTP Server)" & vbCrLf &
                "   - UDP Port 18999 (Discovery)" & vbCrLf &
                "4. Allow the connection > All profiles",
                "Setup Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            End Try
        End Try
    End Sub

    Private Sub SetupFirewallRuleNetsh(port As Integer)
        ' Fallback method using netsh (works on older Windows)
        Try
            ' Create TCP rule
            Dim psiTCP As New ProcessStartInfo()
            psiTCP.FileName = "netsh"
            psiTCP.Arguments = $"advfirewall firewall add rule name=""PlayTray HTTP Server (Port {port})"" dir=in action=allow protocol=TCP localport={port}"
            psiTCP.Verb = "runas"
            psiTCP.UseShellExecute = True
            psiTCP.CreateNoWindow = False

            Dim processTCP As Process = Process.Start(psiTCP)
            processTCP.WaitForExit()

            ' Create UDP rule
            Dim psiUDP As New ProcessStartInfo()
            psiUDP.FileName = "netsh"
            psiUDP.Arguments = "advfirewall firewall add rule name=""PlayTray Discovery Broadcast (Port 18999)"" dir=in action=allow protocol=UDP localport=18999"
            psiUDP.Verb = "runas"
            psiUDP.UseShellExecute = True
            psiUDP.CreateNoWindow = False

            Dim processUDP As Process = Process.Start(psiUDP)
            processUDP.WaitForExit()

            If processTCP.ExitCode = 0 AndAlso processUDP.ExitCode = 0 Then
                MessageBox.Show(
                "Firewall rules created successfully!" & vbCrLf & vbCrLf &
                "Both TCP and UDP rules have been added.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Else
                Throw New Exception("netsh command failed")
            End If

        Catch ex As Exception
            Throw ' Re-throw to main handler
        End Try
    End Sub
    Private Sub btnTestFirewall_Click(sender As Object, e As EventArgs) Handles btnTestFirewall.Click
        Try
            Dim port As Integer = CInt(numServerPort.Value)
            Dim bindAddress As String = GetSelectedBindAddress()

            ' Get actual IP if localhost selected
            Dim testIP As String = bindAddress
            If bindAddress = "localhost" Then
                testIP = "127.0.0.1"
            ElseIf bindAddress = "*" Then
                ' Get first non-localhost IP
                Dim hostName As String = System.Net.Dns.GetHostName()
                Dim addresses = System.Net.Dns.GetHostAddresses(hostName)
                For Each addr In addresses
                    If addr.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork AndAlso
                   Not addr.ToString().StartsWith("127.") Then
                        testIP = addr.ToString()
                        Exit For
                    End If
                Next
            End If

            ' Show test info
            Dim testUrl As String = $"http://{testIP}:{port}/status"

            Dim msg As String = "Test this URL from another device on your network:" & vbCrLf & vbCrLf &
                           testUrl & vbCrLf & vbCrLf &
                           "You can also use PowerShell on this PC:" & vbCrLf &
                           $"curl {testUrl}" & vbCrLf & vbCrLf &
                           "Or open in browser: " & testUrl

            MessageBox.Show(msg, "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Optionally copy to clipboard
            Clipboard.SetText(testUrl)
            MessageBox.Show("URL copied to clipboard!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTroubleshooting_Click(sender As Object, e As EventArgs) Handles btnTroubleshooting.Click
        Dim guide As String = "FIREWALL TROUBLESHOOTING GUIDE" & vbCrLf &
                         "================================" & vbCrLf & vbCrLf &
                         "1. CHECK FIREWALL RULES:" & vbCrLf &
                         "   PowerShell (as Admin):" & vbCrLf &
                         "   Get-NetFirewallRule | Where {$_.DisplayName -like '*PlayTray*'}" & vbCrLf & vbCrLf &
                         "2. CHECK PORT IS LISTENING:" & vbCrLf &
                         "   netstat -an | findstr :8999" & vbCrLf &
                         "   Should show: TCP 0.0.0.0:8999 or your IP:8999" & vbCrLf & vbCrLf &
                         "3. TEST LOCALLY FIRST:" & vbCrLf &
                         "   curl http://localhost:8999/status" & vbCrLf &
                         "   (Must work before testing remotely)" & vbCrLf & vbCrLf &
                         "4. TEST FROM SAME PC:" & vbCrLf &
                         "   curl http://192.168.x.x:8999/status" & vbCrLf &
                         "   (Use your actual IP)" & vbCrLf & vbCrLf &
                         "5. TEST FROM OTHER DEVICE:" & vbCrLf &
                         "   curl http://192.168.x.x:8999/status" & vbCrLf & vbCrLf &
                         "6. COMMON ISSUES:" & vbCrLf &
                         "   - Firewall rule is OUTBOUND (should be INBOUND)" & vbCrLf &
                         "   - Rule applies to Domain only (should be Any/All)" & vbCrLf &
                         "   - Multiple conflicting rules" & vbCrLf &
                         "   - Router firewall (separate from Windows)" & vbCrLf &
                         "   - Antivirus blocking" & vbCrLf & vbCrLf &
                         "7. MANUAL RULE CREATION:" & vbCrLf &
                         "   Windows Firewall > Advanced Settings >" & vbCrLf &
                         "   Inbound Rules > New Rule >" & vbCrLf &
                         "   Port > TCP > 8999 > Allow > All profiles"

        MessageBox.Show(guide, "Troubleshooting", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub lnkGetToken_LinkClicked_1(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkGetToken.LinkClicked

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        ' Use path from modGlobals
        If File.Exists(TrackHistoryFile) Then
            Process.Start("notepad.exe", TrackHistoryFile)
        Else
            MessageBox.Show("File not found. No tracks have been logged yet.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        ' Use path from modGlobals
        If File.Exists(BelovedFile) Then
            Process.Start("notepad.exe", BelovedFile)
        Else
            MessageBox.Show("File not found. No beloved tracks have been saved yet.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        If ValidateDiscogsToken() = True Then
            MessageBox.Show("Discogs token appears valid.", "Validation Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function ValidateDiscogsToken() As Boolean
        Dim token As String = txtDiscogsToken.Text.Trim()

        ' Allow empty (will use default or disable feature)
        If String.IsNullOrWhiteSpace(token) Then
            Return True
        End If

        ' Discogs tokens are typically 40 characters alphanumeric
        If token.Length < 20 Then
            MessageBox.Show("Discogs token appears to be invalid. Tokens are usually 40 characters long.",
                           "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function
End Class
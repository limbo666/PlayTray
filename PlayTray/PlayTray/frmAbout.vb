Imports Un4seen.Bass
Imports System.Reflection
Imports System.Diagnostics

Public Class frmAbout

    Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load app information
        LoadAppInfo()

        ' Load system information
        LoadSystemInfo()

        ' Load keyboard shortcuts
        LoadKeyboardShortcuts()

        lblCredits0.Text = "This program is a remake of XtreMP3 music player"


        lblCredits1.Text = "Developer:" & vbCrLf &
                               "• Nikos Georgousis" & vbCrLf & vbCrLf &
                              "Icons and Visuals:" & vbCrLf &
"• Program icon inspired by the Sony Xplod™ subwoofer design" & vbCrLf &
"  Sony and Xplod are trademarks of Sony Corporation." & vbCrLf &
                               "• System icons from Windows and custom created" & vbCrLf

        lblCredits2.Text = "Third-Party Libraries:" & vbCrLf &
                               "• Audio decoding by BASS Audio Library by Un4seen Developments"

        lnkBass.Text = "https://www.un4seen.com/"


        lblCredits3.Text = "• Cover art is powered by Discogs and requires free personal token"

        lnkDiscogs.Text = "https://www.discogs.com/"

        ' Set form properties
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterParent
    End Sub

    Private Sub LoadAppInfo()
        Try
            ' App name and version
            lblAppName.Text = APP_NAME
            lblVersion.Text = "Version " & APP_VERSION

            ' Copyright
            lblCopyright.Text = "© " & DateTime.Now.Year.ToString() & " Hand Water Pump"

            ' Description
            lblDescription.Text = "Internet Radio Player for System Tray." & vbCrLf & "Lightweight streaming audio player with hotkey support"

            ' Set app icon if available
            If Me.Icon IsNot Nothing Then
                picIcon.Image = My.Resources.XplodeClearMIDI
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub LoadSystemInfo()
        Try
            Dim info As New System.Text.StringBuilder()

            ' .NET Framework version
            info.AppendLine("Framework: .NET Framework " & Environment.Version.ToString())

            ' BASS Audio version
            Dim bassVersion As Integer = Bass.BASS_GetVersion()
            Dim major As Integer = bassVersion >> 24
            Dim minor As Integer = (bassVersion >> 16) And &HFF
            Dim revision As Integer = (bassVersion >> 8) And &HFF
            Dim build As Integer = bassVersion And &HFF
            info.AppendLine($"BASS Audio: {major}.{minor}.{revision}.{build}")

            ' Operating System
            info.AppendLine("OS: " & Environment.OSVersion.VersionString)

            ' System architecture
            info.AppendLine("Architecture: " & If(Environment.Is64BitOperatingSystem, "64-bit", "32-bit"))

            ' Display info
            txtSystemInfo.Text = info.ToString()

        Catch ex As Exception
            txtSystemInfo.Text = "System information unavailable"
        End Try
    End Sub

    Private Sub LoadKeyboardShortcuts()
        Try
            Dim shortcuts As New System.Text.StringBuilder()

            ' Get modifier string from settings
            Dim modifierStr As String = "Ctrl+Shift"
            If g_SettingsManager IsNot Nothing Then
                Select Case g_SettingsManager.HotkeyModifier
                    Case 0
                        shortcuts.AppendLine("Hotkeys are disabled")
                        txtShortcuts.Text = shortcuts.ToString()
                        Return
                    Case 1
                        modifierStr = "Ctrl+Shift"
                    Case 2
                        modifierStr = "Ctrl+Alt"
                    Case 3
                        modifierStr = "Ctrl+Alt+Shift"
                End Select
            End If

            ' Basic controls
            shortcuts.AppendLine("=== PLAYBACK ===")
            shortcuts.AppendLine($"{modifierStr}+P - Play/Resume")
            shortcuts.AppendLine($"{modifierStr}+S - Stop")
            shortcuts.AppendLine($"Left-click tray - Play/Stop toggle")
            shortcuts.AppendLine("")

            ' Volume
            shortcuts.AppendLine("=== VOLUME ===")
            shortcuts.AppendLine($"{modifierStr}++ - Volume Up")
            shortcuts.AppendLine($"{modifierStr}+- - Volume Down")
            shortcuts.AppendLine($"{modifierStr}+M - Timed Mute")
            shortcuts.AppendLine("")

            ' Favorites
            shortcuts.AppendLine("=== FAVORITES ===")
            shortcuts.AppendLine($"{modifierStr}+1-9 - Play favorite 1-9")
            shortcuts.AppendLine($"{modifierStr}+0 - Play favorite 10")
            shortcuts.AppendLine("")

            ' Other functions
            shortcuts.AppendLine("=== OTHER ===")
            shortcuts.AppendLine($"{modifierStr}+T - Show Tips")
            shortcuts.AppendLine($"{modifierStr}+O - Open Link")
            shortcuts.AppendLine($"{modifierStr}+B - Save to Beloved")
            shortcuts.AppendLine($"{modifierStr}+C - Open Cover window")
            shortcuts.AppendLine($"{modifierStr}+X - Exit")
            shortcuts.AppendLine("")

            ' Multimedia keys
            If g_SettingsManager IsNot Nothing AndAlso g_SettingsManager.MultimediaKeys Then
                shortcuts.AppendLine("=== MULTIMEDIA KEYS ===")
                shortcuts.AppendLine("Media Play/Pause - Toggle")
                shortcuts.AppendLine("Media Stop - Stop")
                shortcuts.AppendLine("Media Next - Next favorite")
                shortcuts.AppendLine("Media Previous - Previous favorite")
            End If

            txtShortcuts.Text = shortcuts.ToString()

        Catch ex As Exception
            txtShortcuts.Text = "Shortcuts information unavailable"
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCheckUpdates_Click(sender As Object, e As EventArgs) Handles btnCheckUpdates.Click
        ' Placeholder for future update checking
        MessageBox.Show("Update checking feature coming soon!" & vbCrLf & vbCrLf &
                       "Current Version: " & APP_VERSION,
                       "Check for Updates",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information)
    End Sub

    Private Sub lnkWebsite_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDocumentation.LinkClicked
        Try
            ' Replace with your actual website
            Process.Start("https://github.com/limbo666/PlayTray")
        Catch ex As Exception
            MessageBox.Show("Could not open website", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lnkDocumentation_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDocumentation.LinkClicked
        Try
            ' Replace with your actual website
            Process.Start("https://github.com/limbo666/PlayTray")
        Catch ex As Exception
            MessageBox.Show("Could not open website", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lnkLicense_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkLicense.LinkClicked
        Try
            ' Show license dialog or open license file
            Dim licensePath As String = IO.Path.Combine(AppPath, "LICENSE.txt")

            If IO.File.Exists(licensePath) Then
                Process.Start("notepad.exe", licensePath)
            Else
                ' Show inline license info
                MessageBox.Show(
         "MIT License" & vbCrLf & vbCrLf &
         "Copyright (c) " & DateTime.Now.Year.ToString() & " PlayTray" & vbCrLf & vbCrLf &
         "Permission is hereby granted, free of charge, to any person obtaining a copy " &
         "of this software and associated documentation files (the ""Software""), to deal " &
         "in the Software without restriction, including without limitation the rights " &
         "to use, copy, modify, merge, publish, distribute, sublicense, and/or sell " &
         "copies of the Software, and to permit persons to whom the Software is " &
         "furnished to do so, subject to the following conditions:" & vbCrLf & vbCrLf &
         "The above copyright notice and this permission notice shall be included in all " &
         "copies or substantial portions of the Software." & vbCrLf & vbCrLf &
         "THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, " &
         "INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR " &
         "PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE " &
         "FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, " &
         "ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE." & vbCrLf & vbCrLf &
         "------------------------------------------------------------" & vbCrLf &
         "Third-Party Libraries" & vbCrLf &
         "------------------------------------------------------------" & vbCrLf &
         "This software uses the BASS audio library" & vbCrLf &
         "(c) 1999–2025 Un4seen Developments Ltd." & vbCrLf &
         "www.un4seen.com" & vbCrLf & vbCrLf &
         "BASS is a proprietary library used under its free license for " &
         "non-commercial applications. It is not covered by the MIT License. " &
         "If you modify or redistribute this application, you must ensure your " &
         "use of BASS complies with its license terms from Un4seen Developments." & vbCrLf &
         "For details, visit: https://www.un4seen.com/",
         "License Information",
         MessageBoxButtons.OK,
         MessageBoxIcon.Information
     )

            End If
        Catch ex As Exception
            MessageBox.Show("License information unavailable", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCredits_Click(sender As Object, e As EventArgs) Handles btnCredits.Click
        Dim credits As String = "PLAYTRAY CREDITS" & vbCrLf &
                               "================" & vbCrLf & vbCrLf &
                               "Development:" & vbCrLf &
                               "• Main Developer: Nikos Georgousis" & vbCrLf & vbCrLf &
                               "Third-Party Libraries:" & vbCrLf &
                               "• BASS Audio Library by Un4seen Developments" & vbCrLf &
                               "  https://www.un4seen.com/" & vbCrLf & vbCrLf &
                               "Icons and Visuals:" & vbCrLf &
"• Program icon inspired by the Sony Xplod™ subwoofer design (photo reference)" & vbCrLf &
"  Sony and Xplod are trademarks of Sony Corporation." & vbCrLf &
                               "• System icons from Windows" & vbCrLf & vbCrLf &
                               "This program is a remake of XtreMP3 music player"

        MessageBox.Show(credits, "Credits", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCopyInfo_Click(sender As Object, e As EventArgs) Handles btnCopyInfo.Click
        Try
            ' Copy system info to clipboard for bug reports
            Dim info As New System.Text.StringBuilder()
            info.AppendLine("=== PLAYTRAY SYSTEM INFORMATION ===")
            info.AppendLine("")
            info.AppendLine("App Version: " & APP_VERSION)
            info.AppendLine(txtSystemInfo.Text)
            info.AppendLine("")
            info.AppendLine("Settings:")
            info.AppendLine("• Stations: " & If(g_StationManager?.Stations.Count, 0).ToString())
            info.AppendLine("• Server Enabled: " & If(g_SettingsManager?.EnableServer, False).ToString())
            info.AppendLine("• Server Port: " & If(g_SettingsManager?.ServerPort, 0).ToString())
            info.AppendLine("• Hotkey Modifier: " & If(g_SettingsManager?.HotkeyModifier, 0).ToString())

            Clipboard.SetText(info.ToString())
            MessageBox.Show("System information copied to clipboard!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Could not copy to clipboard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnCheckUpdates.Click

    End Sub

    Private Sub lnkBass_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkBass.LinkClicked


        Try
            ' Replace with your actual website
            Process.Start("https://www.un4seen.com/")
        Catch ex As Exception
            MessageBox.Show("Could not open website", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lblCredits0_Click(sender As Object, e As EventArgs) Handles lblCredits0.Click

    End Sub

    Private Sub lnkDiscogs_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkDiscogs.LinkClicked
        Try
            ' Replace with your actual website
            Process.Start("https://www.discogs.com/")
        Catch ex As Exception
            MessageBox.Show("Could not open website", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lblDescription_Click(sender As Object, e As EventArgs) Handles lblDescription.Click

    End Sub
End Class
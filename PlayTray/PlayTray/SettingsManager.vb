Imports System.IO

Public Class SettingsManager
    Private _filePath As String
    Private _settings As Dictionary(Of String, Dictionary(Of String, String))

    ' General Settings
    Public Property AutoPlay As Boolean = False
    Public Property LastStation As String = ""
    Public Property ShowSplash As Boolean = True
    Public Property ShowTips As Boolean = True
    Public Property ExtractTitle As Boolean = True
    Public Property SaveTrackHistory As Boolean = False
    Public Property CheckForUpdates As Boolean = False

    ' Hotkeys
    Public Property HotkeyModifier As Integer = 1  ' 0=None, 1=CTRL+SHIFT, 2=CTRL+ALT, 3=CTRL+ALT+SHIFT
    Public Property MultimediaKeys As Boolean = True

    ' Network
    Public Property EnableServer As Boolean = False
    Public Property ServerPort As Integer = 8999
    Public Property ServerBindAddress As String = ""

    ' Broadcast
    Public Property BroadcastEnabled As Boolean = True
    Public Property BroadcastDebug As Boolean = False

    ' Mute
    Public Property TimedMuteSeconds As Integer = 60
    Public Property MuteLevel As Integer = 20  ' 0-100, 0=complete mute

    ' Auto Close
    Public Property AutoCloseEnabled As Boolean = False
    Public Property InactivityMinutes As Integer = 30

    ' Logging
    Public Property LogLevel As Integer = 1  ' 0=None, 1=Errors, 2=Info, 3=Debug

    ' Volume
    Public Property LastVolume As Integer = 75

    Public Property LastPlaylistFolder As String = ""

    ' Discogs
    Public Property DiscogsToken As String = "QgOopqODvzsCHYFcFDUNJuMlnRlupRCsLCgzaJUz"

    ' MusixMatch Lyrics
    Public Property MusixMatchToken As String = ""  ' User must provide their own

    Public Sub New(filePath As String)
        _filePath = filePath
        _settings = New Dictionary(Of String, Dictionary(Of String, String))()
    End Sub

    Public Function LoadSettings() As Boolean
        Try
            If Not File.Exists(_filePath) Then
                CreateDefaultSettings()
                Return True
            End If

            ParseIniFile()
            ApplyLoadedSettings()

            Return True

        Catch ex As Exception
            MessageBox.Show("Error loading settings: " & ex.Message, "Error")
            Return False
        End Try
    End Function

    Public Function SaveSettings() As Boolean
        Try
            UpdateSettingsDictionary()
            WriteIniFile()
            Return True

        Catch ex As Exception
            MessageBox.Show("Error saving settings: " & ex.Message, "Error")
            Return False
        End Try
    End Function

    Private Sub ParseIniFile()
        _settings.Clear()

        Dim lines() As String = File.ReadAllLines(_filePath)
        Dim currentSection As String = ""

        For Each line In lines
            Dim trimmedLine As String = line.Trim()

            ' Skip empty lines and comments
            If String.IsNullOrEmpty(trimmedLine) OrElse trimmedLine.StartsWith(";") Then
                Continue For
            End If

            ' Section header
            If trimmedLine.StartsWith("[") AndAlso trimmedLine.EndsWith("]") Then
                currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2)
                If Not _settings.ContainsKey(currentSection) Then
                    _settings.Add(currentSection, New Dictionary(Of String, String)())
                End If
                Continue For
            End If

            ' Key=Value pair
            Dim equalsIndex As Integer = trimmedLine.IndexOf("=")
            If equalsIndex > 0 AndAlso Not String.IsNullOrEmpty(currentSection) Then
                Dim key As String = trimmedLine.Substring(0, equalsIndex).Trim()
                Dim value As String = trimmedLine.Substring(equalsIndex + 1).Trim()

                If _settings.ContainsKey(currentSection) Then
                    If _settings(currentSection).ContainsKey(key) Then
                        _settings(currentSection)(key) = value
                    Else
                        _settings(currentSection).Add(key, value)
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub ApplyLoadedSettings()
        ' General
        AutoPlay = GetBoolValue("General", "AutoPlay", False)
        LastStation = GetStringValue("General", "LastStation", "")
        ShowSplash = GetBoolValue("General", "ShowSplash", True)
        ShowTips = GetBoolValue("General", "ShowTips", True)
        ExtractTitle = GetBoolValue("General", "ExtractTitle", True)
        SaveTrackHistory = GetBoolValue("General", "SaveTrackHistory", False)
        CheckForUpdates = GetBoolValue("General", "CheckForUpdates", False)

        ' Hotkeys
        HotkeyModifier = GetIntValue("Hotkeys", "Modifier", 1)
        MultimediaKeys = GetBoolValue("Hotkeys", "MultimediaKeys", True)

        ' Network
        EnableServer = GetBoolValue("Network", "EnableServer", False)
        ServerPort = GetIntValue("Network", "ServerPort", 8999)
        ServerBindAddress = GetStringValue("Network", "ServerBindAddress", "localhost")



        ' Migrate old localhost settings to empty (auto-detect)
        If ServerBindAddress = "localhost" OrElse ServerBindAddress = "127.0.0.1" Then
            ServerBindAddress = ""
        End If

        ' Broadcast
        BroadcastEnabled = GetBoolValue("Broadcast", "Enabled", True)
        BroadcastDebug = GetBoolValue("Broadcast", "Debug", False)

        ' Mute
        TimedMuteSeconds = GetIntValue("Mute", "TimedMuteSeconds", 60)
        MuteLevel = GetIntValue("Mute", "MuteLevel", 20)

        ' AutoClose
        AutoCloseEnabled = GetBoolValue("AutoClose", "Enabled", False)
        InactivityMinutes = GetIntValue("AutoClose", "InactivityMinutes", 30)

        ' Logging
        LogLevel = GetIntValue("Logging", "LogLevel", 1)

        ' Volume
        LastVolume = GetIntValue("Volume", "LastVolume", 75)

        ' Playlist
        LastPlaylistFolder = GetStringValue("Playlist", "LastPlaylistFolder", "")

        ' Discogs
        DiscogsToken = GetStringValue("Discogs", "DiscogsToken", "QgOopqODvzsCHYFcFDUNJuMlnRlupRCsLCgzaJUz")

        ' MusixMatch
        MusixMatchToken = GetStringValue("MusixMatch", "Token", "")
    End Sub

    Private Sub UpdateSettingsDictionary()
        _settings.Clear()

        ' General section
        Dim generalSection As New Dictionary(Of String, String)()
        generalSection.Add("AutoPlay", AutoPlay.ToString())
        generalSection.Add("LastStation", LastStation)
        generalSection.Add("ShowSplash", ShowSplash.ToString())
        generalSection.Add("ShowTips", ShowTips.ToString())
        generalSection.Add("ExtractTitle", ExtractTitle.ToString())
        generalSection.Add("SaveTrackHistory", SaveTrackHistory.ToString())
        generalSection.Add("CheckForUpdates", CheckForUpdates.ToString())
        _settings.Add("General", generalSection)

        ' Hotkeys section
        Dim hotkeysSection As New Dictionary(Of String, String)()
        hotkeysSection.Add("Modifier", HotkeyModifier.ToString())
        hotkeysSection.Add("MultimediaKeys", If(MultimediaKeys, "1", "0"))
        _settings.Add("Hotkeys", hotkeysSection)

        ' Network section
        Dim networkSection As New Dictionary(Of String, String)()
        networkSection.Add("EnableServer", EnableServer.ToString())
        networkSection.Add("ServerPort", ServerPort.ToString())
        networkSection.Add("ServerBindAddress", ServerBindAddress)
        _settings.Add("Network", networkSection)


        ' Broadcast section
        Dim broadcastSection As New Dictionary(Of String, String)()
        broadcastSection.Add("Enabled", BroadcastEnabled.ToString())
        broadcastSection.Add("Debug", BroadcastDebug.ToString())
        _settings.Add("Broadcast", broadcastSection)

        ' Mute section
        Dim muteSection As New Dictionary(Of String, String)()
        muteSection.Add("TimedMuteSeconds", TimedMuteSeconds.ToString())
        muteSection.Add("MuteLevel", MuteLevel.ToString())
        _settings.Add("Mute", muteSection)

        ' AutoClose section
        Dim autoCloseSection As New Dictionary(Of String, String)()
        autoCloseSection.Add("Enabled", AutoCloseEnabled.ToString())
        autoCloseSection.Add("InactivityMinutes", InactivityMinutes.ToString())
        _settings.Add("AutoClose", autoCloseSection)

        ' Logging section
        Dim loggingSection As New Dictionary(Of String, String)()
        loggingSection.Add("LogLevel", LogLevel.ToString())
        _settings.Add("Logging", loggingSection)

        ' Volume section
        Dim volumeSection As New Dictionary(Of String, String)()
        volumeSection.Add("LastVolume", LastVolume.ToString())
        _settings.Add("Volume", volumeSection)

        ' Playlist section
        Dim playlistSection As New Dictionary(Of String, String)()
        playlistSection.Add("LastPlaylistFolder", LastPlaylistFolder)
        _settings.Add("Playlist", playlistSection)


        ' Discogs section
        Dim discogsSection As New Dictionary(Of String, String)()
        discogsSection.Add("DiscogsToken", DiscogsToken)
        _settings.Add("Discogs", discogsSection)

        ' MusixMatch section
        Dim musixMatchSection As New Dictionary(Of String, String)()
        musixMatchSection.Add("Token", MusixMatchToken)
        _settings.Add("MusixMatch", musixMatchSection)

    End Sub

    Private Sub WriteIniFile()
        Dim lines As New List(Of String)()

        For Each section In _settings.Keys
            lines.Add("[" & section & "]")

            For Each key In _settings(section).Keys
                lines.Add(key & "=" & _settings(section)(key))
            Next

            lines.Add("")  ' Blank line between sections
        Next

        File.WriteAllLines(_filePath, lines.ToArray())
    End Sub

    Private Sub CreateDefaultSettings()
        ' Set default values
        AutoPlay = False
        ShowSplash = True
        ShowTips = True
        LastVolume = 75

        ' Save defaults
        SaveSettings()
    End Sub

    ' Helper methods
    Private Function GetStringValue(section As String, key As String, defaultValue As String) As String
        If _settings.ContainsKey(section) Then
            If _settings(section).ContainsKey(key) Then
                Return _settings(section)(key)
            End If
        End If
        Return defaultValue
    End Function

    Private Function GetIntValue(section As String, key As String, defaultValue As Integer) As Integer
        Dim strValue As String = GetStringValue(section, key, defaultValue.ToString())
        Dim result As Integer
        If Integer.TryParse(strValue, result) Then
            Return result
        End If
        Return defaultValue
    End Function

    Private Function GetBoolValue(section As String, key As String, defaultValue As Boolean) As Boolean
        Dim strValue As String = GetStringValue(section, key, defaultValue.ToString())

        ' Handle various boolean representations
        If strValue.Equals("1") OrElse strValue.Equals("True", StringComparison.OrdinalIgnoreCase) Then
            Return True
        ElseIf strValue.Equals("0") OrElse strValue.Equals("False", StringComparison.OrdinalIgnoreCase) Then
            Return False
        End If

        Return defaultValue
    End Function
End Class
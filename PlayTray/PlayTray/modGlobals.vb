Imports Un4seen.Bass

Module modGlobals
    ' Application Info
    Public Const APP_NAME As String = "Play Tray"
    Public APP_VERSION As String = "1.0.0.0 - dummy"

    ' BASS Audio
    Public g_StreamHandle As Integer = 0
    Public g_CurrentStation As Station = Nothing
    Public g_IsPlaying As Boolean = False
    Public g_CurrentVolume As Integer = 75

    ' Managers - RENAMED to avoid conflicts
    Public g_StationManager As StationManager = Nothing
    Public g_SettingsManager As SettingsManager = Nothing
    Public g_HotkeyManager As HotkeyManager = Nothing
    Public g_CurrentTrackInfo As String = ""
    Public g_TrackHistory As TrackHistory = Nothing

    Public g_HTTPServer As HTTPServer = Nothing
    Public g_BroadcastService As BroadcastService = Nothing

    ' Paths
    Public ReadOnly Property AppPath As String
        Get
            Return Application.StartupPath
        End Get
    End Property

    Public ReadOnly Property SettingsFile As String
        Get
            Return IO.Path.Combine(AppPath, "PlayTray_settings.ini")
        End Get
    End Property

    Public ReadOnly Property StationsFile As String
        Get
            Return IO.Path.Combine(AppPath, "Stations.json")
        End Get
    End Property

    Public ReadOnly Property LogFile As String
        Get
            Return IO.Path.Combine(AppPath, "PlayTray_log.txt")
        End Get
    End Property

    ' Playback folder structure
    Public ReadOnly Property PlaybackFolder As String
        Get
            Return IO.Path.Combine(AppPath, "Playback")
        End Get
    End Property

    Public ReadOnly Property CoversFolder As String
        Get
            Return IO.Path.Combine(PlaybackFolder, "Covers")
        End Get
    End Property

    Public ReadOnly Property TrackHistoryFile As String
        Get
            Return IO.Path.Combine(PlaybackFolder, "track_history.txt")
        End Get
    End Property

    Public ReadOnly Property BelovedFile As String
        Get
            Return IO.Path.Combine(PlaybackFolder, "Beloved.txt")
        End Get
    End Property

    Public ReadOnly Property SelectedTracksHtml As String
        Get
            Return IO.Path.Combine(PlaybackFolder, "Selected_Tracks.html")
        End Get
    End Property

    ' Helper method to ensure folders exist
    Public Sub EnsurePlaybackFoldersExist()
        Try
            If Not IO.Directory.Exists(PlaybackFolder) Then
                IO.Directory.CreateDirectory(PlaybackFolder)
            End If
            If Not IO.Directory.Exists(CoversFolder) Then
                IO.Directory.CreateDirectory(CoversFolder)
            End If
        Catch ex As Exception
            ' Silent fail - folders will be created on demand
        End Try
    End Sub
End Module

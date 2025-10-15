Imports Microsoft.Win32

Public Class RegistryExporter
    ' Registry paths
    Private Const LEGACY_BASE_PATH As String = "Software\VB and VBA Program Settings\XtreMP3\LCDSmartieIntergration"
    Private Const NEW_BASE_PATH As String = "Software\PlayTray"

    ' Status codes for LCD Smartie
    Public Enum PlayerStatus
        NotRunning = 0
        Stopped = 1
        Connecting = 3
        Playing = 2
        ErrorState = 10
    End Enum

    Public Shared Sub UpdateRegistry(status As PlayerStatus, stationName As String, trackInfo As String, volume As Integer)
        Try
            ' Update legacy registry location (XtreMP3 format)
            UpdateLegacyRegistry(status, stationName, trackInfo)

            ' Update new registry location (PlayTray format)
            UpdateNewRegistry(status, stationName, trackInfo, volume)

        Catch ex As Exception
            ' Silent fail - don't interrupt playback for registry errors
        End Try
    End Sub

    Private Shared Sub UpdateLegacyRegistry(status As PlayerStatus, stationName As String, trackInfo As String)
        Try
            Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(LEGACY_BASE_PATH)
                If key IsNot Nothing Then
                    ' RawStatus: Text representation
                    key.SetValue("RawStatus", GetStatusText(status), RegistryValueKind.String)

                    ' Station: Station name (even when stopped)
                    key.SetValue("Station", If(String.IsNullOrEmpty(stationName), "", stationName), RegistryValueKind.String)

                    ' Status: Numeric code
                    key.SetValue("Status", CInt(status), RegistryValueKind.DWord)

                    ' Status: Numeric as string
                    key.SetValue("Status", If(String.IsNullOrEmpty(status), "", CInt(status)), RegistryValueKind.String)

                    ' Title: Track info (legacy XtreMP3 key name)
                    key.SetValue("Title", If(String.IsNullOrEmpty(trackInfo), "", trackInfo), RegistryValueKind.String)
                End If
            End Using

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Shared Sub UpdateNewRegistry(status As PlayerStatus, stationName As String, trackInfo As String, volume As Integer)
        Try
            Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(NEW_BASE_PATH)
                If key IsNot Nothing Then
                    ' PlayingState: Text representation
                    key.SetValue("PlayingState", GetStatusText(status), RegistryValueKind.String)

                    ' StationName: Current station
                    key.SetValue("StationName", If(String.IsNullOrEmpty(stationName), "", stationName), RegistryValueKind.String)

                    ' TrackInfo: Current track
                    key.SetValue("TrackInfo", If(String.IsNullOrEmpty(trackInfo), "", trackInfo), RegistryValueKind.String)

                    ' Volume: 0-100
                    key.SetValue("Volume", volume, RegistryValueKind.DWord)

                    ' StatusCode: Numeric representation
                    key.SetValue("StatusCode", CInt(status), RegistryValueKind.DWord)
                End If
            End Using

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Shared Function GetStatusText(status As PlayerStatus) As String
        Select Case status
            Case PlayerStatus.NotRunning
                Return "Not Running"
            Case PlayerStatus.Stopped
                Return "Stopped"
            Case PlayerStatus.Connecting
                Return "Connecting"
            Case PlayerStatus.Playing
                Return "Playing"
            Case PlayerStatus.ErrorState
                Return "Error"
            Case Else
                Return "Unknown"
        End Select
    End Function

    Public Shared Sub ClearRegistry()
        ' Clear registry on app exit
        Try
            UpdateRegistry(PlayerStatus.NotRunning, "", "", 0)
        Catch
            ' Silent fail
        End Try
    End Sub
End Class

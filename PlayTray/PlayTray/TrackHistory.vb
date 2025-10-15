Imports System.IO

Public Class TrackHistory
    Private historyFilePath As String
    Private belovedFilePath As String
    Private lastStation As String = ""
    Private lastDate As String = ""

    Public Sub New(historyFile As String, belovedFile As String)
        historyFilePath = historyFile
        belovedFilePath = belovedFile
    End Sub

    Public Sub LogTrack(stationName As String, trackInfo As String)
        Try
            If String.IsNullOrWhiteSpace(trackInfo) Then Return
            If trackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then Return

            Dim now As DateTime = DateTime.Now
            Dim currentDate As String = now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim currentTime As String = now.ToString("HH:mm:ss")

            ' Check if we need date header
            Dim needsDateHeader As Boolean = (lastDate <> now.ToString("yyyy-MM-dd"))

            ' Check if we need station header
            Dim needsStationHeader As Boolean = (lastStation <> stationName) OrElse needsDateHeader

            ' Prepare log entry
            Dim logLines As New List(Of String)()

            ' Add date header if needed
            If needsDateHeader Then
                If File.Exists(historyFilePath) Then
                    logLines.Add("") ' Blank line before new date
                End If
                logLines.Add("======" & currentDate & "======")
                lastDate = now.ToString("yyyy-MM-dd")
            End If

            ' Add station header if needed
            If needsStationHeader Then
                logLines.Add("=======" & stationName & "=======")
                lastStation = stationName
            End If

            ' Add track entry
            logLines.Add(currentTime & vbTab & trackInfo)

            ' Write to file
            File.AppendAllLines(historyFilePath, logLines)

        Catch ex As Exception
            ' Silent fail - don't interrupt playback
        End Try
    End Sub

    Public Sub SaveBeloved(stationName As String, trackInfo As String)
        Try
            If String.IsNullOrWhiteSpace(trackInfo) Then
                ' No track playing, inform user
                MessageBox.Show("No track is currently playing.", "Save Beloved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If trackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("Still connecting to stream.", "Save Beloved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim now As DateTime = DateTime.Now
            Dim timestamp As String = now.ToString("yyyy-MM-dd HH:mm:ss")

            ' Format: DateTime | Station | Track
            Dim logEntry As String = timestamp & vbTab & stationName & vbTab & trackInfo

            ' Append to beloved file
            File.AppendAllText(belovedFilePath, logEntry & Environment.NewLine)

            ' Show confirmation (brief)
            MessageBox.Show("Saved to Beloved!" & vbCrLf & trackInfo, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error saving beloved track: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ResetStationTracking()
        ' Reset tracking when manually changing stations
        lastStation = ""
    End Sub
End Class

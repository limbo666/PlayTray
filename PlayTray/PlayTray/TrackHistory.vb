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

    ''' <summary>
    ''' Save track to beloved list. Returns result for UI handling.
    ''' </summary>
    Public Function SaveBeloved(stationName As String, trackInfo As String) As BelovedSaveResult
        Try
            ' Validate track info
            If String.IsNullOrWhiteSpace(trackInfo) Then
                Return New BelovedSaveResult With {
                .Success = False,
                .Message = "No track is currently playing"
            }
            End If

            If trackInfo.Equals("Connecting...", StringComparison.OrdinalIgnoreCase) Then
                Return New BelovedSaveResult With {
                .Success = False,
                .Message = "Still connecting to stream"
            }
            End If

            ' Save to file
            Dim now As DateTime = DateTime.Now
            Dim timestamp As String = now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim logEntry As String = timestamp & vbTab & stationName & vbTab & trackInfo

            File.AppendAllText(belovedFilePath, logEntry & Environment.NewLine)

            ' Return success
            Return New BelovedSaveResult With {
            .Success = True,
            .Message = trackInfo,
            .stationName = stationName
        }

        Catch ex As Exception
            Return New BelovedSaveResult With {
            .Success = False,
            .Message = "Error: " & ex.Message
        }
        End Try
    End Function
    Public Sub ResetStationTracking()
        ' Reset tracking when manually changing stations
        lastStation = ""
    End Sub
End Class


''' <summary>
''' Result object for beloved save operation
''' </summary>
Public Class BelovedSaveResult
    Public Property Success As Boolean
    Public Property Message As String
    Public Property StationName As String
End Class

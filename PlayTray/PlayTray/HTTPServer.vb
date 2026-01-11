Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.IO

Public Class HTTPServer
    Private listener As HttpListener
    Private listenerThread As Thread
    Private _isRunning As Boolean = False
    Private port As Integer

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return _isRunning
        End Get
    End Property

    Public Sub New(serverPort As Integer)
        port = serverPort
    End Sub

    Public Function StartServer(bindAddress As String) As Boolean
        Try
            If _isRunning Then Return True

            listener = New HttpListener()

            ' PROFESSIONAL FIX:
            ' We use the "Strong Wildcard" (+). 
            ' This tells Windows: "Traffic on Port 8999 for ANY IP address on this machine."
            ' This requires the 'netsh' reservation: netsh http add urlacl url=http://+:8999/ user=Everyone
            listener.Prefixes.Add($"http://+:{port}/")

            listener.Start()
            _isRunning = True

            ' Start background thread
            listenerThread = New Thread(AddressOf ProcessRequests)
            listenerThread.IsBackground = True
            listenerThread.Start()

            Return True

        Catch ex As HttpListenerException
            _isRunning = False
            ' Error 5 = Access Denied (User needs to run the netsh script)
            If ex.ErrorCode = 5 Then
                Throw New Exception("Access Denied. Please run the Network Setup script as Administrator.")
            Else
                Throw New Exception($"Server Error {ex.ErrorCode}: {ex.Message}")
            End If
        Catch ex As Exception
            _isRunning = False
            Throw
        End Try
    End Function

    Public Sub StopServer()
        Try
            _isRunning = False
            If listener IsNot Nothing Then
                listener.Close() ' Use Close, not Stop, to fully release the port
                listener = Nothing
            End If
            If listenerThread IsNot Nothing Then
                If listenerThread.IsAlive Then listenerThread.Abort()
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub ProcessRequests()
        While _isRunning AndAlso listener IsNot Nothing AndAlso listener.IsListening
            Try
                Dim context = listener.GetContext()
                ThreadPool.QueueUserWorkItem(AddressOf HandleRequest, context)
            Catch ex As Exception
                ' Listener was stopped
                Return
            End Try
        End While
    End Sub

    Private Sub HandleRequest(state As Object)
        Dim context As HttpListenerContext = DirectCast(state, HttpListenerContext)
        Dim request As HttpListenerRequest = context.Request
        Dim response As HttpListenerResponse = context.Response

        Try
            ' Parse command from URL path
            Dim path As String = request.Url.AbsolutePath.ToLower().Trim("/")

            ' Route to appropriate handler
            Dim jsonResponse As String = ""

            Select Case path
                Case "status"
                    jsonResponse = GetStatus()
                Case "play"
                    jsonResponse = DoPlay()
                Case "stop"
                    jsonResponse = DoStop()
                Case "volume"
                    jsonResponse = GetVolume()
                Case "setvolume"
                    Dim vol As String = request.QueryString("level")
                    Dim volInt As Integer
                    If Integer.TryParse(vol, volInt) Then
                        jsonResponse = SetVolume(volInt)
                    Else
                        jsonResponse = CreateErrorResponse("Invalid volume")
                    End If
                Case "station"
                    jsonResponse = GetCurrentStation()
                Case "playstation"
                    Dim pos As String = request.QueryString("position")
                    jsonResponse = PlayStation(pos)
                Case "liststations"
                    jsonResponse = ListStations()
                Case "tips"
                    jsonResponse = ShowTips()
                Case "beloved"
                    jsonResponse = SaveBeloved()
                Case "mute"
                    jsonResponse = DoMute()
                Case "unmute"
                    jsonResponse = DoUnmute()
                Case "next"
                    jsonResponse = DoNext()
                Case "previous"
                    jsonResponse = DoPrevious()
                Case ""
                    jsonResponse = GetApiInfo()
                Case Else
                    jsonResponse = CreateErrorResponse("Unknown command: " & path)
            End Select

            Dim buffer() As Byte = Encoding.UTF8.GetBytes(jsonResponse)
            response.ContentType = "application/json"
            response.ContentLength64 = buffer.Length
            response.OutputStream.Write(buffer, 0, buffer.Length)
            response.OutputStream.Close()

        Catch ex As Exception
            Try
                Dim errorJson As String = CreateErrorResponse(ex.Message)
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(errorJson)
                response.ContentType = "application/json"
                response.ContentLength64 = buffer.Length
                response.OutputStream.Write(buffer, 0, buffer.Length)
                response.OutputStream.Close()
            Catch
                ' Silent fail
            End Try
        End Try
    End Sub

    ' --- API HANDLERS ---

    Private Function GetStatus() As String
        Dim status As String = If(g_IsPlaying, "playing", "stopped")
        Dim stationName As String = If(g_CurrentStation IsNot Nothing, g_CurrentStation.Name, "")
        Dim trackInfo As String = g_CurrentTrackInfo

        Return CreateJsonResponse(True, "Status retrieved", New Dictionary(Of String, Object) From {
            {"status", status},
            {"station", stationName},
            {"track", trackInfo},
            {"volume", g_CurrentVolume}
        })
    End Function

    Private Function DoPlay() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerPlay()
                            End Sub)
            Return CreateJsonResponse(True, "Playback started")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function DoStop() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerStop()
                            End Sub)
            Return CreateJsonResponse(True, "Playback stopped")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function GetVolume() As String
        Return CreateJsonResponse(True, "Volume retrieved", New Dictionary(Of String, Object) From {
            {"volume", g_CurrentVolume}
        })
    End Function

    Private Function SetVolume(level As Integer) As String
        If level < 0 OrElse level > 100 Then Return CreateErrorResponse("Volume must be 0-100")
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.ApplyVolume(level)
                            End Sub)
            Return CreateJsonResponse(True, $"Volume set to {level}")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function GetCurrentStation() As String
        If g_CurrentStation IsNot Nothing Then
            Return CreateJsonResponse(True, "Current station", New Dictionary(Of String, Object) From {
                {"position", g_CurrentStation.Position},
                {"name", g_CurrentStation.Name},
                {"link", g_CurrentStation.Link}
            })
        End If
        Return CreateJsonResponse(True, "No station selected")
    End Function

    Private Function PlayStation(positionStr As String) As String
        Dim position As Integer
        If Integer.TryParse(positionStr, position) Then
            If g_StationManager IsNot Nothing Then
                Dim station = g_StationManager.GetStation(position)
                If station IsNot Nothing Then
                    Dim mainForm As frmMain = GetMainForm()
                    If mainForm IsNot Nothing Then
                        mainForm.Invoke(Sub() mainForm.PlayStationByPosition(position))
                        Return CreateJsonResponse(True, "Playing station: " & station.Name)
                    End If
                End If
            End If
        End If
        Return CreateErrorResponse("Invalid position")
    End Function

    Private Function ListStations() As String
        If g_StationManager IsNot Nothing Then
            Dim stationList As New List(Of Dictionary(Of String, Object))
            For Each station In g_StationManager.Stations
                stationList.Add(New Dictionary(Of String, Object) From {
                    {"position", station.Position},
                    {"name", station.Name},
                    {"link", station.Link}
                })
            Next
            Return CreateJsonResponse(True, "Station list", New Dictionary(Of String, Object) From {
                {"stations", stationList},
                {"count", stationList.Count}
            })
        End If
        Return CreateErrorResponse("Station manager not available")
    End Function

    Private Function ShowTips() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.ShowTipsFromHotkey()
                            End Sub)
            Return CreateJsonResponse(True, "Tips window shown")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function SaveBeloved() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerSaveBeloved()
                            End Sub)
            Return CreateJsonResponse(True, "Track saved")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function DoMute() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerTimedMute()
                            End Sub)
            Return CreateJsonResponse(True, "Mute started")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function DoUnmute() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerUnmute()
                            End Sub)
            Return CreateJsonResponse(True, "Volume restored")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function DoNext() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerNextStation()
                            End Sub)
            Return CreateJsonResponse(True, "Next station")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function DoPrevious() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerPreviousStation()
                            End Sub)
            Return CreateJsonResponse(True, "Previous station")
        End If
        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function GetApiInfo() As String
        Return CreateJsonResponse(True, "PlayTray API Ready", New Dictionary(Of String, Object) From {
            {"endpoints", New String() {
                "/status", "/play", "/stop", "/volume", "/setvolume", "/station", "/playstation", "/liststations", "/next", "/previous", "/tips", "/beloved", "/mute", "/unmute"
            }}
        })
    End Function

    ' --- HELPERS ---

    Private Function GetMainForm() As frmMain
        Try
            Return DirectCast(Application.OpenForms("frmMain"), frmMain)
        Catch
            Return Nothing
        End Try
    End Function

    Private Function CreateJsonResponse(success As Boolean, message As String, Optional data As Dictionary(Of String, Object) = Nothing) As String
        Dim sb As New StringBuilder()
        sb.Append("{")
        sb.Append($"""success"":{success.ToString().ToLower()},")
        sb.Append($"""message"":""{EscapeJson(message)}""")

        If data IsNot Nothing Then
            For Each kvp In data
                sb.Append(",")
                sb.Append($"""{kvp.Key}"":")
                sb.Append(FormatJsonValue(kvp.Value))
            Next
        End If

        sb.Append("}")
        Return sb.ToString()
    End Function

    Private Function CreateErrorResponse(errorMessage As String) As String
        Return CreateJsonResponse(False, errorMessage)
    End Function

    Private Function FormatJsonValue(value As Object) As String
        If value Is Nothing Then
            Return "null"
        ElseIf TypeOf value Is String Then
            Return $"""{EscapeJson(value.ToString())}"""
        ElseIf TypeOf value Is Boolean Then
            Return value.ToString().ToLower()
        ElseIf TypeOf value Is Integer OrElse TypeOf value Is Double Then
            Return value.ToString()
        ElseIf TypeOf value Is List(Of Dictionary(Of String, Object)) Then
            Dim list = DirectCast(value, List(Of Dictionary(Of String, Object)))
            Dim items As New List(Of String)
            For Each dict In list
                items.Add(DictToJson(dict))
            Next
            Return "[" & String.Join(",", items) & "]"
        ElseIf TypeOf value Is String() Then
            Dim arr = DirectCast(value, String())
            Dim items As New List(Of String)
            For Each item In arr
                items.Add($"""{EscapeJson(item)}""")
            Next
            Return "[" & String.Join(",", items) & "]"
        Else
            Return $"""{EscapeJson(value.ToString())}"""
        End If
    End Function

    Private Function DictToJson(dict As Dictionary(Of String, Object)) As String
        Dim items As New List(Of String)
        For Each kvp In dict
            items.Add($"""{kvp.Key}"":{FormatJsonValue(kvp.Value)}")
        Next
        Return "{" & String.Join(",", items) & "}"
    End Function

    Private Function EscapeJson(str As String) As String
        If String.IsNullOrEmpty(str) Then Return ""
        Return str.Replace("\", "\\").Replace("""", "\""").Replace(vbCrLf, "\n").Replace(vbCr, "\n").Replace(vbLf, "\n")
    End Function
End Class
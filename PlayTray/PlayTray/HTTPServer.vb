'curl http : //localhost:8999/status
'curl http : //localhost:8999/play
'curl http : //localhost:8999/setvolume?level=75
'curl http : //localhost:8999/playstation?position=1
'http: //localhost:8999/            gives the commands supported

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



    Private _isNetworkBound As Boolean = False  ' ADD THIS LINE

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return _isRunning
        End Get
    End Property


    Public ReadOnly Property IsNetworkAccessible As Boolean  ' ADD THIS PROPERTY
        Get
            Return _isNetworkBound
        End Get
    End Property

    Public Sub New(serverPort As Integer)
        port = serverPort
    End Sub

    Public Function StartServer(bindAddress As String) As Boolean
        Try
            If _isRunning Then
                Return True
            End If

            ' Validate bind address
            If String.IsNullOrWhiteSpace(bindAddress) Then
                Throw New ArgumentException("Bind address cannot be empty")
            End If

            ' Create listener
            listener = New HttpListener()

            ' ALWAYS bind to localhost and 127.0.0.1
            listener.Prefixes.Add($"http://localhost:{port}/")
            listener.Prefixes.Add($"http://127.0.0.1:{port}/")

            ' Also bind to selected network interface (if not localhost)
            If bindAddress <> "localhost" AndAlso bindAddress <> "127.0.0.1" AndAlso bindAddress <> "*" Then
                Try
                    ' Validate it's a proper IPv4 address
                    Dim testIP As IPAddress = Nothing
                    If IPAddress.TryParse(bindAddress, testIP) AndAlso
                   testIP.AddressFamily = AddressFamily.InterNetwork Then
                        listener.Prefixes.Add($"http://{bindAddress}:{port}/")
                        _isNetworkBound = True
                    Else
                        ' Invalid IP format, skip network binding
                        _isNetworkBound = False
                    End If
                Catch ex As Exception
                    ' Failed to bind to network address, continue with localhost only
                    _isNetworkBound = False
                End Try
            ElseIf bindAddress = "*" Then
                ' Wildcard binding
                Try
                    listener.Prefixes.Clear() ' Remove localhost bindings
                    listener.Prefixes.Add($"http://*:{port}/")
                    _isNetworkBound = True
                Catch ex As Exception
                    ' Wildcard failed, fall back to localhost
                    listener.Prefixes.Clear()
                    listener.Prefixes.Add($"http://localhost:{port}/")
                    listener.Prefixes.Add($"http://127.0.0.1:{port}/")
                    _isNetworkBound = False
                End Try
            End If

            ' Start listening
            Try
                listener.Start()
                _isRunning = True

                ' Start processing thread
                listenerThread = New Thread(AddressOf ProcessRequests)
                listenerThread.IsBackground = True
                listenerThread.Start()

                Return True

            Catch ex As Exception
                _isRunning = False
                _isNetworkBound = False
                Throw New Exception($"Failed to start server on port {port}. Error: {ex.Message}")
            End Try

        Catch ex As Exception
            _isRunning = False
            _isNetworkBound = False
            Throw
        End Try
    End Function


    Public Function GetBoundAddresses() As List(Of String)
        Dim addresses As New List(Of String)()

        Try
            If listener IsNot Nothing AndAlso listener.IsListening Then
                For Each prefix In listener.Prefixes
                    addresses.Add(prefix)
                Next
            End If
        Catch
            ' Return empty list on error
        End Try

        Return addresses
    End Function

    Public Sub StopServer()
        Try
            _isRunning = False

            If listener IsNot Nothing Then
                listener.Stop()
                listener.Close()
            End If

            If listenerThread IsNot Nothing Then
                listenerThread.Abort()
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub ProcessRequests()
        While _isRunning
            Try
                Dim context As HttpListenerContext = listener.GetContext()
                ThreadPool.QueueUserWorkItem(AddressOf HandleRequest, context)
            Catch ex As Exception
                ' Listener stopped or error
                If Not _isRunning Then
                    Exit While
                End If
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
                    ' frmMain.StartSoftFlash()
                    jsonResponse = GetStatus()

                Case "play"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = DoPlay()

                Case "stop"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = DoStop()

                Case "volume"
                    '   frmMain.StartSoftFlash()
                    jsonResponse = GetVolume()

                Case "setvolume"
                    '   frmMain.StartSoftFlash()
                    Dim vol As String = request.QueryString("level")
                    jsonResponse = SetVolume(vol)

                Case "station"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = GetCurrentStation()

                Case "playstation"
                    '  frmMain.StartSoftFlash()
                    Dim pos As String = request.QueryString("position")
                    jsonResponse = PlayStation(pos)

                Case "liststations"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = ListStations()

                Case "tips"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = ShowTips()

                Case "beloved"
                    ' frmMain.StartSoftFlash()
                    jsonResponse = SaveBeloved()


                Case "mute"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = DoMute()

                Case "unmute"
                    'frmMain.StartSoftFlash()
                    jsonResponse = DoUnmute()

                Case "next"
                    '  frmMain.StartSoftFlash()
                    jsonResponse = DoNext()

                Case "previous"
                    ' frmMain.StartSoftFlash()
                    jsonResponse = DoPrevious()

                Case ""
                    ' Root - show API info
                    jsonResponse = GetApiInfo()

                Case Else
                    jsonResponse = CreateErrorResponse("Unknown command: " & path)
            End Select

            ' Send response
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

    'Private Function DoNext() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerNextStation())
    '        Return CreateJsonResponse(True, "Playing next station")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function

    'Private Function DoPrevious() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerPreviousStation())
    '        Return CreateJsonResponse(True, "Playing previous station")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function


    Private Function DoNext() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerNextStation()
                            End Sub)
            Return CreateJsonResponse(True, "Playing next station")
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
            Return CreateJsonResponse(True, "Playing previous station")
        End If

        Return CreateErrorResponse("Main form not found")
    End Function

    'Private Function DoMute() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerTimedMute())
    '        Return CreateJsonResponse(True, "Timed mute activated")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function


    Private Function DoMute() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerTimedMute()
                            End Sub)
            Return CreateJsonResponse(True, "Timed mute started")
        End If

        Return CreateErrorResponse("Main form not found")
    End Function

    'Private Function DoUnmute() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerUnmute())
    '        Return CreateJsonResponse(True, "Volume restored")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function

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

    ' API Command Handlers

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

    'Private Function DoPlay() As String
    '    If g_IsPlaying Then
    '        Return CreateJsonResponse(True, "Already playing")
    '    End If

    '    ' Trigger play on main form
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerPlay())
    '        Return CreateJsonResponse(True, "Playback started")
    '    Else
    '        Return CreateErrorResponse("Main form not found")
    '    End If
    'End Function

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

    'Private Function DoStop() As String
    '    If Not g_IsPlaying Then
    '        Return CreateJsonResponse(True, "Already stopped")
    '    End If

    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerStop())
    '        Return CreateJsonResponse(True, "Playback stopped")
    '    Else
    '        Return CreateErrorResponse("Main form not found")
    '    End If
    'End Function


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

    'Private Function SetVolume(volumeStr As String) As String
    '    Dim volume As Integer
    '    If Integer.TryParse(volumeStr, volume) Then
    '        If volume < 0 Then volume = 0
    '        If volume > 100 Then volume = 100

    '        g_CurrentVolume = volume

    '        Dim mainForm As frmMain = GetMainForm()
    '        If mainForm IsNot Nothing Then
    '            mainForm.Invoke(Sub() mainForm.ApplyVolume(volume))
    '        End If

    '        Return CreateJsonResponse(True, "Volume set to " & volume, New Dictionary(Of String, Object) From {
    '            {"volume", volume}
    '        })
    '    Else
    '        Return CreateErrorResponse("Invalid volume value. Must be 0-100.")
    '    End If
    'End Function


    Private Function SetVolume(level As Integer) As String
        If level < 0 OrElse level > 100 Then
            Return CreateErrorResponse("Volume must be between 0 and 100")
        End If

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
        Else
            Return CreateJsonResponse(True, "No station selected")
        End If
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
                Else
                    Return CreateErrorResponse("Station not found at position " & position)
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

    'Private Function ShowTips() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.ShowTipsFromHotkey())
    '        Return CreateJsonResponse(True, "Tips displayed")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function


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

    'Private Function SaveBeloved() As String
    '    Dim mainForm As frmMain = GetMainForm()
    '    If mainForm IsNot Nothing Then
    '        mainForm.Invoke(Sub() mainForm.TriggerSaveBeloved())
    '        Return CreateJsonResponse(True, "Beloved track saved")
    '    End If

    '    Return CreateErrorResponse("Main form not found")
    'End Function

    Private Function SaveBeloved() As String
        Dim mainForm As frmMain = GetMainForm()
        If mainForm IsNot Nothing Then
            mainForm.Invoke(Sub()
                                mainForm.StartSoftFlash()
                                mainForm.TriggerSaveBeloved()
                            End Sub)
            Return CreateJsonResponse(True, "Track saved to beloved list")
        End If

        Return CreateErrorResponse("Main form not found")
    End Function

    Private Function GetApiInfo() As String
        Return CreateJsonResponse(True, "PlayTray API Server", New Dictionary(Of String, Object) From {
            {"version", "1.0"},
            {"endpoints", New String() {
                "/status - Get current playback status",
                "/play - Start playback",
                "/stop - Stop playback",
                "/volume - Get current volume",
                "/setvolume?level=50 - Set volume (0-100)",
                "/station - Get current station info",
                "/playstation?position=1 - Play station by position (1-10)",
                "/liststations - List all favorite stations",
                 "/next - Play next favorite station",
    "/previous - Play previous favorite station",
                "/tips - Show tips window",
                "/beloved - Save current track to beloved",
                    "/mute - Start timed mute",
    "/unmute - Restore volume"
            }}
        })
    End Function

    ' Helper Functions

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
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Public Class BroadcastService
    Private Const BROADCAST_PORT As Integer = 18999
    Private Const BROADCAST_INTERVAL As Integer = 5000 ' 5 seconds

    Private udpClient As UdpClient
    Private broadcastTimer As Timer
    Private isRunning As Boolean = False
    Private debugEnabled As Boolean = False
    Private logFilePath As String = ""

    Public Sub New(enableDebug As Boolean, logPath As String)
        debugEnabled = enableDebug
        logFilePath = logPath
    End Sub

    Public Function StartBroadcast() As Boolean
        Try
            If isRunning Then
                Return True
            End If

            ' Create UDP client for broadcasting
            udpClient = New UdpClient()
            udpClient.EnableBroadcast = True

            ' Start broadcast timer
            broadcastTimer = New Timer()
            broadcastTimer.Interval = BROADCAST_INTERVAL
            AddHandler broadcastTimer.Tick, AddressOf BroadcastTimer_Tick
            broadcastTimer.Start()

            isRunning = True

            ' Send first broadcast immediately
            SendBroadcast()

            LogDebug("Broadcast service started")

            Return True

        Catch ex As Exception
            LogDebug($"Failed to start broadcast: {ex.Message}")
            Return False
        End Try
    End Function

    Public Sub StopBroadcast()
        Try
            isRunning = False

            If broadcastTimer IsNot Nothing Then
                broadcastTimer.Stop()
                broadcastTimer.Dispose()
                broadcastTimer = Nothing
            End If

            If udpClient IsNot Nothing Then
                udpClient.Close()
                udpClient = Nothing
            End If

            LogDebug("Broadcast service stopped")

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub BroadcastTimer_Tick(sender As Object, e As EventArgs)
        SendBroadcast()
    End Sub

    Private Sub SendBroadcast()
        Try
            ' Build JSON payload
            Dim payload As String = BuildPayload()

            If String.IsNullOrEmpty(payload) Then
                Return
            End If

            ' Convert to bytes
            Dim data As Byte() = Encoding.UTF8.GetBytes(payload)

            ' Broadcast to all interfaces
            Dim endpoint As New IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT)
            udpClient.Send(data, data.Length, endpoint)

            LogDebug($"Broadcast sent: {payload}")

        Catch ex As Exception
            LogDebug($"Broadcast error: {ex.Message}")
        End Try
    End Sub

    Private Function BuildPayload() As String
        Try
            ' Get current status
            Dim status As String = If(g_IsPlaying, "playing", "stopped")
            Dim stationName As String = If(g_CurrentStation IsNot Nothing, g_CurrentStation.Name, "")
            Dim trackInfo As String = g_CurrentTrackInfo

            ' Get computer name
            Dim hostname As String = Environment.MachineName

            ' Get local IP addresses
            Dim localIP As String = GetLocalIPAddress()

            ' Get HTTP port
            Dim httpPort As Integer = 8999
            If g_SettingsManager IsNot Nothing Then
                httpPort = g_SettingsManager.ServerPort
            End If

            ' Build JSON manually (simple, no external library needed)
            Dim json As New StringBuilder()
            json.Append("{")
            json.Append($"""service"":""PlayTray"",")
            json.Append($"""version"":""{APP_VERSION}"",")
            json.Append($"""hostname"":""{EscapeJson(hostname)}"",")
            json.Append($"""ip"":""{localIP}"",")
            json.Append($"""port"":{httpPort},")
            json.Append($"""status"":""{status}"",")
            json.Append($"""station"":""{EscapeJson(stationName)}"",")
            json.Append($"""track"":""{EscapeJson(trackInfo)}"",")
            json.Append($"""volume"":{g_CurrentVolume},")
            json.Append($"""timestamp"":{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}")
            json.Append("}")

            Return json.ToString()

        Catch ex As Exception
            LogDebug($"Payload build error: {ex.Message}")
            Return ""
        End Try
    End Function

    Private Function GetLocalIPAddress() As String
        Try
            ' Get the IP address the HTTP server is bound to
            If g_SettingsManager IsNot Nothing Then
                Dim bindAddress As String = g_SettingsManager.ServerBindAddress

                ' If it's a specific IP address (not wildcard), use it
                If Not String.IsNullOrWhiteSpace(bindAddress) AndAlso
               bindAddress <> "*" AndAlso
               bindAddress <> "localhost" AndAlso
               bindAddress <> "127.0.0.1" Then
                    Return bindAddress
                End If
            End If

            ' If wildcard or not set, get first non-loopback IP
            Dim host = Dns.GetHostEntry(Dns.GetHostName())

            For Each ip In host.AddressList
                If ip.AddressFamily = AddressFamily.InterNetwork Then
                    ' Skip loopback
                    If Not ip.ToString().StartsWith("127.") Then
                        Return ip.ToString()
                    End If
                End If
            Next

            Return "127.0.0.1" ' Fallback

        Catch ex As Exception
            Return "127.0.0.1"
        End Try
    End Function

    Private Function EscapeJson(str As String) As String
        If String.IsNullOrEmpty(str) Then Return ""

        Return str.Replace("\", "\\") _
                  .Replace("""", "\""") _
                  .Replace(vbCrLf, "\n") _
                  .Replace(vbCr, "\n") _
                  .Replace(vbLf, "\n") _
                  .Replace(vbTab, "\t")
    End Function

    Private Sub LogDebug(message As String)
        If Not debugEnabled Then Return

        Try
            Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim logMessage As String = $"[{timestamp}] {message}"

            File.AppendAllText(logFilePath, logMessage & Environment.NewLine)

        Catch ex As Exception
            ' Silent fail - don't interrupt broadcasting
        End Try
    End Sub
End Class
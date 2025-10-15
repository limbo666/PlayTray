Imports System.IO
Imports System.Text

Public Class SelectedTracksDocument
    Private filePath As String

    Public Sub New(documentPath As String)
        filePath = documentPath

        ' Create initial HTML structure if file doesn't exist
        If Not File.Exists(filePath) Then
            CreateNewDocument()
        End If
    End Sub

    Private Sub CreateNewDocument()
        Dim html As New StringBuilder()
        html.AppendLine("<!DOCTYPE html>")
        html.AppendLine("<html>")
        html.AppendLine("<head>")
        html.AppendLine("    <meta charset='UTF-8'>")
        html.AppendLine("    <title>Selected Tracks</title>")
        html.AppendLine("    <style>")
        html.AppendLine("        body { font-family: 'Segoe UI', Arial, sans-serif; padding: 20px; background: #f5f5f5; }")
        html.AppendLine("        .track-entry { background: white; margin: 20px 0; padding: 20px; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }")
        html.AppendLine("        .album-art { width: 300px; height: 300px; object-fit: cover; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.2); }")
        html.AppendLine("        .track-info { margin-top: 15px; font-size: 16px; color: #333; font-weight: bold; }")
        html.AppendLine("        .timestamp { font-size: 12px; color: #999; margin-top: 5px; }")
        html.AppendLine("        h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }")
        html.AppendLine("    </style>")
        html.AppendLine("</head>")
        html.AppendLine("<body>")
        html.AppendLine("    <h1>🎵 Selected Tracks</h1>")
        html.AppendLine("    <div id='tracks'>")
        html.AppendLine("    <!-- Tracks will be appended here -->")
        html.AppendLine("    </div>")
        html.AppendLine("</body>")
        html.AppendLine("</html>")

        File.WriteAllText(filePath, html.ToString())
    End Sub

    Public Sub AppendTrack(imagePath As String, trackInfo As String)
        Try
            If Not File.Exists(filePath) Then
                CreateNewDocument()
            End If

            ' Read existing HTML
            Dim html As String = File.ReadAllText(filePath)

            ' Build new track entry with relative path
            Dim timestamp As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            Dim trackEntry As New StringBuilder()

            ' Convert absolute path to relative path
            ' HTML is in Playback folder, images are in Playback/Covers folder
            Dim relativePath As String = GetRelativeImagePath(imagePath)

            trackEntry.AppendLine("        <div class='track-entry'>")
            trackEntry.AppendLine($"            <img class='album-art' src='{relativePath}' alt='Album Art'>")
            trackEntry.AppendLine($"            <div class='track-info'>{EscapeHtml(trackInfo)}</div>")
            trackEntry.AppendLine($"            <div class='timestamp'>Added: {timestamp}</div>")
            trackEntry.AppendLine("        </div>")

            ' Insert before closing tags
            Dim insertPoint As Integer = html.LastIndexOf("</div>")
            If insertPoint > 0 Then
                html = html.Insert(insertPoint, trackEntry.ToString())
            End If

            ' Save updated HTML
            File.WriteAllText(filePath, html)

        Catch ex As Exception
            Throw New Exception($"Failed to append track: {ex.Message}")
        End Try
    End Sub

    Public Sub OpenDocument()
        Try
            If File.Exists(filePath) Then
                Process.Start(filePath)
            End If
        Catch ex As Exception
            Throw New Exception($"Failed to open document: {ex.Message}")
        End Try
    End Sub

    Private Function EscapeHtml(text As String) As String
        Return text.Replace("&", "&amp;") _
                   .Replace("<", "&lt;") _
                   .Replace(">", "&gt;") _
                   .Replace("""", "&quot;") _
                   .Replace("'", "&#39;")
    End Function

    ''' <summary>
    ''' Converts absolute image path to relative path for HTML
    ''' HTML is in Playback folder, images are in Playback/Covers folder
    ''' </summary>
    Private Function GetRelativeImagePath(absolutePath As String) As String
        Try
            ' Get filename from absolute path
            Dim filename As String = IO.Path.GetFileName(absolutePath)

            ' Return relative path from HTML location to Covers folder
            ' HTML is in Playback/Selected_Tracks.html
            ' Images are in Playback/Covers/filename.jpg
            ' So relative path is: Covers/filename.jpg
            Return $"Covers/{filename}"

        Catch ex As Exception
            ' Fallback to filename only
            Return IO.Path.GetFileName(absolutePath)
        End Try
    End Function
End Class
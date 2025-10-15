Imports System.IO
Imports System.Text.RegularExpressions

Public Class PlaylistParser
    Public Shared Function GetFirstUrl(filePath As String) As String
        Try
            If Not File.Exists(filePath) Then
                Return Nothing
            End If

            Dim extension As String = Path.GetExtension(filePath).ToLower()

            Select Case extension
                Case ".m3u", ".m3u8"
                    Return ParseM3U(filePath)
                Case ".pls"
                    Return ParsePLS(filePath)
                Case Else
                    Return Nothing
            End Select

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function ParseM3U(filePath As String) As String
        Try
            Dim lines() As String = File.ReadAllLines(filePath)

            For Each line In lines
                Dim trimmedLine As String = line.Trim()

                ' Skip empty lines and comments
                If String.IsNullOrEmpty(trimmedLine) Then Continue For
                If trimmedLine.StartsWith("#") Then Continue For

                ' Check if it's a valid URL
                If trimmedLine.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse
                   trimmedLine.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                    Return trimmedLine
                End If
            Next

        Catch ex As Exception
            ' Return nothing on error
        End Try

        Return Nothing
    End Function

    Private Shared Function ParsePLS(filePath As String) As String
        Try
            Dim lines() As String = File.ReadAllLines(filePath)

            For Each line In lines
                Dim trimmedLine As String = line.Trim()

                ' Look for File1=URL pattern (first entry)
                If trimmedLine.StartsWith("File1=", StringComparison.OrdinalIgnoreCase) Then
                    Dim url As String = trimmedLine.Substring(6).Trim()

                    ' Validate URL
                    If url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse
                       url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                        Return url
                    End If
                End If
            Next

        Catch ex As Exception
            ' Return nothing on error
        End Try

        Return Nothing
    End Function
End Class
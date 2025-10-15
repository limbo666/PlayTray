Imports System.Net
Imports System.Text.RegularExpressions

''' <summary>
''' Service to fetch lyrics from MusixMatch API
''' Free tier: 2000 calls/day, returns 30% of lyrics
''' </summary>
Public Class LyricsService
    Private Const BASE_URL As String = "https://api.musixmatch.com/ws/1.1/"
    Private apiKey As String

    Public Sub New(musixMatchApiKey As String)
        apiKey = musixMatchApiKey
    End Sub

    ''' <summary>
    ''' Search for track and get lyrics
    ''' Returns lyrics text or error message
    ''' </summary>
    Public Function GetLyrics(trackName As String, artistName As String) As LyricsResult
        Try
            ' Validate API key
            If String.IsNullOrWhiteSpace(apiKey) Then
                Return New LyricsResult With {
                    .Success = False,
                    .ErrorMessage = "No MusixMatch API key configured. Please add your key in Settings → Advanced."
                }
            End If

            ' Step 1: Search for track
            Dim trackId As Integer = SearchTrack(trackName, artistName)

            If trackId = 0 Then
                Return New LyricsResult With {
                    .Success = False,
                    .ErrorMessage = "Track not found on MusixMatch. Try searching manually or check spelling."
                }
            End If

            ' Step 2: Get lyrics by track ID
            Dim lyrics As String = GetLyricsByTrackId(trackId)

            If String.IsNullOrEmpty(lyrics) Then
                Return New LyricsResult With {
                    .Success = False,
                    .ErrorMessage = "Lyrics not available for this track."
                }
            End If

            ' Success
            Return New LyricsResult With {
                .Success = True,
                .Lyrics = lyrics,
                .TrackId = trackId
            }

        Catch ex As WebException
            ' Network or API error
            Return New LyricsResult With {
                .Success = False,
                .ErrorMessage = "Network error: " & ex.Message
            }

        Catch ex As Exception
            ' General error
            Return New LyricsResult With {
                .Success = False,
                .ErrorMessage = "Error: " & ex.Message
            }
        End Try
    End Function

    ''' <summary>
    ''' Search for track and return track_id
    ''' </summary>
    Private Function SearchTrack(trackName As String, artistName As String) As Integer
        Try
            Using client As New WebClient()
                client.Encoding = System.Text.Encoding.UTF8

                ' Build search query
                Dim query As String = If(String.IsNullOrEmpty(artistName),
                    Uri.EscapeDataString(trackName),
                    Uri.EscapeDataString(trackName & " " & artistName))

                Dim url As String = $"{BASE_URL}track.search?q={query}&apikey={apiKey}"

                ' Make request
                Dim response As String = client.DownloadString(url)

                ' Parse JSON response (simple regex, no JSON library needed)
                Dim trackIdMatch = Regex.Match(response, """track_id""\s*:\s*(\d+)")

                If trackIdMatch.Success Then
                    Return Integer.Parse(trackIdMatch.Groups(1).Value)
                End If

                Return 0

            End Using

        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Get lyrics text by track_id
    ''' </summary>
    Private Function GetLyricsByTrackId(trackId As Integer) As String
        Try
            Using client As New WebClient()
                client.Encoding = System.Text.Encoding.UTF8

                Dim url As String = $"{BASE_URL}track.lyrics.get?track_id={trackId}&apikey={apiKey}"

                ' Make request
                Dim response As String = client.DownloadString(url)

                ' Parse lyrics body from JSON
                Dim lyricsMatch = Regex.Match(response, """lyrics_body""\s*:\s*""([^""]+)""")

                If lyricsMatch.Success Then
                    Dim lyrics As String = lyricsMatch.Groups(1).Value

                    ' Unescape JSON string
                    lyrics = lyrics.Replace("\n", Environment.NewLine)
                    lyrics = lyrics.Replace("\r", "")
                    lyrics = lyrics.Replace("\/", "/")
                    lyrics = System.Text.RegularExpressions.Regex.Unescape(lyrics)

                    Return lyrics.Trim()
                End If

                Return ""

            End Using

        Catch ex As Exception
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Parse track name to extract artist and title
    ''' Assumes format: "Artist - Track" or "Track"
    ''' </summary>
    Public Shared Sub ParseTrackInfo(trackInfo As String, ByRef artist As String, ByRef track As String)
        Try
            If String.IsNullOrWhiteSpace(trackInfo) Then
                artist = ""
                track = ""
                Return
            End If

            ' Check if contains " - " separator
            Dim separatorIndex As Integer = trackInfo.IndexOf(" - ")

            If separatorIndex > 0 Then
                ' Format: "Artist - Track"
                artist = trackInfo.Substring(0, separatorIndex).Trim()
                track = trackInfo.Substring(separatorIndex + 3).Trim()
            Else
                ' No separator, treat as track name only
                artist = ""
                track = trackInfo.Trim()
            End If

        Catch ex As Exception
            artist = ""
            track = trackInfo
        End Try
    End Sub
End Class

''' <summary>
''' Result object for lyrics fetch operation
''' </summary>
Public Class LyricsResult
    Public Property Success As Boolean
    Public Property Lyrics As String
    Public Property ErrorMessage As String
    Public Property TrackId As Integer
End Class
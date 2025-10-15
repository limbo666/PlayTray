Imports System.IO

Public Class frmLyrics
    Private currentTrackInfo As String = ""
    Private currentLyrics As String = ""

    Private Sub frmLyrics_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form setup
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(500, 400)
        Me.Size = New Size(600, 500)
        Me.Text = "Track Lyrics"

        ' Style controls
        txtLyrics.Multiline = True
        txtLyrics.ScrollBars = ScrollBars.Vertical
        txtLyrics.ReadOnly = True
        txtLyrics.Font = New Font("Segoe UI", 10)
        txtLyrics.BackColor = Color.White

        lblTrackTitle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblTrackTitle.Text = "No track selected"

        lblResult.Font = New Font("Segoe UI", 8)
        lblResult.Text = ""
        lblResult.ForeColor = Color.Gray

        btnFetchLyrics.Text = "Fetch Lyrics"
        btnSaveLyrics.Text = "Save Lyrics"
        btnSaveLyrics.Enabled = False
        btnClose.Text = "Close"
    End Sub

    ''' <summary>
    ''' Show form with track info from main form
    ''' </summary>
    Public Sub ShowLyrics(trackInfo As String)
        currentTrackInfo = trackInfo
        lblTrackTitle.Text = trackInfo

        ' Clear previous lyrics
        txtLyrics.Clear()
        currentLyrics = ""
        btnSaveLyrics.Enabled = False
        lblResult.Text = "Click 'Fetch Lyrics' to load lyrics from MusixMatch"
        lblResult.ForeColor = Color.Gray

        Me.Show()
    End Sub

    Private Sub btnFetchLyrics_Click(sender As Object, e As EventArgs) Handles btnFetchLyrics.Click
        Try
            ' Validate track info
            If String.IsNullOrWhiteSpace(currentTrackInfo) Then
                lblResult.Text = "No track information available"
                lblResult.ForeColor = Color.Red
                Return
            End If

            ' Check for API key
            If g_SettingsManager Is Nothing OrElse String.IsNullOrWhiteSpace(g_SettingsManager.MusixMatchToken) Then
                lblResult.Text = "MusixMatch API key not configured. Go to Settings → Advanced"
                lblResult.ForeColor = Color.Red

                Dim msgResult = MessageBox.Show(
                    "MusixMatch API key is required to fetch lyrics." & vbCrLf & vbCrLf &
                    "Get your FREE API key at:" & vbCrLf &
                    "https://developer.musixmatch.com" & vbCrLf & vbCrLf &
                    "Then add it in Settings → Advanced → MusixMatch Token" & vbCrLf & vbCrLf &
                    "Open MusixMatch website now?",
                    "API Key Required",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information)

                If msgResult = DialogResult.Yes Then
                    Process.Start("https://developer.musixmatch.com")
                End If

                Return
            End If

            ' Show loading
            lblResult.Text = "Fetching lyrics from MusixMatch..."
            lblResult.ForeColor = Color.Blue
            txtLyrics.Text = "Loading..."
            btnFetchLyrics.Enabled = False
            Application.DoEvents()

            ' Parse track info
            Dim artist As String = ""
            Dim track As String = ""
            lyricsService.ParseTrackInfo(currentTrackInfo, artist, track)

            ' Fetch lyrics - create service
            Dim lyricsService2 As New LyricsService(g_SettingsManager.MusixMatchToken)

            ' Fetch lyrics - call API
            Dim lyricsResult As LyricsResult = lyricsService2.GetLyrics(track, artist)

            ' Enable button
            btnFetchLyrics.Enabled = True
            If lyricsResult.Success Then
                ' Success - display lyrics
                currentLyrics = lyricsResult.Lyrics
                txtLyrics.Text = lyricsResult.Lyrics

                ' Add notice about free tier limitation
                lblResult.Text = "✓ Lyrics loaded (Free tier shows 30% of lyrics)"
                lblResult.ForeColor = Color.Green

                btnSaveLyrics.Enabled = True
            Else
                ' Error
                txtLyrics.Text = "Lyrics not available"
                lblResult.Text = "✗ " & lyricsResult.ErrorMessage
                lblResult.ForeColor = Color.Red
                btnSaveLyrics.Enabled = False
            End If

        Catch ex As Exception
            btnFetchLyrics.Enabled = True
            lblResult.Text = "Error: " & ex.Message
            lblResult.ForeColor = Color.Red
            txtLyrics.Text = ""
        End Try
    End Sub

    Private Sub btnSaveLyrics_Click(sender As Object, e As EventArgs) Handles btnSaveLyrics.Click
        Try
            If String.IsNullOrEmpty(currentLyrics) Then
                MessageBox.Show("No lyrics to save", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Ensure Playback folder exists
            EnsurePlaybackFoldersExist()

            ' Create lyrics filename
            Dim lyricsFolder As String = IO.Path.Combine(PlaybackFolder, "Lyrics")
            If Not IO.Directory.Exists(lyricsFolder) Then
                IO.Directory.CreateDirectory(lyricsFolder)
            End If

            ' Sanitize filename
            Dim cleanFileName As String = currentTrackInfo
            For Each c In IO.Path.GetInvalidFileNameChars()
                cleanFileName = cleanFileName.Replace(c, "_")
            Next

            Dim filePath As String = IO.Path.Combine(lyricsFolder, cleanFileName & ".txt")

            ' Save lyrics
            File.WriteAllText(filePath, "Track: " & currentTrackInfo & vbCrLf &
                             "Fetched: " & DateTime.Now.ToString() & vbCrLf &
                             "Source: MusixMatch" & vbCrLf &
                             vbCrLf &
                             currentLyrics)

            MessageBox.Show("Lyrics saved to:" & vbCrLf & filePath, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error saving lyrics: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' ESC key to close
    Private Sub frmLyrics_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnFetchLyrics.Click

    End Sub

    Private Sub lblResult_Click(sender As Object, e As EventArgs) Handles lblResult.Click

    End Sub
End Class
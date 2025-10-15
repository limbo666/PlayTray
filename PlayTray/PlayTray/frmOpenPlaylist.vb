Imports System.IO

Imports System.Linq
Public Class frmOpenPlaylist

    Private Sub frmOpenPlaylist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set initial folder from settings
        If g_SettingsManager IsNot Nothing AndAlso Not String.IsNullOrEmpty(g_SettingsManager.LastPlaylistFolder) Then
            If Directory.Exists(g_SettingsManager.LastPlaylistFolder) Then
                txtFilePath.Text = g_SettingsManager.LastPlaylistFolder
            End If
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Try
            Using ofd As New OpenFileDialog()
                ' Set up filters
                ofd.Filter = "Playlist Files|*.pls;*.m3u;*.m3u8|PLS Files (*.pls)|*.pls|M3U Files (*.m3u;*.m3u8)|*.m3u;*.m3u8|All Files (*.*)|*.*"
                ofd.FilterIndex = 1
                ofd.Title = "Open Playlist File"

                ' Set initial directory from settings or last selected
                If Not String.IsNullOrEmpty(txtFilePath.Text) AndAlso Directory.Exists(Path.GetDirectoryName(txtFilePath.Text)) Then
                    ofd.InitialDirectory = Path.GetDirectoryName(txtFilePath.Text)
                ElseIf g_SettingsManager IsNot Nothing AndAlso Not String.IsNullOrEmpty(g_SettingsManager.LastPlaylistFolder) Then
                    If Directory.Exists(g_SettingsManager.LastPlaylistFolder) Then
                        ofd.InitialDirectory = g_SettingsManager.LastPlaylistFolder
                    End If
                End If

                If ofd.ShowDialog() = DialogResult.OK Then
                    txtFilePath.Text = ofd.FileName

                    ' Save folder location to settings
                    If g_SettingsManager IsNot Nothing Then
                        g_SettingsManager.LastPlaylistFolder = Path.GetDirectoryName(ofd.FileName)
                        g_SettingsManager.SaveSettings()
                    End If
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error browsing for file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        Try
            ' Validate file path
            If String.IsNullOrWhiteSpace(txtFilePath.Text) Then
                MessageBox.Show("Please select a playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If Not File.Exists(txtFilePath.Text) Then
                MessageBox.Show("File not found: " & txtFilePath.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Parse playlist and get first URL
            Dim url As String = PlaylistParser.GetFirstUrl(txtFilePath.Text)

            If String.IsNullOrEmpty(url) Then
                MessageBox.Show("Could not find any valid stream URL in the playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Show the extracted URL
            lblExtractedURL.Text = url
            lblExtractedURL.Visible = True

            ' Save to recent URLs
            AddToRecentUrls(url)

            ' Create temporary station
            Dim fileName As String = Path.GetFileNameWithoutExtension(txtFilePath.Text)
            Dim tempStation As New Station()
            tempStation.Name = fileName
            tempStation.Link = url
            tempStation.Position = 0

            ' Play the stream
            Dim mainForm As frmMain = DirectCast(Application.OpenForms("frmMain"), frmMain)
            If mainForm IsNot Nothing Then
                g_CurrentStation = tempStation
                mainForm.PlayStreamFromLink(url, fileName)
            End If

            ' Show success message
            MessageBox.Show("Playing first stream from playlist:" & vbCrLf & url, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Close form
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error playing playlist: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub AddToRecentUrls(url As String)
        Try
            Dim recentFilePath As String = IO.Path.Combine(AppPath, "recent_urls.txt")

            ' Read existing URLs
            Dim urls As New List(Of String)()
            If File.Exists(recentFilePath) Then
                urls.AddRange(File.ReadAllLines(recentFilePath))
            End If

            ' Remove if already exists
            If urls.Contains(url) Then
                urls.Remove(url)
            End If

            ' Add to end (most recent)
            urls.Add(url)

            ' Keep only last 10
            If urls.Count > 10 Then
                urls = urls.Skip(urls.Count - 10).ToList()
            End If

            ' Save
            File.WriteAllLines(recentFilePath, urls.ToArray())

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub


End Class
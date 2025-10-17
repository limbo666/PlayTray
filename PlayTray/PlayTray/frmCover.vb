Imports System.IO

Public Class frmCover
    Private currentTrackTitle As String = ""
    Private currentImagePath As String = ""

    Private Sub frmCover_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form setup
        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ShowInTaskbar = False

        ' Set size (adjust as needed)
        '  Me.Size = New Size(500, 600)

        ' Create rounded corners
        CreateRoundedCorners()

        ' Set background color (same as tips form)
        Me.BackColor = Color.FromArgb(45, 45, 48) ' Dark gray

        ' Style picture box
        picCover.BackColor = Color.Transparent
        picCover.SizeMode = PictureBoxSizeMode.Zoom

        ' Style label
        LblTitle.ForeColor = Color.White
        LblTitle.BackColor = Color.Transparent
        LblTitle.Font = New Font("Segoe UI", 8, FontStyle.Regular)
        LblTitle.TextAlign = ContentAlignment.MiddleCenter
        LblTitle.Cursor = Cursors.Hand

        ' Style button
        btnClose.BackColor = Color.FromArgb(60, 60, 63)
        btnClose.ForeColor = Color.White
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.FlatAppearance.BorderSize = 0
        btnClose.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btnClose.Cursor = Cursors.Hand


        btnSaveToDocument.BackColor = Color.FromArgb(60, 60, 63)
        btnSaveToDocument.ForeColor = Color.White
        btnSaveToDocument.FlatStyle = FlatStyle.Flat
        btnSaveToDocument.FlatAppearance.BorderSize = 0
        btnSaveToDocument.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btnSaveToDocument.Cursor = Cursors.Hand


        btnOpenDoc.BackColor = Color.FromArgb(60, 60, 63)
        btnOpenDoc.ForeColor = Color.White
        btnOpenDoc.FlatStyle = FlatStyle.Flat
        btnOpenDoc.FlatAppearance.BorderSize = 0
        btnOpenDoc.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btnOpenDoc.Cursor = Cursors.Hand


    End Sub

    Private Sub CreateRoundedCorners()
        Try
            Dim radius As Integer = 15
            Dim path As New System.Drawing.Drawing2D.GraphicsPath()
            path.AddArc(0, 0, radius, radius, 180, 90)
            path.AddArc(Me.Width - radius, 0, radius, radius, 270, 90)
            path.AddArc(Me.Width - radius, Me.Height - radius, radius, radius, 0, 90)
            path.AddArc(0, Me.Height - radius, radius, radius, 90, 90)
            path.CloseFigure()
            Me.Region = New Region(path)
        Catch ex As Exception
            ' If rounding fails, continue with square corners
        End Try
    End Sub

    Public Sub ShowCover(imagePath As String, trackTitle As String)
        '  MessageBox.Show("ShowCover called with: " & imagePath, "Debug 1")

        Try
            currentImagePath = imagePath
            currentTrackTitle = trackTitle

            '   MessageBox.Show("About to check file exists", "Debug 2")

            ' Load image
            If Not String.IsNullOrEmpty(imagePath) AndAlso File.Exists(imagePath) Then
                '        MessageBox.Show("File exists, loading image", "Debug 3")

                ' Dispose old image
                If picCover.Image IsNot Nothing Then
                    picCover.Image.Dispose()
                    picCover.Image = Nothing
                End If

                '       MessageBox.Show("About to load from stream", "Debug 4")

                ' Load new image (use stream to avoid file locking)
                Using fs As New FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                    picCover.Image = Image.FromStream(fs)
                End Using

                '      MessageBox.Show("Image loaded successfully", "Debug 5")
            Else
                '      MessageBox.Show("File does not exist or path is empty", "Debug - File Check Failed")
            End If

            ' Set title
            LblTitle.Text = trackTitle

            '  MessageBox.Show("About to call ShowDialog", "Debug 6")

            ' Show form
            Me.Show()

            '  MessageBox.Show("ShowDialog returned", "Debug 7")

        Catch ex As Exception
            MessageBox.Show("EXCEPTION in ShowCover: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles LblTitle.Click
        Try
            If Not String.IsNullOrEmpty(currentTrackTitle) Then
                Clipboard.SetText(currentTrackTitle)

                ' Show brief confirmation
                Dim originalText As String = LblTitle.Text
                LblTitle.Text = "✓ Copied to clipboard!"

                ' Reset after 1 second
                Dim tmr As New Timer()
                tmr.Interval = 1000
                AddHandler tmr.Tick, Sub()
                                         LblTitle.Text = originalText
                                         tmr.Stop()
                                         tmr.Dispose()
                                     End Sub
                tmr.Start()
            End If

        Catch ex As Exception
            MessageBox.Show("Could not copy to clipboard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub picCover_Click(sender As Object, e As EventArgs) Handles picCover.Click
        Try
            If String.IsNullOrEmpty(currentImagePath) OrElse Not File.Exists(currentImagePath) Then
                MessageBox.Show("Source image not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Using sfd As New SaveFileDialog()
                sfd.Filter = "JPEG Image|*.jpg|PNG Image|*.png|All Files|*.*"
                sfd.DefaultExt = "jpg"

                ' Clean filename
                Dim cleanTitle As String = currentTrackTitle
                For Each c In IO.Path.GetInvalidFileNameChars()
                    cleanTitle = cleanTitle.Replace(c, "-")
                Next
                sfd.FileName = cleanTitle

                If sfd.ShowDialog() = DialogResult.OK Then
                    ' Copy the original file
                    File.Copy(currentImagePath, sfd.FileName, True)

                    MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("Error saving image: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        ' Close on image click too
        Me.Close()
    End Sub

    Private Sub frmCover_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Dispose image to release file
        If picCover.Image IsNot Nothing Then
            picCover.Image.Dispose()
            picCover.Image = Nothing
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Draw border
        Using pen As New Pen(Color.FromArgb(100, 100, 100), 2)
            e.Graphics.DrawRectangle(pen, 1, 1, Me.Width - 3, Me.Height - 3)
        End Using
    End Sub

    ' Allow dragging form by clicking anywhere
    Private dragging As Boolean = False
    Private dragCursorPoint As Point
    Private dragFormPoint As Point

    Private Sub frmCover_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        dragging = True
        dragCursorPoint = Cursor.Position
        dragFormPoint = Me.Location
    End Sub

    Private Sub frmCover_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If dragging Then
            Dim diff As Point = Point.Subtract(Cursor.Position, New Size(dragCursorPoint))
            Me.Location = Point.Add(dragFormPoint, New Size(diff))
        End If
    End Sub

    Private Sub frmCover_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        dragging = False
    End Sub

    ' ESC key to close
    Private Sub frmCover_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnSaveToDocument_Click(sender As Object, e As EventArgs) Handles btnSaveToDocument.Click

        Try
            If String.IsNullOrEmpty(currentImagePath) OrElse Not File.Exists(currentImagePath) Then
                MessageBox.Show("No album art available to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Ensure Playback folders exist
            EnsurePlaybackFoldersExist()

            ' Use path from modGlobals
            Dim docPath As String = SelectedTracksHtml

            ' Create HTML document handler
            Dim htmlDoc As New SelectedTracksDocument(docPath)

            ' Append track
            htmlDoc.AppendTrack(currentImagePath, currentTrackTitle)

            ' Success - ask to open
            'Dim result = MessageBox.Show(
            '    "Track saved to Selected_Tracks.html!" & vbCrLf & vbCrLf &
            '    "Open document?",
            '    "Saved",
            '    MessageBoxButtons.YesNo,
            '    MessageBoxIcon.Information)

            'If result = DialogResult.Yes Then
            '    htmlDoc.OpenDocument()
            'End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnOpenDoc_Click(sender As Object, e As EventArgs) Handles btnOpenDoc.Click
        ' Ensure Playback folders exist
        EnsurePlaybackFoldersExist()

        ' Use path from modGlobals
        Dim docPath As String = SelectedTracksHtml

        ' Create HTML document handler
        Dim htmlDoc As New SelectedTracksDocument(docPath)

        htmlDoc.OpenDocument()

    End Sub
End Class
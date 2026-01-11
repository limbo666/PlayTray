
Imports System.IO
Public Class frmTips
    ' Animation variables
    Private targetY As Integer
    Private startY As Integer
    Private currentY As Integer
    Private animationSpeed As Integer = 20
    Private isAnimatingIn As Boolean = False
    Private isAnimatingOut As Boolean = False

    ' Display timer
    Private displayDuration As Integer = 4000 ' 4 seconds



    Public Sub SetImage(img As Image)
        Try
            If img Is Nothing Then
                ' Logic to CLEAR the image
                If picAlbumArt.Image IsNot Nothing Then
                    picAlbumArt.Image.Dispose()
                    picAlbumArt.Image = Nothing
                End If
                picAlbumArt.Visible = False
                picAlbumArt.Tag = Nothing ' Clear the stored path
                picAlbumArt.Cursor = Cursors.Default
            Else
                ' Logic to SET the image
                picAlbumArt.Image = img
                picAlbumArt.Visible = True
                picAlbumArt.Cursor = Cursors.Hand
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub
    Private Sub frmTips_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form setup
        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.Manual
        Me.TopMost = True
        Me.ShowInTaskbar = False

        ' Create rounded corners
        CreateRoundedCorners()

        ' Set background color
        Me.BackColor = Color.FromArgb(45, 45, 48) ' Dark gray

        ' Style labels
        lblStation.ForeColor = Color.White
        lblStation.BackColor = Color.Transparent

        lblTrack.ForeColor = Color.LightGray
        lblTrack.BackColor = Color.Transparent

        lblStatus.ForeColor = Color.Gray
        lblStatus.BackColor = Color.Transparent

        ' Hide album art by default
        picAlbumArt.Visible = False

        ' Position form off-screen (below)
        PositionFormOffScreen()
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

    Private Sub PositionFormOffScreen()
        ' Get screen working area (excludes taskbar)
        Dim screen As Screen = Screen.PrimaryScreen
        Dim workingArea As Rectangle = screen.WorkingArea

        ' Position in bottom-right corner
        Dim margin As Integer = 10
        Dim xPos As Integer = workingArea.Right - Me.Width - margin

        ' Start below the screen
        startY = workingArea.Bottom + Me.Height
        targetY = workingArea.Bottom - Me.Height - margin

        Me.Location = New Point(xPos, startY)
        currentY = startY
    End Sub

    Public Sub ShowTips(stationName As String, trackInfo As String, status As String, Optional albumArtPath As String = "")
        Try
            ' Update content
            lblStation.Text = stationName
            lblTrack.Text = trackInfo
            lblStatus.Text = status

            ' Handle album art
            LoadAlbumArt(albumArtPath)

            ' Show form if hidden
            If Not Me.Visible Then
                Me.Show()
            End If

            ' Start slide-in animation
            AnimateIn()

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub LoadAlbumArt(albumArtPath As String)
        Try
            ' Clear existing image
            If picAlbumArt.Image IsNot Nothing Then
                picAlbumArt.Image.Dispose()
                picAlbumArt.Image = Nothing
            End If

            ' Load new image if available
            If Not String.IsNullOrEmpty(albumArtPath) AndAlso IO.File.Exists(albumArtPath) Then
                ' Load image from file (use stream to avoid file locking)
                Using fs As New IO.FileStream(albumArtPath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                    picAlbumArt.Image = Image.FromStream(fs)
                End Using

                ' Store path in Tag for later use
                picAlbumArt.Tag = albumArtPath  ' CRITICAL: Make sure this line exists

                ' Change cursor to indicate clickable
                picAlbumArt.Cursor = Cursors.Hand

                picAlbumArt.Visible = True
            Else
                picAlbumArt.Tag = Nothing
                picAlbumArt.Cursor = Cursors.Default
                picAlbumArt.Visible = False
            End If

        Catch ex As Exception
            ' Hide picture box on error
            picAlbumArt.Visible = False
        End Try
    End Sub

    Public Sub UpdateAlbumArt(albumArtPath As String)
        Try
            ' Update album art while tips are showing
            LoadAlbumArt(albumArtPath)  ' This should set the Tag
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub AnimateIn()
        isAnimatingIn = True
        isAnimatingOut = False
        tmrAnimation.Enabled = True
    End Sub

    Private Sub AnimateOut()
        isAnimatingIn = False
        isAnimatingOut = True
        tmrAnimation.Enabled = True
    End Sub

    Private Sub tmrAnimation_Tick(sender As Object, e As EventArgs) Handles tmrAnimation.Tick
        If isAnimatingIn Then
            ' Slide up
            currentY -= animationSpeed

            If currentY <= targetY Then
                currentY = targetY
                Me.Location = New Point(Me.Location.X, currentY)
                tmrAnimation.Enabled = False
                isAnimatingIn = False

                ' Start display timer
                tmrDisplay.Interval = displayDuration
                tmrDisplay.Enabled = True
            Else
                Me.Location = New Point(Me.Location.X, currentY)
            End If

        ElseIf isAnimatingOut Then
            ' Slide down
            currentY += animationSpeed

            If currentY >= startY Then
                currentY = startY
                Me.Location = New Point(Me.Location.X, currentY)
                tmrAnimation.Enabled = False
                isAnimatingOut = False
                Me.Hide()
            Else
                Me.Location = New Point(Me.Location.X, currentY)
            End If
        End If
    End Sub

    Private Sub tmrDisplay_Tick(sender As Object, e As EventArgs) Handles tmrDisplay.Tick
        tmrDisplay.Enabled = False
        AnimateOut()
    End Sub

    Private Sub frmTips_Click(sender As Object, e As EventArgs) Handles Me.Click, lblStation.Click, lblTrack.Click, lblStatus.Click
        ' Dismiss on click (except album art)
        DismissTips()
    End Sub

    Private Sub picAlbumArt_Click(sender As Object, e As EventArgs) Handles picAlbumArt.Click
        ' Show full-size cover viewer
        ' MsgBox("Clicked")
        ShowCoverViewer()

    End Sub
    Public Sub ShowCoverViewer()
        Try
            ' Get current track info
            '   Dim trackTitle As String = lblStation.Text & " - " & lblTrack.Text

            Dim trackTitle As String = lblTrack.Text

            ' Debug: Check if image exists
            If picAlbumArt.Image Is Nothing Then
                '  MessageBox.Show("No image loaded in picAlbumArt", "Debug")
                Return
            End If

            ' Debug: Check if Tag is set
            If picAlbumArt.Tag Is Nothing Then
                '  MessageBox.Show("picAlbumArt.Tag is Nothing - path not stored!", "Debug")
                Return
            End If

            Dim imagePath As String = picAlbumArt.Tag.ToString()

            ' Debug: Check path
            '  MessageBox.Show("Image path: " & imagePath, "Debug")

            If String.IsNullOrEmpty(imagePath) Then
                MessageBox.Show("Image path is empty", "Debug")
                Return
            End If

            If Not IO.File.Exists(imagePath) Then
                '    MessageBox.Show("File does not exist: " & imagePath, "Debug")
                Return
            End If

            ' Show cover viewer
            '  MessageBox.Show("About to show frmCover", "Debug")
            Dim coverForm As New frmCover()
            coverForm.ShowCover(imagePath, trackTitle)
            '  MessageBox.Show("frmCover closed", "Debug")

        Catch ex As Exception
            '  MessageBox.Show("Error in ShowCoverViewer: " & ex.Message & vbCrLf & ex.StackTrace, "Error")
        End Try
    End Sub
    'Private Sub ShowCoverViewer()
    '    Try
    '        ' Get current track info
    '        Dim trackTitle As String = lblStation.Text & " - " & lblTrack.Text

    '        ' Get image path from current image
    '        If picAlbumArt.Image IsNot Nothing AndAlso picAlbumArt.Tag IsNot Nothing Then
    '            Dim imagePath As String = picAlbumArt.Tag.ToString()

    '            If Not String.IsNullOrEmpty(imagePath) AndAlso IO.File.Exists(imagePath) Then
    '                ' Show cover viewer
    '                Dim coverForm As New frmCover()
    '                coverForm.ShowCover(imagePath, trackTitle)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        ' Silent fail
    '    End Try
    'End Sub

    Public Sub DismissTips()
        tmrDisplay.Enabled = False

        ' Dispose image before closing
        If picAlbumArt.Image IsNot Nothing Then
            picAlbumArt.Image.Dispose()
            picAlbumArt.Image = Nothing
        End If

        AnimateOut()
    End Sub



    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Draw border
        Using pen As New Pen(Color.FromArgb(100, 100, 100), 1)
            e.Graphics.DrawRectangle(pen, 0, 0, Me.Width - 1, Me.Height - 1)
        End Using
    End Sub
End Class
Public Class frmStats
    ' Display timer
    Private displayDuration As Integer = 3000 ' 3 seconds

    Private Sub frmStats_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form setup
        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.Manual
        Me.TopMost = True
        Me.ShowInTaskbar = False

        ' Set background color (same as tips)
        Me.BackColor = Color.FromArgb(45, 45, 48)

        ' Style labels
        lblTitle.ForeColor = Color.White
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

        lblStationLabel.ForeColor = Color.LightGray
        lblStationLabel.BackColor = Color.Transparent
        lblStation.ForeColor = Color.White
        lblStation.BackColor = Color.Transparent

        lblBitrateLabel.ForeColor = Color.LightGray
        lblBitrateLabel.BackColor = Color.Transparent
        lblBitrate.ForeColor = Color.White
        lblBitrate.BackColor = Color.Transparent

        lblCodecLabel.ForeColor = Color.LightGray
        lblCodecLabel.BackColor = Color.Transparent
        lblCodec.ForeColor = Color.White
        lblCodec.BackColor = Color.Transparent

        ' Position above taskbar
        PositionAboveTaskbar()
    End Sub

    Private Sub PositionAboveTaskbar()
        ' Get screen working area
        Dim screen As Screen = Screen.PrimaryScreen
        Dim workingArea As Rectangle = screen.WorkingArea

        ' Position in bottom-right corner, just above taskbar
        Dim margin As Integer = 10
        Dim xPos As Integer = workingArea.Right - Me.Width - margin
        Dim yPos As Integer = workingArea.Bottom - Me.Height - margin

        Me.Location = New Point(xPos, yPos)
    End Sub

    Public Sub ShowStats(stationName As String, bitrate As Integer, codec As String)
        Try
            ' Update content
            lblStation.Text = stationName
            lblBitrate.Text = If(bitrate > 0, bitrate.ToString() & " kbps", "Unknown")
            lblCodec.Text = codec

            ' Show form
            If Not Me.Visible Then
                Me.Show()
            End If

            ' Start auto-hide timer
            tmrAutoHide.Interval = displayDuration
            tmrAutoHide.Enabled = True

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub tmrAutoHide_Tick(sender As Object, e As EventArgs) Handles tmrAutoHide.Tick
        tmrAutoHide.Enabled = False
        Me.Hide()
    End Sub

    Private Sub frmStats_Click(sender As Object, e As EventArgs) Handles Me.Click, lblTitle.Click, lblStation.Click, lblBitrate.Click, lblCodec.Click, lblStationLabel.Click, lblBitrateLabel.Click, lblCodecLabel.Click
        ' Dismiss on click
        tmrAutoHide.Enabled = False
        Me.Hide()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Draw border
        Using pen As New Pen(Color.FromArgb(100, 100, 100), 1)
            e.Graphics.DrawRectangle(pen, 0, 0, Me.Width - 1, Me.Height - 1)
        End Using
    End Sub

    ' ESC to close
    Private Sub frmStats_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Hide()
        End If
    End Sub
End Class
Imports Un4seen.Bass

Public Class frmLevel
    Private autoCloseSeconds As Integer = 3
    Private lastInteraction As DateTime

    Private Sub frmLevel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Form setup
        '  Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
        Me.TopMost = True
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.Manual
        Me.Size = New Size(60, 220)

        ' Create rounded corners
        CreateRoundedCorners()

        ' Set background color (same as tips form)
        Me.BackColor = Color.FromArgb(45, 45, 48) ' Dark gray



        ' Style label
        lblVolume.ForeColor = Color.White
        lblVolume.BackColor = Color.Transparent
        lblVolume.Font = New Font("Segoe UI", 8, FontStyle.Regular)
        lblVolume.TextAlign = ContentAlignment.MiddleCenter
        lblVolume.Cursor = Cursors.Hand

        ' Style button
        btnClose.BackColor = Color.FromArgb(60, 60, 63)
        btnClose.ForeColor = Color.White
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.FlatAppearance.BorderSize = 0
        btnClose.Font = New Font("Segoe UI", 8, FontStyle.Bold)
        btnClose.Cursor = Cursors.Hand


        ' Position near system tray
        PositionNearTray()

        ' Set up trackbar
        tbVolume.Minimum = 0
        tbVolume.Maximum = 100
        tbVolume.TickFrequency = 10
        tbVolume.Value = g_CurrentVolume

        ' Update label
        UpdateVolumeLabel()

        ' Start auto-close timer
        tmrAutoClose.Interval = 1000 ' Check every second
        tmrAutoClose.Enabled = True
        lastInteraction = DateTime.Now
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

    Private Sub PositionNearTray()
        Try
            ' Get screen working area (excludes taskbar)
            Dim screen As Screen = Screen.PrimaryScreen
            Dim workingArea As Rectangle = screen.WorkingArea

            ' Position in bottom-right corner, slightly above taskbar
            Dim margin As Integer = 10
            Dim xPos As Integer = workingArea.Right - Me.Width - margin
            Dim yPos As Integer = workingArea.Bottom - Me.Height - margin

            Me.Location = New Point(xPos, yPos)

        Catch ex As Exception
            ' Fallback to center screen
            Me.StartPosition = FormStartPosition.CenterScreen
        End Try
    End Sub

    Private Sub tbVolume_Scroll(sender As Object, e As EventArgs) Handles tbVolume.Scroll
        ' Update volume
        g_CurrentVolume = tbVolume.Value
        UpdateVolumeLabel()
        ApplyVolumeToStream()

        ' Reset auto-close timer
        lastInteraction = DateTime.Now
    End Sub

    Private Sub tbVolume_ValueChanged(sender As Object, e As EventArgs) Handles tbVolume.ValueChanged
        ' Update volume
        g_CurrentVolume = tbVolume.Value
        UpdateVolumeLabel()

        ' Reset auto-close timer
        lastInteraction = DateTime.Now
    End Sub

    Private Sub UpdateVolumeLabel()
        lblVolume.Text = g_CurrentVolume.ToString() & "%"
    End Sub

    Private Sub ApplyVolumeToStream()
        Try
            ' Apply to BASS
            If g_StreamHandle <> 0 Then
                Bass.BASS_ChannelSetAttribute(g_StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, g_CurrentVolume / 100.0F)
            End If

            ' Save to settings
            If g_SettingsManager IsNot Nothing Then
                g_SettingsManager.LastVolume = g_CurrentVolume
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub tmrAutoClose_Tick(sender As Object, e As EventArgs) Handles tmrAutoClose.Tick
        ' Check if enough time has passed since last interaction
        Dim elapsed As TimeSpan = DateTime.Now - lastInteraction

        If elapsed.TotalSeconds >= autoCloseSeconds Then
            ' Save settings before closing
            If g_SettingsManager IsNot Nothing Then
                g_SettingsManager.SaveSettings()
            End If

            Me.Close()
        End If
    End Sub

    Private Sub frmLevel_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        ' Reset timer on mouse movement
        lastInteraction = DateTime.Now
    End Sub

    Private Sub lblVolume_MouseMove(sender As Object, e As MouseEventArgs) Handles lblVolume.MouseMove
        lastInteraction = DateTime.Now
    End Sub

    Private Sub frmLevel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Save volume on close
        ApplyVolumeToStream()
    End Sub

    ' Allow keyboard control
    Private Sub frmLevel_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                If tbVolume.Value < tbVolume.Maximum Then
                    tbVolume.Value += 5
                End If
                e.Handled = True

            Case Keys.Down
                If tbVolume.Value > tbVolume.Minimum Then
                    tbVolume.Value -= 5
                End If
                e.Handled = True

            Case Keys.Escape
                Me.Close()
                e.Handled = True
        End Select

        lastInteraction = DateTime.Now
    End Sub

    Private Sub tbVolume_KeyDown(sender As Object, e As KeyEventArgs) Handles tbVolume.KeyDown
        lastInteraction = DateTime.Now
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
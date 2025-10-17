Public Class frmSplash
    Private Sub tmrClose_Tick(sender As Object, e As EventArgs) Handles tmrClose.Tick
        Me.Close()
    End Sub

    Private Sub frmSplash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Me.StartPosition = FormStartPosition.Manual
        Me.TopMost = True
        Me.ShowInTaskbar = False
        lblVersion.Text = "Version " & Application.ProductVersion
        ' Create rounded corners
        CreateRoundedCorners()

        ' Set background color
        Me.BackColor = Color.FromArgb(45, 45, 48) ' Dark gray
        Me.Size = New Size(190, 190) ' Adjust size as needed
        tmrClose.Interval = 3000 ' Display for 3 seconds
        tmrClose.Enabled = True


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

    Private Sub lblVersion_Click(sender As Object, e As EventArgs) Handles lblVersion.Click

    End Sub
End Class
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTips
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.picAlbumArt = New System.Windows.Forms.PictureBox()
        Me.lblStation = New System.Windows.Forms.Label()
        Me.lblTrack = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.tmrAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.tmrDisplay = New System.Windows.Forms.Timer(Me.components)
        CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picAlbumArt
        '
        Me.picAlbumArt.BackColor = System.Drawing.Color.DarkGray
        Me.picAlbumArt.Location = New System.Drawing.Point(261, 24)
        Me.picAlbumArt.Name = "picAlbumArt"
        Me.picAlbumArt.Size = New System.Drawing.Size(76, 74)
        Me.picAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picAlbumArt.TabIndex = 0
        Me.picAlbumArt.TabStop = False
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblStation.ForeColor = System.Drawing.Color.LightGray
        Me.lblStation.Location = New System.Drawing.Point(12, 9)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(66, 24)
        Me.lblStation.TabIndex = 1
        Me.lblStation.Text = "Station"
        '
        'lblTrack
        '
        Me.lblTrack.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblTrack.ForeColor = System.Drawing.Color.LightGray
        Me.lblTrack.Location = New System.Drawing.Point(13, 36)
        Me.lblTrack.Name = "lblTrack"
        Me.lblTrack.Size = New System.Drawing.Size(242, 48)
        Me.lblTrack.TabIndex = 2
        Me.lblTrack.Text = "Track"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Khaki
        Me.lblStatus.Location = New System.Drawing.Point(13, 89)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(44, 16)
        Me.lblStatus.TabIndex = 3
        Me.lblStatus.Text = "Status"
        '
        'tmrAnimation
        '
        Me.tmrAnimation.Interval = 10
        '
        'tmrDisplay
        '
        Me.tmrDisplay.Interval = 5000
        '
        'frmTips
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Maroon
        Me.ClientSize = New System.Drawing.Size(350, 120)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblTrack)
        Me.Controls.Add(Me.lblStation)
        Me.Controls.Add(Me.picAlbumArt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmTips"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmTips"
        Me.TopMost = True
        CType(Me.picAlbumArt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents picAlbumArt As PictureBox
    Friend WithEvents lblStation As Label
    Friend WithEvents lblTrack As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents tmrAnimation As Timer
    Friend WithEvents tmrDisplay As Timer
End Class

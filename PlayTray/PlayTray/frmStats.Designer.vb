<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStats
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStats))
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblStationLabel = New System.Windows.Forms.Label()
        Me.lblBitrateLabel = New System.Windows.Forms.Label()
        Me.lblCodecLabel = New System.Windows.Forms.Label()
        Me.lblCodec = New System.Windows.Forms.Label()
        Me.lblBitrate = New System.Windows.Forms.Label()
        Me.lblStation = New System.Windows.Forms.Label()
        Me.tmrAutohide = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(12, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(140, 17)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Stream Info"
        '
        'lblStationLabel
        '
        Me.lblStationLabel.AutoSize = True
        Me.lblStationLabel.Location = New System.Drawing.Point(12, 20)
        Me.lblStationLabel.Name = "lblStationLabel"
        Me.lblStationLabel.Size = New System.Drawing.Size(43, 13)
        Me.lblStationLabel.TabIndex = 1
        Me.lblStationLabel.Text = "Station:"
        '
        'lblBitrateLabel
        '
        Me.lblBitrateLabel.AutoSize = True
        Me.lblBitrateLabel.Location = New System.Drawing.Point(12, 33)
        Me.lblBitrateLabel.Name = "lblBitrateLabel"
        Me.lblBitrateLabel.Size = New System.Drawing.Size(45, 13)
        Me.lblBitrateLabel.TabIndex = 2
        Me.lblBitrateLabel.Text = "BitRate:"
        '
        'lblCodecLabel
        '
        Me.lblCodecLabel.AutoSize = True
        Me.lblCodecLabel.Location = New System.Drawing.Point(12, 46)
        Me.lblCodecLabel.Name = "lblCodecLabel"
        Me.lblCodecLabel.Size = New System.Drawing.Size(41, 13)
        Me.lblCodecLabel.TabIndex = 3
        Me.lblCodecLabel.Text = "Codec:"
        '
        'lblCodec
        '
        Me.lblCodec.AutoSize = True
        Me.lblCodec.Location = New System.Drawing.Point(57, 46)
        Me.lblCodec.Name = "lblCodec"
        Me.lblCodec.Size = New System.Drawing.Size(16, 13)
        Me.lblCodec.TabIndex = 6
        Me.lblCodec.Text = "..."
        '
        'lblBitrate
        '
        Me.lblBitrate.AutoSize = True
        Me.lblBitrate.Location = New System.Drawing.Point(57, 33)
        Me.lblBitrate.Name = "lblBitrate"
        Me.lblBitrate.Size = New System.Drawing.Size(16, 13)
        Me.lblBitrate.TabIndex = 5
        Me.lblBitrate.Text = "..."
        '
        'lblStation
        '
        Me.lblStation.AutoSize = True
        Me.lblStation.Location = New System.Drawing.Point(57, 20)
        Me.lblStation.Name = "lblStation"
        Me.lblStation.Size = New System.Drawing.Size(16, 13)
        Me.lblStation.TabIndex = 4
        Me.lblStation.Text = "..."
        '
        'tmrAutohide
        '
        Me.tmrAutohide.Interval = 1000
        '
        'frmStats
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(208, 66)
        Me.Controls.Add(Me.lblCodec)
        Me.Controls.Add(Me.lblBitrate)
        Me.Controls.Add(Me.lblStation)
        Me.Controls.Add(Me.lblCodecLabel)
        Me.Controls.Add(Me.lblBitrateLabel)
        Me.Controls.Add(Me.lblStationLabel)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStats"
        Me.ShowInTaskbar = False
        Me.Text = "frmStats"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblStationLabel As Label
    Friend WithEvents lblBitrateLabel As Label
    Friend WithEvents lblCodecLabel As Label
    Friend WithEvents lblCodec As Label
    Friend WithEvents lblBitrate As Label
    Friend WithEvents lblStation As Label
    Friend WithEvents tmrAutohide As Timer
End Class

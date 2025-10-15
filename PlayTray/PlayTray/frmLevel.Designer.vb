<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLevel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLevel))
        Me.tbVolume = New System.Windows.Forms.TrackBar()
        Me.lblVolume = New System.Windows.Forms.Label()
        Me.tmrAutoClose = New System.Windows.Forms.Timer(Me.components)
        Me.btnClose = New System.Windows.Forms.Button()
        CType(Me.tbVolume, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbVolume
        '
        Me.tbVolume.Location = New System.Drawing.Point(12, 19)
        Me.tbVolume.Maximum = 100
        Me.tbVolume.Name = "tbVolume"
        Me.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.tbVolume.Size = New System.Drawing.Size(45, 169)
        Me.tbVolume.TabIndex = 0
        Me.tbVolume.TickFrequency = 10
        '
        'lblVolume
        '
        Me.lblVolume.AutoSize = True
        Me.lblVolume.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblVolume.Location = New System.Drawing.Point(14, 193)
        Me.lblVolume.Name = "lblVolume"
        Me.lblVolume.Size = New System.Drawing.Size(33, 13)
        Me.lblVolume.TabIndex = 1
        Me.lblVolume.Text = "100%"
        '
        'tmrAutoClose
        '
        Me.tmrAutoClose.Enabled = True
        Me.tmrAutoClose.Interval = 5000
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.DarkGray
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(37, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(20, 20)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "X"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmLevel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(61, 211)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblVolume)
        Me.Controls.Add(Me.tbVolume)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLevel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Level"
        Me.TopMost = True
        CType(Me.tbVolume, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbVolume As TrackBar
    Friend WithEvents lblVolume As Label
    Friend WithEvents tmrAutoClose As Timer
    Friend WithEvents btnClose As Button
End Class

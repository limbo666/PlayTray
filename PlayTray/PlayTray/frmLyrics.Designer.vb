<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLyrics
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
        Me.lblTrackTitle = New System.Windows.Forms.Label()
        Me.txtLyrics = New System.Windows.Forms.TextBox()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.btnFetchLyrics = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSaveLyrics = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTrackTitle
        '
        Me.lblTrackTitle.AutoSize = True
        Me.lblTrackTitle.Location = New System.Drawing.Point(38, 368)
        Me.lblTrackTitle.Name = "lblTrackTitle"
        Me.lblTrackTitle.Size = New System.Drawing.Size(65, 13)
        Me.lblTrackTitle.TabIndex = 0
        Me.lblTrackTitle.Text = "lblTrackTitle"
        '
        'txtLyrics
        '
        Me.txtLyrics.Location = New System.Drawing.Point(3, 2)
        Me.txtLyrics.Multiline = True
        Me.txtLyrics.Name = "txtLyrics"
        Me.txtLyrics.Size = New System.Drawing.Size(560, 350)
        Me.txtLyrics.TabIndex = 1
        '
        'lblResult
        '
        Me.lblResult.AutoSize = True
        Me.lblResult.Location = New System.Drawing.Point(38, 398)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(47, 13)
        Me.lblResult.TabIndex = 4
        Me.lblResult.Text = "lblResult"
        '
        'btnFetchLyrics
        '
        Me.btnFetchLyrics.Location = New System.Drawing.Point(488, 358)
        Me.btnFetchLyrics.Name = "btnFetchLyrics"
        Me.btnFetchLyrics.Size = New System.Drawing.Size(75, 32)
        Me.btnFetchLyrics.TabIndex = 5
        Me.btnFetchLyrics.Text = "Get Lyrics"
        Me.btnFetchLyrics.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(488, 398)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 32)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSaveLyrics
        '
        Me.btnSaveLyrics.Location = New System.Drawing.Point(407, 358)
        Me.btnSaveLyrics.Name = "btnSaveLyrics"
        Me.btnSaveLyrics.Size = New System.Drawing.Size(75, 32)
        Me.btnSaveLyrics.TabIndex = 7
        Me.btnSaveLyrics.Text = "Save Lyrics"
        Me.btnSaveLyrics.UseVisualStyleBackColor = True
        '
        'frmLyrics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(568, 438)
        Me.Controls.Add(Me.btnSaveLyrics)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnFetchLyrics)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.txtLyrics)
        Me.Controls.Add(Me.lblTrackTitle)
        Me.Name = "frmLyrics"
        Me.Text = "frmLyrics"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTrackTitle As Label
    Friend WithEvents txtLyrics As TextBox
    Friend WithEvents lblResult As Label
    Friend WithEvents btnFetchLyrics As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSaveLyrics As Button
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCover
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.picCover = New System.Windows.Forms.PictureBox()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.btnSaveToDocument = New System.Windows.Forms.Button()
        Me.btnOpenDoc = New System.Windows.Forms.Button()
        CType(Me.picCover, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.DarkGray
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(527, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(20, 20)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "X"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'picCover
        '
        Me.picCover.Location = New System.Drawing.Point(17, 26)
        Me.picCover.Name = "picCover"
        Me.picCover.Size = New System.Drawing.Size(521, 485)
        Me.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picCover.TabIndex = 1
        Me.picCover.TabStop = False
        '
        'LblTitle
        '
        Me.LblTitle.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.LblTitle.Location = New System.Drawing.Point(26, 512)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(501, 33)
        Me.LblTitle.TabIndex = 2
        Me.LblTitle.Text = "..."
        Me.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSaveToDocument
        '
        Me.btnSaveToDocument.Location = New System.Drawing.Point(366, 539)
        Me.btnSaveToDocument.Name = "btnSaveToDocument"
        Me.btnSaveToDocument.Size = New System.Drawing.Size(86, 23)
        Me.btnSaveToDocument.TabIndex = 3
        Me.btnSaveToDocument.Text = "Save to Doc"
        Me.btnSaveToDocument.UseVisualStyleBackColor = True
        '
        'btnOpenDoc
        '
        Me.btnOpenDoc.Location = New System.Drawing.Point(458, 539)
        Me.btnOpenDoc.Name = "btnOpenDoc"
        Me.btnOpenDoc.Size = New System.Drawing.Size(86, 23)
        Me.btnOpenDoc.TabIndex = 4
        Me.btnOpenDoc.Text = "Open Doc"
        Me.btnOpenDoc.UseVisualStyleBackColor = True
        '
        'frmCover
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Maroon
        Me.ClientSize = New System.Drawing.Size(550, 574)
        Me.Controls.Add(Me.btnOpenDoc)
        Me.Controls.Add(Me.btnSaveToDocument)
        Me.Controls.Add(Me.LblTitle)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.picCover)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmCover"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmCover"
        CType(Me.picCover, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents picCover As PictureBox
    Friend WithEvents LblTitle As Label
    Friend WithEvents btnSaveToDocument As Button
    Friend WithEvents btnOpenDoc As Button
End Class

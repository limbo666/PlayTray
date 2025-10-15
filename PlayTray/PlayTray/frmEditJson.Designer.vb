<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditJson
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditJson))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStationInfo = New System.Windows.Forms.TextBox()
        Me.txtStationWebAddress = New System.Windows.Forms.TextBox()
        Me.txtStationFilters = New System.Windows.Forms.TextBox()
        Me.txtStationRemark = New System.Windows.Forms.TextBox()
        Me.txtStationLink = New System.Windows.Forms.TextBox()
        Me.txtStationName = New System.Windows.Forms.TextBox()
        Me.numStationID = New System.Windows.Forms.NumericUpDown()
        Me.lblStationInfo = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.numStationID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtStationInfo)
        Me.GroupBox1.Controls.Add(Me.txtStationWebAddress)
        Me.GroupBox1.Controls.Add(Me.txtStationFilters)
        Me.GroupBox1.Controls.Add(Me.txtStationRemark)
        Me.GroupBox1.Controls.Add(Me.txtStationLink)
        Me.GroupBox1.Controls.Add(Me.txtStationName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 50)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(610, 276)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Station info"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(171, 233)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Station Web Address:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(191, 195)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Station Info Link:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(222, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Filters List:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(226, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Remarks:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(248, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Link:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(240, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Name:"
        '
        'txtStationInfo
        '
        Me.txtStationInfo.Location = New System.Drawing.Point(284, 188)
        Me.txtStationInfo.Name = "txtStationInfo"
        Me.txtStationInfo.Size = New System.Drawing.Size(304, 20)
        Me.txtStationInfo.TabIndex = 10
        '
        'txtStationWebAddress
        '
        Me.txtStationWebAddress.Location = New System.Drawing.Point(284, 230)
        Me.txtStationWebAddress.Name = "txtStationWebAddress"
        Me.txtStationWebAddress.Size = New System.Drawing.Size(304, 20)
        Me.txtStationWebAddress.TabIndex = 9
        '
        'txtStationFilters
        '
        Me.txtStationFilters.Location = New System.Drawing.Point(284, 146)
        Me.txtStationFilters.Name = "txtStationFilters"
        Me.txtStationFilters.Size = New System.Drawing.Size(304, 20)
        Me.txtStationFilters.TabIndex = 8
        '
        'txtStationRemark
        '
        Me.txtStationRemark.Location = New System.Drawing.Point(284, 104)
        Me.txtStationRemark.Name = "txtStationRemark"
        Me.txtStationRemark.Size = New System.Drawing.Size(304, 20)
        Me.txtStationRemark.TabIndex = 7
        '
        'txtStationLink
        '
        Me.txtStationLink.Location = New System.Drawing.Point(284, 62)
        Me.txtStationLink.Name = "txtStationLink"
        Me.txtStationLink.Size = New System.Drawing.Size(304, 20)
        Me.txtStationLink.TabIndex = 6
        '
        'txtStationName
        '
        Me.txtStationName.Location = New System.Drawing.Point(284, 20)
        Me.txtStationName.Name = "txtStationName"
        Me.txtStationName.Size = New System.Drawing.Size(304, 20)
        Me.txtStationName.TabIndex = 0
        '
        'numStationID
        '
        Me.numStationID.Location = New System.Drawing.Point(111, 14)
        Me.numStationID.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numStationID.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numStationID.Name = "numStationID"
        Me.numStationID.Size = New System.Drawing.Size(40, 20)
        Me.numStationID.TabIndex = 0
        Me.numStationID.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblStationInfo
        '
        Me.lblStationInfo.AutoSize = True
        Me.lblStationInfo.Location = New System.Drawing.Point(34, 17)
        Me.lblStationInfo.Name = "lblStationInfo"
        Me.lblStationInfo.Size = New System.Drawing.Size(74, 13)
        Me.lblStationInfo.TabIndex = 1
        Me.lblStationInfo.Text = "Select station:"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(385, 332)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 32)
        Me.btnOK.TabIndex = 17
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(547, 332)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "OK"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(466, 332)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 32)
        Me.btnApply.TabIndex = 20
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'frmEditJson
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblStationInfo)
        Me.Controls.Add(Me.numStationID)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditJson"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Stations"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.numStationID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents numStationID As NumericUpDown
    Friend WithEvents lblStationInfo As Label
    Friend WithEvents txtStationName As TextBox
    Friend WithEvents txtStationInfo As TextBox
    Friend WithEvents txtStationWebAddress As TextBox
    Friend WithEvents txtStationFilters As TextBox
    Friend WithEvents txtStationRemark As TextBox
    Friend WithEvents txtStationLink As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
End Class

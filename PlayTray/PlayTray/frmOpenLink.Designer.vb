<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpenLink
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOpenLink))
        Me.lblURL = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblPosition = New System.Windows.Forms.Label()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.txtURL = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.cboRecentURLs = New System.Windows.Forms.ComboBox()
        Me.numPosition = New System.Windows.Forms.NumericUpDown()
        Me.btnPlayNow = New System.Windows.Forms.Button()
        Me.btnAddToFavorites = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.dgvFilters = New System.Windows.Forms.DataGridView()
        Me.Find = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Replace = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnAddFilter = New System.Windows.Forms.Button()
        Me.btnRemoveFilter = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblRecentStreams = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        CType(Me.numPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvFilters, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblURL
        '
        Me.lblURL.AutoSize = True
        Me.lblURL.Location = New System.Drawing.Point(222, 35)
        Me.lblURL.Name = "lblURL"
        Me.lblURL.Size = New System.Drawing.Size(68, 13)
        Me.lblURL.TabIndex = 0
        Me.lblURL.Text = "Stream URL:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(218, 37)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(74, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Station Name:"
        '
        'lblPosition
        '
        Me.lblPosition.AutoSize = True
        Me.lblPosition.Location = New System.Drawing.Point(202, 115)
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.Size = New System.Drawing.Size(88, 13)
        Me.lblPosition.TabIndex = 3
        Me.lblPosition.Text = "Favorite Position:"
        '
        'lblRemarks
        '
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Location = New System.Drawing.Point(238, 77)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(52, 13)
        Me.lblRemarks.TabIndex = 2
        Me.lblRemarks.Text = "Remarks:"
        '
        'txtURL
        '
        Me.txtURL.Location = New System.Drawing.Point(293, 32)
        Me.txtURL.Name = "txtURL"
        Me.txtURL.Size = New System.Drawing.Size(296, 20)
        Me.txtURL.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(296, 34)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(293, 20)
        Me.txtName.TabIndex = 5
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(296, 74)
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(293, 20)
        Me.txtRemarks.TabIndex = 6
        '
        'cboRecentURLs
        '
        Me.cboRecentURLs.FormattingEnabled = True
        Me.cboRecentURLs.Location = New System.Drawing.Point(297, 74)
        Me.cboRecentURLs.Name = "cboRecentURLs"
        Me.cboRecentURLs.Size = New System.Drawing.Size(292, 21)
        Me.cboRecentURLs.TabIndex = 7
        '
        'numPosition
        '
        Me.numPosition.Location = New System.Drawing.Point(297, 113)
        Me.numPosition.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numPosition.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numPosition.Name = "numPosition"
        Me.numPosition.Size = New System.Drawing.Size(53, 20)
        Me.numPosition.TabIndex = 8
        Me.numPosition.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnPlayNow
        '
        Me.btnPlayNow.Location = New System.Drawing.Point(465, 484)
        Me.btnPlayNow.Name = "btnPlayNow"
        Me.btnPlayNow.Size = New System.Drawing.Size(75, 32)
        Me.btnPlayNow.TabIndex = 9
        Me.btnPlayNow.Text = "Play"
        Me.btnPlayNow.UseVisualStyleBackColor = True
        '
        'btnAddToFavorites
        '
        Me.btnAddToFavorites.Location = New System.Drawing.Point(466, 109)
        Me.btnAddToFavorites.Name = "btnAddToFavorites"
        Me.btnAddToFavorites.Size = New System.Drawing.Size(120, 32)
        Me.btnAddToFavorites.TabIndex = 10
        Me.btnAddToFavorites.Text = "Add To Favorites"
        Me.btnAddToFavorites.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(546, 484)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 32)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'dgvFilters
        '
        Me.dgvFilters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvFilters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFilters.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Find, Me.Replace})
        Me.dgvFilters.Location = New System.Drawing.Point(296, 19)
        Me.dgvFilters.Name = "dgvFilters"
        Me.dgvFilters.RowHeadersWidth = 100
        Me.dgvFilters.Size = New System.Drawing.Size(295, 104)
        Me.dgvFilters.TabIndex = 13
        '
        'Find
        '
        Me.Find.HeaderText = "Find"
        Me.Find.Name = "Find"
        '
        'Replace
        '
        Me.Replace.HeaderText = "Replace"
        Me.Replace.Name = "Replace"
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Location = New System.Drawing.Point(395, 129)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(95, 32)
        Me.btnAddFilter.TabIndex = 14
        Me.btnAddFilter.Text = "+ Add Filter"
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Location = New System.Drawing.Point(496, 129)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(95, 32)
        Me.btnRemoveFilter.TabIndex = 15
        Me.btnRemoveFilter.Text = "- Remove Filter"
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dgvFilters)
        Me.GroupBox1.Controls.Add(Me.btnRemoveFilter)
        Me.GroupBox1.Controls.Add(Me.btnAddFilter)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 303)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(610, 175)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filters (Find/Replace):"
        '
        'lblRecentStreams
        '
        Me.lblRecentStreams.AutoSize = True
        Me.lblRecentStreams.Location = New System.Drawing.Point(208, 77)
        Me.lblRecentStreams.Name = "lblRecentStreams"
        Me.lblRecentStreams.Size = New System.Drawing.Size(84, 13)
        Me.lblRecentStreams.TabIndex = 17
        Me.lblRecentStreams.Text = "Recent streams:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnAddToFavorites)
        Me.GroupBox2.Controls.Add(Me.lblName)
        Me.GroupBox2.Controls.Add(Me.lblRemarks)
        Me.GroupBox2.Controls.Add(Me.lblPosition)
        Me.GroupBox2.Controls.Add(Me.txtName)
        Me.GroupBox2.Controls.Add(Me.txtRemarks)
        Me.GroupBox2.Controls.Add(Me.numPosition)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 132)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(610, 165)
        Me.GroupBox2.TabIndex = 18
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Favorite"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtURL)
        Me.GroupBox3.Controls.Add(Me.lblURL)
        Me.GroupBox3.Controls.Add(Me.lblRecentStreams)
        Me.GroupBox3.Controls.Add(Me.cboRecentURLs)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(610, 114)
        Me.GroupBox3.TabIndex = 19
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Link"
        '
        'frmOpenLink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 523)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnPlayNow)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOpenLink"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Open link"
        CType(Me.numPosition, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvFilters, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblURL As Label
    Friend WithEvents lblName As Label
    Friend WithEvents lblPosition As Label
    Friend WithEvents lblRemarks As Label
    Friend WithEvents txtURL As TextBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtRemarks As TextBox
    Friend WithEvents cboRecentURLs As ComboBox
    Friend WithEvents numPosition As NumericUpDown
    Friend WithEvents btnPlayNow As Button
    Friend WithEvents btnAddToFavorites As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents dgvFilters As DataGridView
    Friend WithEvents Find As DataGridViewTextBoxColumn
    Friend WithEvents Replace As DataGridViewTextBoxColumn
    Friend WithEvents btnAddFilter As Button
    Friend WithEvents btnRemoveFilter As Button
    Friend WithEvents lblRecentStreams As Label
    Friend WithEvents GroupBox2 As GroupBox
    Public WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
End Class

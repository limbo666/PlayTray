Imports System.IO
Imports System.Linq

Public Class frmOpenLink
    Private recentUrls As New List(Of String)
    Private Const MAX_RECENT_URLS As Integer = 10

    'Private Sub frmOpenLink_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    ' Load recent URLs
    '    LoadRecentUrls()

    '    ' Set default position to next available slot
    '    numPosition.Minimum = 1
    '    numPosition.Maximum = 10
    '    numPosition.Value = GetNextAvailablePosition()

    '    ' Focus URL textbox
    '    txtURL.Focus()
    'End Sub

    Private Sub LoadRecentUrls()
        Try
            Dim recentFilePath As String = IO.Path.Combine(AppPath, "recent_urls.txt")

            If IO.File.Exists(recentFilePath) Then
                Dim lines() As String = IO.File.ReadAllLines(recentFilePath)

                recentUrls.Clear()
                cboRecentURLs.Items.Clear()

                For Each line In lines
                    If Not String.IsNullOrWhiteSpace(line) Then
                        recentUrls.Add(line.Trim())
                        cboRecentURLs.Items.Add(line.Trim())
                    End If
                Next
            End If

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub SaveRecentUrls()
        Try
            Dim recentFilePath As String = IO.Path.Combine(AppPath, "recent_urls.txt")

            ' Keep only last MAX_RECENT_URLS
            If recentUrls.Count > MAX_RECENT_URLS Then
                recentUrls = recentUrls.Skip(recentUrls.Count - MAX_RECENT_URLS).ToList()
            End If

            IO.File.WriteAllLines(recentFilePath, recentUrls.ToArray())

        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub AddToRecentUrls(url As String)
        If String.IsNullOrWhiteSpace(url) Then Return

        ' Remove if already exists
        If recentUrls.Contains(url) Then
            recentUrls.Remove(url)
        End If

        ' Add to end (most recent)
        recentUrls.Add(url)

        ' Save
        SaveRecentUrls()
    End Sub

    Private Function GetNextAvailablePosition() As Integer
        If g_StationManager Is Nothing Then Return 1

        ' Find first unused position from 1-10
        For i As Integer = 1 To 10
            If g_StationManager.GetStation(i) Is Nothing Then
                Return i
            End If
        Next

        ' All positions filled, return 1
        Return 1
    End Function

    Private Function ValidateInputs() As Boolean
        ' Validate URL
        If String.IsNullOrWhiteSpace(txtURL.Text) Then
            MessageBox.Show("Please enter a stream URL", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtURL.Focus()
            Return False
        End If

        ' Basic URL validation
        Dim url As String = txtURL.Text.Trim()
        If Not url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso
           Not url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("URL must start with http:// or https://", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtURL.Focus()
            Return False
        End If

        ' Validate station name (required for adding to favorites)
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Please enter a station name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtName.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub btnPlayNow_Click(sender As Object, e As EventArgs) Handles btnPlayNow.Click
        Try
            Dim url As String = txtURL.Text.Trim()

            If String.IsNullOrWhiteSpace(url) Then
                MessageBox.Show("Please enter a stream URL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Basic URL validation
            If Not url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso
               Not url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("URL must start with http:// or https://", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Create temporary station
            Dim tempStation As New Station()
            tempStation.Name = If(String.IsNullOrWhiteSpace(txtName.Text), "Temporary Stream", txtName.Text.Trim())
            tempStation.Link = url
            tempStation.Remarks = txtRemarks.Text.Trim()
            tempStation.Position = CInt(numPosition.Value)

            ' Add to recent URLs
            AddToRecentUrls(url)

            ' Get main form reference and play
            Dim mainForm As frmMain = DirectCast(Application.OpenForms("frmMain"), frmMain)
            If mainForm IsNot Nothing Then
                g_CurrentStation = tempStation
                mainForm.PlayStreamFromLink(url, tempStation.Name)
            End If

            ' Don't close the form, let user add to favorites if they want

        Catch ex As Exception
            MessageBox.Show("Error playing stream: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAddToFavorites_Click(sender As Object, e As EventArgs) Handles btnAddToFavorites.Click
        Try
            If Not ValidateInputs() Then Return

            ' Check if position is already occupied
            Dim position As Integer = CInt(numPosition.Value)
            Dim existingStation = g_StationManager.GetStation(position)

            If existingStation IsNot Nothing Then
                Dim result = MessageBox.Show(
                    $"Position {position} is already occupied by '{existingStation.Name}'.{vbCrLf}Do you want to replace it?",
                    "Confirm Replace",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result <> DialogResult.Yes Then
                    Return
                End If
            End If

            ' Create new station
            Dim newStation As New Station()
            newStation.Position = position
            newStation.Name = txtName.Text.Trim()
            newStation.Link = txtURL.Text.Trim()
            newStation.Remarks = txtRemarks.Text.Trim()
            newStation.DateAdded = DateTime.Now
            newStation.FiltersList = GetFiltersFromGrid()  ' ADD THIS LINE
            ' Add to station manager
            g_StationManager.AddStation(newStation)

            ' Add to recent URLs
            AddToRecentUrls(newStation.Link)

            ' Update favorites menu in main form
            Dim mainForm As frmMain = DirectCast(Application.OpenForms("frmMain"), frmMain)
            If mainForm IsNot Nothing Then
                mainForm.RefreshFavoritesMenu()
            End If

            MessageBox.Show("Station added to favorites successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear form for next entry
            ClearForm()

        Catch ex As Exception
            MessageBox.Show("Error adding to favorites: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboRecentURLs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRecentURLs.SelectedIndexChanged
        If cboRecentURLs.SelectedIndex >= 0 Then
            txtURL.Text = cboRecentURLs.SelectedItem.ToString()
        End If
    End Sub

    Private Sub ClearForm()
        txtURL.Clear()
        txtName.Clear()
        txtRemarks.Clear()
        dgvFilters.Rows.Clear()  ' ADD THIS LINE
        numPosition.Value = GetNextAvailablePosition()
        txtURL.Focus()
    End Sub


    Private Sub frmOpenLink_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load recent URLs
        LoadRecentUrls()

        ' Set default position to next available slot
        numPosition.Minimum = 1
        numPosition.Maximum = 10
        numPosition.Value = GetNextAvailablePosition()

        ' Setup filter grid
        SetupFilterGrid()

        ' Focus URL textbox
        txtURL.Focus()
    End Sub

    Private Sub SetupFilterGrid()
        Try
            ' Clear existing columns
            dgvFilters.Columns.Clear()
            dgvFilters.Rows.Clear()

            ' Add columns
            dgvFilters.Columns.Add("Find", "Find Text")
            dgvFilters.Columns.Add("Replace", "Replace With")

            ' Set column properties
            dgvFilters.Columns(0).Width = 150
            dgvFilters.Columns(1).Width = 150
            dgvFilters.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
            dgvFilters.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable

            ' Grid properties
            dgvFilters.AllowUserToAddRows = False
            dgvFilters.AllowUserToDeleteRows = False
            dgvFilters.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvFilters.MultiSelect = False
            dgvFilters.RowHeadersVisible = False

        Catch ex As Exception
            MessageBox.Show("Error setting up filter grid: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub btnAddFilter_Click(sender As Object, e As EventArgs) Handles btnAddFilter.Click
        Try
            ' Add empty row
            dgvFilters.Rows.Add("", "")

            ' Select the new row and focus first cell for editing
            If dgvFilters.Rows.Count > 0 Then
                dgvFilters.CurrentCell = dgvFilters.Rows(dgvFilters.Rows.Count - 1).Cells(0)
                dgvFilters.BeginEdit(True)
            End If

        Catch ex As Exception
            MessageBox.Show("Error adding filter: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub btnRemoveFilter_Click(sender As Object, e As EventArgs) Handles btnRemoveFilter.Click
        Try
            If dgvFilters.SelectedRows.Count > 0 Then
                dgvFilters.Rows.Remove(dgvFilters.SelectedRows(0))
            End If
        Catch ex As Exception
            MessageBox.Show("Error removing filter: " & ex.Message, "Error")
        End Try
    End Sub

    Private Function GetFiltersFromGrid() As List(Of StationFilter)
        Dim filterList As New List(Of StationFilter)()

        Try
            For Each row As DataGridViewRow In dgvFilters.Rows
                Dim findText As String = ""
                Dim replaceText As String = ""

                If row.Cells(0).Value IsNot Nothing Then
                    findText = row.Cells(0).Value.ToString().Trim()
                End If

                If row.Cells(1).Value IsNot Nothing Then
                    replaceText = row.Cells(1).Value.ToString().Trim()
                End If

                ' Only add if Find text is not empty
                If Not String.IsNullOrEmpty(findText) Then
                    Dim newFilter As New StationFilter(findText, replaceText)
                    filterList.Add(newFilter)
                End If
            Next
        Catch ex As Exception
            ' Return empty list on error
        End Try

        Return filterList
    End Function

    Private Sub LoadFiltersToGrid(filterList As List(Of StationFilter))
        Try
            dgvFilters.Rows.Clear()

            If filterList IsNot Nothing Then
                For Each filterItem In filterList
                    dgvFilters.Rows.Add(filterItem.Find, filterItem.Replace)
                Next
            End If
        Catch ex As Exception
            ' Silent fail
        End Try
    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged

    End Sub
End Class
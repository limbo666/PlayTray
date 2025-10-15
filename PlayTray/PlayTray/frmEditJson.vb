Imports System.IO

Public Class frmEditJson
    ' Working copy of stations (in-memory, changes here until saved)
    Private workingStations As New Dictionary(Of Integer, Station)
    Private currentStationId As Integer = 1
    Private hasChanges As Boolean = False

    Private Sub frmEditJson_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Setup NumericUpDown
            numStationID.Minimum = 1
            numStationID.Maximum = 10
            numStationID.Value = 1

            ' Load all stations from JSON into working copy
            LoadStationsFromJson()

            ' Display first station
            currentStationId = 1
            DisplayStation(1)

            hasChanges = False

        Catch ex As Exception
            MessageBox.Show("Error loading form: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub LoadStationsFromJson()
        Try
            workingStations.Clear()

            ' Load all 10 stations from StationManager
            If g_StationManager IsNot Nothing Then
                For i As Integer = 1 To 10
                    Dim station As Station = g_StationManager.GetStation(i)

                    If station IsNot Nothing Then
                        ' Create a deep copy of the station
                        Dim stationCopy As New Station With {
                        .Position = i,  ' ADD THIS LINE - Set position
                        .Name = station.Name,
                        .Link = station.Link,
                        .Remarks = station.Remarks,
                        .StationWebAddress = station.StationWebAddress
                    }

                        ' Copy filters list (deep copy) - avoid "Filter" keyword
                        If station.FiltersList IsNot Nothing Then
                            stationCopy.FiltersList = New List(Of StationFilter)()
                            For Each filterItem In station.FiltersList
                                Dim newFilter As New StationFilter()
                                newFilter.Find = filterItem.Find
                                newFilter.Replace = filterItem.Replace
                                stationCopy.FiltersList.Add(newFilter)
                            Next
                        End If

                        workingStations.Add(i, stationCopy)
                    Else
                        ' Create empty station for empty slots
                        Dim emptyStation As New Station With {.Position = i}  ' CHANGED - Set position
                        workingStations.Add(i, emptyStation)
                    End If
                Next
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading stations from JSON: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DisplayStation(stationId As Integer)
        Try
            If workingStations.ContainsKey(stationId) Then
                Dim station As Station = workingStations(stationId)

                ' Load station data into controls
                txtStationName.Text = If(station.Name, "")
                txtStationLink.Text = If(station.Link, "")
                txtStationRemark.Text = If(station.Remarks, "")
                txtStationWebAddress.Text = If(station.StationWebAddress, "")

                ' Load filters (format: Find|Replace, one per line)
                txtStationFilters.Text = ""
                If station.FiltersList IsNot Nothing AndAlso station.FiltersList.Count > 0 Then
                    Dim filterLines As New List(Of String)()
                    For Each filterItem In station.FiltersList
                        filterLines.Add($"{filterItem.Find}|{filterItem.Replace}")
                    Next
                    txtStationFilters.Text = String.Join(Environment.NewLine, filterLines)
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Error displaying station: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SaveCurrentStationToWorkingCopy()
        Try
            ' Ensure station exists in dictionary
            If Not workingStations.ContainsKey(currentStationId) Then
                workingStations.Add(currentStationId, New Station())
            End If

            Dim station As Station = workingStations(currentStationId)

            ' Save form values to working copy
            station.Position = currentStationId  ' ADD THIS LINE - Set position
            station.Name = txtStationName.Text.Trim()
            station.Link = txtStationLink.Text.Trim()
            station.Remarks = txtStationRemark.Text.Trim()
            station.StationWebAddress = txtStationWebAddress.Text.Trim()

            ' Parse filters (format: Find|Replace, one per line) - avoid "Filter" keyword
            station.FiltersList = New List(Of StationFilter)()
            If Not String.IsNullOrWhiteSpace(txtStationFilters.Text) Then
                Dim lines() As String = txtStationFilters.Text.Split(New String() {Environment.NewLine, vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                For Each line In lines
                    line = line.Trim()
                    If line.Contains("|") Then
                        Dim parts() As String = line.Split(New Char() {"|"c}, 2)
                        If parts.Length >= 2 Then
                            Dim newFilter As New StationFilter()
                            newFilter.Find = parts(0).Trim()
                            newFilter.Replace = parts(1).Trim()
                            station.FiltersList.Add(newFilter)
                        End If
                    End If
                Next
            End If

            hasChanges = True

        Catch ex As Exception
            MessageBox.Show("Error saving current station: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub numStationID_ValueChanged(sender As Object, e As EventArgs) Handles numStationID.ValueChanged
        Try
            Dim newStationId As Integer = CInt(numStationID.Value)

            If newStationId <> currentStationId Then
                ' Save current station to working copy before switching
                SaveCurrentStationToWorkingCopy()

                ' Switch to new station
                currentStationId = newStationId

                ' Load new station from working copy (not from JSON)
                DisplayStation(currentStationId)
            End If

        Catch ex As Exception
            MessageBox.Show("Error changing station: " & ex.Message, "Error")
        End Try
    End Sub

    Private Sub SaveAllStationsToJson()
        Try
            ' Save current station to working copy first
            SaveCurrentStationToWorkingCopy()

            ' Update StationManager with all changes
            If g_StationManager IsNot Nothing Then
                ' Ensure the Stations list has 10 items
                While g_StationManager.Stations.Count < 10
                    g_StationManager.Stations.Add(New Station())
                End While

                ' Update each station directly in the Stations list
                For Each kvp In workingStations
                    Dim position As Integer = kvp.Key
                    Dim station As Station = kvp.Value

                    ' Directly update the station (1-based to 0-based index)
                    If position >= 1 AndAlso position <= 10 Then
                        Dim index As Integer = position - 1

                        ' Make sure index is valid
                        If index >= 0 AndAlso index < g_StationManager.Stations.Count Then
                            ' Check if Link is empty - if so, set to null/empty station
                            If String.IsNullOrWhiteSpace(station.Link) Then
                                ' Replace with empty/null station (won't be saved to JSON)
                                g_StationManager.Stations(index) = Nothing
                            Else
                                ' Valid station with Link - save it
                                g_StationManager.Stations(index) = station
                            End If
                        End If
                    End If
                Next

                ' Save to JSON file
                g_StationManager.SaveStations()

                hasChanges = False

                '   MessageBox.Show("All stations saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Station manager not available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Error saving stations to JSON: " & ex.Message & vbCrLf & vbCrLf & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        ' Save all changes and close
        SaveAllStationsToJson()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        ' Save all changes but keep form open
        SaveAllStationsToJson()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' Check if there are unsaved changes
        'If hasChanges Then
        '    Dim result = MessageBox.Show(
        '        "You have unsaved changes. Discard them?",
        '        "Unsaved Changes",
        '        MessageBoxButtons.YesNo,
        '        MessageBoxIcon.Question,
        '        MessageBoxDefaultButton.Button2)

        '    If result = DialogResult.No Then
        '        Return ' Don't close
        '    End If
        'End If

        'Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmEditJson_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Warn about unsaved changes when closing via X button
        If e.CloseReason = CloseReason.UserClosing AndAlso hasChanges AndAlso Me.DialogResult <> DialogResult.OK Then
            Dim result = MessageBox.Show(
                "You have unsaved changes. Save before closing?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question)

            Select Case result
                Case DialogResult.Yes
                    SaveAllStationsToJson()
                Case DialogResult.Cancel
                    e.Cancel = True ' Don't close
            End Select
        End If
    End Sub
End Class
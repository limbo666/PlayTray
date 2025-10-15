Imports System.IO
Imports System.Linq
Imports Newtonsoft.Json

Public Class StationManager
    Private _stations As List(Of Station)
    Private _filePath As String

    Public ReadOnly Property Stations As List(Of Station)
        Get
            Return _stations
        End Get
    End Property

    Public Sub New(filePath As String)
        _filePath = filePath
        _stations = New List(Of Station)()
    End Sub

    Public Function LoadStations() As Boolean
        Try
            If Not File.Exists(_filePath) Then
                CreateDefaultStations()
                Return True
            End If

            Dim jsonContent As String = File.ReadAllText(_filePath)
            Dim data = JsonConvert.DeserializeObject(Of StationData)(jsonContent)

            If data IsNot Nothing AndAlso data.stations IsNot Nothing Then
                _stations = data.stations
                Return True
            End If

            Return False

        Catch ex As Exception
            MessageBox.Show("Error loading stations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function


    Public Function SaveStations() As Boolean
        Try
            ' Filter out null stations and stations with empty Link
            Dim validStations As New List(Of Station)()

            For Each station In _stations
                If station IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(station.Link) Then
                    validStations.Add(station)
                End If
            Next

            ' Create data object with only valid stations
            Dim data As New StationData With {
            .stations = validStations
        }

            ' Serialize to JSON
            Dim jsonContent As String = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented)

            ' Write to file
            File.WriteAllText(_filePath, jsonContent)

            Return True

        Catch ex As Exception
            MessageBox.Show("Error saving stations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    'Public Function SaveStations() As Boolean
    '    Try
    '        Dim data As New StationData With {
    '            .stations = _stations
    '        }

    '        Dim jsonContent As String = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented)
    '        File.WriteAllText(_filePath, jsonContent)

    '        Return True

    '    Catch ex As Exception
    '        MessageBox.Show("Error saving stations: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Return False
    '    End Try
    'End Function

    Public Function GetStation(position As Integer) As Station
        For Each s In _stations
            If s.Position = position Then
                Return s
            End If
        Next
        Return Nothing
    End Function

    Public Function GetStationByName(name As String) As Station
        For Each s In _stations
            If s.Name.Equals(name, StringComparison.OrdinalIgnoreCase) Then
                Return s
            End If
        Next
        Return Nothing
    End Function

    Public Sub AddStation(station As Station)
        Dim existing = GetStation(station.Position)
        If existing IsNot Nothing Then
            _stations.Remove(existing)
        End If

        _stations.Add(station)
        SortStations()
        SaveStations()
    End Sub

    Public Sub RemoveStation(position As Integer)
        Dim station = GetStation(position)
        If station IsNot Nothing Then
            _stations.Remove(station)
            SaveStations()
        End If
    End Sub

    Private Sub SortStations()
        _stations.Sort(Function(x, y) x.Position.CompareTo(y.Position))
    End Sub

    Private Sub CreateDefaultStations()
        _stations.Clear()

        Dim station1 As New Station()
        station1.Position = 1
        station1.Name = "Zeppelin"
        station1.Link = "https://radiostreaming.ert.gr/ert-zeppelin"
        station1.Remarks = "Greek rock radio"
        station1.DateAdded = DateTime.Now
        station1.StationWebAddress = "https://www.ert.gr/"
        station1.StationInfoLink = ""
        _stations.Add(station1)

        Dim station2 As New Station()
        station2.Position = 2
        station2.Name = "Radio Paradise AAC"
        station2.Link = "http://stream.radioparadise.com/aac-128"
        station2.Remarks = "Eclectic music"
        station2.DateAdded = DateTime.Now
        station2.StationWebAddress = "https://radioparadise.com/"
        station2.StationInfoLink = ""
        _stations.Add(station2)

        Dim station3 As New Station()
        station3.Position = 3
        station3.Name = "0N Indie"
        station3.Link = "https://0n-indie.radionetz.de/0n-indie.mp3"
        station3.Remarks = "Indie music"
        station3.DateAdded = DateTime.Now
        station3.StationWebAddress = "https://www.0nlineradio.com/"
        station3.StationInfoLink = ""
        _stations.Add(station3)

        SaveStations()
    End Sub

    Private Class StationData
        Public Property stations As List(Of Station)
    End Class
End Class
Imports System.Collections.Generic
Public Class Station


    Public Property Position As Integer
    Public Property Name As String
    Public Property Link As String
    Public Property Remarks As String
    Public Property FiltersList As List(Of StationFilter)  ' CHANGED from Filters
    Public Property DateAdded As DateTime
    Public Property StationInfoLink As String
    Public Property StationWebAddress As String

    Public Sub New()
        FiltersList = New List(Of StationFilter)()  ' CHANGED
        DateAdded = DateTime.Now
        Remarks = ""
        StationInfoLink = ""
        StationWebAddress = ""
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class StationFilter
    Public Property Find As String
    Public Property Replace As String

    Public Sub New()
        Find = ""
        Replace = ""
    End Sub

    Public Sub New(findText As String, replaceText As String)
        Find = findText
        Replace = replaceText
    End Sub
End Class
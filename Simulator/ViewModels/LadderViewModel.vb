Public Class LadderViewModel
    Inherits ScanableViewModel

    Private _Rungs As ObservableCollection(Of RungViewModel)
    Public ReadOnly Property Rungs As ObservableCollection(Of RungViewModel)
        Get
            If _Rungs Is Nothing Then _Rungs = New ObservableCollection(Of RungViewModel)
            Return _Rungs
        End Get
    End Property

    Public Sub Add(rung As RungViewModel)
        rung.Ladder = Me
        rung.Number = Rungs.Count + 1
        Rungs.Add(rung)
    End Sub

    Protected Overrides Function OnEvaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel) As Boolean
        Return True
    End Function

    Public Function GetLongestRungWithoutWiresLength() As Integer
        Dim NumElements As Integer = 0
        Dim RungsWithoutWires = (From r In Rungs Where
                                 (From e In r.Elements Where
                                 Not TypeOf e Is WireViewModel).Count < 1)
        For Each R In RungsWithoutWires
            If R.Elements.Count > NumElements Then NumElements = R.Elements.Count
        Next
        Return NumElements
    End Function

    Public Sub PadRungs()
        Dim LongestRungLength = GetLongestRungWithoutWiresLength()
        For Each R In Rungs
            R.Resize(LongestRungLength)
        Next
    End Sub

End Class

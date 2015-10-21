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
        SelectedRung = rung
    End Sub

    Private _SelectedRung As RungViewModel = Nothing
    Public Property SelectedRung As RungViewModel
        Get
            Return _SelectedRung
        End Get
        Set(ByVal Value As RungViewModel)
            SetProperty(Function() SelectedRung, _SelectedRung, Value)
        End Set
    End Property

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

#Region "DeleteSelectedRungCommand"

    Private _DeleteSelectedRungCommand As ICommand
    Public ReadOnly Property DeleteSelectedRungCommand As ICommand
        Get
            If _DeleteSelectedRungCommand Is Nothing Then _DeleteSelectedRungCommand = New RelayCommand(AddressOf DeleteSelectedRung, AddressOf CanDeleteSelectedRung)
            Return _DeleteSelectedRungCommand
        End Get
    End Property

    Private Function CanDeleteSelectedRung(obj As Object) As Boolean
        Return SelectedRung IsNot Nothing
    End Function

    Private Sub DeleteSelectedRung(obj As Object)
        Dim LastIndex = Rungs.IndexOf(SelectedRung)
        Rungs.Remove(SelectedRung)
        If LastIndex >= Rungs.Count - 1 Then
            SelectedRung = Rungs.LastOrDefault
        Else
            SelectedRung = Rungs(LastIndex)
        End If
        ReindexRungNumbers()
    End Sub

    Private Sub ReindexRungNumbers()
        Dim CurRunId As Integer = 1
        For Each R In Rungs
            R.Number = CurRunId
            CurRunId += 1
        Next
    End Sub

#End Region

#Region "AppendRungCommand"

    Private _AppendRungCommand As ICommand
    Public ReadOnly Property AppendRungCommand As ICommand
        Get
            If _AppendRungCommand Is Nothing Then _AppendRungCommand = New RelayCommand(AddressOf AppendRung, AddressOf CanAppendRung)
            Return _AppendRungCommand
        End Get
    End Property

    Private Function CanAppendRung(obj As Object) As Boolean
        Return True
    End Function

    Private Sub AppendRung(obj As Object)
        Rungs.Add(New RungViewModel With {.Ladder = Me, .Number = Rungs.Count + 1})
        SelectedRung = Rungs.LastOrDefault
    End Sub

#End Region

End Class

Public Class RungViewModel
    Inherits ScanableViewModel
    Implements IDropTarget

    Private WithEvents _Elements As ObservableCollection(Of ElementViewModel)
    Public ReadOnly Property Elements As ObservableCollection(Of ElementViewModel)
        Get
            If _Elements Is Nothing Then _Elements = New ObservableCollection(Of ElementViewModel)
            Return _Elements
        End Get
    End Property

    Private _SelectedElement As ElementViewModel = Nothing
    Public Property SelectedElement As ElementViewModel
        Get
            Return _SelectedElement
        End Get
        Set(ByVal Value As ElementViewModel)
            SetProperty(Function() SelectedElement, _SelectedElement, Value)
        End Set
    End Property

    Private _Number As Integer = -1
    Public Property Number As Integer
        Get
            Return _Number
        End Get
        Set(ByVal Value As Integer)
            SetProperty(Function() Number, _Number, Value)
        End Set
    End Property

    Private _IsBranch As Boolean = False
    Public Property IsBranch As Boolean
        Get
            Return _IsBranch
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsBranch, _IsBranch, Value)
        End Set
    End Property

    Public ReadOnly Property CanBeScanned As Boolean
        Get
            Dim Last = Elements.Last
            Return Last IsNot Nothing AndAlso TypeOf Last Is CoilViewModel
        End Get
    End Property

    Private _Ladder As LadderViewModel = Nothing
    Public Property Ladder As LadderViewModel
        Get
            Return _Ladder
        End Get
        Set(ByVal Value As LadderViewModel)
            SetProperty(Function() Ladder, _Ladder, Value)
        End Set
    End Property

    Protected Overrides Function OnEvaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel) As Boolean
        Return Elements.Last IsNot Nothing AndAlso Elements.Last.IsSet
    End Function

    Public Function GetPreviousElement(element As ElementViewModel) As ElementViewModel
        Dim ElementsCopy = Elements.ToList
        If ElementsCopy.First Is element Then Return Nothing
        Dim Index = ElementsCopy.IndexOf(element)
        Return ElementsCopy(Index - 1)
    End Function

    Public Function GetNextElement(element As ElementViewModel) As ElementViewModel
        Dim ElementsCopy = Elements.ToList
        If Elements.Last Is element Then Return Nothing
        Dim Index = Elements.IndexOf(element)
        Return ElementsCopy(Index + 1)
    End Function

    Public ReadOnly Property FirstElement As ElementViewModel
        Get
            Return Elements.FirstOrDefault
        End Get
    End Property

    Public ReadOnly Property LastElement As ElementViewModel
        Get
            Return Elements.LastOrDefault
        End Get
    End Property

    Public Sub Resize(newSize As Integer)
        If Elements.Count.Equals(newSize) Then Return
        Dim LE = Elements.Last
        Dim LEIndex = Elements.IndexOf(LE)
        While Elements.Count < newSize
            Dim VM As New WireViewModel
            VM.Rung = Me
            Elements.Insert(LEIndex - 1, VM)
        End While
        For Each E In Elements
            E.Invalidate()
        Next
    End Sub

    Public Sub RemoveLines()
        Dim Lines = (From el In Elements Where TypeOf el Is WireViewModel)
        For Each L In Lines
            Elements.Remove(L)
        Next
    End Sub

    Public Sub DragOver(dropInfo As IDropInfo) Implements IDropTarget.DragOver
        If Not TypeOf dropInfo.Data Is ElementViewModel Then
            dropInfo.NotHandled = True
            Return
        End If
        Dim source = DirectCast(dropInfo.Data, ElementViewModel)
        Dim AcceptItem As Boolean = True
        If TypeOf source Is CoilViewModel Then
            Dim CoilsExist As Boolean = False
            For Each El In Elements
                If TypeOf El Is CoilViewModel Then
                    dropInfo.Effects = DragDropEffects.None
                    AcceptItem = False
                    Exit For
                End If
            Next
        End If
        If AcceptItem Then
            If source IsNot Nothing AndAlso source.IsTemplate Then
                dropInfo.Effects = DragDropEffects.Copy
            Else
                dropInfo.Effects = DragDropEffects.Move
            End If
        End If
        If AcceptItem Then GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo)
    End Sub

    Public Sub Drop(dropInfo As IDropInfo) Implements IDropTarget.Drop
        Dim Element = DirectCast(dropInfo.Data, ElementViewModel)
        Dim SourceCollection = DirectCast(dropInfo.DragInfo.SourceCollection, ObservableCollection(Of ElementViewModel))
        If Element.IsTemplate Then
            Element = DirectCast(Element.Clone, ElementViewModel)
            Element.IsTemplate = False
        Else
            SourceCollection.Remove(Element)
        End If
        If Element Is Nothing Then
            dropInfo.Effects = DragDropEffects.None
            Return
        End If
        Element.Rung = Me
        If Elements.Count = 0 Or dropInfo.InsertIndex >= Elements.Count Or TypeOf Element Is CoilViewModel Then
            Elements.Add(Element)
        Else
            Dim LE = LastElement
            If LE Is Nothing Then
                Elements.Add(Element)
            Else
                Elements.Insert(dropInfo.InsertIndex, Element)
            End If
        End If
        Ladder.PadRungs()
        SelectedElement = Element
        If SourceCollection IsNot Nothing Then SourceCollection.ToList.ForEach(Sub(x) x.Invalidate())
    End Sub

#Region "DeleteSelectedElementCommand"

    Private _DeleteSelectedElementCommand As ICommand
    Public ReadOnly Property DeleteSelectedElementCommand As ICommand
        Get
            If _DeleteSelectedElementCommand Is Nothing Then _DeleteSelectedElementCommand = New RelayCommand(AddressOf DeleteSelectedElement, AddressOf CanDeleteSelectedElement)
            Return _DeleteSelectedElementCommand
        End Get
    End Property

    Private Function CanDeleteSelectedElement(obj As Object) As Boolean
        Return SelectedElement IsNot Nothing
    End Function

    Private Sub DeleteSelectedElement(obj As Object)
        Dim LastIndex = Elements.IndexOf(SelectedElement)
        Elements.Remove(SelectedElement)
        If LastIndex >= Elements.Count - 1 Then
            SelectedElement = Elements.LastOrDefault
        Else
            SelectedElement = Elements(LastIndex)
        End If
    End Sub

#End Region

End Class

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

    Public ReadOnly Property TotalElementCount As Integer
        Get
            Return Elements.Count
        End Get
    End Property

    Public ReadOnly Property WireElementCount As Integer
        Get
            Return TotalElementCount - NonWireElementCount
        End Get
    End Property

    Public ReadOnly Property NonWireElementCount As Integer
        Get
            Dim RetVal As Integer = 0
            For Each E In Elements
                If Not TypeOf E Is WireViewModel Then RetVal += 1
            Next
            Return RetVal
        End Get
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

    Public Sub AddElement(element As ElementViewModel)
        Elements.Add(element)
        ResizeLadderRungs()
    End Sub

    Public Sub InsertElement(element As ElementViewModel, index As Integer)
        Elements.Insert(index, element)
        ResizeLadderRungs()
    End Sub

    Private Sub ResizeLadderRungs()
        If Ladder IsNot Nothing Then Ladder.ResizeRungs()
    End Sub

    Public Sub RemoveElement(element As ElementViewModel)
        Dim LastIndex = Elements.IndexOf(element)
        Elements.Remove(element)
        If element Is SelectedElement Then
            If LastIndex >= Elements.Count - 1 Then
                SelectedElement = Elements.LastOrDefault
            Else
                SelectedElement = Elements(LastIndex)
            End If
        End If
        ResizeLadderRungs()
    End Sub

    Public Sub RemoveSelectedElement()
        RemoveElement(SelectedElement)
    End Sub

    Public Sub Resize(newSize As Integer)
        If Elements.Count = newSize Or newSize < NonWireElementCount Then Return

        If newSize > Elements.Count Then
            Dim LE = Elements.Last
            Dim LEIndex = Elements.IndexOf(LE)
            While Elements.Count < newSize
                Dim VM As New WireViewModel
                VM.Rung = Me
                Elements.Insert(LEIndex, VM)
            End While
        ElseIf newSize < Elements.Count Then
            Dim NumWiresToRemove = Elements.Count - newSize
            For I = 0 To NumWiresToRemove - 1
                Dim Wires = (From e In Elements Where TypeOf e Is WireViewModel)
                Elements.Remove(Wires.Last)
            Next
        End If
        For Each E In Elements
            E.Invalidate()
        Next
    End Sub

    Public Sub RemoveWires()
        Dim Lines = (From el In Elements Where TypeOf el Is WireViewModel)
        For Each L In Lines
            RemoveElement(L)
        Next
    End Sub

    Private _IsReceiverOfCurrentDraggedItem As Boolean
    Public ReadOnly Property IsReceiverOfCurrentDraggedItem As Boolean
        Get
            Return _IsReceiverOfCurrentDraggedItem
        End Get
    End Property

    Private Function CheckIfDraggedItemCanBeAccepted(item As Object) As Boolean
        Dim IsAcceptable = CanAcceptDraggedItem(item)
        SetProperty(Function() IsReceiverOfCurrentDraggedItem, _IsReceiverOfCurrentDraggedItem, IsAcceptable)
        Return IsAcceptable
    End Function

    Protected Overridable Function CanAcceptDraggedItem(item As Object) As Boolean
        Dim IsItemNothing = item Is Nothing
        Dim IsItemCorrectType = TypeOf item Is ElementViewModel
        Return Not IsItemNothing And IsItemCorrectType
    End Function

    Public Sub DragOver(dropInfo As IDropInfo) Implements IDropTarget.DragOver
        'Initial assumption is that we cannot accept the dragged item
        dropInfo.Effects = DragDropEffects.None
        dropInfo.NotHandled = True

        'Check if we can accept the dragged item
        If CheckIfDraggedItemCanBeAccepted(dropInfo.Data) Then
            'We can so update the dropInfo with correct info
            dropInfo.Effects = DragDropEffects.Copy
            dropInfo.NotHandled = False

            'Since we can accept the item and we know it's a type of ElementViewModel then cast the dropInfo.Data to ElementViewModel
            Dim Element = DirectCast(dropInfo.Data, ElementViewModel)
            'If we have a template than don't move, just copy
            If Element.IsTemplate Then dropInfo.Effects = DragDropEffects.Copy

        End If
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo)
    End Sub

    Public Sub Drop(dropInfo As IDropInfo) Implements IDropTarget.Drop
        'We don't need null items or sourc collections
        If dropInfo.Data Is Nothing Or dropInfo.DragInfo.SourceCollection Is Nothing Then Return
        'We require an element and a source collection
        If Not TypeOf dropInfo.Data Is ElementViewModel Or Not TypeOf dropInfo.DragInfo.SourceCollection Is ObservableCollection(Of ElementViewModel) Then Return

        SetProperty(Function() IsReceiverOfCurrentDraggedItem, _IsReceiverOfCurrentDraggedItem, False)

        Dim Element = DirectCast(dropInfo.Data, ElementViewModel)
        Dim SourceCollection = DirectCast(dropInfo.DragInfo.SourceCollection, ObservableCollection(Of ElementViewModel))

        'If this element is a template (dragged from the available elements panel)
        If Element.IsTemplate Then
            'Then copy (clone) the element
            Element = DirectCast(Element.Clone, ElementViewModel)
            'and set it to be a usable element
            Element.IsTemplate = False
            'We also don't want to remove the element because that would prevent future use of the template element in the available elements panel
        Else
            'Since this element is not a template (we received it from another rung) then we remove it from the source collection
            SourceCollection.Remove(Element)
        End If

        'The elements new home is here
        Element.Rung = Me

        'If this is our first element, the insert postion is after our last element, or if it's an end-only element (currently only CoilViewModel)
        If Elements.Count = 0 Or dropInfo.InsertIndex >= Elements.Count Or TypeOf Element Is CoilViewModel Then
            'then we add the element to the end of our existing elements collection
            AddElement(Element)
        Else
            'Otherwise we insert it at the dropInfo.InsertIndex position
            InsertElement(Element, dropInfo.InsertIndex)
        End If

        'Tell our ladder to update the other rungs to match our element count (this may go away later and be replaced by a specified maximum rung element count)
        Ladder.ResizeRungs()

        'Set the selected element of our elements collecftion as the newly added element for user friendliness
        SelectedElement = Element

        'Tell the source collection that it needs to update (invalidate) it's elements so they can cause a redraw for visual effect
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
        RemoveSelectedElement()
    End Sub

#End Region

End Class

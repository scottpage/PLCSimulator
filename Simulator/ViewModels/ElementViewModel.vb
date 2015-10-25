Public MustInherit Class ElementViewModel
    Inherits ScanableViewModel
    Implements ICloneable, IDragSource, IDropTarget

    Protected Sub New(tag As TagViewModel, actionType As ElementActionType)
        _Id = Guid.NewGuid
        _Tag = tag
        _ActionType = actionType
        _IsSelectable = True
    End Sub

    Protected Sub New(tag As TagViewModel, actionType As ElementActionType, isSelectable As Boolean)
        _Id = Guid.NewGuid
        _Tag = tag
        _ActionType = actionType
        _IsSelectable = isSelectable
    End Sub

    Protected Sub New(tag As TagViewModel, id As Guid, actionType As ElementActionType)
        _Id = id
        _Tag = tag
        _ActionType = actionType
        _IsSelectable = True
    End Sub

    Protected Sub New(tag As TagViewModel, id As Guid, actionType As ElementActionType, isSelectable As Boolean)
        _Id = id
        _Tag = tag
        _ActionType = actionType
        _IsSelectable = isSelectable
    End Sub

    Private _ActionType As ElementActionType = ElementActionType.None
    Public ReadOnly Property ActionType As ElementActionType
        Get
            Return _ActionType
        End Get
    End Property

    Private _Id As Guid
    Public Property Id As Guid
        Get
            Return _Id
        End Get
        Set(ByVal Value As Guid)
            SetProperty(Function() Id, _Id, Value)
        End Set
    End Property

    Private _IsTemplate As Boolean = False
    Public Property IsTemplate As Boolean
        Get
            Return _IsTemplate
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsTemplate, _IsTemplate, Value)
        End Set
    End Property

    Private _IsSelectable As Boolean = True
    Public ReadOnly Property IsSelectable As Boolean
        Get
            Return _IsSelectable
        End Get
    End Property

    Private _SupportsTags As Boolean = True
    Public Overridable ReadOnly Property SupportsTags As Boolean
        Get
            Return _SupportsTags
        End Get
    End Property

    Private _Rung As RungViewModel = Nothing
    Public Property Rung As RungViewModel
        Get
            Return _Rung
        End Get
        Set(ByVal Value As RungViewModel)
            SetProperty(Function() Rung, _Rung, Value)
        End Set
    End Property

    Public ReadOnly Property IsSet As Boolean
        Get
            Return _EvaulationResult
        End Get
    End Property

    Public ReadOnly Property IsRungLeftEnd As Boolean
        Get
            Return GetPreviousElement() Is Nothing
        End Get
    End Property

    Public ReadOnly Property IsRungRightEnd As Boolean
        Get
            Return GetNextElement() Is Nothing
        End Get
    End Property

    Private _Tag As TagViewModel = Nothing
    Public Property Tag As TagViewModel
        Get
            Return _Tag
        End Get
        Set(value As TagViewModel)
            SetProperty(Function() Tag, _Tag, value)
        End Set
    End Property

    Private _EvaulationResult As Boolean = False

    Protected Overrides Function OnEvaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel) As Boolean
        Dim PreviousElement = DirectCast(previousScanable, ElementViewModel)
        Dim NextElement = DirectCast(nextScanable, ElementViewModel)
        _EvaulationResult = OnEvaluateElement(PreviousElement, NextElement)
        Return _EvaulationResult
    End Function

    Protected Overridable Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        Return (previousElement Is Nothing) Or (previousElement IsNot Nothing AndAlso previousElement.IsSet)
    End Function

    Protected Overrides Sub OnEvaluationComplete()
        OnPropertyChanged(Function() IsSet)
        Invalidate()
    End Sub

    Private Function GetPreviousElement() As ElementViewModel
        If Rung Is Nothing Then Return Nothing
        Return Rung.GetPreviousElement(Me)
    End Function

    Private Function GetNextElement() As ElementViewModel
        If Rung Is Nothing Then Return Nothing
        Return Rung.GetNextElement(Me)
    End Function

    Public MustOverride Function Clone() As Object Implements ICloneable.Clone

    Public Overridable Sub Invalidate()
        OnPropertyChanged(Function() IsRungLeftEnd)
        OnPropertyChanged(Function() IsRungRightEnd)
    End Sub

#Region "RemoveCommand"

    Private _RemoveCommand As ICommand
    Public ReadOnly Property RemoveCommand As ICommand
        Get
            If _RemoveCommand Is Nothing Then _RemoveCommand = New RelayCommand(AddressOf Remove, AddressOf CanRemove)
            Return _RemoveCommand
        End Get
    End Property

    Private Function CanRemove(obj As Object) As Boolean
        Return Rung IsNot Nothing AndAlso Rung.Elements.Contains(Me)
    End Function

    Private Sub Remove(obj As Object)
        Rung.DeleteSelectedElementCommand.Execute(Nothing)
    End Sub

#End Region

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
        Dim IsTagNothing = Tag Is Nothing
        Dim IsItemSameTypeAsTag = Not IsTagNothing AndAlso Tag.GetType.Equals(item.GetType)
        Return IsItemNothing Or IsItemSameTypeAsTag
    End Function

    Public Overridable Sub DragOver(dropInfo As IDropInfo) Implements IDropTarget.DragOver
        'Initial assumption is that we cannot accept the dragged item
        dropInfo.Effects = DragDropEffects.None
        dropInfo.NotHandled = True

        'Since this is an element the only type allowed, at the moment, is TagViewModel
        'Checking for Tag not nothing fixes issue #1 in master (drag over non tag element)
        'If Not TypeOf dropInfo.Data Is TagViewModel Or Tag Is Nothing Then Return  'This may not be needed since we are checking for an acceptable item later

        'Check if we can accept the dragged item
        If CheckIfDraggedItemCanBeAccepted(dropInfo.Data) Then
            'We can so update the dropInfo with correct info
            dropInfo.Effects = DragDropEffects.Copy
            dropInfo.NotHandled = False
        End If
        'Let Gong handle the rest
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo)
    End Sub

    Public Overridable Sub Drop(dropInfo As IDropInfo) Implements IDropTarget.Drop
        If dropInfo.Data Is Nothing Then
            Tag = Nothing
        Else
            If IsReceiverOfCurrentDraggedItem Then Tag = DirectCast(dropInfo.Data, TagViewModel)
        End If
        SetProperty(Function() IsReceiverOfCurrentDraggedItem, _IsReceiverOfCurrentDraggedItem, False)
    End Sub

    Public Function CanStartDrag(dragInfo As IDragInfo) As Boolean Implements IDragSource.CanStartDrag
        Return dragInfo.MouseButton = MouseButton.Left And IsSelectable
    End Function

    Public Sub DragCancelled() Implements IDragSource.DragCancelled
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.DragCancelled()
    End Sub

    Public Sub Dropped(dropInfo As IDropInfo) Implements IDragSource.Dropped
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.Dropped(dropInfo)
    End Sub

    Public Sub StartDrag(dragInfo As IDragInfo) Implements IDragSource.StartDrag
        If IsTemplate Then
            dragInfo.Effects = DragDropEffects.Copy
        Else
            dragInfo.Effects = DragDropEffects.Move
        End If
        dragInfo.Data = Me
        GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.StartDrag(dragInfo)
    End Sub

End Class

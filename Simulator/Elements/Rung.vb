Public Class Rung
    Inherits StackPanel

    Sub New()
        SetValue(ElementsPropertyKey, New UIElementCollection(Me, Me))
        Orientation = Controls.Orientation.Horizontal
        HorizontalAlignment = Windows.HorizontalAlignment.Stretch
        MinHeight = 150
        'AddHandler Elements.CollectionChanged, AddressOf Elements_Changed
    End Sub

    Public ReadOnly Property Elements As UIElementCollection
        Get
            Return DirectCast(GetValue(Rung.ElementsProperty), UIElementCollection)
        End Get
    End Property

    Private Shared ReadOnly ElementsPropertyKey As DependencyPropertyKey = _
                            DependencyProperty.RegisterReadOnly("Elements", _
                            GetType(UIElementCollection), GetType(Rung), _
                            New PropertyMetadata(Nothing))

    Public Shared ReadOnly ElementsProperty As DependencyProperty = _
                           ElementsPropertyKey.DependencyProperty

    'Private Sub Elements_Changed(sender As Object, e As Specialized.NotifyCollectionChangedEventArgs)
    '    Select Case e.Action
    '        Case Specialized.NotifyCollectionChangedAction.Add
    '            For Each NI In e.NewItems
    '                Dim R = DirectCast(NI, Element)
    '                AddVisualChild(R)
    '            Next
    '        Case Specialized.NotifyCollectionChangedAction.Remove
    '            For Each NI In e.OldItems
    '                Dim R = DirectCast(NI, Element)
    '                RemoveVisualChild(R)
    '            Next
    '        Case Specialized.NotifyCollectionChangedAction.Reset
    '            For I = 0 To VisualChildrenCount - 1
    '                RemoveVisualChild(GetVisualChild(I))
    '            Next
    '    End Select
    'End Sub

    Protected NotOverridable Overrides ReadOnly Property LogicalOrientation As Orientation
        Get
            Return Orientation.Horizontal
        End Get
    End Property

    Protected Overrides Sub OnRender(dc As DrawingContext)
        MyBase.OnRender(dc)
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        If Not TypeOf e.Source Is Element Or e.Source Is Nothing Then MyBase.OnDragEnter(e) : Return
        AddVisualChild(CType(e.Source, Visual))
    End Sub

End Class

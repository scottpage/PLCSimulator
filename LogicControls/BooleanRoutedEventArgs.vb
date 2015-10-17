Public Class BooleanRoutedEventArgs
    Inherits RoutedEventArgs

    Public Sub New(routedEvent As RoutedEvent, oldValue As Boolean, newValue As Boolean)
        MyBase.New(routedEvent)
        _OldValue = oldValue
        _NewValue = newValue
    End Sub

    Private _OldValue As Boolean
    Public ReadOnly Property OldValue As Boolean
        Get
            Return _OldValue
        End Get
    End Property

    Private _NewValue As Boolean
    Public ReadOnly Property NewValue As Boolean
        Get
            Return _NewValue
        End Get
    End Property

End Class

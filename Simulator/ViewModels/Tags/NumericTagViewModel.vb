Public Class NumericTagViewModel
    Inherits TagViewModel

    Private _Value As Single
    Public Property Value As Single
        Get
            Return _Value
        End Get
        Set(ByVal Value As Single)
            SetProperty(Function() Value, _Value, Value)
        End Set
    End Property

End Class

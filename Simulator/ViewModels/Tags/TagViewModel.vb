Public MustInherit Class TagViewModel
    Inherits ViewModelBase

    Private _Name As String = String.Empty
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            SetProperty(Function() Name, _Name, Value)
        End Set
    End Property

End Class

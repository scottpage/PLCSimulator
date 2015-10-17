Public MustInherit Class DiscreteViewModel
    Inherits ElementViewModel

    Protected Sub New()
        MyBase.New(New DiscreteTagViewModel, ElementActionType.Toggle)
    End Sub

    Protected Sub New(id As Guid)
        MyBase.New(New DiscreteTagViewModel, id, ElementActionType.Toggle)
    End Sub

    Public ReadOnly Property DiscreteTag As DiscreteTagViewModel
        Get
            Return DirectCast(Tag, DiscreteTagViewModel)
        End Get
    End Property

End Class

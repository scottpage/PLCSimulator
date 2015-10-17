Public MustInherit Class NumericElementViewModel
    Inherits ElementViewModel

    Protected Sub New(tag As NumericTagViewModel)
        MyBase.New(tag, ElementActionType.Numeric)
    End Sub

    Protected Sub New(tag As NumericTagViewModel, id As Guid)
        MyBase.New(tag, id, ElementActionType.Numeric)
    End Sub

    Public Overrides Function Clone() As Object
        Return Nothing
    End Function

End Class

Public Class CoilViewModel
    Inherits DiscreteViewModel

    Public Sub New()
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(id)
    End Sub

    Public Overrides Function Clone() As Object
        Return New CoilViewModel With {.Tag = Tag, .Rung = Rung, .IsTemplate = IsTemplate}
    End Function

    Protected Overrides Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        DiscreteTag.Value = previousElement.IsSet
        Return previousElement.IsSet
    End Function

End Class

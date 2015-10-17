Public Class ContactViewModel
    Inherits DiscreteViewModel

    Public Sub New()
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(id)
    End Sub

    Private _IsNormallyClosed As Boolean = False
    Public Property IsNormallyClosed As Boolean
        Get
            Return _IsNormallyClosed
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsNormallyClosed, _IsNormallyClosed, Value)
        End Set
    End Property

    Protected Overrides Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        If Not MyBase.OnEvaluateElement(previousElement, nextElement) Then Return False
        If (Not DiscreteTag.Value And IsNormallyClosed) Then Return True
        If (DiscreteTag.Value And Not IsNormallyClosed) Then Return True
        Return False
    End Function

    Public Overrides Function Clone() As Object
        Return New ContactViewModel With {.Tag = Tag, .Rung = Rung, .IsNormallyClosed = IsNormallyClosed, .IsTemplate = IsTemplate}
    End Function

End Class

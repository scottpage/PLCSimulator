Public Class WireViewModel
    Inherits ElementViewModel

    Public Sub New()
        MyBase.New(Nothing, ElementActionType.None)
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(Nothing, id, ElementActionType.None)
    End Sub

    Public Overrides Function Clone() As Object
        Return New WireViewModel
    End Function

End Class

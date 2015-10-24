Public Class WireViewModel
    Inherits ElementViewModel

    Public Sub New()
        MyBase.New(Nothing, ElementActionType.None, False)
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(Nothing, id, ElementActionType.None, False)
    End Sub

    Public Overrides ReadOnly Property SupportsTags As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function Clone() As Object
        Return New WireViewModel
    End Function

End Class

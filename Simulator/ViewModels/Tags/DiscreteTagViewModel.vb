Public Class DiscreteTagViewModel
    Inherits TagViewModel

    Private _Value As Boolean = False
    Public Property Value As Boolean
        Get
            Return _Value
        End Get
        Set(ByVal newValue As Boolean)
            SetProperty(Function() Value, _Value, newValue)
        End Set
    End Property

#Region "Commands"

#Region "ToggleCommand"

    Private _ToggleCommand As ICommand
    Public ReadOnly Property ToggleCommand As ICommand
        Get
            If _ToggleCommand Is Nothing Then _ToggleCommand = New RelayCommand(AddressOf Toggle, AddressOf CanToggle)
            Return _ToggleCommand
        End Get
    End Property

    Private Function CanToggle(obj As Object) As Boolean
        Return True
    End Function

    Private Sub Toggle(obj As Object)
        Value = Not Value
    End Sub

#End Region



#End Region

End Class

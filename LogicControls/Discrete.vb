Public MustInherit Class Discrete
    Inherits LogicControl

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Discrete), New FrameworkPropertyMetadata(GetType(Discrete)))
    End Sub

    Public Property State As Boolean
        Get
            Return Convert.ToBoolean(GetValue(StateProperty))
        End Get

        Set(ByVal value As Boolean)
            SetValue(StateProperty, value)
        End Set
    End Property

    Public Shared ReadOnly StateProperty As DependencyProperty = _
                           DependencyProperty.Register("State", _
                           GetType(Boolean), GetType(Discrete), _
                           New PropertyMetadata(False))

    Private _ToggleStateCommand As ICommand
    Public ReadOnly Property ToggleStateCommand As ICommand
        Get
            If _ToggleStateCommand Is Nothing Then _ToggleStateCommand = New RelayCommand(AddressOf Toggle, AddressOf CanToggle)
            Return _ToggleStateCommand
        End Get
    End Property

    Private Function CanToggle(obj As Object) As Boolean
        Return True
    End Function

    Private Sub Toggle(obj As Object)
        State = Not State
    End Sub

    Protected Overrides Sub OnBuildContextMenu(menu As ContextMenu)
        MyBase.OnBuildContextMenu(menu)
        Dim ToggleItem As New MenuItem
        ToggleItem.Name = "ToggleMenuItem"
        ToggleItem.Header = "Toggle"
        ToggleItem.Command = ToggleStateCommand
        menu.Items.Add(ToggleItem)
    End Sub

    Protected Overrides Sub OnReset()
        MyBase.OnReset()
        State = DirectCast(Discrete.StateProperty.DefaultMetadata.DefaultValue, Boolean)
    End Sub

End Class

Public Class Contact
    Inherits Discrete

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Contact), New FrameworkPropertyMetadata(GetType(Contact)))
    End Sub

    Public Property IsNormallyClosed As Boolean
        Get
            Return Convert.ToBoolean(GetValue(IsNormallyClosedProperty))
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsNormallyClosedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IsNormallyClosedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsNormallyClosed", _
                           GetType(Boolean), GetType(Contact), _
                           New PropertyMetadata(False))

    Private _IsNormallyClosedItem As MenuItem

    Protected Overrides Sub OnBuildContextMenu(menu As ContextMenu)
        MyBase.OnBuildContextMenu(menu)
        _IsNormallyClosedItem = New MenuItem
        _IsNormallyClosedItem.IsCheckable = True
        _IsNormallyClosedItem.IsChecked = IsNormallyClosed
        _IsNormallyClosedItem.Name = "ToggleIsNormallyClosedMenuItem"
        _IsNormallyClosedItem.Header = "Normally closed"
        _IsNormallyClosedItem.Command = ToggleIsNormallyClosedCommand
        menu.Items.Add(_IsNormallyClosedItem)
    End Sub

    Protected Overrides Sub OnReset()
        MyBase.OnReset()
        IsNormallyClosed = DirectCast(Contact.IsNormallyClosedProperty.DefaultMetadata.DefaultValue, Boolean)
        _IsNormallyClosedItem.IsChecked = IsNormallyClosed
    End Sub

    Private _ToggleIsNormallyClosedCommand As ICommand
    Public ReadOnly Property ToggleIsNormallyClosedCommand As ICommand
        Get
            If _ToggleIsNormallyClosedCommand Is Nothing Then _ToggleIsNormallyClosedCommand = New RelayCommand(AddressOf ToggleIsNormallyClosed, AddressOf CanToggleIsNormallyClosed)
            Return _ToggleIsNormallyClosedCommand
        End Get
    End Property

    Private Function CanToggleIsNormallyClosed(obj As Object) As Boolean
        Return True
    End Function

    Private Sub ToggleIsNormallyClosed(obj As Object)
        IsNormallyClosed = Not IsNormallyClosed
    End Sub

End Class

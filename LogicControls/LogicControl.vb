Public MustInherit Class LogicControl
    Inherits Control

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(LogicControl), New FrameworkPropertyMetadata(GetType(LogicControl)))
    End Sub

    Public Custom Event IsSelectedChanged As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(IsSelectedChangedEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(IsSelectedChangedEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Shared ReadOnly IsSelectedChangedEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("IsSelectedChanged", _
                      RoutingStrategy.Bubble, _
                      GetType(RoutedEventHandler), GetType(LogicControl))

    Public Custom Event PreviewIsSelectedChanged As RoutedEventHandler
        AddHandler(ByVal value As RoutedEventHandler)
            Me.AddHandler(PreviewIsSelectedChangedEvent, value)
        End AddHandler
        RemoveHandler(ByVal value As RoutedEventHandler)
            Me.RemoveHandler(PreviewIsSelectedChangedEvent, value)
        End RemoveHandler
        RaiseEvent(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.RaiseEvent(e)
        End RaiseEvent
    End Event

    Public Shared ReadOnly PreviewIsSelectedChangedEvent As RoutedEvent = _
                      EventManager.RegisterRoutedEvent("PreviewIsSelectedChanged", _
                      RoutingStrategy.Tunnel, _
                      GetType(RoutedEventHandler), GetType(LogicControl))

    Public Property IsSelected As Boolean
        Get
            Return Convert.ToBoolean(GetValue(IsSelectedProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(IsSelectedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IsSelectedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSelected", _
                           GetType(Boolean), GetType(LogicControl), _
                           New PropertyMetadata(False))

    Public Property TagName As String
        Get
            Return GetValue(TagNameProperty).ToString
        End Get

        Set(ByVal value As String)
            SetValue(TagNameProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TagNameProperty As DependencyProperty = _
                           DependencyProperty.Register("TagName", _
                           GetType(String), GetType(LogicControl), _
                           New PropertyMetadata(String.Empty))




    Protected Overrides Sub OnPreviewMouseUp(e As MouseButtonEventArgs)
        If e.ClickCount.Equals(1) And e.ChangedButton = MouseButton.Left Then
            Dim WasSelected = IsSelected
            IsSelected = Not IsSelected
            If Not WasSelected.Equals(IsSelected) Then OnIsSelectedChanged(WasSelected, IsSelected)
        End If
        MyBase.OnPreviewMouseUp(e)
    End Sub

    Protected Overridable Sub OnIsSelectedChanged(oldValue As Boolean, newValue As Boolean)
        If _TriggerIsSelectedChangedEvents Then
            RaiseIsSelectedChangedEvent(oldValue, newValue)
            RaisePreviewIsSelectedEvent(oldValue, newValue)
        End If
    End Sub

    Protected Sub RaiseIsSelectedChangedEvent(oldValue As Boolean, newValue As Boolean)
        Dim Args As New BooleanRoutedEventArgs(IsSelectedChangedEvent, oldValue, newValue)
        MyBase.RaiseEvent(Args)
    End Sub

    Protected Sub RaisePreviewIsSelectedEvent(oldValue As Boolean, newValue As Boolean)
        Dim Args As New BooleanRoutedEventArgs(PreviewIsSelectedChangedEvent, oldValue, newValue)
        MyBase.RaiseEvent(Args)
    End Sub

    Private _TriggerIsSelectedChangedEvents As Boolean = True
    Public Sub UnSelect(triggerEvents As Boolean)
        _TriggerIsSelectedChangedEvents = triggerEvents
        IsSelected = False
        _TriggerIsSelectedChangedEvents = True
    End Sub

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        BuildContextMenu()
    End Sub

    Private Sub BuildContextMenu()
        Dim ResetItem As MenuItem = Nothing
        If ContextMenu Is Nothing Then ContextMenu = New ContextMenu
        OnBuildContextMenu(ContextMenu)
        For Each I In ContextMenu.Items
            If TypeOf I Is MenuItem Then
                Dim MI = DirectCast(I, MenuItem)
                If MI.Name.Equals("ResetMenuItem") Then
                    ResetItem = MI
                    Exit For
                End If
            End If
        Next
        If ResetItem Is Nothing Then
            ResetItem = New MenuItem
            ResetItem.Name = "ResetMenuItem"
            ResetItem.Header = "_Reset"
            ResetItem.Command = ResetCommand
            ContextMenu.Items.Insert(ContextMenu.Items.Count, New Separator)
            ContextMenu.Items.Insert(ContextMenu.Items.Count, ResetItem)
        End If
    End Sub

    Protected Overridable Sub OnBuildContextMenu(menu As ContextMenu)
    End Sub

    Private _ResetCommand As ICommand
    Public ReadOnly Property ResetCommand As ICommand
        Get
            If _ResetCommand Is Nothing Then _ResetCommand = New RelayCommand(AddressOf Reset, AddressOf CanReset)
            Return _ResetCommand
        End Get
    End Property

    Private Function CanReset(obj As Object) As Boolean
        Return True
    End Function

    Private Sub Reset(obj As Object)
        OnReset()
    End Sub

    Protected Overridable Sub OnReset()
        IsSelected = DirectCast(LogicControl.IsSelectedProperty.DefaultMetadata.DefaultValue, Boolean)
    End Sub

End Class

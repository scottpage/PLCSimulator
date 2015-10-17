Imports System.ComponentModel

Public Class Branch
    Inherits Discrete

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Branch), New FrameworkPropertyMetadata(GetType(Branch)))
    End Sub

    Public Property BranchType As BranchType
        Get
            Return DirectCast(GetValue(BranchTypeProperty), BranchType)
        End Get

        Set(ByVal value As BranchType)
            SetValue(BranchTypeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly BranchTypeProperty As DependencyProperty = _
                           DependencyProperty.Register("BranchType", _
                           GetType(BranchType), GetType(Branch), _
                           New PropertyMetadata(BranchType.ThroughDown, New PropertyChangedCallback(AddressOf BranchType_PropertyChanged)))

    Protected Overrides Sub OnReset()
        MyBase.OnReset()
        BranchType = DirectCast(Branch.BranchTypeProperty.DefaultMetadata.DefaultValue, BranchType)
        UpdateBranchTypeMenuItems()
    End Sub

    Private Sub UpdateBranchTypeMenuItems()
        If ContextMenu Is Nothing Then Return
        _ThroughDownMenuItem.IsChecked = BranchType = LogicControls.BranchType.ThroughDown
        _ThroughUpMenuItem.IsChecked = BranchType = LogicControls.BranchType.ThroughUp
        _LeftDownMenuItem.IsChecked = BranchType = LogicControls.BranchType.LeftDown
        _LeftUpMenuItem.IsChecked = BranchType = LogicControls.BranchType.LeftUp
    End Sub

    Private _BranchTypeMenuItem As MenuItem
    Private _ThroughDownMenuItem As MenuItem
    Private _ThroughUpMenuItem As MenuItem
    Private _LeftDownMenuItem As MenuItem
    Private _LeftUpMenuItem As MenuItem

    Protected Overrides Sub OnBuildContextMenu(menu As ContextMenu)
        MyBase.OnBuildContextMenu(menu)
        _BranchTypeMenuItem = New MenuItem
        _BranchTypeMenuItem.Name = "BranchTypeMenuItem"
        _BranchTypeMenuItem.Header = "Type"
        menu.Items.Add(_BranchTypeMenuItem)

        _ThroughDownMenuItem = New MenuItem
        _ThroughDownMenuItem.Name = "ThroughDownMenuItem"
        _ThroughDownMenuItem.IsCheckable = True
        _ThroughDownMenuItem.Header = "Through down"
        _ThroughDownMenuItem.Command = SelectThroughDownCommand
        _BranchTypeMenuItem.Items.Add(_ThroughDownMenuItem)

        _ThroughUpMenuItem = New MenuItem
        _ThroughUpMenuItem.Name = "ThroughUpMenuItem"
        _ThroughUpMenuItem.IsCheckable = True
        _ThroughUpMenuItem.Header = "Through up"
        _ThroughUpMenuItem.Command = SelectThroughUpCommand
        _BranchTypeMenuItem.Items.Add(_ThroughUpMenuItem)

        _LeftDownMenuItem = New MenuItem
        _LeftDownMenuItem.Name = "LeftDownMenuItem"
        _LeftDownMenuItem.IsCheckable = True
        _LeftDownMenuItem.Header = "Left down"
        _LeftDownMenuItem.Command = SelectLeftDownCommand
        _BranchTypeMenuItem.Items.Add(_LeftDownMenuItem)

        _LeftUpMenuItem = New MenuItem
        _LeftUpMenuItem.Name = "LeftUpMenuItem"
        _LeftUpMenuItem.IsCheckable = True
        _LeftUpMenuItem.Header = "Left up"
        _LeftUpMenuItem.Command = SelectLeftUpCommand
        _BranchTypeMenuItem.Items.Add(_LeftUpMenuItem)

        UpdateBranchTypeMenuItems()
    End Sub

    Private Shared Sub BranchType_PropertyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim B = DirectCast(d, Branch)
        B.UpdateBranchTypeMenuItems()
    End Sub

#Region "BranchType Selection Commands"

#Region "Select ThroughDown Command"

    Private _SelectThroughDownCommand As ICommand
    Public ReadOnly Property SelectThroughDownCommand As ICommand
        Get
            If _SelectThroughDownCommand Is Nothing Then _SelectThroughDownCommand = New RelayCommand(AddressOf SelectThroughDown, AddressOf CanSelectThroughDown)
            Return _SelectThroughDownCommand
        End Get
    End Property

    Private Function CanSelectThroughDown(obj As Object) As Boolean
        Return Not BranchType = LogicControls.BranchType.ThroughDown
    End Function

    Private Sub SelectThroughDown(obj As Object)
        BranchType = LogicControls.BranchType.ThroughDown
    End Sub

#End Region

#Region "Select ThroughUp Command"

    Private _SelectThroughUpCommand As ICommand
    Public ReadOnly Property SelectThroughUpCommand As ICommand
        Get
            If _SelectThroughUpCommand Is Nothing Then _SelectThroughUpCommand = New RelayCommand(AddressOf SelectThroughUp, AddressOf CanSelectThroughUp)
            Return _SelectThroughUpCommand
        End Get
    End Property

    Private Function CanSelectThroughUp(obj As Object) As Boolean
        Return Not BranchType = LogicControls.BranchType.ThroughUp
    End Function

    Private Sub SelectThroughUp(obj As Object)
        BranchType = LogicControls.BranchType.ThroughUp
    End Sub

#End Region

#Region "Select LeftDown Command"

    Private _SelectLeftDownCommand As ICommand
    Public ReadOnly Property SelectLeftDownCommand As ICommand
        Get
            If _SelectLeftDownCommand Is Nothing Then _SelectLeftDownCommand = New RelayCommand(AddressOf SelectLeftDown, AddressOf CanSelectLeftDown)
            Return _SelectLeftDownCommand
        End Get
    End Property

    Private Function CanSelectLeftDown(obj As Object) As Boolean
        Return Not BranchType = LogicControls.BranchType.LeftDown
    End Function

    Private Sub SelectLeftDown(obj As Object)
        BranchType = LogicControls.BranchType.LeftDown
    End Sub

#End Region

#Region "Select LeftUp Command"

    Private _SelectLeftUpCommand As ICommand
    Public ReadOnly Property SelectLeftUpCommand As ICommand
        Get
            If _SelectLeftUpCommand Is Nothing Then _SelectLeftUpCommand = New RelayCommand(AddressOf SelectLeftUp, AddressOf CanSelectLeftUp)
            Return _SelectLeftUpCommand
        End Get
    End Property

    Private Function CanSelectLeftUp(obj As Object) As Boolean
        Return Not BranchType = LogicControls.BranchType.LeftUp
    End Function

    Private Sub SelectLeftUp(obj As Object)
        BranchType = LogicControls.BranchType.LeftUp
    End Sub

#End Region

#Region "Select RightDown Command"



#End Region

#Region "Select RightDown Command"



#End Region

#End Region

End Class

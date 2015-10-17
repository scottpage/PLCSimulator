﻿Public MustInherit Class ElementViewModel
    Inherits ScanableViewModel
    Implements ICloneable

    Protected Sub New(tag As TagViewModel, actionType As ElementActionType)
        _Id = Guid.NewGuid
        _Tag = tag
        _ActionType = actionType
    End Sub

    Protected Sub New(tag As TagViewModel, id As Guid, actionType As ElementActionType)
        _Id = id
        _Tag = tag
        _ActionType = actionType
    End Sub

    Private _ActionType As ElementActionType = ElementActionType.None
    Public ReadOnly Property ActionType As ElementActionType
        Get
            Return _ActionType
        End Get
    End Property

    Private _Id As Guid
    Public Property Id As Guid
        Get
            Return _Id
        End Get
        Set(ByVal Value As Guid)
            SetProperty(Function() Id, _Id, Value)
        End Set
    End Property

    Private _IsTemplate As Boolean = False
    Public Property IsTemplate As Boolean
        Get
            Return _IsTemplate
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsTemplate, _IsTemplate, Value)
        End Set
    End Property

    Private _Rung As RungViewModel = Nothing
    Public Property Rung As RungViewModel
        Get
            Return _Rung
        End Get
        Set(ByVal Value As RungViewModel)
            SetProperty(Function() Rung, _Rung, Value)
        End Set
    End Property

    Public ReadOnly Property IsSet As Boolean
        Get
            Return _EvaulationResult
        End Get
    End Property

    Public ReadOnly Property IsRungLeftEnd As Boolean
        Get
            Return GetPreviousElement() Is Nothing
        End Get
    End Property

    Public ReadOnly Property IsRungRightEnd As Boolean
        Get
            Return GetNextElement() Is Nothing
        End Get
    End Property

    Private _Tag As TagViewModel = Nothing
    Public Property Tag As TagViewModel
        Get
            Return _Tag
        End Get
        Set(value As TagViewModel)
            SetProperty(Function() Tag, _Tag, value)
        End Set
    End Property

    Private _EvaulationResult As Boolean = False

    Protected Overrides Function OnEvaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel) As Boolean
        Dim PreviousElement = DirectCast(previousScanable, ElementViewModel)
        Dim NextElement = DirectCast(nextScanable, ElementViewModel)
        _EvaulationResult = OnEvaluateElement(PreviousElement, NextElement)
        Return _EvaulationResult
    End Function

    Protected Overridable Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        Return (previousElement Is Nothing) Or (previousElement IsNot Nothing AndAlso previousElement.IsSet)
    End Function

    Protected Overrides Sub OnEvaluationComplete()
        OnPropertyChanged(Function() IsSet)
        Invalidate()
    End Sub

    Private Function GetPreviousElement() As ElementViewModel
        If Rung Is Nothing Then Return Nothing
        Return Rung.GetPreviousElement(Me)
    End Function

    Private Function GetNextElement() As ElementViewModel
        If Rung Is Nothing Then Return Nothing
        Return Rung.GetNextElement(Me)
    End Function

    Public MustOverride Function Clone() As Object Implements ICloneable.Clone

    Public Overridable Sub Invalidate()
        OnPropertyChanged(Function() IsRungLeftEnd)
        OnPropertyChanged(Function() IsRungRightEnd)
    End Sub

#Region "Commands"

    Private _RemoveCommand As ICommand
    Public ReadOnly Property RemoveCommand As ICommand
        Get
            If _RemoveCommand Is Nothing Then _RemoveCommand = New RelayCommand(AddressOf Remove, AddressOf CanRemove)
            Return _RemoveCommand
        End Get
    End Property

    Private Function CanRemove(obj As Object) As Boolean
        Return Rung IsNot Nothing AndAlso Rung.Elements.Contains(Me)
    End Function

    Private Sub Remove(obj As Object)
        Rung.Elements.Remove(Me)
    End Sub

#End Region

End Class
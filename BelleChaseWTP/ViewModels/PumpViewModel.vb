Imports System.ComponentModel
Imports System
Imports System.Windows.Threading

Public Class PumpViewModel
    Inherits ViewModelBase

    Private _IsAuto As Boolean = False
    Public Property IsAuto As Boolean
        Get
            Return _IsAuto
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsAuto, _IsAuto, Value)
        End Set
    End Property

    Private _IsLead As Boolean = False
    Public Property IsLead As Boolean
        Get
            Return _IsLead
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsLead, _IsLead, Value)
        End Set
    End Property

    Private _IsLag As Boolean = False
    Public Property IsLag As Boolean
        Get
            Return _IsLag
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsLag, _IsLag, Value)
        End Set
    End Property

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return Call2Run And Not Faulted
        End Get
    End Property

    Private _Call2Run As Boolean = False
    Public ReadOnly Property Call2Run As Boolean
        Get
            Return _Call2Run
        End Get
    End Property

    Private _MotorOverloadTrip As Boolean = False
    Public ReadOnly Property MotorOverloadTrip As Boolean
        Get
            Return _MotorOverloadTrip
        End Get
    End Property

    Private _SimulationMode As PumpSimulationMode = PumpSimulationMode.None
    Public Property SimulationMode As PumpSimulationMode
        Get
            Return _SimulationMode
        End Get
        Set(ByVal Value As PumpSimulationMode)
            SetProperty(Function() SimulationMode, _SimulationMode, Value)
        End Set
    End Property

    Private _Fail2Run As Boolean = False
    Public ReadOnly Property Fail2Run As Boolean
        Get
            Return _Fail2Run
        End Get
    End Property

    Public ReadOnly Property Faulted As Boolean
        Get
            Return MotorOverloadTrip Or Fail2Run
        End Get
    End Property

#Region "Commands"

#Region "SwitchIsAutoCommand"

    Private _SwitchIsAutoCommand As ICommand
    Public ReadOnly Property SwitchIsAutoCommand As ICommand
        Get
            If _SwitchIsAutoCommand Is Nothing Then _SwitchIsAutoCommand = New RelayCommand(AddressOf SwitchIsAuto, AddressOf CanSwitchIsAuto)
            Return _SwitchIsAutoCommand
        End Get
    End Property

    Private Function CanSwitchIsAuto(obj As Object) As Boolean
        Return True
    End Function

    Private Sub SwitchIsAuto(obj As Object)
        IsAuto = Not IsAuto
    End Sub

#End Region

#Region "StartStopCommand"

    Private WithEvents _Fail2RunTimer As DispatcherTimer

    Private _StartStopCommand As ICommand
    Public ReadOnly Property StartStopCommand As ICommand
        Get
            If _StartStopCommand Is Nothing Then _StartStopCommand = New RelayCommand(AddressOf StartStop, AddressOf CanStartStop)
            Return _StartStopCommand
        End Get
    End Property

    Private Function CanStartStop(obj As Object) As Boolean
        Return Not Faulted
    End Function

    Private Sub StartStop(obj As Object)
        If IsRunning Then
            [Stop]()
        Else
            Start()
        End If
    End Sub

    Public Sub Start()
        _Fail2RunTimer = New DispatcherTimer
        _Fail2RunTimer.Interval = TimeSpan.FromSeconds(5)
        _Fail2RunTimer.Start()
        SetProperty(Function() Call2Run, _Call2Run, True)
    End Sub

    Public Sub [Stop]()
        If _Fail2RunTimer IsNot Nothing Then
            _Fail2RunTimer.Stop()
            _Fail2RunTimer = Nothing
        End If
        OnPropertyChanged(Function() Faulted)
        OnPropertyChanged(Function() IsRunning)
    End Sub

    Private Sub _Fail2RunTimer_Tick(sender As Object, e As EventArgs) Handles _Fail2RunTimer.Tick
        _Fail2RunTimer.Stop()
        _Fail2RunTimer = Nothing
        Select Case SimulationMode
            Case PumpSimulationMode.None
                SetProperty(Function() Fail2Run, _Fail2Run, False)
                SetProperty(Function() MotorOverloadTrip, _MotorOverloadTrip, False)
            Case PumpSimulationMode.FailToRun
                SetProperty(Function() Fail2Run, _Fail2Run, True)
                SetProperty(Function() MotorOverloadTrip, _MotorOverloadTrip, False)
            Case PumpSimulationMode.MotorOverload
                SetProperty(Function() Fail2Run, _Fail2Run, False)
                SetProperty(Function() MotorOverloadTrip, _MotorOverloadTrip, True)
        End Select
        OnPropertyChanged(Function() Faulted)
        OnPropertyChanged(Function() IsRunning)
    End Sub

#End Region

#End Region

End Class

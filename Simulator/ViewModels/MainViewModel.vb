Public NotInheritable Class MainViewModel
    Inherits ViewModelBase

    Private Shared ReadOnly _Instance As New MainViewModel
    Public Shared ReadOnly Property Instance As MainViewModel
        Get
            Return _Instance
        End Get
    End Property

    Public Sub New()
        'TODO:  Make private on release

        Dim Ladder As New LadderViewModel

        AvailableElements.Add(New WireViewModel With {.IsTemplate = True})
        AvailableElements.Add(New ContactViewModel With {.IsNormallyClosed = False, .IsTemplate = True})
        AvailableElements.Add(New ContactViewModel With {.IsNormallyClosed = True, .IsTemplate = True})
        AvailableElements.Add(New CoilViewModel With {.IsTemplate = True})
        AvailableElements.Add(New FloatComparisonElementViewModel With {.IsTemplate = True})

        For I = 0 To 50
            Tags.Add(New DiscreteTagViewModel With {.Name = String.Format("Discrete Tag {0}", I.ToString)})
            Tags.Add(New NumericTagViewModel With {.Name = String.Format("Numeric Tag {0}", I.ToString)})
        Next

        For I2 = 1 To 5
            Dim R As New RungViewModel
            For I = 1 To 7
                Dim D As New ContactViewModel
                D.Tag.Name = String.Format("Tag {0}", I.ToString)
                D.IsNormallyClosed = True
                D.DiscreteTag.Value = I > 3
                D.Rung = R
                D.Invalidate()
                R.Elements.Add(D)
            Next
            R.Elements.Add(New CoilViewModel With {.Rung = R, .IsTemplate = False, .Tag = R.Elements.First.Tag})
            Ladder.Add(R)
        Next
        Ladders.Add(Ladder)
        SelectedLadder = Ladder
    End Sub

    Private _ScanTimeBetweenRungs As Integer = 250
    Public Property ScanTimeBetweenRungs As Integer
        Get
            Return _ScanTimeBetweenRungs
        End Get
        Set(ByVal Value As Integer)
            If Value < 10 Then Value = 10
            SetProperty(Function() ScanTimeBetweenRungs, _ScanTimeBetweenRungs, Value)
        End Set
    End Property

    Private _ScanTimeBetweenElements As Integer = 250
    Public Property ScanTimeBetweenElements As Integer
        Get
            Return _ScanTimeBetweenElements
        End Get
        Set(ByVal Value As Integer)
            If Value < 10 Then Value = 10
            SetProperty(Function() ScanTimeBetweenElements, _ScanTimeBetweenElements, Value)
        End Set
    End Property

    Private _AvailableElements As ObservableCollection(Of ElementViewModel)
    Public ReadOnly Property AvailableElements As ObservableCollection(Of ElementViewModel)
        Get
            If _AvailableElements Is Nothing Then _AvailableElements = New ObservableCollection(Of ElementViewModel)
            Return _AvailableElements
        End Get
    End Property

    Private _Tags As ObservableCollection(Of TagViewModel)
    Public ReadOnly Property Tags As ObservableCollection(Of TagViewModel)
        Get
            If _Tags Is Nothing Then _Tags = New ObservableCollection(Of TagViewModel)
            Return _Tags
        End Get
    End Property

    Private _SelectedTag As TagViewModel = Nothing
    Public Property SelectedTag As TagViewModel
        Get
            Return _SelectedTag
        End Get
        Set(ByVal Value As TagViewModel)
            SetProperty(Function() SelectedTag, _SelectedTag, Value)
        End Set
    End Property

    Private _Ladders As ObservableCollection(Of LadderViewModel)
    Public ReadOnly Property Ladders As ObservableCollection(Of LadderViewModel)
        Get
            If _Ladders Is Nothing Then _Ladders = New ObservableCollection(Of LadderViewModel)
            Return _Ladders
        End Get
    End Property

    Private _SelectedLadder As LadderViewModel = Nothing
    Public Property SelectedLadder As LadderViewModel
        Get
            Return _SelectedLadder
        End Get
        Set(ByVal Value As LadderViewModel)
            SetProperty(Function() SelectedLadder, _SelectedLadder, Value)
        End Set
    End Property

    Private _IsRunning As Boolean
    Public ReadOnly Property IsRunning As Boolean
        Get
            Return _IsRunning
        End Get
    End Property

    Private _IsPaused As Boolean
    Public ReadOnly Property IsPaused As Boolean
        Get
            Return _IsPaused
        End Get
    End Property

#Region "Commands"

    Private _StartStopCommand As ICommand
    Public ReadOnly Property StartStopCommand As ICommand
        Get
            If _StartStopCommand Is Nothing Then _StartStopCommand = New RelayCommand(AddressOf StartStop, AddressOf CanStartStop)
            Return _StartStopCommand
        End Get
    End Property

    Private Function CanStartStop(obj As Object) As Boolean
        Return True
    End Function

    Private Sub StartStop(obj As Object)
        If IsRunning Then
            [Stop]()
        Else
            Start()
        End If
    End Sub

    Private _ScanTread As System.Threading.Thread

    Public Sub Start()
        _ScanTread = New System.Threading.Thread(AddressOf StartAsync)
        _ScanTread.IsBackground = True
        _ScanTread.Start()
    End Sub

    Private _CurrentRung As RungViewModel
    Private _CurrentElement As ElementViewModel
    Private _WasPaused As Boolean
    Private Sub StartAsync()
        SetProperty(Function() IsRunning, _IsRunning, True)
        Try
            While IsRunning And Not IsPaused
                SelectedLadder.IsScanning = True
                Dim RungsCopy = SelectedLadder.Rungs.ToArray
                If RungsCopy.Count.Equals(0) Then Continue While
                For RI = 0 To RungsCopy.Count - 1
                    If Not IsRunning Then
                        _CurrentRung = Nothing
                        _CurrentElement = Nothing
                        Exit While
                    End If
                    If _WasPaused Then
                    Else
                        _CurrentRung = RungsCopy(RI)
                    End If
                    If Not _CurrentRung.CanBeScanned Then Continue For
                    _CurrentRung.IsScanning = True
                    Dim ElementsCopy = _CurrentRung.Elements.ToArray
                    If ElementsCopy.Count.Equals(0) Then Continue While
                    For EI = 0 To ElementsCopy.Count - 1
                        If Not IsRunning Then
                            _CurrentRung = Nothing
                            _CurrentElement = Nothing
                            Exit While
                        End If
                        If EI > _CurrentRung.Elements.Count - 1 Then Continue For
                        If _WasPaused Then
                            _WasPaused = False
                        Else
                            _CurrentElement = _CurrentRung.Elements(EI)
                        End If
                        _CurrentElement.IsScanning = True
                        If IsPaused Then
                            _WasPaused = True
                            Exit While
                        End If
                        Dim PreviousElement = _CurrentRung.GetPreviousElement(_CurrentElement)
                        Dim NextElement = _CurrentRung.GetNextElement(_CurrentElement)
                        _CurrentElement.Evaluate(PreviousElement, NextElement)
                        System.Threading.Thread.Sleep(ScanTimeBetweenElements)
                        _CurrentElement.IsScanning = False
                    Next
                    System.Threading.Thread.Sleep(ScanTimeBetweenRungs)
                    _CurrentRung.IsScanning = False
                Next
                SelectedLadder.IsScanning = False
                System.Threading.Thread.Sleep(10)
            End While
        Catch ex As Exception
            'Thread aborted
        End Try
        SelectedLadder.IsScanning = False
    End Sub

    Public Sub [Stop]()
        SetProperty(Function() IsRunning, _IsRunning, False)
        If SelectedLadder IsNot Nothing Then
            SelectedLadder.Reset()
            For Each R In SelectedLadder.Rungs
                R.Reset()
                For Each E In R.Elements
                    E.Reset()
                Next
            Next
        End If
    End Sub

#Region "PauseSimulationCommand"

    Private _PauseSimulationCommand As ICommand
    Public ReadOnly Property PauseSimulationCommand As ICommand
        Get
            If _PauseSimulationCommand Is Nothing Then _PauseSimulationCommand = New RelayCommand(AddressOf PauseSimulation, AddressOf CanPauseSimulation)
            Return _PauseSimulationCommand
        End Get
    End Property

    Private Function CanPauseSimulation(obj As Object) As Boolean
        Return IsRunning And Not IsPaused Or IsRunning And IsPaused
    End Function

    Private Sub PauseSimulation(obj As Object)
        SetProperty(Function() IsPaused, _IsPaused, Not IsPaused)
    End Sub

#End Region



#Region "ExitCommand"

    Private _ExitCommand As ICommand
    Public ReadOnly Property ExitCommand As ICommand
        Get
            If _ExitCommand Is Nothing Then _ExitCommand = New RelayCommand(AddressOf [Exit], AddressOf CanExit)
            Return _ExitCommand
        End Get
    End Property

    Private Function CanExit(obj As Object) As Boolean
        Return True
    End Function

    Private Sub [Exit](obj As Object)
        Application.Current.Shutdown()
    End Sub

#End Region

#End Region

End Class

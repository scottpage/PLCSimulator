Class Application

    Private WithEvents _MainWindow As MainWindow
    Private _ExitClosedMainWindow As Boolean

    Public ReadOnly Property MyApplicationMainWindow As MainWindow
        Get
            Return _MainWindow
        End Get
    End Property

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)
        _MainWindow = New MainWindow
        _MainWindow.Title = My.Application.Info.Title
        'TODO:  Uncomment after release
        _MainWindow.DataContext = MainViewModel.Instance
        _MainWindow.Show()
    End Sub

    Protected Overrides Sub OnExit(e As ExitEventArgs)
        MainViewModel.Instance.Stop()
        If _MainWindow IsNot Nothing Then
            _ExitClosedMainWindow = True
            _MainWindow.Close()
        End If
        My.Settings.Save()
        MyBase.OnExit(e)
    End Sub

    Private Sub _MainWindow_Closed(sender As Object, e As EventArgs) Handles _MainWindow.Closed
        _MainWindow = Nothing
        If Not _ExitClosedMainWindow Then Shutdown()
    End Sub

End Class

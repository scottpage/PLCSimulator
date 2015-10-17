Class Application

    Private WithEvents _MainWindow As MainWindow = Nothing
    Private _ExitClosedMainWindow As Boolean = False

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)
        _MainWindow = New MainWindow
        'TODO:  Uncomment after release
        _MainWindow.DataContext = MainViewModel.Instance
        _MainWindow.Show()
    End Sub

    Protected Overrides Sub OnExit(e As ExitEventArgs)
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

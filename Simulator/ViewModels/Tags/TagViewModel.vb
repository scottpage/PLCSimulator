Public MustInherit Class TagViewModel
    Inherits ViewModelBase

    Private _Name As String = String.Empty
    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            SetProperty(Function() Name, _Name, Value)
        End Set
    End Property

#Region "EditCommand"

    Private _EditCommandCommand As ICommand
    Public ReadOnly Property EditCommand As ICommand
        Get
            If _EditCommandCommand Is Nothing Then _EditCommandCommand = New RelayCommand(AddressOf Edit, AddressOf CanEdit)
            Return _EditCommandCommand
        End Get
    End Property

    Private Function CanEdit(obj As Object) As Boolean
        Return True
    End Function

    Private Sub Edit(obj As Object)
        Dim ETW As New EditTagWindow
        Dim ETVM As New EditTagViewModel(Me)
        ETW.DataContext = ETVM
        ETW.Owner = My.Application.MyApplicationMainWindow
        ETW.WindowStartupLocation = WindowStartupLocation.CenterOwner
        ETW.ShowDialog()
        ETW.DataContext = Nothing
        ETW = Nothing
    End Sub

#End Region

End Class

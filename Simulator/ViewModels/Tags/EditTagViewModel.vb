Public Class EditTagViewModel
    Inherits ViewModelBase

    Public Sub New(tag As TagViewModel)
        _Tag = tag
        TagName = tag.Name
    End Sub

    Private _Tag As TagViewModel = Nothing
    Public ReadOnly Property Tag As TagViewModel
        Get
            Return _Tag
        End Get
    End Property

    Private _TagName As String
    Public Property TagName As String
        Get
            Return _TagName
        End Get
        Set(ByVal Value As String)
            SetProperty(Function() TagName, _TagName, Value)
        End Set
    End Property

#Region "UpdateTagCommand"

    Private _UpdateTagCommand As ICommand
    Public ReadOnly Property UpdateTagCommand As ICommand
        Get
            If _UpdateTagCommand Is Nothing Then _UpdateTagCommand = New RelayCommand(AddressOf UpdateTag, AddressOf CanUpdateTag)
            Return _UpdateTagCommand
        End Get
    End Property

    Private Function CanUpdateTag(obj As Object) As Boolean
        Return Not String.IsNullOrWhiteSpace(TagName)
    End Function

    Private Sub UpdateTag(obj As Object)
        Tag.Name = TagName
    End Sub

#End Region

End Class

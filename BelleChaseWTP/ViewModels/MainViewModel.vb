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
    End Sub

    Private _Pump1 As New PumpViewModel
    Public ReadOnly Property Pump1 As PumpViewModel
        Get
            Return _Pump1
        End Get
    End Property

    Private _Pump2 As New PumpViewModel
    Public ReadOnly Property Pump2 As PumpViewModel
        Get
            Return _Pump2
        End Get
    End Property

    Private _Pump3 As New PumpViewModel
    Public ReadOnly Property Pump3 As PumpViewModel
        Get
            Return _Pump3
        End Get
    End Property

#Region "Commands"



#End Region

End Class

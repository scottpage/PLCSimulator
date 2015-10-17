Public MustInherit Class ScanableViewModel
    Inherits ViewModelBase

    Private _IsScanning As Boolean = False
    Public Property IsScanning As Boolean
        Get
            Return _IsScanning
        End Get
        Set(ByVal Value As Boolean)
            SetProperty(Function() IsScanning, _IsScanning, Value)
        End Set
    End Property

    Public Sub Evaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel)
        OnEvaluate(previousScanable, nextScanable)
        OnEvaluationComplete()
    End Sub

    Protected MustOverride Function OnEvaluate(previousScanable As ScanableViewModel, nextScanable As ScanableViewModel) As Boolean

    Protected Overridable Sub OnEvaluationComplete()
    End Sub

    Public Overrides Sub Reset()
        MyBase.Reset()
        IsScanning = False
    End Sub

End Class

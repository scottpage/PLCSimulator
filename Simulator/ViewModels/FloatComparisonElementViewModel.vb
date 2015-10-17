Public Class FloatComparisonElementViewModel
    Inherits DiscreteViewModel

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(id)
    End Sub

    Private _Value1 As Single
    Public Property Value1 As Single
        Get
            Return _Value1
        End Get
        Set(ByVal Value As Single)
            SetProperty(Function() Value1, _Value1, Value)
            Dim ValueTypeCode = Value.GetType.IsValueType
        End Set
    End Property

    Private _Value2 As Single
    Public Property Value2 As Single
        Get
            Return _Value2
        End Get
        Set(ByVal Value As Single)
            SetProperty(Function() Value2, _Value2, Value)
        End Set
    End Property

    Private _Operation As ComparisonOperation = ComparisonOperation.EqualTo
    Public Property Operation As ComparisonOperation
        Get
            Return _Operation
        End Get
        Set(ByVal Value As ComparisonOperation)
            SetProperty(Function() Operation, _Operation, Value)
        End Set
    End Property

    Public Overrides Function Clone() As Object
        Return New FloatComparisonElementViewModel With {.Value1 = Value1, .Value2 = Value2}
    End Function

    Protected Overrides Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        If Not MyBase.OnEvaluateElement(previousElement, nextElement) Then Return False
        Dim ComparisonResult = Compare()
        Return ComparisonResult.Equals(Operation)
    End Function

    Private Function Compare() As ComparisonOperation
        Dim Result As ComparisonOperation = ComparisonOperation.Invalid
        If Value1.CompareTo(Value2) < 0 Then Result = ComparisonOperation.LessThan
        If Value1.CompareTo(Value2) > 0 Then Result = ComparisonOperation.GreaterThan
        If Value1.CompareTo(Value2) <= 0 Then Result = ComparisonOperation.LessThanOrEqualTo
        If Value1.CompareTo(Value2) >= 0 Then Result = ComparisonOperation.GreaterThanOrEqualTo
        If Value1.CompareTo(Value2) = 0 Then Result = ComparisonOperation.EqualTo
        Return Result
    End Function

End Class

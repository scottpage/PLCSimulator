Public Class FloatComparisonElementViewModel
    Inherits DiscreteViewModel

    Dim Tag1 As NumericTagViewModel

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(id As Guid)
        MyBase.New(id)
    End Sub

    Public Shadows Property Tag As NumericTagViewModel
        Get
            If MyBase.Tag IsNot Nothing AndAlso TypeOf MyBase.Tag Is NumericTagViewModel Then Return DirectCast(MyBase.Tag, NumericTagViewModel)
            Return Nothing
        End Get
        Set(value As NumericTagViewModel)
            MyBase.Tag = value
        End Set
    End Property

    Private _Tag2 As NumericTagViewModel
    Public Property Tag2 As NumericTagViewModel
        Get
            Return _Tag2
        End Get
        Set(ByVal Value As NumericTagViewModel)
            SetProperty(Function() Tag2, _Tag2, Value)
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
        Return New FloatComparisonElementViewModel With {.Tag = Tag, .Tag2 = Tag2}
    End Function

    Protected Overrides Function OnEvaluateElement(previousElement As ElementViewModel, nextElement As ElementViewModel) As Boolean
        If Not MyBase.OnEvaluateElement(previousElement, nextElement) Then Return False
        Dim ComparisonResult = Compare()
        Return ComparisonResult.Equals(Operation)
    End Function

    Private Function Compare() As ComparisonOperation
        Dim Result As ComparisonOperation = ComparisonOperation.Invalid
        If Tag IsNot Nothing And Tag2 IsNot Nothing Then
            If Tag.Value.CompareTo(Tag2.Value) < 0 Then Result = ComparisonOperation.LessThan
            If Tag.Value.CompareTo(Tag2.Value) > 0 Then Result = ComparisonOperation.GreaterThan
            If Tag.Value.CompareTo(Tag2.Value) <= 0 Then Result = ComparisonOperation.LessThanOrEqualTo
            If Tag.Value.CompareTo(Tag2.Value) >= 0 Then Result = ComparisonOperation.GreaterThanOrEqualTo
            If Tag.Value.CompareTo(Tag2.Value) = 0 Then Result = ComparisonOperation.EqualTo
        End If
        Return Result
    End Function

    Public Overrides Sub DragOver(dropInfo As IDropInfo)
        If TypeOf dropInfo.Data Is NumericTagViewModel Then
            dropInfo.Effects = DragDropEffects.Copy
            GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo)
        Else
            MyBase.DragOver(dropInfo)
        End If
    End Sub

    Public Overrides Sub Drop(dropInfo As IDropInfo)
        If TypeOf dropInfo.Data Is NumericTagViewModel Then
            Dim VM = DirectCast(dropInfo.Data, NumericTagViewModel)
            If dropInfo.DropPosition.Y < dropInfo.VisualTarget.RenderSize.Height / 2 Then
                Tag = VM
            Else
                Tag2 = VM
            End If
        Else
            MyBase.Drop(dropInfo)
        End If
    End Sub

End Class

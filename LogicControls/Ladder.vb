Public Class Ladder
    Inherits ItemsControl

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Ladder), New FrameworkPropertyMetadata(GetType(Ladder)))
    End Sub

    Sub New()
        AllowDrop = True
    End Sub

    Public Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()
        Dim LogicControl_IsSelectedHandler As New RoutedEventHandler(AddressOf LogicControl_IsSelectedChanged)
    End Sub

    Private Sub LogicControl_IsSelectedChanged(sender As Object, e As RoutedEventArgs)
        If e.Source Is Nothing OrElse Not e.Source.GetType.IsSubclassOf(GetType(LogicControl)) Then Return
        If Not TypeOf e Is BooleanRoutedEventArgs Then Return
        Dim LC = DirectCast(e.Source, LogicControl)
        Dim LCEventArgs = DirectCast(e, BooleanRoutedEventArgs)
        If Not LCEventArgs.OldValue And LCEventArgs.NewValue Then
            For Each I In Items
                If TypeOf I Is Rung Then
                    Dim LR = DirectCast(I, Rung)
                    For Each I2 In LR.Items
                        If TypeOf I2 Is LogicControl Then
                            Dim LC2 = DirectCast(I2, LogicControl)
                            If LC Is LC2 Then Continue For
                            LC2.IsSelected = False
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        If Not TypeOf e.Source Is Rung Or
            Not e.AllowedEffects = DragDropEffects.Copy Or
            Not e.AllowedEffects = DragDropEffects.Move Or
            Not e.Data.GetDataPresent(GetType(Rung)) Then MyBase.OnDragEnter(e)
    End Sub

    Protected Overrides Sub OnDrop(e As DragEventArgs)
        If Not TypeOf e.Source Is Rung OrElse Not e.AllowedEffects = DragDropEffects.Copy Or e.AllowedEffects = DragDropEffects.Move Then MyBase.OnDrop(e)
        e.Handled = True
        Dim LC = DirectCast(e.Source, Rung)

    End Sub

End Class

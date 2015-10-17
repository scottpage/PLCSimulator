Public Module UIHelper

    Public Function GetUIElement(container As ItemsControl, position As Point) As UIElement
        Dim ElementAtPosition = DirectCast(container.InputHitTest(position), UIElement)
        If ElementAtPosition Is Nothing Then Return Nothing
        While ElementAtPosition IsNot Nothing
            Dim TestUIElement = container.ItemContainerGenerator.ItemFromContainer(ElementAtPosition)
            If TestUIElement Is DependencyProperty.UnsetValue Then
                ElementAtPosition = DirectCast(VisualTreeHelper.GetParent(ElementAtPosition), UIElement)
            Else
                Return ElementAtPosition
            End If
        End While
        Return Nothing
    End Function

    Public Function IsPositionAboveElement(element As UIElement, relativePosition As Point) As Boolean
        Dim FE As FrameworkElement = Nothing
        If TypeOf element Is FrameworkElement Then FE = DirectCast(element, FrameworkElement)
        If relativePosition.Y < FE.ActualHeight / 2 Then Return True
        Return False
    End Function

End Module

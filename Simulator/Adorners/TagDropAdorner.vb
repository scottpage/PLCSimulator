Public Class TagDropAdorner
    Inherits Adorner

    Public Sub New(adornedElement As UIElement)
        MyBase.New(adornedElement)
    End Sub

    Protected Overrides Sub OnRender(drawingContext As DrawingContext)
        MyBase.OnRender(drawingContext)
        Dim adornedElementRect As New Rect(AdornedElement.DesiredSize)
        Dim renderBrush As New SolidColorBrush(Colors.Green)
        renderBrush.Opacity = 0.2
        Dim renderPen As New Pen(New SolidColorBrush(Colors.Navy), 1.5)
        Dim renderRadius As Double
        renderRadius = 5.0

        'Draw a circle at each corner.
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius)
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius)
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius)
        drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius)
    End Sub

End Class

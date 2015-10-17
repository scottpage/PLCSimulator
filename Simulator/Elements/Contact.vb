Public Class Contact
    Inherits Discrete

    Public Property IsNormallyClosed As Boolean
        Get
            Return DirectCast(GetValue(IsNormallyClosedProperty), Boolean)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsNormallyClosedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IsNormallyClosedProperty As DependencyProperty = _
                           DependencyProperty.Register("IsNormallyClosed", _
                           GetType(Boolean), GetType(Contact), _
                           New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.AffectsRender))

    Protected Overrides Sub OnRenderElement(dc As DrawingContext, linePen As Pen, midVertLeftPoint As Point, MidVertRightPoint As Point)
        Dim WorkingRect As New Rect(RenderSize.Width * 0.2#, RenderSize.Height * 0.2#, RenderSize.Width * 0.6#, RenderSize.Height * 0.6#)

        Dim LeftHorzLineMidLeft As New Point(0.0#, midVertLeftPoint.Y)
        Dim LeftHorzLineMidRight As New Point(WorkingRect.Left, midVertLeftPoint.Y)
        Dim LeftVertLineTop As New Point(WorkingRect.Left, WorkingRect.Top)
        Dim LeftVertLineBottom As New Point(WorkingRect.Left, WorkingRect.Bottom)
        dc.DrawLine(linePen, LeftHorzLineMidLeft, LeftHorzLineMidRight)
        dc.DrawLine(linePen, LeftVertLineTop, LeftVertLineBottom)

        Dim RightHorzLineMidLeft As New Point(WorkingRect.Right, MidVertRightPoint.Y)
        Dim RightHorzLineMidRight As New Point(RenderSize.Width, MidVertRightPoint.Y)
        Dim RightVertLineTop As New Point(WorkingRect.Right, WorkingRect.Top)
        Dim RightVertLineBottom As New Point(WorkingRect.Right, WorkingRect.Bottom)
        dc.DrawLine(linePen, RightHorzLineMidLeft, RightHorzLineMidRight)
        dc.DrawLine(linePen, RightVertLineTop, RightVertLineBottom)
        MyBase.OnRenderElement(dc, linePen, midVertLeftPoint, MidVertRightPoint)
    End Sub

End Class

Public MustInherit Class Discrete
    Inherits Element

    Protected Overrides Sub OnRenderElement(dc As DrawingContext, linePen As Pen, midVertLeftPoint As Point, MidVertRightPoint As Point)
        If String.IsNullOrWhiteSpace(TagName) Then Return
        Dim TopHeight = RenderSize.Height * 0.2#
        Dim HorzMid = RenderSize.Width * 0.5#
        Dim FT As New FormattedText(TagName, Globalization.CultureInfo.InstalledUICulture,
                                    Windows.FlowDirection.LeftToRight,
                                    New Typeface(FontFamily,
                                                 FontStyle,
                                                 FontWeight,
                                                 FontStretch),
                                                 FontSize,
                                                 FontForeground)
        dc.DrawText(FT, New Point(HorzMid - FT.Width * 0.5#, TopHeight * 0.5# - FT.Height * 0.5#))
    End Sub

End Class

Public MustInherit Class Element
    Inherits UIElement

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
        'DefaultStyleKeyProperty.OverrideMetadata(GetType(LogicElement), new FrameworkPropertyMetadata(GetType(Element)))
    End Sub

    Sub New()
        AddHandler MouseEnter, AddressOf LogicElement_MouseEnter
        AddHandler MouseLeave, AddressOf LogicElement_MouseLeave
    End Sub

    Private Sub LogicElement_MouseEnter(sender As Object, e As MouseEventArgs)
        InvalidateVisual()
    End Sub

    Private Sub LogicElement_MouseLeave(sender As Object, e As MouseEventArgs)
        InvalidateVisual()
    End Sub

#Region "Font DPs"

    <Category("Font")>
    Public Property FontStyle As FontStyle
        Get
            Return DirectCast(GetValue(FontStyleProperty), FontStyle)
        End Get

        Set(ByVal value As FontStyle)
            SetValue(FontStyleProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontStyleProperty As DependencyProperty = _
                           DependencyProperty.Register("FontStyle", _
                           GetType(FontStyle), GetType(Element), _
                           New FrameworkPropertyMetadata(FontStyles.Normal, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Font")>
    Public Property FontFamily As FontFamily
        Get
            Return DirectCast(GetValue(FontFamilyProperty), FontFamily)
        End Get

        Set(ByVal value As FontFamily)
            SetValue(FontFamilyProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontFamilyProperty As DependencyProperty = _
                           DependencyProperty.Register("FontFamily", _
                           GetType(FontFamily), GetType(Element), _
                           New FrameworkPropertyMetadata(New FontFamily("Segoe UI"), FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Font")>
    Public Property FontWeight As FontWeight
        Get
            Return DirectCast(GetValue(FontWeightProperty), FontWeight)
        End Get

        Set(ByVal value As FontWeight)
            SetValue(FontWeightProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontWeightProperty As DependencyProperty = _
                           DependencyProperty.Register("FontWeight", _
                           GetType(FontWeight), GetType(Element), _
                           New FrameworkPropertyMetadata(FontWeights.Normal, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Font")>
    Public Property FontStretch As FontStretch
        Get
            Return DirectCast(GetValue(FontStretchProperty), FontStretch)
        End Get

        Set(ByVal value As FontStretch)
            SetValue(FontStretchProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontStretchProperty As DependencyProperty = _
                       DependencyProperty.Register("FontStretch", _
                       GetType(FontStretch), GetType(Element), _
                       New FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Font")>
    Public Property FontSize As Double
        Get
            Return DirectCast(GetValue(FontSizeProperty), Double)
        End Get

        Set(ByVal value As Double)
            SetValue(FontSizeProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontSizeProperty As DependencyProperty = _
                       DependencyProperty.Register("FontSize", _
                       GetType(Double), GetType(Element), _
                       New FrameworkPropertyMetadata(12.0#, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Font")>
    Public Property FontForeground As Brush
        Get
            Return DirectCast(GetValue(FontForegroundProperty), Brush)
        End Get

        Set(ByVal value As Brush)
            SetValue(FontForegroundProperty, value)
        End Set
    End Property

    Public Shared ReadOnly FontForegroundProperty As DependencyProperty = _
                        DependencyProperty.Register("FontForeground", _
                        GetType(Brush), GetType(Element), _
                        New FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender))



#End Region

    <Category("Common"), Description("Gets or Sets the tag name of this element.")>
    Public Property TagName As String
        Get
            Return DirectCast(GetValue(TagNameProperty), String)
        End Get

        Set(ByVal value As String)
            SetValue(TagNameProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TagNameProperty As DependencyProperty = _
                           DependencyProperty.Register("TagName", _
                           GetType(String), GetType(Discrete), _
                           New FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Common")>
    Public Property IsSet As Boolean
        Get
            Return DirectCast(GetValue(IsSetProperty), Boolean)
        End Get

        Set(ByVal value As Boolean)
            SetValue(IsSetProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IsSetProperty As DependencyProperty = _
                           DependencyProperty.Register("IsSet", _
                           GetType(Boolean), GetType(Element), _
                           New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Appearance")>
    Public Property SetColor As Color
        Get
            Return DirectCast(GetValue(SetColorProperty), Color)
        End Get

        Set(ByVal value As Color)
            SetValue(SetColorProperty, value)
        End Set
    End Property

    Public Shared ReadOnly SetColorProperty As DependencyProperty = _
                           DependencyProperty.Register("SetColor", _
                           GetType(Color), GetType(Element), _
                           New FrameworkPropertyMetadata(Colors.LimeGreen, FrameworkPropertyMetadataOptions.AffectsRender))

    <Category("Appearance")>
    Public Property UnSetColor As Color
        Get
            Return DirectCast(GetValue(UnSetColorProperty), Color)
        End Get

        Set(ByVal value As Color)
            SetValue(UnSetColorProperty, value)
        End Set
    End Property

    Public Shared ReadOnly UnSetColorProperty As DependencyProperty = _
                           DependencyProperty.Register("UnSetColor", _
                           GetType(Color), GetType(Element), _
                           New FrameworkPropertyMetadata(Colors.Red, FrameworkPropertyMetadataOptions.AffectsRender))

    Protected Overrides ReadOnly Property VisualChildrenCount As Integer
        Get
            Return 0
        End Get
    End Property

    Protected Overrides Function GetVisualChild(index As Integer) As Visual
        Return Nothing
    End Function

    Protected NotOverridable Overrides Sub OnRender(drawingContext As DrawingContext)
        Dim DC = drawingContext
        Dim MidVert = RenderSize.Height / 2.0#
        Dim MidHorz = RenderSize.Width / 2.0#
        Dim MidVertLeftPoint As New Point(0.0#, MidVert)
        Dim MidVertRightPoint As New Point(RenderSize.Width, MidVert)
        Dim BGRectPen As New Pen(If(IsMouseOver, Brushes.Yellow, Brushes.Transparent), 3.0#)
        Dim BGRect As New Rect(RenderSize)
        Dim LinePenBrush As New SolidColorBrush(If(IsSet, SetColor, UnSetColor))
        Dim LinePen As New Pen(LinePenBrush, 3.0#)
        DC.DrawRectangle(Brushes.Transparent, BGRectPen, BGRect)
        OnRenderElement(DC, LinePen, MidVertLeftPoint, MidVertRightPoint)
        MyBase.OnRender(drawingContext)
    End Sub

    Protected MustOverride Sub OnRenderElement(dc As DrawingContext, linePen As Pen, midVertLeftPoint As Point, MidVertRightPoint As Point)

End Class

''' <summary>
''' Provides a means to specify DataTemplates to be selected from within WPF code
''' </summary>
Public Class DynamicTemplateSelector
    Inherits DataTemplateSelector
    ''' <summary>
    ''' Generic attached property specifying <see cref="Template"/>s
    ''' used by the <see cref="DynamicTemplateSelector"/>
    ''' </summary>
    ''' <remarks>
    ''' This attached property will allow you to set the templates you wish to be available whenever
    ''' a control's TemplateSelector is set to an instance of <see cref="DynamicTemplateSelector"/>
    ''' </remarks>
    Public Shared ReadOnly TemplatesProperty As DependencyProperty = DependencyProperty.RegisterAttached("Templates", GetType(TemplateCollection), GetType(DynamicTemplateSelector), New FrameworkPropertyMetadata(New TemplateCollection(), FrameworkPropertyMetadataOptions.[Inherits]))

    ''' <summary>
    ''' Gets the value of the <paramref name="element"/>'s attached <see cref="TemplatesProperty"/>
    ''' </summary>
    ''' <param name="element">The <see cref="UIElement"/> who's attached template's property you wish to retrieve</param>
    ''' <returns>The templates used by the givem <paramref name="element"/>
    ''' when using the <see cref="DynamicTemplateSelector"/></returns>
    Public Shared Function GetTemplates(element As UIElement) As TemplateCollection
        Return DirectCast(element.GetValue(TemplatesProperty), TemplateCollection)
    End Function

    ''' <summary>
    ''' Sets the value of the <paramref name="element"/>'s attached <see cref="TemplatesProperty"/>
    ''' </summary>
    ''' <param name="element">The element to set the property on</param>
    ''' <param name="collection">The collection of <see cref="Template"/>s to apply to this element</param>
    Public Shared Sub SetTemplates(element As UIElement, collection As TemplateCollection)
        element.SetValue(TemplatesProperty, collection)
    End Sub

    ''' <summary>
    ''' Overriden base method to allow the selection of the correct DataTemplate
    ''' </summary>
    ''' <param name="item">The item for which the template should be retrieved</param>
    ''' <param name="container">The object containing the current item</param>
    ''' <returns>The <see cref="DataTemplate"/> to use when rendering the <paramref name="item"/></returns>
    Public Overrides Function SelectTemplate(item As Object, container As System.Windows.DependencyObject) As System.Windows.DataTemplate
        'This should ensure that the item we are getting is in fact capable of holding our property
        'before we attempt to retrieve it.
        If Not (TypeOf container Is UIElement) Then
            Return MyBase.SelectTemplate(item, container)
        End If

        'First, we gather all the templates associated with the current control through our dependency property
        Dim templates As TemplateCollection = GetTemplates(TryCast(container, UIElement))
        If templates Is Nothing OrElse templates.Count = 0 Then
            MyBase.SelectTemplate(item, container)
        End If

        'Then we go through them checking if any of them match our criteria
        For Each template In templates
            'In this case, we are checking whether the type of the item
            'is the same as the type supported by our DataTemplate
            If template.Value.IsInstanceOfType(item) Then
                'And if it is, then we return that DataTemplate
                Return template.DataTemplate
            End If
        Next

        'If all else fails, then we go back to using the default DataTemplate
        Return MyBase.SelectTemplate(item, container)
    End Function
End Class

''' <summary>
''' Holds a collection of <see cref="Template"/> items
''' for application as a control's DataTemplate.
''' </summary>
Public Class TemplateCollection
    Inherits List(Of Template)

End Class

''' <summary>
''' Provides a link between a value and a <see cref="DataTemplate"/>
''' for the <see cref="DynamicTemplateSelector"/>
''' </summary>
''' <remarks>
''' In this case, our value is a <see cref="System.Type"/> which we are attempting to match
''' to a <see cref="DataTemplate"/>
''' </remarks>
Public Class Template
    Inherits DependencyObject
    ''' <summary>
    ''' Provides the value used to match this <see cref="DataTemplate"/> to an item
    ''' </summary>
    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(Type), GetType(Template))

    ''' <summary>
    ''' Provides the <see cref="DataTemplate"/> used to render items matching the <see cref="Value"/>
    ''' </summary>
    Public Shared ReadOnly DataTemplateProperty As DependencyProperty = DependencyProperty.Register("DataTemplate", GetType(DataTemplate), GetType(Template))

    ''' <summary>
    ''' Gets or Sets the value used to match this <see cref="DataTemplate"/> to an item
    ''' </summary>
    Public Property Value() As Type
        Get
            Return DirectCast(GetValue(ValueProperty), Type)
        End Get
        Set(value As Type)
            SetValue(ValueProperty, value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the <see cref="DataTemplate"/> used to render items matching the <see cref="Value"/>
    ''' </summary>
    Public Property DataTemplate() As DataTemplate
        Get
            Return DirectCast(GetValue(DataTemplateProperty), DataTemplate)
        End Get
        Set(value As DataTemplate)
            SetValue(DataTemplateProperty, value)
        End Set
    End Property
End Class

Public Class Wire
    Inherits Discrete

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Wire), New FrameworkPropertyMetadata(GetType(Wire)))
    End Sub

End Class

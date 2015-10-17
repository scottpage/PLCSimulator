Public Class Rung
    Inherits ItemsControl

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(Rung), New FrameworkPropertyMetadata(GetType(Rung)))
    End Sub

End Class

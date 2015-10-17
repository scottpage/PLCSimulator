Public Class EmptyCell
    Inherits Control

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(EmptyCell), New FrameworkPropertyMetadata(GetType(EmptyCell)))
    End Sub

End Class

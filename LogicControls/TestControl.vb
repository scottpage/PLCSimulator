Public Class TestControl
    Inherits System.Windows.Controls.Control

    Shared Sub New()
        DefaultStyleKeyProperty.OverrideMetadata(GetType(TestControl), New FrameworkPropertyMetadata(GetType(TestControl)))
    End Sub

End Class

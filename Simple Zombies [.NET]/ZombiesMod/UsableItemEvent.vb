Imports System

Namespace ZombiesMod
    <Serializable> _
    Public Class UsableItemEvent
        ' Methods
        Public Sub New(ByVal [event] As ItemEvent, ByVal eventArgument As Object)
            Me.Event = [event]
            Me.EventArgument = eventArgument
        End Sub


        ' Fields
        Public [Event] As ItemEvent
        Public EventArgument As Object
    End Class
End Namespace


Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Public Class PlayerExtended
        ' Methods
        Public Shared Sub IgnoreLowPriorityShockingEvents(ByVal player As Player, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(player.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_IGNORE_LOW_PRIORITY_SHOCKING_EVENTS, Hash), argumentArray1)
        End Sub

    End Class
End Namespace


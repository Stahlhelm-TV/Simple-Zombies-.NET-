Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Public Class EntityExtended
        ' Methods
        Public Shared Sub Fade(ByVal entity As Entity, ByVal state As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(entity.Handle, InputArgument), If(state, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.NETWORK_FADE_IN_ENTITY, Hash), argumentArray1)
        End Sub

        Public Shared Function HasClearLineOfSight(ByVal entity As Entity, ByVal target As Entity, ByVal visionDistance As Single) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(entity.Handle, InputArgument), DirectCast(target.Handle, InputArgument) }
            Return (Function.Call(Of Boolean)(DirectCast(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT, Hash), argumentArray1) AndAlso (SystemExtended.VDist(entity.get_Position, target.get_Position) < visionDistance))
        End Function

        Public Shared Function IsPlayingAnim(ByVal entity As Entity, ByVal animSet As String, ByVal animName As String) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(entity.Handle, InputArgument), DirectCast(animSet, InputArgument), DirectCast(animName, InputArgument), 3 }
            Return Function.Call(Of Boolean)(DirectCast(Hash.IS_ENTITY_PLAYING_ANIM, Hash), argumentArray1)
        End Function

    End Class
End Namespace


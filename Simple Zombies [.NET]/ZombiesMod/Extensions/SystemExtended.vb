Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Public Class SystemExtended
        ' Methods
        Public Shared Function VDist(ByVal v As Vector3, ByVal [to] As Vector3) As Single
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(v.X, InputArgument), DirectCast(v.Y, InputArgument), DirectCast(v.Z, InputArgument), DirectCast([to].X, InputArgument), DirectCast([to].Y, InputArgument), DirectCast([to].Z, InputArgument) }
            Return Function.Call(Of Single)(DirectCast(Hash.VDIST, Hash), argumentArray1)
        End Function

    End Class
End Namespace


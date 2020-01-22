Imports GTA
Imports GTA.Math
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Public Class V3Extended
        ' Methods
        Public Shared Function IsOnScreen(ByVal vector3 As Vector3) As Boolean
            Dim vector As Vector3 = GameplayCamera.get_Position
            Return (Vector3.Angle(DirectCast((vector3 - vector), Vector3), GameplayCamera.get_Direction) < GameplayCamera.FieldOfView)
        End Function

    End Class
End Namespace


Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Public Class PropExt
        ' Methods
        Public Shared Function GetDoorLockState(ByVal prop As Prop) As Boolean
            Dim flag As Boolean = False
            Dim num As Integer = 0
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(prop.get_Model.Hash, InputArgument), DirectCast(prop.get_Position.X, InputArgument), DirectCast(prop.get_Position.Y, InputArgument), DirectCast(prop.get_Position.Z, InputArgument), DirectCast(DirectCast(AddressOf flag, IntPtr), InputArgument), DirectCast(DirectCast(AddressOf num, IntPtr), InputArgument) }
            Function.Call(DirectCast(Hash.GET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Hash), argumentArray1)
            Return flag
        End Function

        Public Shared Sub SetStateOfDoor(ByVal prop As Prop, ByVal locked As Boolean, ByVal heading As DoorState)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(prop.get_Model.Hash, InputArgument), DirectCast(prop.get_Position.X, InputArgument), DirectCast(prop.get_Position.Y, InputArgument), DirectCast(prop.get_Position.Z, InputArgument), DirectCast(locked, InputArgument), DirectCast(heading, InputArgument), 1 }
            Function.Call(DirectCast(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Hash), argumentArray1)
        End Sub

    End Class
End Namespace


Imports GTA
Imports GTA.Native
Imports System

Namespace ZombiesMod.Extensions
    Public Class GameExtended
        ' Methods
        Public Shared Sub DisableWeaponWheel()
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponWheelLeftRight, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponWheelNext, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponWheelPrev, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponWheelUpDown, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.NextWeapon, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.DropWeapon, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.PrevWeapon, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponSpecial, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.WeaponSpecial2, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.SelectWeapon, Control))
        End Sub

        Public Shared Function GetMobilePhoneId() As Integer
            Dim argument As New OutputArgument
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(argument, InputArgument) }
            Function.Call(DirectCast(Hash.GET_MOBILE_PHONE_RENDER_ID, Hash), argumentArray1)
            Return argument.GetResult(Of Integer)
        End Function

    End Class
End Namespace


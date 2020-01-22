Imports GTA.Native
Imports System

Namespace ZombiesMod
    Public Interface IWeapon
        ' Properties
        Property Ammo As Integer

        Property Hash As WeaponHash

        Property Components As WeaponComponent()

    End Interface
End Namespace


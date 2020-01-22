Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class Weapon
        Implements IWeapon
        ' Methods
        Public Sub New(ByVal ammo As Integer, ByVal hash As WeaponHash, ByVal components As WeaponComponent())
            Me.Ammo = ammo
            Me.Hash = hash
            Me.Components = components
        End Sub


        ' Properties
        Public Property Ammo As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property Hash As WeaponHash
            Get
            Set(ByVal value As WeaponHash)
        End Property
        Public Property Components As WeaponComponent()
            Get
            Set(ByVal value As WeaponComponent())
        End Property
    End Class
End Namespace


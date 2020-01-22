Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class WeaponInventoryItem
        Inherits InventoryItemBase
        Implements IWeapon, ICraftable
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal ammo As Integer, ByVal weaponHash As WeaponHash, ByVal weaponComponents As WeaponComponent())
            MyBase.New(amount, maxAmount, id, description)
            Me.Ammo = ammo
            Me.Hash = weaponHash
            Me.Components = weaponComponents
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
        Public Property RequiredComponents As CraftableItemComponent()
            Get
            Set(ByVal value As CraftableItemComponent())
        End Property
        Public Property Components As WeaponComponent()
            Get
            Set(ByVal value As WeaponComponent())
        End Property
    End Class
End Namespace


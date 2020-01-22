Imports GTA
Imports GTA.Math
Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class WeaponStorageInventoryItem
        Inherits BuildableInventoryItem
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal propName As String, ByVal blipSprite As BlipSprite, ByVal blipColor As BlipColor, ByVal groundOffset As Vector3, ByVal interactable As Boolean, ByVal isDoor As Boolean, ByVal canBePickedUp As Boolean, ByVal weaponsList As List(Of Weapon))
            MyBase.New(amount, maxAmount, id, description, propName, blipSprite, blipColor, groundOffset, interactable, isDoor, canBePickedUp)
            Me.WeaponsList = weaponsList
        End Sub


        ' Properties
        Public Property WeaponsList As List(Of Weapon)
            Get
            Set(ByVal value As List(Of Weapon))
        End Property
    End Class
End Namespace


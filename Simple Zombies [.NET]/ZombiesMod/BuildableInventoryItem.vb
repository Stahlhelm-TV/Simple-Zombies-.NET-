Imports GTA
Imports GTA.Math
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class BuildableInventoryItem
        Inherits InventoryItemBase
        Implements IProp, ICraftable
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal propName As String, ByVal blipSprite As BlipSprite, ByVal blipColor As BlipColor, ByVal groundOffset As Vector3, ByVal interactable As Boolean, ByVal isDoor As Boolean, ByVal canBePickedUp As Boolean)
            MyBase.New(amount, maxAmount, id, description)
            Me.PropName = propName
            Me.BlipSprite = blipSprite
            Me.BlipColor = blipColor
            Me.GroundOffset = groundOffset
            Me.Interactable = interactable
            Me.IsDoor = isDoor
            Me.CanBePickedUp = canBePickedUp
        End Sub


        ' Properties
        Public Property PropName As String
            Get
            Set(ByVal value As String)
        End Property
        Public Property BlipSprite As BlipSprite
            Get
            Set(ByVal value As BlipSprite)
        End Property
        Public Property BlipColor As BlipColor
            Get
            Set(ByVal value As BlipColor)
        End Property
        Public Property GroundOffset As Vector3
            Get
            Set(ByVal value As Vector3)
        End Property
        Public Property Interactable As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public Property IsDoor As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public Property CanBePickedUp As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public Property RequiredComponents As CraftableItemComponent()
            Get
            Set(ByVal value As CraftableItemComponent())
        End Property
    End Class
End Namespace


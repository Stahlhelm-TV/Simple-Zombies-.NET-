Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class UsableInventoryItem
        Inherits InventoryItemBase
        Implements ICraftable
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal itemEvents As UsableItemEvent())
            MyBase.New(amount, maxAmount, id, description)
            Me.ItemEvents = itemEvents
        End Sub


        ' Properties
        Public Property ItemEvents As UsableItemEvent()
            Get
            Set(ByVal value As UsableItemEvent())
        End Property
        Public Property RequiredComponents As CraftableItemComponent()
            Get
            Set(ByVal value As CraftableItemComponent())
        End Property
    End Class
End Namespace


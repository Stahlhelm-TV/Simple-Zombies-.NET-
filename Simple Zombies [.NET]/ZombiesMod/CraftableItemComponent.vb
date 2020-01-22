Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class CraftableItemComponent
        ' Methods
        Public Sub New(ByVal resource As InventoryItemBase, ByVal requiredAmount As Integer)
            Me.Resource = resource
            Me.RequiredAmount = requiredAmount
        End Sub


        ' Properties
        Public Property Resource As InventoryItemBase
            Get
            Set(ByVal value As InventoryItemBase)
        End Property
        Public Property RequiredAmount As Integer
            Get
            Set(ByVal value As Integer)
        End Property
    End Class
End Namespace


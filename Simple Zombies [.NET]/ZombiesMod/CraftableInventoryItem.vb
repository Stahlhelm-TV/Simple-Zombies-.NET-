Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class CraftableInventoryItem
        Inherits InventoryItemBase
        Implements ICraftable, IValidatable
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal validation As Func(Of Boolean))
            MyBase.New(amount, maxAmount, id, description)
            Me.Validation = validation
        End Sub


        ' Properties
        Public Property RequiredComponents As CraftableItemComponent()
            Get
            Set(ByVal value As CraftableItemComponent())
        End Property
        Public Property Validation As Func(Of Boolean)
            Get
            Set(ByVal value As Func(Of Boolean))
        End Property
    End Class
End Namespace


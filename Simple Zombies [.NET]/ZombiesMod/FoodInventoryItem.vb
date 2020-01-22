Imports GTA
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class FoodInventoryItem
        Inherits InventoryItemBase
        Implements IFood, IAnimatable, ICraftable
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String, ByVal animationDict As String, ByVal animationName As String, ByVal animationFlags As AnimationFlags, ByVal animationDuration As Integer, ByVal foodType As FoodType, ByVal restorationAmount As Single)
            MyBase.New(amount, maxAmount, id, description)
            Me.AnimationDict = animationDict
            Me.AnimationName = animationName
            Me.AnimationFlags = animationFlags
            Me.AnimationDuration = animationDuration
            Me.FoodType = foodType
            Me.RestorationAmount = restorationAmount
        End Sub


        ' Properties
        Public Property AnimationDict As String
            Get
            Set(ByVal value As String)
        End Property
        Public Property AnimationName As String
            Get
            Set(ByVal value As String)
        End Property
        Public Property AnimationFlags As AnimationFlags
            Get
            Set(ByVal value As AnimationFlags)
        End Property
        Public Property AnimationDuration As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property FoodType As FoodType
            Get
            Set(ByVal value As FoodType)
        End Property
        Public Property RestorationAmount As Single
            Get
            Set(ByVal value As Single)
        End Property
        Public Property RequiredComponents As CraftableItemComponent()
            Get
            Set(ByVal value As CraftableItemComponent())
        End Property
        Public Property NearbyResource As NearbyResource
            Get
            Set(ByVal value As NearbyResource)
        End Property
    End Class
End Namespace


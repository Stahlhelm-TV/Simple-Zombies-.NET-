Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class MapProp
        Implements IMapObject, IIdentifier, IProp, ISpatial, IHandleable, IDeletable
        ' Methods
        Public Sub New(ByVal id As String, ByVal propName As String, ByVal blipSprite As BlipSprite, ByVal blipColor As BlipColor, ByVal groundOffset As Vector3, ByVal interactable As Boolean, ByVal isDoor As Boolean, ByVal canBePickedUp As Boolean, ByVal rotation As Vector3, ByVal position As Vector3, ByVal handle As Integer, ByVal weapons As List(Of Weapon))
            Me.Id = id
            Me.PropName = propName
            Me.BlipSprite = blipSprite
            Me.BlipColor = blipColor
            Me.GroundOffset = groundOffset
            Me.Interactable = interactable
            Me.IsDoor = isDoor
            Me.CanBePickedUp = canBePickedUp
            Me.Rotation = rotation
            Me.Position = position
            Me.Handle = handle
            Me.Weapons = weapons
        End Sub

        Public Sub Delete()
            Dim handle As Integer = Me.Handle
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(DirectCast(AddressOf handle, IntPtr), InputArgument) }
            Function.Call(DirectCast(Hash.DELETE_ENTITY, Hash), argumentArray1)
            Me.Handle = handle
        End Sub

        Public Function Exists() As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(Me.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.DOES_ENTITY_EXIST, Hash), argumentArray1)
        End Function


        ' Properties
        Public Property Id As String
            Get
            Set(ByVal value As String)
        End Property
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
        Public Property Rotation As Vector3
            Get
            Set(ByVal value As Vector3)
        End Property
        Public Property Position As Vector3
            Get
            Set(ByVal value As Vector3)
        End Property
        Property Handle As Integer
            Public Get
            Public Set(ByVal value As Integer)
        End Property
        Public Property Weapons As List(Of Weapon)
            Get
            Set(ByVal value As List(Of Weapon))
        End Property
    End Class
End Namespace


Imports GTA
Imports GTA.Math
Imports System

Namespace ZombiesMod
    Public Interface IProp
        ' Properties
        Property PropName As String

        Property BlipSprite As BlipSprite

        Property BlipColor As BlipColor

        Property GroundOffset As Vector3

        Property Interactable As Boolean

        Property IsDoor As Boolean

        Property CanBePickedUp As Boolean

    End Interface
End Namespace


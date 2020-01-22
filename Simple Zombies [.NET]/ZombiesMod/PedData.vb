Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports ZombiesMod.PlayerManagement

Namespace ZombiesMod
    <Serializable> _
    Public Class PedData
        Implements ISpatial, IHandleable, IDeletable
        ' Methods
        Public Sub New(ByVal handle As Integer, ByVal hash As Integer, ByVal rotation As Vector3, ByVal position As Vector3, ByVal task As PedTask, ByVal weapons As List(Of Weapon))
            Me.Handle = handle
            Me.Hash = hash
            Me.Rotation = rotation
            Me.Position = position
            Me.Task = task
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
        Property Handle As Integer
            Public Get
            Public Set(ByVal value As Integer)
        End Property
        Public Property Hash As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property Rotation As Vector3
            Get
            Set(ByVal value As Vector3)
        End Property
        Public Property Position As Vector3
            Get
            Set(ByVal value As Vector3)
        End Property
        Public Property Task As PedTask
            Get
            Set(ByVal value As PedTask)
        End Property
        Public Property Weapons As List(Of Weapon)
            Get
            Set(ByVal value As List(Of Weapon))
        End Property
    End Class
End Namespace


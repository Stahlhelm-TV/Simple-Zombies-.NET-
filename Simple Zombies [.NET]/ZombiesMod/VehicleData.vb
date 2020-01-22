Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class VehicleData
        Implements ISpatial, IHandleable, IDeletable
        ' Methods
        Public Sub New(ByVal handle As Integer, ByVal hash As Integer, ByVal rotation As Vector3, ByVal position As Vector3, ByVal primaryColor As VehicleColor, ByVal secondaryColor As VehicleColor, ByVal health As Integer, ByVal engineHealth As Single, ByVal heading As Single, ByVal neonLights As VehicleNeonLight(), ByVal mods As List(Of Tuple(Of VehicleMod, Integer)), ByVal toggleMods As VehicleToggleMod(), ByVal windowTint As VehicleWindowTint, ByVal wheelType As VehicleWheelType, ByVal neonColor As Color, ByVal livery As Integer, ByVal wheels1 As Boolean, ByVal wheels2 As Boolean)
            Me.Handle = handle
            Me.Hash = hash
            Me.Rotation = rotation
            Me.Position = position
            Me.PrimaryColor = primaryColor
            Me.SecondaryColor = secondaryColor
            Me.Health = health
            Me.EngineHealth = engineHealth
            Me.Heading = heading
            Me.NeonLights = neonLights
            Me.Mods = mods
            Me.ToggleMods = toggleMods
            Me.WindowTint = windowTint
            Me.WheelType = wheelType
            Me.NeonColor = neonColor
            Me.Livery = livery
            Me.Wheels1 = wheels1
            Me.Wheels2 = wheels2
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
        Public Property PrimaryColor As VehicleColor
            Get
            Set(ByVal value As VehicleColor)
        End Property
        Public Property SecondaryColor As VehicleColor
            Get
            Set(ByVal value As VehicleColor)
        End Property
        Public Property Health As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property EngineHealth As Single
            Get
            Set(ByVal value As Single)
        End Property
        Public Property Heading As Single
            Get
            Set(ByVal value As Single)
        End Property
        Public Property NeonLights As VehicleNeonLight()
            Get
            Set(ByVal value As VehicleNeonLight())
        End Property
        Public Property Mods As List(Of Tuple(Of VehicleMod, Integer))
            Get
            Set(ByVal value As List(Of Tuple(Of VehicleMod, Integer)))
        End Property
        Public Property ToggleMods As VehicleToggleMod()
            Get
            Set(ByVal value As VehicleToggleMod())
        End Property
        Public Property WindowTint As VehicleWindowTint
            Get
            Set(ByVal value As VehicleWindowTint)
        End Property
        Public Property WheelType As VehicleWheelType
            Get
            Set(ByVal value As VehicleWheelType)
        End Property
        Public Property NeonColor As Color
            Get
            Set(ByVal value As Color)
        End Property
        Public Property Livery As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property Wheels1 As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public Property Wheels2 As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
    End Class
End Namespace


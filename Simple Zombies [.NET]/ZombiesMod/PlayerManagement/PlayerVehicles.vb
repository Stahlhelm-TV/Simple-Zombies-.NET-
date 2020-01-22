Imports GTA
Imports GTA.Native
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports ZombiesMod
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.PlayerManagement
    Public Class PlayerVehicles
        Inherits Script
        ' Methods
        Public Sub New()
            PlayerVehicles.Instance = Me
            AddHandler MyBase.Aborted, New EventHandler(AddressOf Me.OnAborted)
        End Sub

        Private Shared Sub AddBlipToVehicle(ByVal vehicle As Vehicle)
            Dim blip As Blip = vehicle.AddBlip
            blip.set_Sprite(PlayerVehicles.GetSprite(vehicle))
            blip.set_Color(DirectCast(BlipColor.PurpleDark, BlipColor))
            blip.set_Name(vehicle.get_FriendlyName)
            blip.set_Scale(0.85!)
        End Sub

        Private Shared Sub AddKit(ByVal vehicle As Vehicle, ByVal data As VehicleData)
            If (Not Object.ReferenceEquals(data, Nothing) AndAlso (Not vehicle Is Nothing)) Then
                vehicle.InstallModKit
                Dim neonLights As VehicleNeonLight() = data.NeonLights
                If (neonLights Is Nothing) Then
                    Dim local1 As VehicleNeonLight() = neonLights
                Else
                    Enumerable.ToList(Of VehicleNeonLight)(neonLights).ForEach(h => vehicle.SetNeonLightsOn(h, True))
                End If
                Dim mods As List(Of Tuple(Of VehicleMod, Integer)) = data.Mods
                If (mods Is Nothing) Then
                    Dim local2 As List(Of Tuple(Of VehicleMod, Integer)) = mods
                Else
                    mods.ForEach(m => vehicle.SetMod(m.Item1, m.Item2, True))
                End If
                Dim toggleMods As VehicleToggleMod() = data.ToggleMods
                If (toggleMods Is Nothing) Then
                    Dim local3 As VehicleToggleMod() = toggleMods
                Else
                    Enumerable.ToList(Of VehicleToggleMod)(toggleMods).ForEach(h => vehicle.ToggleMod(h, True))
                End If
                vehicle.set_WindowTint(data.WindowTint)
                vehicle.set_WheelType(data.WheelType)
                vehicle.set_NeonLightsColor(data.NeonColor)
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(vehicle.Handle, InputArgument), DirectCast(data.Livery, InputArgument) }
                Function.Call(DirectCast(Hash.SET_VEHICLE_LIVERY, Hash), argumentArray1)
            End If
        End Sub

        Public Sub Deserialize()
            If (Me._vehicleCollection Is Nothing) Then
                Dim objA As VehicleCollection = Serializer.Deserialize(Of VehicleCollection)("./scripts/Vehicles.dat")
                If Object.ReferenceEquals(objA, Nothing) Then
                    objA = New VehicleCollection
                End If
                Me._vehicleCollection = objA
                AddHandler Me._vehicleCollection.ListChanged, sender => Me.Serialize(False)
                Dim data As VehicleData
                For Each data In Me._vehicleCollection
                    Dim vehicle As Vehicle = World.CreateVehicle(DirectCast(data.Hash, Model), data.Position)
                    If (vehicle Is Nothing) Then
                        UI.Notify("Failed to load vehicle.")
                        Exit For
                    End If
                    vehicle.set_PrimaryColor(data.PrimaryColor)
                    vehicle.set_SecondaryColor(data.SecondaryColor)
                    vehicle.set_Health(data.Health)
                    vehicle.set_EngineHealth(data.EngineHealth)
                    vehicle.set_Rotation(data.Rotation)
                    data.Handle = vehicle.Handle
                    PlayerVehicles.AddKit(vehicle, data)
                    PlayerVehicles.AddBlipToVehicle(vehicle)
                    Me._vehicles.Add(vehicle)
                    vehicle.set_IsPersistent(True)
                    AddHandler New EntityEventWrapper(DirectCast(vehicle, Entity)).Died, New OnDeathEvent(AddressOf Me.WrapperOnDied)
                Next
            End If
        End Sub

        Private Shared Function GetSprite(ByVal vehicle As Vehicle) As BlipSprite
            Return If((vehicle.get_ClassType = DirectCast(CInt(VehicleClass.Motorcycles), VehicleClass)), DirectCast(BlipSprite.PersonalVehicleBike, BlipSprite), If((vehicle.get_ClassType = DirectCast(CInt(VehicleClass.Boats), VehicleClass)), DirectCast(BlipSprite.Boat, BlipSprite), If((vehicle.get_ClassType = DirectCast(CInt(VehicleClass.Helicopters), VehicleClass)), DirectCast(BlipSprite.Helicopter, BlipSprite), If((vehicle.get_ClassType = DirectCast(CInt(VehicleClass.Planes), VehicleClass)), DirectCast(BlipSprite.Plane, BlipSprite), DirectCast(BlipSprite.PersonalVehicleCar, BlipSprite)))))
        End Function

        Private Sub OnAborted(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Enumerable.ToList(Of VehicleData)(Me._vehicleCollection).ForEach(vehicle => vehicle.Delete)
        End Sub

        Public Sub SaveVehicle(ByVal vehicle As Vehicle)
            If Object.ReferenceEquals(Me._vehicleCollection, Nothing) Then
                Me.Deserialize
            End If
            Dim vehicleData As VehicleData = Enumerable.ToList(Of VehicleData)(Me._vehicleCollection).Find(v => (v.Handle = vehicle.Handle))
            If (Not vehicleData Is Nothing) Then
                PlayerVehicles.UpdateDataSpecific(vehicleData, vehicle)
                Me.Serialize(True)
            Else
                Dim neonLights As VehicleNeonLight() = Enumerable.ToArray(Of VehicleNeonLight)(Enumerable.Where(Of VehicleNeonLight)(DirectCast(Enum.GetValues(GetType(VehicleNeonLight)), VehicleNeonLight()), New Func(Of VehicleNeonLight, Boolean)(AddressOf vehicle.IsNeonLightsOn)))
                Dim values As VehicleMod() = DirectCast(Enum.GetValues(GetType(VehicleMod)), VehicleMod())
                Dim mods As New List(Of Tuple(Of VehicleMod, Integer))
                Enumerable.ToList(Of VehicleMod)(values).ForEach(Function (ByVal h As VehicleMod) 
                    Dim mod As Integer = vehicle.GetMod(h)
                    If ([mod] <> -1) Then
                        mods.Add(New Tuple(Of VehicleMod, Integer)(h, [mod]))
                    End If
                End Function)
                Dim toggleMods As VehicleToggleMod() = Enumerable.ToArray(Of VehicleToggleMod)(Enumerable.Where(Of VehicleToggleMod)(DirectCast(Enum.GetValues(GetType(VehicleToggleMod)), VehicleToggleMod()), New Func(Of VehicleToggleMod, Boolean)(AddressOf vehicle.IsToggleModOn)))
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(vehicle.Handle, InputArgument) }
                Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(vehicle.Handle, InputArgument), &H17 }
                Dim argumentArray3 As InputArgument() = New InputArgument() { DirectCast(vehicle.Handle, InputArgument), &H18 }
                Dim item As New VehicleData(vehicle.Handle, vehicle.get_Model.Hash, vehicle.get_Rotation, vehicle.get_Position, vehicle.get_PrimaryColor, vehicle.get_SecondaryColor, vehicle.Health, vehicle.EngineHealth, vehicle.Heading, neonLights, mods, toggleMods, vehicle.get_WindowTint, vehicle.get_WheelType, vehicle.get_NeonLightsColor, Function.Call(Of Integer)(DirectCast(Hash.GET_VEHICLE_LIVERY, Hash), argumentArray1), Function.Call(Of Boolean)(DirectCast(Hash.GET_VEHICLE_MOD_VARIATION, Hash), argumentArray2), Function.Call(Of Boolean)(DirectCast(Hash.GET_VEHICLE_MOD_VARIATION, Hash), argumentArray3))
                Me._vehicleCollection.Add(item)
                Me._vehicles.Add(vehicle)
                vehicle.set_IsPersistent(True)
                AddHandler New EntityEventWrapper(DirectCast(vehicle, Entity)).Died, New OnDeathEvent(AddressOf Me.WrapperOnDied)
                PlayerVehicles.AddBlipToVehicle(vehicle)
            End If
        End Sub

        Public Sub Serialize(ByVal Optional notify As Boolean = False)
            If Not Object.ReferenceEquals(Me._vehicleCollection, Nothing) Then
                Me.UpdateVehicleData
                Serializer.Serialize(Of VehicleCollection)("./scripts/Vehicles.dat", Me._vehicleCollection)
                If notify Then
                    UI.Notify(If((Me._vehicleCollection.Count <= 0), "No vehicles.", "~p~Vehicles~s~ saved!"))
                End If
            End If
        End Sub

        Private Shared Sub UpdateDataSpecific(ByVal vehicleData As VehicleData, ByVal vehicle As Vehicle)
            vehicleData.Position = vehicle.get_Position
            vehicleData.Rotation = vehicle.get_Rotation
            vehicleData.Health = vehicle.Health
            vehicleData.EngineHealth = vehicle.EngineHealth
            vehicleData.PrimaryColor = vehicle.get_PrimaryColor
            vehicleData.SecondaryColor = vehicle.get_SecondaryColor
        End Sub

        Private Sub UpdateVehicleData()
            If (Me._vehicleCollection.Count > 0) Then
                Enumerable.ToList(Of VehicleData)(Me._vehicleCollection).ForEach(Function (ByVal v As VehicleData) 
                    Dim vehicle As Vehicle = Me._vehicles.Find(i => (i.Handle = v.Handle))
                    If (Not vehicle Is Nothing) Then
                        PlayerVehicles.UpdateDataSpecific(v, vehicle)
                    End If
                End Function)
            End If
        End Sub

        Private Sub WrapperOnDied(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            UI.Notify("Your vehicle was ~r~destroyed~s~!")
            Me._vehicleCollection.Remove(Enumerable.ToList(Of VehicleData)(Me._vehicleCollection).Find(v => (v.Handle = entity.Handle)))
            Dim blip1 As Blip = entity.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            sender.Dispose
        End Sub


        ' Properties
        Property Instance As PlayerVehicles
            <CompilerGenerated> _
            Public Shared Get
                Return PlayerVehicles.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As PlayerVehicles)
                PlayerVehicles.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Private _vehicleCollection As VehicleCollection
        Private ReadOnly _vehicles As List(Of Vehicle) = New List(Of Vehicle)

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub <OnAborted>b__3_0(ByVal vehicle As VehicleData)
                vehicle.Delete
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__3_0 As Action(Of VehicleData)
        End Class
    End Class
End Namespace


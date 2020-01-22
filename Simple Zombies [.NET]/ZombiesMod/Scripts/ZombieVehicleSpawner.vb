Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports ZombiesMod
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static

Namespace ZombiesMod.Scripts
    Public Class ZombieVehicleSpawner
        Inherits Script
        Implements ISpawner
        ' Methods
        Public Sub New()
            ZombieVehicleSpawner.Instance = Me
            Me._minZombies = MyBase.get_Settings.GetValue(Of Integer)("spawning", "min_spawned_zombies", Me._minZombies)
            Me._maxZombies = MyBase.get_Settings.GetValue(Of Integer)("spawning", "max_spawned_zombies", Me._maxZombies)
            Me._minVehicles = MyBase.get_Settings.GetValue(Of Integer)("spawning", "min_spawned_vehicles", Me._minVehicles)
            Me._maxVehicles = MyBase.get_Settings.GetValue(Of Integer)("spawning", "max_spawned_vehicles", Me._maxVehicles)
            Me._spawnDistance = MyBase.get_Settings.GetValue(Of Integer)("spawning", "spawn_distance", Me._spawnDistance)
            Me._minSpawnDistance = MyBase.get_Settings.GetValue(Of Integer)("spawning", "min_spawn_distance", Me._minSpawnDistance)
            Me._zombieHealth = MyBase.get_Settings.GetValue(Of Integer)("zombies", "zombie_health", Me._zombieHealth)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "min_spawned_zombies", Me._minZombies)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "max_spawned_zombies", Me._maxZombies)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "min_spawned_vehicles", Me._minVehicles)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "max_spawned_vehicles", Me._maxVehicles)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "spawn_distance", Me._spawnDistance)
            MyBase.get_Settings.SetValue(Of Integer)("spawning", "min_spawn_distance", Me._minSpawnDistance)
            MyBase.get_Settings.SetValue(Of Integer)("zombies", "zombie_health", Me._zombieHealth)
            MyBase.get_Settings.Save
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, (sender, args) => Me.ClearAll
            MyBase.set_Interval(100)
        End Sub

        Private Sub ClearAll()
            Do While True
                If (Me._peds.Count <= 0) Then
                    Do While (Me._vehicles.Count > 0)
                        Me._vehicles(0).Delete
                        Me._vehicles.RemoveAt(0)
                    Loop
                    Return
                End If
                Me._peds(0).Delete
                Me._peds.RemoveAt(0)
            Loop
        End Sub

        Private Shared Function Exists(ByVal arg As Entity) As Boolean
            Return ((Not arg Is Nothing) AndAlso arg.Exists)
        End Function

        Public Function IsInvalidZone(ByVal spawn As Vector3) As Boolean
            Return (Not Array.Find(Of String)(Me.InvalidZoneNames, z => (z = World.GetZoneName(spawn))) Is Nothing)
        End Function

        Public Function IsValidSpawn(ByVal spawnPoint As Vector3) As Boolean
            Return (Me.SpawnBlocker.FindIndex(spawn => (SystemExtended.VDist(spawn, spawnPoint) < 150!)) <= -1)
        End Function

        Private Sub OnTick(ByVal sender As Object, ByVal e As EventArgs)
            If Not Me.Spawn Then
                Me.ClearAll
            Else
                If Not MenuConrtoller.MenuPool.IsAnyMenuOpen Then
                    If (ZombieCreator.IsNightFall AndAlso Not Me._nightFall) Then
                        UiExtended.DisplayHelpTextThisFrame("Nightfall approaches. Zombies are far more ~r~aggressive~s~ at night.", False)
                        Me._nightFall = True
                    ElseIf Not ZombieCreator.IsNightFall Then
                        Me._nightFall = False
                    End If
                End If
                Me.SpawnVehicles
                Me.SpawnPeds
            End If
        End Sub

        Private Shared Sub OpenRandomDoor(ByVal veh As Vehicle)
            Dim doors As VehicleDoor() = veh.GetDoors
            Dim door As VehicleDoor = doors(Database.Random.Next(doors.Length))
            veh.OpenDoor(door, False, True)
        End Sub

        Private Shared Sub SmashRandomWindow(ByVal veh As Vehicle)
            Dim windowArray As VehicleWindow() = Enumerable.ToArray(Of VehicleWindow)((From v In DirectCast(Enum.GetValues(GetType(VehicleWindow)), VehicleWindow())
                Where Function.Call(Of Boolean)(DirectCast(Hash.IS_VEHICLE_WINDOW_INTACT, Hash), New InputArgument() { DirectCast(veh.Handle, InputArgument), DirectCast(v, InputArgument) })
                Select v))
            Dim window As VehicleWindow = windowArray(Database.Random.Next(windowArray.Length))
            veh.SmashWindow(window)
        End Sub

        Private Sub SpawnPeds()
            Me._peds = Enumerable.ToList(Of Ped)(Enumerable.Where(Of Ped)(Me._peds, New Func(Of Ped, Boolean)(AddressOf ZombieVehicleSpawner.Exists)))
            If (Me._peds.Count < Me._maxZombies) Then
                Dim num As Integer = 0
                Do While True
                    If (num < (Me._maxZombies - Me._peds.Count)) Then
                        Dim vector3 As Vector3 = ZombieVehicleSpawner.PlayerPed.get_Position
                        Dim nextPositionOnStreet As Vector3 = World.GetNextPositionOnStreet(vector3.Around(CSng(Me._spawnDistance)))
                        If Me.IsValidSpawn(nextPositionOnStreet) Then
                            Dim vector2 As Vector3 = nextPositionOnStreet.Around(5!)
                            If (Not V3Extended.IsOnScreen(vector2) AndAlso (SystemExtended.VDist(vector2, ZombieVehicleSpawner.PlayerPosition) >= Me._minSpawnDistance)) Then
                                Dim ped As Ped = World.CreateRandomPed(vector2)
                                If (Not ped Is Nothing) Then
                                    Me._peds.Add(DirectCast(ZombieCreator.InfectPed(ped, Me._zombieHealth, False), Ped))
                                End If
                                num += 1
                                Continue Do
                            End If
                        End If
                    End If
                    Exit Do
                Loop
            End If
        End Sub

        Private Sub SpawnVehicles()
            Me._vehicles = Enumerable.ToList(Of Vehicle)(Enumerable.Where(Of Vehicle)(Me._vehicles, New Func(Of Vehicle, Boolean)(AddressOf ZombieVehicleSpawner.Exists)))
            If (Me._vehicles.Count < Me._maxVehicles) Then
                Dim num As Integer = 0
                Do While True
                    If (num < (Me._maxVehicles - Me._vehicles.Count)) Then
                        Dim vector3 As Vector3 = ZombieVehicleSpawner.PlayerPed.get_Position
                        Dim nextPositionOnStreet As Vector3 = World.GetNextPositionOnStreet(vector3.Around(CSng(Me._spawnDistance)))
                        Dim flag2 As Boolean = Me.IsInvalidZone(nextPositionOnStreet)
                        If (Not flag2 AndAlso Me.IsValidSpawn(nextPositionOnStreet)) Then
                            Dim vector2 As Vector3 = nextPositionOnStreet.Around(2.5!)
                            If (Not V3Extended.IsOnScreen(vector2) AndAlso (SystemExtended.VDist(vector2, ZombieVehicleSpawner.PlayerPosition) >= Me._minSpawnDistance)) Then
                                Dim veh As Vehicle = World.CreateVehicle(Database.GetRandomVehicleModel, vector2)
                                If (Not veh Is Nothing) Then
                                    veh.set_EngineHealth(0!)
                                    veh.MarkAsNoLongerNeeded
                                    veh.set_DirtLevel(14!)
                                    ZombieVehicleSpawner.SmashRandomWindow(veh)
                                    If (Database.Random.NextDouble < 0.5) Then
                                        ZombieVehicleSpawner.SmashRandomWindow(veh)
                                    End If
                                    If (Database.Random.NextDouble < 0.20000000298023224) Then
                                        ZombieVehicleSpawner.OpenRandomDoor(veh)
                                    End If
                                    veh.set_Heading(CSng(Database.Random.Next(1, 360)))
                                    Me._vehicles.Add(veh)
                                End If
                                num += 1
                                Continue Do
                            End If
                        End If
                    End If
                    Exit Do
                Loop
            End If
        End Sub


        ' Properties
        Public Property Spawn As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public ReadOnly Property SpawnBlocker As SpawnBlocker
            Get
        End Property
        Private Shared ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property

        Private Shared ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property

        Property Instance As ZombieVehicleSpawner
            <CompilerGenerated> _
            Public Shared Get
                Return ZombieVehicleSpawner.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As ZombieVehicleSpawner)
                ZombieVehicleSpawner.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Public Const SpawnBlockingDistance As Integer = 150
        Private ReadOnly _maxVehicles As Integer = 10
        Private ReadOnly _maxZombies As Integer = 30
        Private ReadOnly _minVehicles As Integer = 1
        Private ReadOnly _minZombies As Integer = 7
        Private ReadOnly _spawnDistance As Integer = &H4B
        Private ReadOnly _minSpawnDistance As Integer = 50
        Private ReadOnly _zombieHealth As Integer = 750
        Private _nightFall As Boolean
        Private _peds As List(Of Ped) = New List(Of Ped)
        Private _vehicles As List(Of Vehicle) = New List(Of Vehicle)
        Private ReadOnly _classes As VehicleClass() = New VehicleClass() { VehicleClass.OffRoad, VehicleClass.Muscle, VehicleClass.Sedans }
        Public InvalidZoneNames As String() = New String() { "Los Santos International Airport", "Fort Zancudo", "Bolingbroke Penitentiary", "Davis Quartz", "Palmer-Taylor Power Station", "RON Alternates Wind Farm", "Terminal", "Humane Labs and Research" }
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private ReadOnly <SpawnBlocker>k__BackingField As SpawnBlocker = New SpawnBlocker
    End Class
End Namespace


Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static

Namespace ZombiesMod.Controllers
    Public Class WorldController
        Inherits Script
        ' Methods
        Public Sub New()
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, New EventHandler(AddressOf WorldController.OnAborted)
        End Sub

        Private Shared Sub OnAborted(ByVal sender As Object, ByVal e As EventArgs)
            WorldController.Reset
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal e As EventArgs)
            If Not WorldController.Configure Then
                If Not Me._reset Then
                    WorldController.Reset
                    Me._reset = True
                End If
            Else
                WorldExtended.ClearCops(10000!)
                WorldExtended.SetScenarioPedDensityThisMultiplierFrame(0!)
                WorldExtended.SetVehicleDensityMultiplierThisFrame(0!)
                WorldExtended.SetRandomVehicleDensityMultiplierThisFrame(0!)
                WorldExtended.SetParkedVehicleDensityMultiplierThisFrame(0!)
                WorldExtended.SetPedDensityThisMultiplierFrame(0!)
                WorldExtended.SetScenarioPedDensityThisMultiplierFrame(0!)
                Game.set_MaxWantedLevel(0)
                Dim array As Vehicle() = Enumerable.ToArray(Of Vehicle)((From v In World.GetAllVehicles
                    Where Not v.IsPersistent
                    Select v))
                Dim vehicleArray2 As Vehicle() = Array.FindAll(Of Vehicle)(array, v => (v.get_ClassType = DirectCast(CInt(VehicleClass.Planes), VehicleClass)))
                Dim vehicleArray3 As Vehicle() = Array.FindAll(Of Vehicle)(array, v => (v.get_ClassType = DirectCast(CInt(VehicleClass.Trains), VehicleClass)))
                Array.ForEach(Of Vehicle)(Array.FindAll(Of Vehicle)(array, v => (v.get_Driver.Exists AndAlso Not v.get_Driver.IsPlayer)), vehicle => vehicle.Delete)
                Array.ForEach(Of Vehicle)(vehicleArray2, Function (ByVal plane As Vehicle) 
                    If (plane.get_Driver.Exists AndAlso (Not plane.get_Driver.IsPlayer AndAlso Not plane.get_Driver.IsDead)) Then
                        plane.get_Driver.Kill
                    End If
                End Function)
                Array.ForEach(Of Vehicle)(vehicleArray3, Function (ByVal t As Vehicle) 
                    Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(t.Handle, InputArgument), 0! }
                    Function.Call(DirectCast(Hash.SET_TRAIN_SPEED, Hash), argumentArray1)
                End Function)
                ScriptExtended.TerminateScriptByName("re_prison")
                ScriptExtended.TerminateScriptByName("am_prison")
                ScriptExtended.TerminateScriptByName("gb_biker_free_prisoner")
                ScriptExtended.TerminateScriptByName("re_prisonvanbreak")
                ScriptExtended.TerminateScriptByName("am_vehicle_spawn")
                ScriptExtended.TerminateScriptByName("am_taxi")
                ScriptExtended.TerminateScriptByName("audiotest")
                ScriptExtended.TerminateScriptByName("freemode")
                ScriptExtended.TerminateScriptByName("re_prisonerlift")
                ScriptExtended.TerminateScriptByName("am_prison")
                ScriptExtended.TerminateScriptByName("re_lossantosintl")
                ScriptExtended.TerminateScriptByName("re_armybase")
                ScriptExtended.TerminateScriptByName("restrictedareas")
                ScriptExtended.TerminateScriptByName("stripclub")
                ScriptExtended.TerminateScriptByName("re_gangfight")
                ScriptExtended.TerminateScriptByName("re_gang_intimidation")
                ScriptExtended.TerminateScriptByName("spawn_activities")
                ScriptExtended.TerminateScriptByName("am_vehiclespawn")
                ScriptExtended.TerminateScriptByName("traffick_air")
                ScriptExtended.TerminateScriptByName("traffick_ground")
                ScriptExtended.TerminateScriptByName("emergencycall")
                ScriptExtended.TerminateScriptByName("emergencycalllauncher")
                ScriptExtended.TerminateScriptByName("clothes_shop_sp")
                ScriptExtended.TerminateScriptByName("gb_rob_shop")
                ScriptExtended.TerminateScriptByName("gunclub_shop")
                ScriptExtended.TerminateScriptByName("hairdo_shop_sp")
                ScriptExtended.TerminateScriptByName("re_shoprobbery")
                ScriptExtended.TerminateScriptByName("shop_controller")
                ScriptExtended.TerminateScriptByName("re_crashrescue")
                ScriptExtended.TerminateScriptByName("re_rescuehostage")
                ScriptExtended.TerminateScriptByName("fm_mission_controller")
                ScriptExtended.TerminateScriptByName("player_scene_m_shopping")
                ScriptExtended.TerminateScriptByName("shoprobberies")
                ScriptExtended.TerminateScriptByName("re_atmrobbery")
                ScriptExtended.TerminateScriptByName("ob_vend1")
                ScriptExtended.TerminateScriptByName("ob_vend2")
                Dim argumentArray1 As InputArgument() = New InputArgument() { "PRISON_ALARMS", 0 }
                Function.Call(DirectCast(Hash.STOP_ALARM, Hash), argumentArray1)
                Dim argumentArray2 As InputArgument() = New InputArgument() { "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_GENERAL", 0, 0 }
                Function.Call(DirectCast(Hash.CLEAR_AMBIENT_ZONE_STATE, Hash), argumentArray2)
                Dim argumentArray3 As InputArgument() = New InputArgument() { "AZ_COUNTRYSIDE_PRISON_01_ANNOUNCER_WARNING", 0, 0 }
                Function.Call(DirectCast(Hash.CLEAR_AMBIENT_ZONE_STATE, Hash), argumentArray3)
                Dim argumentArray4 As InputArgument() = New InputArgument() { "prop_gate_prison_01" }
                Dim num As Integer = Function.Call(Of Integer)(DirectCast(Hash.GET_HASH_KEY, Hash), argumentArray4)
                Dim argumentArray5 As InputArgument() = New InputArgument() { DirectCast(num, InputArgument), 1845!, 2605!, 45!, False, 0, 0 }
                Function.Call(DirectCast(Hash.SET_STATE_OF_CLOSEST_DOOR_OF_TYPE, Hash), argumentArray5)
                Dim argumentArray6 As InputArgument() = New InputArgument() { "prop_gate_prison_01" }
                Dim num2 As Integer = Function.Call(Of Integer)(DirectCast(Hash.GET_HASH_KEY, Hash), argumentArray6)
                Dim argumentArray7 As InputArgument() = New InputArgument() { DirectCast(num2, InputArgument), 1819.27!, 2608.53!, 44.61!, False, 0, 0 }
                Function.Call(DirectCast(Hash._DOOR_CONTROL, Hash), argumentArray7)
                If Me._reset Then
                    Dim argumentArray8 As InputArgument() = New InputArgument() { False }
                    Function.Call(DirectCast(Hash.CAN_CREATE_RANDOM_COPS, Hash), argumentArray8)
                    Dim argumentArray9 As InputArgument() = New InputArgument() { False }
                    Function.Call(DirectCast(Hash.SET_RANDOM_BOATS, Hash), argumentArray9)
                    Dim argumentArray10 As InputArgument() = New InputArgument() { False }
                    Function.Call(DirectCast(Hash.SET_RANDOM_TRAINS, Hash), argumentArray10)
                    Dim argumentArray11 As InputArgument() = New InputArgument() { False }
                    Function.Call(DirectCast(Hash.SET_GARBAGE_TRUCKS, Hash), argumentArray11)
                    Dim argumentArray12 As InputArgument() = New InputArgument() { False }
                    Function.Call(DirectCast(Hash.SET_DISTANT_CARS_ENABLED, Hash), argumentArray12)
                    Me._reset = False
                End If
            End If
        End Sub

        Private Shared Sub Reset()
            Dim argumentArray1 As InputArgument() = New InputArgument() { True }
            Function.Call(DirectCast(Hash.CAN_CREATE_RANDOM_COPS, Hash), argumentArray1)
            Dim argumentArray2 As InputArgument() = New InputArgument() { True }
            Function.Call(DirectCast(Hash.SET_RANDOM_BOATS, Hash), argumentArray2)
            Dim argumentArray3 As InputArgument() = New InputArgument() { True }
            Function.Call(DirectCast(Hash.SET_RANDOM_TRAINS, Hash), argumentArray3)
            Dim argumentArray4 As InputArgument() = New InputArgument() { True }
            Function.Call(DirectCast(Hash.SET_GARBAGE_TRUCKS, Hash), argumentArray4)
        End Sub


        ' Properties
        Public Shared Property Configure As Boolean
            <CompilerGenerated> _
            Get
                Return WorldController.<Configure>k__BackingField
            End Get
            <CompilerGenerated> _
            Set(ByVal value As Boolean)
                WorldController.<Configure>k__BackingField = value
            End Set
        End Property

        Public Shared Property StopPedsFromSpawning As Boolean
            <CompilerGenerated> _
            Get
                Return WorldController.<StopPedsFromSpawning>k__BackingField
            End Get
            <CompilerGenerated> _
            Set(ByVal value As Boolean)
                WorldController.<StopPedsFromSpawning>k__BackingField = value
            End Set
        End Property

        Public ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property


        ' Fields
        Private _reset As Boolean

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Function <OnTick>b__14_0(ByVal v As Vehicle) As Boolean
                Return Not v.IsPersistent
            End Function

            Friend Function <OnTick>b__14_1(ByVal v As Vehicle) As Boolean
                Return (v.get_ClassType = DirectCast(CInt(VehicleClass.Planes), VehicleClass))
            End Function

            Friend Function <OnTick>b__14_2(ByVal v As Vehicle) As Boolean
                Return (v.get_ClassType = DirectCast(CInt(VehicleClass.Trains), VehicleClass))
            End Function

            Friend Function <OnTick>b__14_3(ByVal v As Vehicle) As Boolean
                Return (v.get_Driver.Exists AndAlso Not v.get_Driver.IsPlayer)
            End Function

            Friend Sub <OnTick>b__14_4(ByVal vehicle As Vehicle)
                vehicle.Delete
            End Sub

            Friend Sub <OnTick>b__14_5(ByVal plane As Vehicle)
                If (plane.get_Driver.Exists AndAlso (Not plane.get_Driver.IsPlayer AndAlso Not plane.get_Driver.IsDead)) Then
                    plane.get_Driver.Kill
                End If
            End Sub

            Friend Sub <OnTick>b__14_6(ByVal t As Vehicle)
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(t.Handle, InputArgument), 0! }
                Function.Call(DirectCast(Hash.SET_TRAIN_SPEED, Hash), argumentArray1)
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__14_0 As Func(Of Vehicle, Boolean)
            Public Shared <>9__14_1 As Predicate(Of Vehicle)
            Public Shared <>9__14_2 As Predicate(Of Vehicle)
            Public Shared <>9__14_3 As Predicate(Of Vehicle)
            Public Shared <>9__14_4 As Action(Of Vehicle)
            Public Shared <>9__14_5 As Action(Of Vehicle)
            Public Shared <>9__14_6 As Action(Of Vehicle)
        End Class
    End Class
End Namespace


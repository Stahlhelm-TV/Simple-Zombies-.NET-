Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Runtime.InteropServices
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Static

Namespace ZombiesMod.Extensions
    Public Class WorldExtended
        ' Methods
        Public Shared Sub ClearAreaOfEverything(ByVal position As Vector3, ByVal radius As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(position.X, InputArgument), DirectCast(position.Y, InputArgument), DirectCast(position.Z, InputArgument), DirectCast(radius, InputArgument), False, False, False, False }
            Function.Call(DirectCast(Hash._CLEAR_AREA_OF_EVERYTHING, Hash), argumentArray1)
        End Sub

        Public Shared Sub ClearCops(ByVal Optional radius As Single = 9000!)
            Dim playerPosition As Vector3 = Database.PlayerPosition
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(playerPosition.X, InputArgument), DirectCast(playerPosition.Y, InputArgument), DirectCast(playerPosition.Z, InputArgument), DirectCast(radius, InputArgument), 0 }
            Function.Call(DirectCast(Hash.CLEAR_AREA_OF_COPS, Hash), argumentArray1)
        End Sub

        Public Shared Function CreateParticleEffectAtCoord(ByVal coord As Vector3, ByVal name As String) As ParticleEffect
            Dim argumentArray1 As InputArgument() = New InputArgument() { "core" }
            Function.Call(DirectCast(Hash.USE_PARTICLE_FX_ASSET, Hash), argumentArray1)
            Dim argumentArray2 As InputArgument() = New InputArgument(12  - 1) {}
            argumentArray2(0) = DirectCast(name, InputArgument)
            argumentArray2(1) = DirectCast(coord.X, InputArgument)
            argumentArray2(2) = DirectCast(coord.Y, InputArgument)
            argumentArray2(3) = DirectCast(coord.Z, InputArgument)
            argumentArray2(4) = 0
            argumentArray2(5) = 0
            argumentArray2(6) = 0
            argumentArray2(7) = 1!
            argumentArray2(8) = 0
            argumentArray2(9) = 0
            argumentArray2(10) = 0
            argumentArray2(11) = 0
            Return New ParticleEffect(Function.Call(Of Integer)(DirectCast(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, Hash), argumentArray2))
        End Function

        Public Shared Sub RemoveAllShockingEvents(ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Dim argumentArray2 As InputArgument() = New InputArgument() { If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.REMOVE_ALL_SHOCKING_EVENTS, Hash), argumentArray2)
        End Sub

        Public Shared Sub SetFrontendRadioActive(ByVal active As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { If(active, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Dim argumentArray2 As InputArgument() = New InputArgument() { If(active, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_FRONTEND_RADIO_ACTIVE, Hash), argumentArray2)
        End Sub

        Public Shared Sub SetParkedVehicleDensityMultiplierThisFrame(ByVal multiplier As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PARKED_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetPedDensityThisMultiplierFrame(ByVal multiplier As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetRandomVehicleDensityMultiplierThisFrame(ByVal multiplier As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.SET_RANDOM_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetScenarioPedDensityThisMultiplierFrame(ByVal multiplier As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.SET_SCENARIO_PED_DENSITY_MULTIPLIER_THIS_FRAME, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetVehicleDensityMultiplierThisFrame(ByVal multiplier As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, Hash), argumentArray1)
        End Sub

    End Class
End Namespace


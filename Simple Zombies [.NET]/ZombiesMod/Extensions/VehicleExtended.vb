Imports GTA
Imports GTA.Native
Imports System

Namespace ZombiesMod.Extensions
    Public Class VehicleExtended
        ' Methods
        Public Shared Function GetModelClass(ByVal vehicleModel As Model) As VehicleClass
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(vehicleModel.Hash, InputArgument) }
            Return Function.Call(Of Integer)(DirectCast(Hash.GET_VEHICLE_CLASS_FROM_NAME, Hash), argumentArray1)
        End Function

    End Class
End Namespace


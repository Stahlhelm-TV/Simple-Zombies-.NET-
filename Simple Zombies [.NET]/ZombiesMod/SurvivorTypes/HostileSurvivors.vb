Imports GTA
Imports GTA.Math
Imports System
Imports System.Collections.Generic
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.SurvivorTypes
    Public Class HostileSurvivors
        Inherits Survivors
        ' Methods
        Public Overrides Sub Abort()
            If (Me._vehicle Is Nothing) Then
                Dim local1 As Vehicle = Me._vehicle
            Else
                Me._vehicle.Delete
            End If
            Do While (Me._peds.Count > 0)
                Dim ped As Ped = Me._peds(0)
                If (Not ped Is Nothing) Then
                    ped.Delete
                End If
                Me._peds.RemoveAt(0)
            Loop
        End Sub

        Public Overrides Sub CleanUp()
            If (Me._vehicle Is Nothing) Then
                Dim local1 As Vehicle = Me._vehicle
            Else
                Dim blip1 As Blip = Me._vehicle.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local2 As Blip = blip1
                Else
                    blip1.Remove
                End If
            End If
            EntityEventWrapper.Dispose(DirectCast(Me._vehicle, Entity))
        End Sub

        Private Sub PedWrapperOnDied(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            Dim blip1 As Blip = entity.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            Me._peds.Remove(TryCast(entity,Ped))
        End Sub

        Private Sub PedWrapperOnDisposed(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            If Me._peds.Contains(TryCast(entity,Ped)) Then
                Me._peds.Remove(TryCast(entity,Ped))
            End If
        End Sub

        Private Sub PedWrapperOnUpdated(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            Dim ped As Ped = TryCast(entity,Ped)
            If (Not ped Is Nothing) Then
                Dim ped1 As Ped
                Dim vehicle1 As Vehicle = ped.get_CurrentVehicle
                If (Not vehicle1 Is Nothing) Then
                    ped1 = vehicle1.get_Driver
                Else
                    Dim local1 As Vehicle = vehicle1
                    ped1 = Nothing
                End If
                If ((ped1 Is ped) AndAlso Not ped.IsInCombat) Then
                    ped.get_Task.DriveTo(ped.get_CurrentVehicle, MyBase.PlayerPosition, 25!, 75!)
                End If
                If (SystemExtended.VDist(ped.get_Position, MyBase.PlayerPosition) > Survivors.DeleteRange) Then
                    ped.Delete
                End If
                If ped.get_CurrentBlip.Exists Then
                    ped.get_CurrentBlip.set_Alpha(If(ped.IsInVehicle, 0, &HFF))
                End If
            End If
        End Sub

        Public Overrides Sub SpawnEntities()
            Dim spawnPoint As Vector3 = MyBase.GetSpawnPoint
            If MyBase.IsValidSpawn(spawnPoint) Then
                Dim vehicle As Vehicle = World.CreateVehicle(Database.GetRandomVehicleModel, spawnPoint, CSng(Database.Random.Next(1, 360)))
                If (vehicle Is Nothing) Then
                    MyBase.Complete
                Else
                    Me._vehicle = vehicle
                    Dim blip As Blip = Me._vehicle.AddBlip
                    blip.set_Name("Enemy Vehicle")
                    blip.set_Sprite(DirectCast(BlipSprite.PersonalVehicleCar, BlipSprite))
                    blip.set_Color(DirectCast(BlipColor.Red, BlipColor))
                    Dim wrapper As New EntityEventWrapper(DirectCast(Me._vehicle, Entity))
                    AddHandler wrapper.Died, New OnDeathEvent(AddressOf Me.VehicleWrapperOnDied)
                    AddHandler wrapper.Updated, New OnWrapperUpdateEvent(AddressOf Me.VehicleWrapperOnUpdated)
                    Dim num As Integer = 0
                    Do While True
                        If (num >= (vehicle.get_PassengerSeats + 1)) Then
                            UI.Notify("~r~Hostiles~s~ nearby!")
                            Exit Do
                        End If
                        Dim flag3 As Boolean = (Me._group.MemberCount >= 6)
                        If (Not flag3 AndAlso vehicle.IsSeatFree(DirectCast(VehicleSeat.Any, VehicleSeat))) Then
                            Dim ped As Ped = vehicle.CreateRandomPedOnSeat(If((num = 0), DirectCast(VehicleSeat.Driver, VehicleSeat), DirectCast(VehicleSeat.Any, VehicleSeat)))
                            If (Not ped Is Nothing) Then
                                ped.get_Weapons.Give(Database.WeaponHashes(Database.Random.Next(Database.WeaponHashes.Length)), &H19, True, True)
                                PedExtended.SetCombatAttributes(ped, CombatAttributes.AlwaysFight, True)
                                PedExtended.SetAlertness(ped, Alertness.FullyAlert)
                                ped.set_RelationshipGroup(Relationships.HostileRelationship)
                                Me._group.Add(ped, (num = 0))
                                ped.AddBlip.set_Name("Enemy")
                                Me._peds.Add(ped)
                                Dim wrapper2 As New EntityEventWrapper(DirectCast(ped, Entity))
                                AddHandler wrapper2.Died, New OnDeathEvent(AddressOf Me.PedWrapperOnDied)
                                AddHandler wrapper2.Updated, New OnWrapperUpdateEvent(AddressOf Me.PedWrapperOnUpdated)
                                AddHandler wrapper2.Disposed, New OnWrapperDisposedEvent(AddressOf Me.PedWrapperOnDisposed)
                            End If
                        End If
                        num += 1
                    Loop
                End If
            End If
        End Sub

        Public Overrides Sub Update()
            If (Me._peds.Count <= 0) Then
                MyBase.Complete
            End If
        End Sub

        Private Sub VehicleWrapperOnDied(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            Dim blip1 As Blip = entity.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            sender.Dispose
            Me._vehicle.MarkAsNoLongerNeeded
            Me._vehicle = Nothing
        End Sub

        Private Sub VehicleWrapperOnUpdated(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            If (Not entity Is Nothing) Then
                entity.get_CurrentBlip.set_Alpha(If(Me._vehicle.get_Driver.Exists, &HFF, 0))
            End If
        End Sub


        ' Fields
        Private ReadOnly _group As PedGroup = New PedGroup
        Private ReadOnly _peds As List(Of Ped) = New List(Of Ped)
        Private _vehicle As Vehicle
    End Class
End Namespace


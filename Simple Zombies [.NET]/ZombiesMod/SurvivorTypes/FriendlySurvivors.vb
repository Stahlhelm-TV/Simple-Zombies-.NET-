Imports GTA
Imports GTA.Math
Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.SurvivorTypes
    Public Class FriendlySurvivors
        Inherits Survivors
        ' Methods
        Public Sub New()
            FriendlySurvivors.Instance = Me
        End Sub

        Public Overrides Sub Abort()
            Do While (Me._peds.Count > 0)
                Me._peds(0).Delete
                Me._peds.RemoveAt(0)
            Loop
        End Sub

        Public Overrides Sub CleanUp()
            Me._peds.ForEach(Function (ByVal ped As Ped) 
                Dim blip1 As Blip = ped.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
                ped.MarkAsNoLongerNeeded
                EntityEventWrapper.Dispose(DirectCast(ped, Entity))
            End Function)
        End Sub

        Private Sub EventWrapperOnDied(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            Me._peds.Remove(TryCast(entity,Ped))
            Dim blip1 As Blip = entity.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            entity.MarkAsNoLongerNeeded
            sender.Dispose
        End Sub

        Private Sub EventWrapperOnDisposed(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            If Me._peds.Contains(TryCast(entity,Ped)) Then
                Me._peds.Remove(TryCast(entity,Ped))
            End If
        End Sub

        Public Sub RemovePed(ByVal item As Ped)
            If Me._peds.Contains(item) Then
                Me._peds.Remove(item)
                item.LeaveGroup
                Dim blip1 As Blip = item.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
                EntityEventWrapper.Dispose(DirectCast(item, Entity))
            End If
        End Sub

        Public Overrides Sub SpawnEntities()
            Dim num As Integer = Database.Random.Next(3, 6)
            Dim spawnPoint As Vector3 = MyBase.GetSpawnPoint
            If MyBase.IsValidSpawn(spawnPoint) Then
                Dim num2 As Integer = 0
                Do While True
                    If (num2 > num) Then
                        UI.Notify("~b~Friendly~s~ survivors nearby.", True)
                        Exit Do
                    End If
                    Dim ped As Ped = World.CreateRandomPed(spawnPoint.Around(5!))
                    If (Not ped Is Nothing) Then
                        Dim blip As Blip = ped.AddBlip
                        blip.set_Color(DirectCast(BlipColor.Blue, BlipColor))
                        blip.set_Name("Friendly")
                        ped.set_RelationshipGroup(Relationships.FriendlyRelationship)
                        ped.get_Task.FightAgainstHatedTargets(9000!)
                        PedExtended.SetAlertness(ped, Alertness.FullyAlert)
                        PedExtended.SetCombatAttributes(ped, CombatAttributes.AlwaysFight, True)
                        ped.get_Weapons.Give(Database.WeaponHashes(Database.Random.Next(Database.WeaponHashes.Length)), &H19, True, True)
                        Me._pedGroup.Add(ped, (num2 = 0))
                        Me._pedGroup.set_FormationType(0)
                        Me._peds.Add(ped)
                        Dim wrapper As New EntityEventWrapper(DirectCast(ped, Entity))
                        AddHandler wrapper.Died, New OnDeathEvent(AddressOf Me.EventWrapperOnDied)
                        AddHandler wrapper.Disposed, New OnWrapperDisposedEvent(AddressOf Me.EventWrapperOnDisposed)
                    End If
                    num2 += 1
                Loop
            End If
        End Sub

        Public Overrides Sub Update()
            If (Me._peds.Count <= 0) Then
                MyBase.Complete
            End If
        End Sub


        ' Properties
        Property Instance As FriendlySurvivors
            <CompilerGenerated> _
            Public Shared Get
                Return FriendlySurvivors.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As FriendlySurvivors)
                FriendlySurvivors.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Private ReadOnly _peds As List(Of Ped) = New List(Of Ped)
        Private ReadOnly _pedGroup As PedGroup = New PedGroup

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub <CleanUp>b__12_0(ByVal ped As Ped)
                Dim blip1 As Blip = ped.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
                ped.MarkAsNoLongerNeeded
                EntityEventWrapper.Dispose(DirectCast(ped, Entity))
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__12_0 As Action(Of Ped)
        End Class
    End Class
End Namespace


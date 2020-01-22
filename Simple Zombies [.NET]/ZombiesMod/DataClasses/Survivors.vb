Imports GTA
Imports GTA.Math
Imports System
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports ZombiesMod.Extensions
Imports ZombiesMod.Scripts
Imports ZombiesMod.Static

Namespace ZombiesMod.DataClasses
    Public MustInherit Class Survivors
        ' Events
        Public Custom Event Completed As SurvivorCompletedEvent
            AddHandler(ByVal value As SurvivorCompletedEvent)
                Dim completed As SurvivorCompletedEvent = Me.Completed
                Do While True
                    Dim a As SurvivorCompletedEvent = completed
                    Dim event4 As SurvivorCompletedEvent = DirectCast(Delegate.Combine(a, value), SurvivorCompletedEvent)
                    completed = Interlocked.CompareExchange(Of SurvivorCompletedEvent)(Me.Completed, event4, a)
                    If Object.ReferenceEquals(completed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As SurvivorCompletedEvent)
                Dim completed As SurvivorCompletedEvent = Me.Completed
                Do While True
                    Dim source As SurvivorCompletedEvent = completed
                    Dim event4 As SurvivorCompletedEvent = DirectCast(Delegate.Remove(source, value), SurvivorCompletedEvent)
                    completed = Interlocked.CompareExchange(Of SurvivorCompletedEvent)(Me.Completed, event4, source)
                    If Object.ReferenceEquals(completed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Protected Sub New()
        End Sub

        Public MustOverride Sub Abort()

        Public MustOverride Sub CleanUp()

        Public Sub Complete()
            If (Me.Completed Is Nothing) Then
                Dim completed As SurvivorCompletedEvent = Me.Completed
            Else
                Me.Completed.Invoke(Me)
            End If
        End Sub

        Public Function GetSpawnPoint() As Vector3
            Return World.GetNextPositionOnStreet(Me.PlayerPosition.Around(Survivors.MaxSpawnDistance))
        End Function

        Public Function IsValidSpawn(ByVal spawn As Vector3) As Boolean
            Dim flag2 As Boolean
            If ((SystemExtended.VDist(spawn, Me.PlayerPosition) >= Survivors.MinSpawnDistance) AndAlso Not ZombieVehicleSpawner.Instance.IsInvalidZone(spawn)) Then
                flag2 = True
            Else
                Me.Complete
                flag2 = False
            End If
            Return flag2
        End Function

        Public MustOverride Sub SpawnEntities()

        Public MustOverride Sub Update()


        ' Properties
        Public ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property

        Public ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property


        ' Fields
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Completed As SurvivorCompletedEvent
        Public Shared MaxSpawnDistance As Single = 650!
        Public Shared MinSpawnDistance As Single = 400!
        Public Shared DeleteRange As Single = 1000!

        ' Nested Types
        Public Delegate Sub SurvivorCompletedEvent(ByVal survivors As Survivors)
    End Class
End Namespace


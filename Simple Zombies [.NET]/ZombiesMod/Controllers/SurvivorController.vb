Imports GTA
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports ZombiesMod
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Static
Imports ZombiesMod.SurvivorTypes

Namespace ZombiesMod.Controllers
    Public Class SurvivorController
        Inherits Script
        Implements ISpawner
        ' Events
        Public Custom Event CreatedSurvivors As OnCreatedSurvivorsEvent
            AddHandler(ByVal value As OnCreatedSurvivorsEvent)
                Dim createdSurvivors As OnCreatedSurvivorsEvent = Me.CreatedSurvivors
                Do While True
                    Dim a As OnCreatedSurvivorsEvent = createdSurvivors
                    Dim event4 As OnCreatedSurvivorsEvent = DirectCast(Delegate.Combine(a, value), OnCreatedSurvivorsEvent)
                    createdSurvivors = Interlocked.CompareExchange(Of OnCreatedSurvivorsEvent)(Me.CreatedSurvivors, event4, a)
                    If Object.ReferenceEquals(createdSurvivors, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnCreatedSurvivorsEvent)
                Dim createdSurvivors As OnCreatedSurvivorsEvent = Me.CreatedSurvivors
                Do While True
                    Dim source As OnCreatedSurvivorsEvent = createdSurvivors
                    Dim event4 As OnCreatedSurvivorsEvent = DirectCast(Delegate.Remove(source, value), OnCreatedSurvivorsEvent)
                    createdSurvivors = Interlocked.CompareExchange(Of OnCreatedSurvivorsEvent)(Me.CreatedSurvivors, event4, source)
                    If Object.ReferenceEquals(createdSurvivors, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub New()
            SurvivorController.Instance = Me
            Me._survivorInterval = MyBase.get_Settings.GetValue(Of Integer)("survivors", "survivor_interval", Me._survivorInterval)
            Me._survivorSpawnChance = MyBase.get_Settings.GetValue(Of Single)("survivors", "survivor_spawn_chance", Me._survivorSpawnChance)
            Me._merryweatherTimeout = MyBase.get_Settings.GetValue(Of Integer)("survivors", "merryweather_timeout", Me._merryweatherTimeout)
            MyBase.get_Settings.SetValue(Of Integer)("survivors", "survivor_interval", Me._survivorInterval)
            MyBase.get_Settings.SetValue(Of Single)("survivors", "survivor_spawn_chance", Me._survivorSpawnChance)
            MyBase.get_Settings.SetValue(Of Integer)("survivors", "merryweather_timeout", Me._merryweatherTimeout)
            MyBase.get_Settings.Save
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, (sender, args) => Me._survivors.ForEach(s => s.Abort)
            AddHandler CreatedSurvivors, New OnCreatedSurvivorsEvent(AddressOf Me.OnCreatedSurvivors)
        End Sub

        Private Sub Create()
            If (Me.Spawn AndAlso (DateTime.UtcNow > Me._currentDelayTime)) Then
                If (Me.CreatedSurvivors Is Nothing) Then
                    Dim createdSurvivors As OnCreatedSurvivorsEvent = Me.CreatedSurvivors
                Else
                    Me.CreatedSurvivors.Invoke
                End If
                Me.SetDelayTime
            End If
        End Sub

        Private Sub Destroy()
            If Not Me.Spawn Then
                Me._survivors.ForEach(Function (ByVal s As Survivors) 
                    Me._survivors.Remove(s)
                    s.Abort
                End Function)
                Me._currentDelayTime = DateTime.UtcNow
            End If
        End Sub

        Private Sub OnCreatedSurvivors()
            Dim flag As Boolean = (Database.Random.NextDouble <= Me._survivorSpawnChance)
            Dim values As EventTypes() = DirectCast(Enum.GetValues(GetType(EventTypes)), EventTypes())
            Dim objA As Survivors = Nothing
            Select Case values(Database.Random.Next(values.Length))
                Case EventTypes.Friendly
                    Dim survivors As New FriendlySurvivors
                    objA = Me.TryCreateEvent(survivors)
                    Exit Select
                Case EventTypes.Hostile
                    If (Database.Random.NextDouble <= 0.20000000298023224) Then
                        Dim survivors As New HostileSurvivors
                        objA = Me.TryCreateEvent(survivors)
                    End If
                    Exit Select
                Case EventTypes.Merryweather
                    Dim survivors As New MerryweatherSurvivors(Me._merryweatherTimeout)
                    objA = Me.TryCreateEvent(survivors)
                    Exit Select
                Case Else
                    Exit Select
            End Select
            If Not Object.ReferenceEquals(objA, Nothing) Then
                Me._survivors.Add(objA)
                objA.SpawnEntities
                AddHandler objA.Completed, Function (ByVal survivors As Survivors) 
                    Me.SetDelayTime
                    survivors.CleanUp
                    Me._survivors.Remove(survivors)
                End Function
            End If
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Create
            Me.Destroy
            Me._survivors.ForEach(s => s.Update)
        End Sub

        Private Sub SetDelayTime()
            Me._currentDelayTime = (DateTime.UtcNow + New TimeSpan(0, 0, Me._survivorInterval))
        End Sub

        Private Function TryCreateEvent(ByVal survivors As Survivors) As Survivors
            Dim survivors2 As Survivors = Nothing
            If (Me._survivors.FindIndex(s => (s.GetType Is survivors.GetType)) <= -1) Then
                survivors2 = survivors
            End If
            Return survivors2
        End Function


        ' Properties
        Public Property Spawn As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Property Instance As SurvivorController
            <CompilerGenerated> _
            Public Shared Get
                Return SurvivorController.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As SurvivorController)
                SurvivorController.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private CreatedSurvivors As OnCreatedSurvivorsEvent
        Private ReadOnly _survivors As List(Of Survivors) = New List(Of Survivors)
        Private ReadOnly _survivorInterval As Integer = 30
        Private ReadOnly _survivorSpawnChance As Single = 0.7!
        Private ReadOnly _merryweatherTimeout As Integer = 120
        Private _currentDelayTime As DateTime

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub ctor>b__9_1(ByVal s As Survivors)
                s.Abort
            End Sub

            Friend Sub <OnTick>b__20_0(ByVal s As Survivors)
                s.Update
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__9_1 As Action(Of Survivors)
            Public Shared <>9__20_0 As Action(Of Survivors)
        End Class

        Public Delegate Sub OnCreatedSurvivorsEvent()
    End Class
End Namespace


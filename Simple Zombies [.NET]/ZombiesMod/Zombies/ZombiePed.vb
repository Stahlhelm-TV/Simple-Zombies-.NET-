Imports GTA
Imports System
Imports System.Diagnostics
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports ZombiesMod.Extensions
Imports ZombiesMod.Scripts
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.Zombies
    Public MustInherit Class ZombiePed
        Inherits Entity
        Implements IEquatable(Of Ped)
        ' Events
        Public Custom Event AttackTarget As OnAttackingTargetEvent
            AddHandler(ByVal value As OnAttackingTargetEvent)
                Dim attackTarget As OnAttackingTargetEvent = Me.AttackTarget
                Do While True
                    Dim a As OnAttackingTargetEvent = attackTarget
                    Dim event4 As OnAttackingTargetEvent = DirectCast(Delegate.Combine(a, value), OnAttackingTargetEvent)
                    attackTarget = Interlocked.CompareExchange(Of OnAttackingTargetEvent)(Me.AttackTarget, event4, a)
                    If Object.ReferenceEquals(attackTarget, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnAttackingTargetEvent)
                Dim attackTarget As OnAttackingTargetEvent = Me.AttackTarget
                Do While True
                    Dim source As OnAttackingTargetEvent = attackTarget
                    Dim event4 As OnAttackingTargetEvent = DirectCast(Delegate.Remove(source, value), OnAttackingTargetEvent)
                    attackTarget = Interlocked.CompareExchange(Of OnAttackingTargetEvent)(Me.AttackTarget, event4, source)
                    If Object.ReferenceEquals(attackTarget, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Custom Event GoToTarget As OnGoingToTargetEvent
            AddHandler(ByVal value As OnGoingToTargetEvent)
                Dim goToTarget As OnGoingToTargetEvent = Me.GoToTarget
                Do While True
                    Dim a As OnGoingToTargetEvent = goToTarget
                    Dim event4 As OnGoingToTargetEvent = DirectCast(Delegate.Combine(a, value), OnGoingToTargetEvent)
                    goToTarget = Interlocked.CompareExchange(Of OnGoingToTargetEvent)(Me.GoToTarget, event4, a)
                    If Object.ReferenceEquals(goToTarget, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnGoingToTargetEvent)
                Dim goToTarget As OnGoingToTargetEvent = Me.GoToTarget
                Do While True
                    Dim source As OnGoingToTargetEvent = goToTarget
                    Dim event4 As OnGoingToTargetEvent = DirectCast(Delegate.Remove(source, value), OnGoingToTargetEvent)
                    goToTarget = Interlocked.CompareExchange(Of OnGoingToTargetEvent)(Me.GoToTarget, event4, source)
                    If Object.ReferenceEquals(goToTarget, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Protected Sub New(ByVal handle As Integer)
            MyBase.New(handle)
            Me._ped = New Ped(handle)
            Me._eventWrapper = New EntityEventWrapper(DirectCast(Me._ped, Entity))
            AddHandler Me._eventWrapper.Died, New OnDeathEvent(AddressOf Me.OnDied)
            AddHandler Me._eventWrapper.Updated, New OnWrapperUpdateEvent(AddressOf Me.Update)
            AddHandler Me._eventWrapper.Aborted, New OnWrapperAbortedEvent(AddressOf Me.Abort)
            Me._currentMovementUpdateTime = DateTime.UtcNow
            AddHandler GoToTarget, New OnGoingToTargetEvent(AddressOf Me.OnGoToTarget)
            AddHandler AttackTarget, New OnAttackingTargetEvent(AddressOf Me.OnAttackTarget)
        End Sub

        Public Sub Abort(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            MyBase.Delete
        End Sub

        Private Function CanHearPed(ByVal ped As Ped) As Boolean
            Dim distance As Single = SystemExtended.VDist(ped.get_Position, Me.get_Position)
            Return ((Not ZombiePed.IsWeaponWellSilenced(ped, distance) OrElse ZombiePed.IsBehindZombie(distance)) OrElse ZombiePed.IsRunningNoticed(ped, distance))
        End Function

        Public Function Equals(ByVal other As Ped) As Boolean
            Return Object.Equals(Me._ped, other)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return If((Not obj Is Nothing), If(Not Object.ReferenceEquals(Me, obj), ((obj.GetType Is MyBase.GetType) AndAlso Me.Equals(DirectCast(obj, ZombiePed))), True), False)
        End Function

        Protected Function Equals(ByVal other As ZombiePed) As Boolean
            Return (MyBase.Equals(DirectCast(other, Entity)) AndAlso Object.Equals(Me._ped, other._ped))
        End Function

        Public Sub ForgetTarget()
            Me._target = Nothing
        End Sub

        Public Overrides Function GetHashCode() As Integer
            Return ((MyBase.GetHashCode * &H18D) Xor If((Not Me._ped Is Nothing), Me._ped.GetHashCode, 0))
        End Function

        Private Sub GetTarget()
            Dim pedArray As Ped() = Enumerable.ToArray(Of Ped)(Enumerable.Where(Of Ped)(World.GetNearbyPeds(Me._ped, ZombiePed.SensingRange), New Func(Of Ped, Boolean)(AddressOf Me.IsGoodTarget)))
            Dim closest As Ped = World.GetClosest(Of Ped)(Me.get_Position, pedArray)
            If ((Not closest Is Nothing) AndAlso (EntityExtended.HasClearLineOfSight(DirectCast(Me._ped, Entity), DirectCast(closest, Entity), ZombiePed.VisionDistance) OrElse Me.CanHearPed(closest))) Then
                Me.Target = closest
            ElseIf If(((Me.Target Is Nothing) OrElse Me.IsGoodTarget(Me.Target)), (Not closest Is Me.Target), True) Then
                Me.Target = Nothing
            End If
        End Sub

        Public Sub InfectTarget(ByVal target As Ped)
            If (Not target.IsPlayer AndAlso (target.Health <= (target.MaxHealth / 4))) Then
                PedExtended.SetToRagdoll(target, &HBB8)
                ZombieCreator.InfectPed(target, Me.MaxHealth, True)
                Me.ForgetTarget
                target.LeaveGroup
                target.get_Weapons.Drop
                EntityEventWrapper.Dispose(DirectCast(target, Entity))
            End If
        End Sub

        Private Shared Function IsBehindZombie(ByVal distance As Single) As Boolean
            Return (distance < ZombiePed.BehindZombieNoticeDistance)
        End Function

        Private Function IsGoodTarget(ByVal ped As Ped) As Boolean
            Return (ped.GetRelationshipWithPed(Me._ped) = DirectCast(CInt(Relationship.Hate), Relationship))
        End Function

        Private Shared Function IsRunningNoticed(ByVal ped As Ped, ByVal distance As Single) As Boolean
            Return (ped.IsSprinting AndAlso (distance < ZombiePed.RunningNoticeDistance))
        End Function

        Private Shared Function IsWeaponWellSilenced(ByVal ped As Ped, ByVal distance As Single) As Boolean
            Return If(ped.IsShooting, (PedExtended.IsCurrentWeaponSileced(ped) AndAlso (distance > ZombiePed.SilencerEffectiveRange)), True)
        End Function

        Public MustOverride Sub OnAttackTarget(ByVal target As Ped)

        Private Sub OnDied(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
            Dim blip1 As Blip = MyBase.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            If (ZombieVehicleSpawner.Instance.IsInvalidZone(entity.get_Position) AndAlso ZombieVehicleSpawner.Instance.IsValidSpawn(entity.get_Position)) Then
                ZombieVehicleSpawner.Instance.SpawnBlocker.Add(entity.get_Position)
            End If
        End Sub

        Public MustOverride Sub OnGoToTarget(ByVal target As Ped)

        Public Shared Widening Operator CType(ByVal v As ZombiePed) As Ped
            Return v._ped
        End Operator

        Private Sub SetWalkStyle()
            If (DateTime.UtcNow > Me._currentMovementUpdateTime) Then
                PedExtended.SetMovementAnimSet(Me._ped, Me.MovementStyle)
                Me.UpdateTime
            End If
        End Sub

        Public Sub Update(ByVal entityEventWrapper As EntityEventWrapper, ByVal entity As Entity)
            If ((SystemExtended.VDist(Me.get_Position, Database.PlayerPosition) > 120!) AndAlso (Not MyBase.IsOnScreen OrElse MyBase.IsDead)) Then
                MyBase.Delete
            End If
            If (Me.PlayAudio AndAlso Me._ped.IsRunning) Then
                PedExtended.DisablePainAudio(Me._ped, False)
                PedExtended.PlayPain(Me._ped, 8)
                PedExtended.PlayFacialAnim(Me._ped, "facials@gen_male@base", "burning_1")
            End If
            Me.GetTarget
            Me.SetWalkStyle
            If (Me._ped.IsOnFire AndAlso Not Me._ped.IsDead) Then
                Me._ped.Kill
            End If
            PedExtended.StopAmbientSpeechThisFrame(Me._ped)
            If Not Me.PlayAudio Then
                PedExtended.StopSpeaking(Me._ped, True)
            End If
            If (Not Me.Target Is Nothing) Then
                If (SystemExtended.VDist(Me.get_Position, Me.Target.get_Position) > ZombiePed.AttackRange) Then
                    Me.AttackingTarget = False
                    Me.GoingToTarget = True
                Else
                    Me.AttackingTarget = True
                    Me.GoingToTarget = False
                End If
            End If
        End Sub

        Private Sub UpdateTime()
            Me._currentMovementUpdateTime = (DateTime.UtcNow + New TimeSpan(0, 0, 0, 5))
        End Sub


        ' Properties
        Public Overridable Property PlayAudio As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Property Target As Ped
            Public Get
                Return Me._target
            End Get
            Private Set(ByVal value As Ped)
                If ((value Is Nothing) AndAlso (Not Me._target Is Nothing)) Then
                    Me._ped.get_Task.WanderAround(Me.get_Position, ZombiePed.WanderRadius)
                    Me.GoingToTarget = Me.AttackingTarget = False
                End If
                Me._target = value
            End Set
        End Property

        Public Property GoingToTarget As Boolean
            Get
                Return Me._goingToTarget
            End Get
            Set(ByVal value As Boolean)
                If (value AndAlso Not Me._goingToTarget) Then
                    If (Me.GoToTarget Is Nothing) Then
                        Dim goToTarget As OnGoingToTargetEvent = Me.GoToTarget
                    Else
                        Me.GoToTarget.Invoke(Me.Target)
                    End If
                End If
                Me._goingToTarget = value
            End Set
        End Property

        Public Property AttackingTarget As Boolean
            Get
                Return Me._attackingTarget
            End Get
            Set(ByVal value As Boolean)
                If ((value AndAlso (Not Me._ped.IsRagdoll AndAlso (Not MyBase.IsDead AndAlso (Not Me._ped.IsClimbing AndAlso (Not Me._ped.IsFalling AndAlso Not Me._ped.IsBeingStunned))))) AndAlso Not Me._ped.IsGettingUp) Then
                    If (Me.AttackTarget Is Nothing) Then
                        Dim attackTarget As OnAttackingTargetEvent = Me.AttackTarget
                    Else
                        Me.AttackTarget.Invoke(Me.Target)
                    End If
                End If
                Me._attackingTarget = value
            End Set
        End Property

        Public Overridable Property MovementStyle As String
            Get
            Set(ByVal value As String)
        End Property

        ' Fields
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private GoToTarget As OnGoingToTargetEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private AttackTarget As OnAttackingTargetEvent
        Public Const MovementUpdateInterval As Integer = 5
        Public Shared ZombieDamage As Integer = 15
        Public Shared SensingRange As Single = 120!
        Public Shared SilencerEffectiveRange As Single = 15!
        Public Shared BehindZombieNoticeDistance As Single = 5!
        Public Shared RunningNoticeDistance As Single = 25!
        Public Shared AttackRange As Single = 1.2!
        Public Shared VisionDistance As Single = 35!
        Public Shared WanderRadius As Single = 100!
        Private _target As Ped
        Private ReadOnly _ped As Ped
        Private _eventWrapper As EntityEventWrapper
        Private _goingToTarget As Boolean
        Private _attackingTarget As Boolean
        Private _currentMovementUpdateTime As DateTime

        ' Nested Types
        Public Delegate Sub OnAttackingTargetEvent(ByVal target As Ped)

        Public Delegate Sub OnGoingToTargetEvent(ByVal target As Ped)
    End Class
End Namespace


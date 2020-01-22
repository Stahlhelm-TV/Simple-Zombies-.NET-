Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static
Imports ZombiesMod.Zombies

Namespace ZombiesMod.Zombies.ZombieTypes
    Public Class Runner
        Inherits ZombiePed
        ' Methods
        Public Sub New(ByVal handle As Integer)
            MyBase.New(handle)
            Me.<PlayAudio>k__BackingField = True
            Me.<MovementStyle>k__BackingField = "move_m@injured"
            Me._ped = DirectCast(Me, Ped)
        End Sub

        Public Overrides Sub OnAttackTarget(ByVal target As Ped)
            If target.IsDead Then
                If Not EntityExtended.IsPlayingAnim(DirectCast(Me._ped, Entity), "amb@world_human_bum_wash@male@high@idle_a", "idle_b") Then
                    Me._ped.get_Task.PlayAnimation("amb@world_human_bum_wash@male@high@idle_a", "idle_b", 8!, -1, DirectCast(AnimationFlags.Loop, AnimationFlags))
                End If
            ElseIf ((((Database.Random.NextDouble < 0.30000001192092896) AndAlso Not Me._jumpAttack) AndAlso (Not target.IsPerformingStealthKill AndAlso Not target.IsGettingUp)) AndAlso Not target.IsRagdoll) Then
                PedExtended.Jump(Me._ped)
                Me._ped.set_Heading((target.get_Position - Me.get_Position).ToHeading)
                Me._jumpAttack = True
                PedExtended.SetToRagdoll(target, &H7D0)
            ElseIf Not EntityExtended.IsPlayingAnim(DirectCast(Me._ped, Entity), "rcmbarry", "bar_1_teleport_aln") Then
                Me._ped.get_Task.PlayAnimation("rcmbarry", "bar_1_teleport_aln", 8!, &H3E8, DirectCast(AnimationFlags.UpperBodyOnly, AnimationFlags))
                If Not target.IsInvincible Then
                    target.ApplyDamage(ZombiePed.ZombieDamage)
                End If
                MyBase.InfectTarget(target)
            End If
        End Sub

        Public Overrides Sub OnGoToTarget(ByVal target As Ped)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(Me._ped.Handle, InputArgument), DirectCast(target.Handle, InputArgument), -1, 0!, 5!, &H40000000, 0 }
            Function.Call(DirectCast(Hash.TASK_GO_TO_ENTITY, Hash), argumentArray1)
        End Sub


        ' Properties
        Public Overrides Property PlayAudio As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public Overrides Property MovementStyle As String
            Get
            Set(ByVal value As String)
        End Property

        ' Fields
        Private ReadOnly _ped As Ped
        Private _jumpAttack As Boolean
    End Class
End Namespace


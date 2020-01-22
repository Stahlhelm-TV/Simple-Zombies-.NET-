Imports GTA
Imports System
Imports System.Runtime.CompilerServices
Imports ZombiesMod.Extensions
Imports ZombiesMod.Zombies

Namespace ZombiesMod.Zombies.ZombieTypes
    Public Class Walker
        Inherits ZombiePed
        ' Methods
        Public Sub New(ByVal handle As Integer)
            MyBase.New(handle)
            Me.<MovementStyle>k__BackingField = "move_m@drunk@verydrunk"
            Me._ped = DirectCast(Me, Ped)
        End Sub

        Public Overrides Sub OnAttackTarget(ByVal target As Ped)
            If target.IsDead Then
                If Not EntityExtended.IsPlayingAnim(DirectCast(Me._ped, Entity), "amb@world_human_bum_wash@male@high@idle_a", "idle_b") Then
                    Me._ped.get_Task.PlayAnimation("amb@world_human_bum_wash@male@high@idle_a", "idle_b", 8!, -1, DirectCast(AnimationFlags.Loop, AnimationFlags))
                End If
            ElseIf Not EntityExtended.IsPlayingAnim(DirectCast(Me._ped, Entity), "rcmbarry", "bar_1_teleport_aln") Then
                Me._ped.get_Task.PlayAnimation("rcmbarry", "bar_1_teleport_aln", 8!, &H3E8, DirectCast(AnimationFlags.UpperBodyOnly, AnimationFlags))
                If Not target.IsInvincible Then
                    target.ApplyDamage(ZombiePed.ZombieDamage)
                End If
                MyBase.InfectTarget(target)
            End If
        End Sub

        Public Overrides Sub OnGoToTarget(ByVal target As Ped)
            Me._ped.get_Task.GoTo(DirectCast(target, Entity))
        End Sub


        ' Properties
        Public Overrides Property MovementStyle As String
            Get
            Set(ByVal value As String)
        End Property

        ' Fields
        Private ReadOnly _ped As Ped
    End Class
End Namespace


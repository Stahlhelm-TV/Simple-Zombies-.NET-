Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static
Imports ZombiesMod.Zombies
Imports ZombiesMod.Zombies.ZombieTypes

Namespace ZombiesMod.Scripts
    Public Class ZombieCreator
        ' Methods
        Public Shared Function InfectPed(ByVal ped As Ped, ByVal health As Integer, ByVal Optional overrideAsFastZombie As Boolean = False) As ZombiePed
            Dim ped2 As ZombiePed
            ped.set_CanPlayGestures(False)
            PedExtended.SetCanPlayAmbientAnims(ped, False)
            PedExtended.SetCanEvasiveDive(ped, False)
            PedExtended.SetPathCanUseLadders(ped, False)
            PedExtended.SetPathCanClimb(ped, False)
            PedExtended.DisablePainAudio(ped, True)
            PedExtended.ApplyDamagePack(ped, 0!, 1!, DamagePack.BigHitByVehicle)
            PedExtended.ApplyDamagePack(ped, 0!, 1!, DamagePack.ExplosionMed)
            ped.set_AlwaysDiesOnLowHealth(False)
            PedExtended.SetAlertness(ped, Alertness.Nuetral)
            PedExtended.SetCombatAttributes(ped, CombatAttributes.AlwaysFight, True)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), 0, 0 }
            Function.Call(DirectCast(Hash.SET_PED_FLEE_ATTRIBUTES, Hash), argumentArray1)
            ped.SetConfigFlag(&H119, True)
            ped.get_Task.WanderAround(ped.get_Position, ZombiePed.WanderRadius)
            ped.set_AlwaysKeepTask(True)
            ped.set_BlockPermanentEvents(True)
            ped.set_IsPersistent(False)
            Dim blip1 As Blip = ped.get_CurrentBlip
            If (blip1 Is Nothing) Then
                Dim local1 As Blip = blip1
            Else
                blip1.Remove
            End If
            ped.set_IsPersistent(True)
            ped.set_RelationshipGroup(Relationships.InfectedRelationship)
            Dim num As Single = 0.055!
            If ZombieCreator.IsNightFall Then
                num = 0.5!
            End If
            Dim span As TimeSpan = World.get_CurrentDayTime
            If ((span.Hours >= 20) OrElse (span.Hours <= 3)) Then
                num = 0.4!
            End If
            If (((Database.Random.NextDouble < num) Or overrideAsFastZombie) AndAlso ZombieCreator.Runners) Then
                ped2 = New Runner(ped.Handle)
            Else
                Dim num2 As Integer
                ped.set_MaxHealth(num2 = health)
                ped.set_Health(num2)
                ped2 = New Walker(ped.Handle)
            End If
            Return ped2
        End Function

        Public Shared Function IsNightFall() As Boolean
            Dim flag2 As Boolean
            If Not ZombieCreator.Runners Then
                flag2 = False
            Else
                Dim span As TimeSpan = World.get_CurrentDayTime
                flag2 = ((span.Hours >= 20) OrElse (span.Hours <= 3))
            End If
            Return flag2
        End Function


        ' Properties
        Public Shared Property Runners As Boolean
            <CompilerGenerated> _
            Get
                Return ZombieCreator.<Runners>k__BackingField
            End Get
            <CompilerGenerated> _
            Set(ByVal value As Boolean)
                ZombieCreator.<Runners>k__BackingField = value
            End Set
        End Property

    End Class
End Namespace


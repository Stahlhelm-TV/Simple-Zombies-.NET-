Imports GTA
Imports GTA.Math
Imports System
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static
Imports ZombiesMod.SurvivorTypes

Namespace ZombiesMod.Scripts
    Public Class RecruitPeds
        Inherits Script
        ' Methods
        Public Sub New()
            AddHandler MyBase.Tick, New EventHandler(AddressOf RecruitPeds.OnTick)
        End Sub

        Private Shared Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If (Not MenuConrtoller.MenuPool.IsAnyMenuOpen AndAlso (RecruitPeds.PlayerPed.get_CurrentPedGroup.MemberCount < 6)) Then
                Dim nearbyPeds As Ped() = World.GetNearbyPeds(RecruitPeds.PlayerPed, 1.5!)
                Dim closest As Ped = World.GetClosest(Of Ped)(RecruitPeds.PlayerPosition, nearbyPeds)
                If ((Not closest Is Nothing) AndAlso (Not closest.IsDead AndAlso (Not closest.IsInCombatAgainst(RecruitPeds.PlayerPed) AndAlso ((closest.GetRelationshipWithPed(RecruitPeds.PlayerPed) <> DirectCast(CInt(Relationship.Hate), Relationship)) AndAlso ((closest.get_RelationshipGroup = Relationships.FriendlyRelationship) AndAlso (Not closest.get_CurrentPedGroup Is RecruitPeds.PlayerPed.get_CurrentPedGroup)))))) Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Enter, Control))
                    UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_ENTER~ to recruit this ped.", False)
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Enter, Control)) Then
                        If (Not FriendlySurvivors.Instance Is Nothing) Then
                            FriendlySurvivors.Instance.RemovePed(closest)
                        End If
                        PedExtended.Recruit(closest, RecruitPeds.PlayerPed)
                        If (RecruitPeds.PlayerPed.get_CurrentPedGroup.MemberCount >= 6) Then
                            UI.Notify("You've reached the max amount of ~b~guards~s~.")
                        End If
                    End If
                End If
            End If
        End Sub


        ' Properties
        Private Shared ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property

        Private Shared ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property


        ' Fields
        Public Const InteractDistance As Single = 1.5!
    End Class
End Namespace


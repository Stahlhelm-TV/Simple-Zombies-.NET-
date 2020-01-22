Imports GTA
Imports GTA.Math
Imports System
Imports ZombiesMod
Imports ZombiesMod.Extensions
Imports ZombiesMod.PlayerManagement
Imports ZombiesMod.Static

Namespace ZombiesMod.Scripts
    Public Class VehicleRepair
        Inherits Script
        ' Methods
        Public Sub New()
            Me._repairTimeMs = MyBase.get_Settings.GetValue(Of Integer)("interaction", "vehicle_repair_time_ms", Me._repairTimeMs)
            MyBase.get_Settings.SetValue(Of Integer)("interaction", "vehicle_repair_time_ms", Me._repairTimeMs)
            MyBase.get_Settings.Save
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, New EventHandler(AddressOf VehicleRepair.OnAborted)
        End Sub

        Private Shared Sub OnAborted(ByVal sender As Object, ByVal e As EventArgs)
            VehicleRepair.PlayerPed.get_Task.ClearAll
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal e As EventArgs)
            If Not Database.PlayerInVehicle Then
                Dim closestVehicle As Vehicle = World.GetClosestVehicle(Database.PlayerPosition, 20!)
                If Object.ReferenceEquals(Me._item, Nothing) Then
                    Me._item = PlayerInventory.Instance.ItemFromName("Vehicle Repair Kit")
                End If
                If (Not Me._selectedVehicle Is Nothing) Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Attack, Control))
                    UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to cancel.", False)
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Attack, Control)) Then
                        VehicleRepair.PlayerPed.get_Task.ClearAllImmediately
                        Me._selectedVehicle.CloseDoor(4, False)
                        Me._selectedVehicle = Nothing
                    ElseIf (VehicleRepair.PlayerPed.TaskSequenceProgress = -1) Then
                        Me._selectedVehicle.set_EngineHealth(1000!)
                        Me._selectedVehicle.CloseDoor(4, False)
                        Me._selectedVehicle = Nothing
                        PlayerInventory.Instance.AddItem(Me._item, -1, ItemType.Item)
                        UI.Notify("Items: -~r~1")
                    End If
                ElseIf ((Not closestVehicle Is Nothing) AndAlso (Not ((Not closestVehicle.get_Model.IsCar OrElse (closestVehicle.EngineHealth >= 1000!)) OrElse MenuConrtoller.MenuPool.IsAnyMenuOpen) AndAlso (Not closestVehicle.IsUpsideDown AndAlso closestVehicle.HasBone("engine")))) Then
                    Dim boneCoord As Vector3 = closestVehicle.GetBoneCoord(closestVehicle.GetBoneIndex("engine"))
                    If ((boneCoord <> Vector3.get_Zero) AndAlso VehicleRepair.PlayerPed.IsInRangeOf(boneCoord, 1.5!)) Then
                        If Not PlayerInventory.Instance.HasItem(Me._item, ItemType.Item) Then
                            UiExtended.DisplayHelpTextThisFrame("You need a vehicle repair kit to fix this engine.", False)
                        Else
                            Game.DisableControlThisFrame(2, DirectCast(Control.Context, Control))
                            UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to repair engine.", False)
                            If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Context, Control)) Then
                                closestVehicle.OpenDoor(4, False, False)
                                VehicleRepair.PlayerPed.get_Weapons.Select(-1569615261, True)
                                Dim vector2 As Vector3 = DirectCast((boneCoord + closestVehicle.get_ForwardVector), Vector3)
                                Dim num As Single = (closestVehicle.get_Position - Database.PlayerPosition).ToHeading
                                Dim sequence As New TaskSequence
                                sequence.get_AddTask.ClearAllImmediately
                                sequence.get_AddTask.GoTo(vector2, False, &H5DC)
                                sequence.get_AddTask.AchieveHeading(num, &H7D0)
                                sequence.get_AddTask.PlayAnimation("mp_intro_seq@", "mp_mech_fix", 8!, -8!, Me._repairTimeMs, DirectCast(AnimationFlags.Loop, AnimationFlags), 1!)
                                sequence.get_AddTask.ClearAll
                                sequence.Close
                                VehicleRepair.PlayerPed.get_Task.PerformSequence(sequence)
                                sequence.Dispose
                                Me._selectedVehicle = closestVehicle
                            End If
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


        ' Fields
        Private _selectedVehicle As Vehicle
        Private _item As InventoryItemBase
        Private ReadOnly _repairTimeMs As Integer = &H1D4C
    End Class
End Namespace


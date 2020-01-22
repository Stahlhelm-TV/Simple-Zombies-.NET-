Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports NativeUI
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports ZombiesMod
Imports ZombiesMod.Extensions
Imports ZombiesMod.PlayerManagement
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.Scripts
    Public Class MapInteraction
        Inherits Script
        ' Methods
        Public Sub New()
            AddHandler PlayerMap.Interacted, New InteractedEvent(AddressOf Me.MapOnInteracted)
            MenuConrtoller.MenuPool.Add(Me._weaponsMenu)
            MenuConrtoller.MenuPool.Add(Me._craftWeaponsMenu)
            Me._storageMenu = MenuConrtoller.MenuPool.AddSubMenu(Me._weaponsMenu, "Storage")
            Me._myWeaponsMenu = MenuConrtoller.MenuPool.AddSubMenu(Me._weaponsMenu, "Give")
            Me._enemyRangeForSleeping = MyBase.get_Settings.GetValue(Of Single)("map_interaction", "enemy_range_for_sleeping", Me._enemyRangeForSleeping)
            Me._sleepHours = MyBase.get_Settings.GetValue(Of Integer)("map_interaction", "sleep_hours", Me._sleepHours)
            MyBase.get_Settings.SetValue(Of Single)("map_interaction", "enemy_range_for_sleeping", Me._enemyRangeForSleeping)
            MyBase.get_Settings.SetValue(Of Integer)("map_interaction", "sleep_hours", Me._sleepHours)
            Dim dictionary1 As New Dictionary(Of WeaponGroup, Integer)
            dictionary1.Add(DirectCast(-1212426201, WeaponGroup), 2)
            dictionary1.Add(DirectCast(-1569042529, WeaponGroup), 5)
            dictionary1.Add(DirectCast(WeaponGroup.MG, WeaponGroup), 3)
            dictionary1.Add(DirectCast(WeaponGroup.PetrolCan, WeaponGroup), 1)
            Me._requiredAmountDictionary = dictionary1
            AddHandler MyBase.Aborted, New EventHandler(AddressOf MapInteraction.OnAborted)
        End Sub

        Private Shared Sub AddBlip(ByVal ped As Ped)
            If Not ped.get_CurrentBlip.Exists Then
                ped.AddBlip.set_Name("Enemy Ped")
                Dim wrapper As New EntityEventWrapper(DirectCast(ped, Entity))
                AddHandler wrapper.Died, Function (ByVal sender As EntityEventWrapper, ByVal entity As Entity) 
                    Dim blip1 As Blip = entity.get_CurrentBlip
                    If (blip1 Is Nothing) Then
                        Dim local1 As Blip = blip1
                    Else
                        blip1.Remove
                    End If
                    sender.Dispose
                End Function
                AddHandler wrapper.Aborted, Function (ByVal sender As EntityEventWrapper, ByVal entity As Entity) 
                    Dim blip1 As Blip = entity.get_CurrentBlip
                    If (blip1 Is Nothing) Then
                        Dim local1 As Blip = blip1
                    Else
                        blip1.Remove
                    End If
                End Function
            End If
        End Sub

        Private Sub CraftAmmo()
            Me._craftWeaponsMenu.Clear
            Dim list As List(Of WeaponGroup) = Enumerable.ToList(Of WeaponGroup)(Enumerable.ToArray(Of WeaponGroup)((From w In DirectCast(Enum.GetValues(GetType(WeaponGroup)), WeaponGroup())
                Where (((w <> DirectCast(DirectCast(WeaponGroup.PetrolCan, UInt32), WeaponGroup)) AndAlso ((w <> DirectCast(-1609580060, WeaponGroup)) AndAlso (w <> DirectCast(-728555052, WeaponGroup)))) AndAlso (w <> DirectCast(-942583726, WeaponGroup)))
                Select w)))
            list.Add(DirectCast(WeaponGroup.AssaultRifle, WeaponGroup))
            Dim weaponGroup As WeaponGroup
            For Each weaponGroup In list.ToArray
                Dim item As New UIMenuItem($"{If((weaponGroup = DirectCast(DirectCast(WeaponGroup.AssaultRifle, UInt32), WeaponGroup)), "Assult Rifle", weaponGroup.ToString)}", $"Craft ammo for {weaponGroup}")
                item.SetLeftBadge(BadgeStyle.Ammo)
                Dim required As Integer = Me.GetRequiredPartsForWeaponGroup(weaponGroup)
                item.Description = $"Required Weapon Parts: ~y~{required}~s~"
                Me._craftWeaponsMenu.AddItem(item)
                AddHandler item.Activated, Function (ByVal sender As UIMenu, ByVal selectedItem As UIMenuItem) 
                    Dim objA As InventoryItemBase = PlayerInventory.Instance.ItemFromName("Weapon Parts")
                    If Not Object.ReferenceEquals(objA, Nothing) Then
                        If (objA.Amount < required) Then
                            UI.Notify("Not enough weapon parts.")
                        Else
                            Dim <>9__2 As Predicate(Of WeaponHash)
                            Dim match As Predicate(Of WeaponHash) = <>9__2
                            If (<>9__2 Is Nothing) Then
                                Dim local1 As Predicate(Of WeaponHash) = <>9__2
                                match = <>9__2 = h => (MapInteraction.PlayerPed.get_Weapons.HasWeapon(h) AndAlso (MapInteraction.PlayerPed.get_Weapons.get_Item(h).get_Group = weaponGroup))
                            End If
                            Dim hash As WeaponHash = Array.Find(Of WeaponHash)(DirectCast(Enum.GetValues(GetType(WeaponHash)), WeaponHash()), match)
                            Dim weapon As Weapon = MapInteraction.PlayerPed.get_Weapons.get_Item(hash)
                            If Not Object.ReferenceEquals(weapon, Nothing) Then
                                Dim num As Integer = (10 * required)
                                If ((weapon.Ammo + num) <= weapon.MaxAmmo) Then
                                    MapInteraction.PlayerPed.get_Weapons.Select(weapon)
                                    If ((weapon.Ammo + num) > weapon.MaxAmmo) Then
                                        weapon.set_Ammo(weapon.MaxAmmo)
                                    Else
                                        weapon.set_Ammo((weapon.Ammo + num))
                                    End If
                                    PlayerInventory.Instance.AddItem(objA, -required, ItemType.Resource)
                                End If
                            End If
                        End If
                    End If
                End Function
            Next
            Me._craftWeaponsMenu.Visible = Not Me._craftWeaponsMenu.Visible
        End Sub

        Private Function GetRequiredPartsForWeaponGroup(ByVal group As WeaponGroup) As Integer
            Return If(Me._requiredAmountDictionary.ContainsKey(group), Me._requiredAmountDictionary(group), 1)
        End Function

        Private Shared Function IsEnemy(ByVal ped As Ped) As Boolean
            Return If((Not ped.IsHuman OrElse (ped.IsDead OrElse (ped.GetRelationshipWithPed(MapInteraction.PlayerPed) <> DirectCast(CInt(Relationship.Hate), Relationship)))), ped.IsInCombatAgainst(MapInteraction.PlayerPed), True)
        End Function

        Private Sub MapOnInteracted(ByVal mapProp As MapProp, ByVal inventoryItem As InventoryItemBase)
            Dim objA As BuildableInventoryItem = TryCast(inventoryItem,BuildableInventoryItem)
            If Not Object.ReferenceEquals(objA, Nothing) Then
                Dim id As String = objA.Id
                If (id = "Tent") Then
                    Me.Sleep(mapProp.Position)
                ElseIf (id = "Weapons Crate") Then
                    Me.UseWeaponsCrate(mapProp)
                ElseIf (id = "Work Bench") Then
                    Me.CraftAmmo
                End If
                If objA.IsDoor Then
                    Dim prop As New Prop(mapProp.Handle)
                    PropExt.SetStateOfDoor(prop, Not PropExt.GetDoorLockState(prop), DoorState.Closed)
                End If
            End If
        End Sub

        Private Shared Sub OnAborted(ByVal sender As Object, ByVal eventArgs As EventArgs)
            MapInteraction.PlayerPed.set_IsVisible(True)
            MapInteraction.PlayerPed.set_FreezePosition(False)
            MapInteraction.Player.set_CanControlCharacter(True)
            If Not MapInteraction.PlayerPed.IsDead Then
                Game.FadeScreenIn(0)
            End If
        End Sub

        Private Sub Sleep(ByVal position As Vector3)
            Dim source As Ped() = Enumerable.ToArray(Of Ped)(Enumerable.Where(Of Ped)(World.GetNearbyPeds(position, Me._enemyRangeForSleeping), New Func(Of Ped, Boolean)(AddressOf MapInteraction.IsEnemy)))
            If Enumerable.Any(Of Ped)(source) Then
                UI.Notify("There are ~r~enemies~s~ nearby.")
                UI.Notify("Marking them on your map.")
                Array.ForEach(Of Ped)(source, New Action(Of Ped)(AddressOf MapInteraction.AddBlip))
            Else
                Dim span As TimeSpan = (World.get_CurrentDayTime + New TimeSpan(0, Me._sleepHours, 0, 0))
                MapInteraction.PlayerPed.set_IsVisible(False)
                MapInteraction.Player.set_CanControlCharacter(False)
                MapInteraction.PlayerPed.set_FreezePosition(True)
                Game.FadeScreenOut(&H7D0)
                Script.Wait(&H7D0)
                World.set_CurrentDayTime(span)
                MapInteraction.PlayerPed.set_IsVisible(True)
                MapInteraction.Player.set_CanControlCharacter(True)
                MapInteraction.PlayerPed.set_FreezePosition(False)
                MapInteraction.PlayerPed.ClearBloodDamage
                Dim weatherArray As Weather() = Enumerable.ToArray(Of Weather)((From w In DirectCast(Enum.GetValues(GetType(Weather)), Weather())
                    Where (((w <> DirectCast(CInt(Weather.Blizzard), Weather)) AndAlso ((w <> DirectCast(CInt(Weather.Christmas), Weather)) AndAlso ((w <> DirectCast(CInt(Weather.Snowing), Weather)) AndAlso (w <> DirectCast(CInt(Weather.Snowlight), Weather))))) AndAlso (w <> DirectCast(CInt(Weather.Unknown), Weather)))
                    Select w))
                World.set_Weather(weatherArray(Database.Random.Next(weatherArray.Length)))
                Script.Wait(&H7D0)
                Game.FadeScreenIn(&H7D0)
            End If
        End Sub

        Private Shared Sub TradeOffWeapons(ByVal item As MapProp, ByVal weapons As List(Of Weapon), ByVal currentMenu As UIMenu, ByVal giveToPlayer As Boolean)
            Dim item2 As New UIMenuItem("Back")
            AddHandler item2.Activated, (sender, selectedItem) => sender.GoBack
            currentMenu.Clear
            currentMenu.AddItem(item2)
            Dim notify As Action =  => PlayerMap.Instance.NotifyListChanged
            weapons.ForEach(Function (ByVal weapon As Weapon) 
                Dim item As New UIMenuItem($"{weapon.Hash}")
                currentMenu.AddItem(item)
                AddHandler item.Activated, Function (ByVal sender As UIMenu, ByVal selectedItem As UIMenuItem) 
                    currentMenu.RemoveItemAt(currentMenu.CurrentSelection)
                    currentMenu.RefreshIndex
                    If Not giveToPlayer Then
                        MapInteraction.PlayerPed.get_Weapons.Remove(weapon.Hash)
                        item.Weapons.Add(weapon)
                        notify.Invoke
                    Else
                        Dim <>9__4 As Action(Of WeaponComponent)
                        MapInteraction.PlayerPed.get_Weapons.Give(weapon.Hash, 0, True, True)
                        MapInteraction.PlayerPed.get_Weapons.get_Item(weapon.Hash).set_Ammo(weapon.Ammo)
                        Dim action As Action(Of WeaponComponent) = <>9__4
                        If (<>9__4 Is Nothing) Then
                            Dim local1 As Action(Of WeaponComponent) = <>9__4
                            action = <>9__4 = component => MapInteraction.PlayerPed.get_Weapons.get_Item(weapon.Hash).SetComponent(component, True)
                        End If
                        Enumerable.ToList(Of WeaponComponent)(weapon.Components).ForEach(action)
                        item.Weapons.Remove(weapon)
                        notify.Invoke
                    End If
                End Function
            End Function)
            currentMenu.RefreshIndex
        End Sub

        Private Sub UseWeaponsCrate(ByVal prop As MapProp)
            Dim weapons As List(Of Weapon)
            If (Not prop Is Nothing) Then
                weapons = prop.Weapons
            Else
                Dim local1 As MapProp = prop
                weapons = Nothing
            End If
            If (Not weapons Is Nothing) Then
                AddHandler Me._weaponsMenu.OnMenuChange, Function (ByVal oldMenu As UIMenu, ByVal newMenu As UIMenu, ByVal forward As Boolean) 
                    If Object.ReferenceEquals(newMenu, Me._storageMenu) Then
                        MapInteraction.TradeOffWeapons(prop, prop.Weapons, Me._storageMenu, True)
                    ElseIf Object.ReferenceEquals(newMenu, Me._myWeaponsMenu) Then
                        Dim playerWeapons As New List(Of Weapon)
                        Dim values As WeaponHash() = DirectCast(Enum.GetValues(GetType(WeaponHash)), WeaponHash())
                        Dim weaponComponents As WeaponComponent() = DirectCast(Enum.GetValues(GetType(WeaponComponent)), WeaponComponent())
                        Enumerable.ToList(Of WeaponHash)(values).ForEach(Function (ByVal hash As WeaponHash) 
                            If ((Not hash Is -1569615261) AndAlso MapInteraction.PlayerPed.get_Weapons.HasWeapon(hash)) Then
                                Dim components As WeaponComponent() = Enumerable.ToArray(Of WeaponComponent)((From c In weaponComponents
                                    Where MapInteraction.PlayerPed.get_Weapons.get_Item(hash).IsComponentActive(c)
                                    Select c))
                                Dim item As New Weapon(MapInteraction.PlayerPed.get_Weapons.get_Item(hash).Ammo, hash, components)
                                playerWeapons.Add(item)
                            End If
                        End Function)
                        MapInteraction.TradeOffWeapons(prop, playerWeapons, Me._myWeaponsMenu, False)
                    End If
                End Function
                Me._weaponsMenu.Visible = Not Me._weaponsMenu.Visible
            End If
        End Sub


        ' Properties
        Private Shared ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property

        Private Shared ReadOnly Property Player As Player
            Get
                Return Database.Player
            End Get
        End Property


        ' Fields
        Private Const AmmoPerPart As Integer = 10
        Private ReadOnly _enemyRangeForSleeping As Single = 50!
        Private ReadOnly _sleepHours As Integer = 8
        Private ReadOnly _weaponsMenu As UIMenu = New UIMenu("Weapon Crate", "SELECT AN OPTION")
        Private ReadOnly _storageMenu As UIMenu
        Private ReadOnly _myWeaponsMenu As UIMenu
        Private ReadOnly _craftWeaponsMenu As UIMenu = New UIMenu("Work Bench", "SELECT AN OPTION")
        Private ReadOnly _requiredAmountDictionary As Dictionary(Of WeaponGroup, Integer)

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub <AddBlip>b__20_0(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
                Dim blip1 As Blip = entity.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
                sender.Dispose
            End Sub

            Friend Sub <AddBlip>b__20_1(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
                Dim blip1 As Blip = entity.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
            End Sub

            Friend Function <CraftAmmo>b__15_0(ByVal w As WeaponGroup) As Boolean
                Return (((w <> DirectCast(DirectCast(WeaponGroup.PetrolCan, UInt32), WeaponGroup)) AndAlso ((w <> DirectCast(-1609580060, WeaponGroup)) AndAlso (w <> DirectCast(-728555052, WeaponGroup)))) AndAlso (w <> DirectCast(-942583726, WeaponGroup)))
            End Function

            Friend Function <Sleep>b__19_0(ByVal w As Weather) As Boolean
                Return (((w <> DirectCast(CInt(Weather.Blizzard), Weather)) AndAlso ((w <> DirectCast(CInt(Weather.Christmas), Weather)) AndAlso ((w <> DirectCast(CInt(Weather.Snowing), Weather)) AndAlso (w <> DirectCast(CInt(Weather.Snowlight), Weather))))) AndAlso (w <> DirectCast(CInt(Weather.Unknown), Weather)))
            End Function

            Friend Sub <TradeOffWeapons>b__18_0(ByVal sender As UIMenu, ByVal selectedItem As UIMenuItem)
                sender.GoBack
            End Sub

            Friend Sub <TradeOffWeapons>b__18_1()
                PlayerMap.Instance.NotifyListChanged
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__15_0 As Func(Of WeaponGroup, Boolean)
            Public Shared <>9__18_0 As ItemActivatedEvent
            Public Shared <>9__18_1 As Action
            Public Shared <>9__19_0 As Func(Of Weather, Boolean)
            Public Shared <>9__20_0 As OnDeathEvent
            Public Shared <>9__20_1 As OnWrapperAbortedEvent
        End Class
    End Class
End Namespace


Imports GTA
Imports GTA.Math
Imports NativeUI
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms
Imports ZombiesMod
Imports ZombiesMod.DataClasses
Imports ZombiesMod.Extensions
Imports ZombiesMod.Static

Namespace ZombiesMod.PlayerManagement
    Public Class PlayerInventory
        Inherits Script
        ' Events
        Public Shared Custom Event BuildableUsed As OnUsedBuildableEvent
            AddHandler(ByVal value As OnUsedBuildableEvent)
                Dim buildableUsed As OnUsedBuildableEvent = PlayerInventory.BuildableUsed
                Do While True
                    Dim a As OnUsedBuildableEvent = buildableUsed
                    Dim event4 As OnUsedBuildableEvent = DirectCast(Delegate.Combine(a, value), OnUsedBuildableEvent)
                    buildableUsed = Interlocked.CompareExchange(Of OnUsedBuildableEvent)(PlayerInventory.BuildableUsed, event4, a)
                    If Object.ReferenceEquals(buildableUsed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnUsedBuildableEvent)
                Dim buildableUsed As OnUsedBuildableEvent = PlayerInventory.BuildableUsed
                Do While True
                    Dim source As OnUsedBuildableEvent = buildableUsed
                    Dim event4 As OnUsedBuildableEvent = DirectCast(Delegate.Remove(source, value), OnUsedBuildableEvent)
                    buildableUsed = Interlocked.CompareExchange(Of OnUsedBuildableEvent)(PlayerInventory.BuildableUsed, event4, source)
                    If Object.ReferenceEquals(buildableUsed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Shared Custom Event FoodUsed As OnUsedFoodEvent
            AddHandler(ByVal value As OnUsedFoodEvent)
                Dim foodUsed As OnUsedFoodEvent = PlayerInventory.FoodUsed
                Do While True
                    Dim a As OnUsedFoodEvent = foodUsed
                    Dim event4 As OnUsedFoodEvent = DirectCast(Delegate.Combine(a, value), OnUsedFoodEvent)
                    foodUsed = Interlocked.CompareExchange(Of OnUsedFoodEvent)(PlayerInventory.FoodUsed, event4, a)
                    If Object.ReferenceEquals(foodUsed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnUsedFoodEvent)
                Dim foodUsed As OnUsedFoodEvent = PlayerInventory.FoodUsed
                Do While True
                    Dim source As OnUsedFoodEvent = foodUsed
                    Dim event4 As OnUsedFoodEvent = DirectCast(Delegate.Remove(source, value), OnUsedFoodEvent)
                    foodUsed = Interlocked.CompareExchange(Of OnUsedFoodEvent)(PlayerInventory.FoodUsed, event4, source)
                    If Object.ReferenceEquals(foodUsed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Shared Custom Event LootedPed As OnLootedEvent
            AddHandler(ByVal value As OnLootedEvent)
                Dim lootedPed As OnLootedEvent = PlayerInventory.LootedPed
                Do While True
                    Dim a As OnLootedEvent = lootedPed
                    Dim event4 As OnLootedEvent = DirectCast(Delegate.Combine(a, value), OnLootedEvent)
                    lootedPed = Interlocked.CompareExchange(Of OnLootedEvent)(PlayerInventory.LootedPed, event4, a)
                    If Object.ReferenceEquals(lootedPed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnLootedEvent)
                Dim lootedPed As OnLootedEvent = PlayerInventory.LootedPed
                Do While True
                    Dim source As OnLootedEvent = lootedPed
                    Dim event4 As OnLootedEvent = DirectCast(Delegate.Remove(source, value), OnLootedEvent)
                    lootedPed = Interlocked.CompareExchange(Of OnLootedEvent)(PlayerInventory.LootedPed, event4, source)
                    If Object.ReferenceEquals(lootedPed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Shared Custom Event WeaponUsed As OnUsedWeaponEvent
            AddHandler(ByVal value As OnUsedWeaponEvent)
                Dim weaponUsed As OnUsedWeaponEvent = PlayerInventory.WeaponUsed
                Do While True
                    Dim a As OnUsedWeaponEvent = weaponUsed
                    Dim event4 As OnUsedWeaponEvent = DirectCast(Delegate.Combine(a, value), OnUsedWeaponEvent)
                    weaponUsed = Interlocked.CompareExchange(Of OnUsedWeaponEvent)(PlayerInventory.WeaponUsed, event4, a)
                    If Object.ReferenceEquals(weaponUsed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnUsedWeaponEvent)
                Dim weaponUsed As OnUsedWeaponEvent = PlayerInventory.WeaponUsed
                Do While True
                    Dim source As OnUsedWeaponEvent = weaponUsed
                    Dim event4 As OnUsedWeaponEvent = DirectCast(Delegate.Remove(source, value), OnUsedWeaponEvent)
                    weaponUsed = Interlocked.CompareExchange(Of OnUsedWeaponEvent)(PlayerInventory.WeaponUsed, event4, source)
                    If Object.ReferenceEquals(weaponUsed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub New()
            Me._inventoryKey = MyBase.get_Settings.GetValue(Of Keys)("keys", "inventory_key", Me._inventoryKey)
            MyBase.get_Settings.SetValue(Of Keys)("keys", "inventory_key", Me._inventoryKey)
            MyBase.get_Settings.Save
            Dim objA As Inventory = Serializer.Deserialize(Of Inventory)("./scripts/Inventory.dat")
            If Object.ReferenceEquals(objA, Nothing) Then
                objA = New Inventory(MenuType.Player, False)
            End If
            Me._inventory = objA
            Me._inventory.LoadMenus
            PlayerInventory.Instance = Me
            MenuConrtoller.MenuPool.Add(Me._mainMenu)
            Dim rectangle As New UIResRectangle
            Me._mainMenu.SetBannerType(rectangle)
            Dim item As New UIMenuItem("Inventory")
            Dim item2 As New UIMenuItem("Resources")
            Dim item3 As New UIMenuCheckboxItem("Edit Mode", True, "Allow yourself to pickup objects.")
            AddHandler item3.CheckboxEvent, (sender, checked) => PlayerMap.Instance.EditMode = checked
            AddHandler New UIMenuItem("Main Menu", "Navigate to the main menu. (For gamepad users)").Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                MenuConrtoller.MenuPool.CloseAllMenus
                ModController.Instance.MainMenu.Visible = True
            End Function
            Dim item5 As New UIMenuCheckboxItem("Developer Mode", Me._inventory.DeveloperMode, "Enable/Disable infinite items and resources.")
            AddHandler item5.CheckboxEvent, Function (ByVal item As UIMenuCheckboxItem, ByVal checked As Boolean) 
                If Not Object.ReferenceEquals(Me._inventory, Nothing) Then
                    If checked Then
                        Dim str As String = Game.GetUserInput(DirectCast(WindowTitle.EnterMessage20, WindowTitle), "", 12)
                        If (String.IsNullOrEmpty(str) OrElse (str.ToLower <> "michael")) Then
                            item.Checked = False
                            UI.Notify("Hint: Tamara Greenway's husband's first name.")
                            Return
                        End If
                    End If
                    Me._inventory.DeveloperMode = checked
                    If checked Then
                        UI.Notify("Developer Mode: ~g~Activated~s~")
                    Else
                        Me._inventory.Items.ForEach(i => i.Amount = 0)
                        Me._inventory.Resources.ForEach(i => i.Amount = 0)
                        Me._inventory.RefreshMenu
                    End If
                    Serializer.Serialize(Of Inventory)("./scripts/Inventory.dat", Me._inventory)
                End If
            End Function
            Me._mainMenu.AddItem(item)
            Me._mainMenu.AddItem(item2)
            Me._mainMenu.BindMenuToItem(Me._inventory.InventoryMenu, item)
            Me._mainMenu.BindMenuToItem(Me._inventory.ResourceMenu, item2)
            Me._mainMenu.AddItem(item3)
            Me._mainMenu.AddItem(item5)
            AddHandler Me._inventory.ItemUsed, New ItemUsedEvent(AddressOf Me.InventoryOnItemUsed)
            AddHandler Me._inventory.AddedItem, (item, amount) => Serializer.Serialize(Of Inventory)("./scripts/Inventory.dat", Me._inventory)
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.KeyUp, New KeyEventHandler(AddressOf Me.OnKeyUp)
            AddHandler PlayerInventory.LootedPed, New OnLootedEvent(AddressOf Me.OnLootedPed)
        End Sub

        Private Shared Sub AddBlipToProp(ByVal item As IProp, ByVal name As String, ByVal entity As Entity)
            If (item.BlipSprite <> DirectCast(CInt(BlipSprite.Standard), BlipSprite)) Then
                Dim blip As Blip = entity.AddBlip
                blip.set_Sprite(item.BlipSprite)
                blip.set_Color(item.BlipColor)
                blip.set_Name(name)
            End If
        End Sub

        Public Function AddItem(ByVal item As InventoryItemBase, ByVal amount As Integer, ByVal type As ItemType) As Boolean
            Return ((Not item Is Nothing) AndAlso Me._inventory.AddItem(item, amount, type))
        End Function

        Private Sub AnimalLoot(ByVal ped As Ped)
            If Not PlayerInventory.PlayerPed.get_Weapons.HasWeapon(-1716189206) Then
                UI.Notify("You need a knife!")
            ElseIf Me._inventory.AddItem(Me.ItemFromName("Raw Meat"), 2, ItemType.Resource) Then
                PlayerInventory.PlayerPed.get_Weapons.Select(-1716189206, True)
                UI.Notify("You gutted the animal for ~g~raw meat~s~.")
                PlayerInventory.PlayerPed.get_Task.PlayAnimation("amb@world_human_gardener_plant@male@base", "base", 8!, &HBB8, DirectCast(AnimationFlags.None, AnimationFlags))
                Me._lootedPeds.Add(ped)
            End If
        End Sub

        Private Sub Controller()
        End Sub

        Private Sub GetWater()
            If (Not PlayerInventory.PlayerPed.IsInVehicle AndAlso (Not PlayerInventory.PlayerPed.IsSwimming AndAlso (PlayerInventory.PlayerPed.IsInWater AndAlso Not EntityExtended.IsPlayingAnim(DirectCast(PlayerInventory.PlayerPed, Entity), "pickup_object", "pickup_low")))) Then
                Dim objA As InventoryItemBase = Me._inventory.Resources.Find(i => (i.Id = "Bottle"))
                If (Not Object.ReferenceEquals(objA, Nothing) AndAlso (objA.Amount > 0)) Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Enter, Control))
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Enter, Control)) Then
                        PlayerInventory.PlayerPed.get_Task.PlayAnimation("pickup_object", "pickup_low")
                        Dim item As InventoryItemBase = Me.ItemFromName("Dirty Water")
                        Me.AddItem(item, 1, ItemType.Resource)
                        Me.AddItem(objA, -1, ItemType.Resource)
                        UI.Notify("Resources: -~r~1", True)
                        UI.Notify("Resources: +~g~1", True)
                    End If
                End If
            End If
        End Sub

        Public Function HasItem(ByVal item As InventoryItemBase, ByVal itemType As ItemType) As Boolean
            Dim flag2 As Boolean
            If Object.ReferenceEquals(item, Nothing) Then
                flag2 = False
            Else
                Dim type As ItemType = itemType
                flag2 = If((type = ItemType.Resource), (Me._inventory.Resources.Contains(item) AndAlso (item.Amount > 0)), If((type = ItemType.Item), (Me._inventory.Items.Contains(item) AndAlso (item.Amount > 0)), False))
            End If
            Return flag2
        End Function

        Private Sub InventoryOnItemUsed(ByVal item As InventoryItemBase, ByVal type As ItemType)
            If (Not Object.ReferenceEquals(item, Nothing) AndAlso (type <> ItemType.Resource)) Then
                If Not (item.GetType Is GetType(FoodInventoryItem)) Then
                    If Not (item.GetType Is GetType(WeaponInventoryItem)) Then
                        If ((Not item.GetType Is GetType(BuildableInventoryItem)) AndAlso (Not item.GetType Is GetType(WeaponStorageInventoryItem))) Then
                            If Not (item.GetType Is GetType(UsableInventoryItem)) Then
                                If ((item.GetType Is GetType(CraftableInventoryItem)) AndAlso Not DirectCast(item, CraftableInventoryItem).Validation.Invoke) Then
                                    Return
                                End If
                            Else
                                Dim event2 As UsableItemEvent
                                For Each event2 In DirectCast(item, UsableInventoryItem).ItemEvents
                                    Dim eventArgument As Integer?
                                    Dim event3 As ItemEvent = event2.Event
                                    If (event3 = ItemEvent.GiveArmor) Then
                                        eventArgument = TryCast(event2.EventArgument,Integer?)
                                        Dim num2 As Integer = If((Not eventArgument Is Nothing), eventArgument.GetValueOrDefault, 0)
                                        Dim playerPed As Ped = PlayerInventory.PlayerPed
                                        playerPed.set_Health((playerPed.Health + num2))
                                    ElseIf (event3 = ItemEvent.GiveHealth) Then
                                        eventArgument = TryCast(event2.EventArgument,Integer?)
                                        Dim num3 As Integer = If((Not eventArgument Is Nothing), eventArgument.GetValueOrDefault, 0)
                                        Dim playerPed As Ped = PlayerInventory.PlayerPed
                                        playerPed.set_Armor((playerPed.Armor + num3))
                                    End If
                                Next
                            End If
                        ElseIf Not PlayerInventory.PlayerPed.IsInVehicle Then
                            Dim item4 As BuildableInventoryItem = DirectCast(item, BuildableInventoryItem)
                            Dim preview As New ItemPreview
                            preview.StartPreview(item4.PropName, item4.GroundOffset, item4.IsDoor)
                            Do While True
                                If Not preview.PreviewComplete Then
                                    Script.Yield
                                    Continue Do
                                End If
                                Dim result As Prop = preview.GetResult
                                If (Not result Is Nothing) Then
                                    PlayerInventory.AddBlipToProp(item4, item4.Id, DirectCast(result, Entity))
                                    If (PlayerInventory.BuildableUsed Is Nothing) Then
                                        Dim buildableUsed As OnUsedBuildableEvent = PlayerInventory.BuildableUsed
                                    Else
                                        PlayerInventory.BuildableUsed.Invoke(item4, result)
                                    End If
                                Else
                                    Return
                                End If
                                Exit Do
                            Loop
                        Else
                            UI.Notify("You can't build while in a vehicle!")
                            Return
                        End If
                    Else
                        Dim weapon As WeaponInventoryItem = DirectCast(item, WeaponInventoryItem)
                        PlayerInventory.PlayerPed.get_Weapons.Give(weapon.Hash, weapon.Ammo, True, True)
                        If (PlayerInventory.WeaponUsed Is Nothing) Then
                            Dim weaponUsed As OnUsedWeaponEvent = PlayerInventory.WeaponUsed
                        Else
                            PlayerInventory.WeaponUsed.Invoke(weapon)
                        End If
                    End If
                Else
                    Dim item2 As FoodInventoryItem = DirectCast(item, FoodInventoryItem)
                    PlayerInventory.PlayerPed.get_Task.PlayAnimation(item2.AnimationDict, item2.AnimationName, 8!, item2.AnimationDuration, item2.AnimationFlags)
                    If (PlayerInventory.FoodUsed Is Nothing) Then
                        Dim foodUsed As OnUsedFoodEvent = PlayerInventory.FoodUsed
                    Else
                        PlayerInventory.FoodUsed.Invoke(item2, item2.FoodType)
                    End If
                End If
                Me._inventory.AddItem(item, -1, type)
            End If
        End Sub

        Public Function ItemFromName(ByVal id As String) As InventoryItemBase
            Dim base2 As InventoryItemBase
            Dim items As List(Of InventoryItemBase)
            If (Not Me._inventory Is Nothing) Then
                items = Me._inventory.Items
            Else
                Dim local1 As Inventory = Me._inventory
                items = Nothing
            End If
            If (items Is Nothing) Then
                base2 = Nothing
            Else
                Dim resources As List(Of InventoryItemBase)
                If (Not Me._inventory Is Nothing) Then
                    resources = Me._inventory.Resources
                Else
                    Dim local2 As Inventory = Me._inventory
                    resources = Nothing
                End If
                base2 = If((Not resources Is Nothing), Array.Find(Of InventoryItemBase)(Enumerable.ToArray(Of InventoryItemBase)(Enumerable.Concat(Of InventoryItemBase)(Me._inventory.Items, Me._inventory.Resources)), i => (i.Id = id)), Nothing)
            End If
            Return base2
        End Function

        Private Sub LootDeadPeds()
            If Not PlayerInventory.PlayerPed.IsInVehicle Then
                Dim closest As Ped = World.GetClosest(Of Ped)(PlayerInventory.PlayerPosition, World.GetNearbyPeds(PlayerInventory.PlayerPed, 1.5!))
                If ((Not closest Is Nothing) AndAlso (closest.IsDead AndAlso Not Me._lootedPeds.Contains(closest))) Then
                    UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to loot.", False)
                    Game.DisableControlThisFrame(2, DirectCast(Control.Context, Control))
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Context, Control)) Then
                        If (PlayerInventory.LootedPed Is Nothing) Then
                            Dim lootedPed As OnLootedEvent = PlayerInventory.LootedPed
                        Else
                            PlayerInventory.LootedPed.Invoke(closest)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub OnKeyUp(ByVal sender As Object, ByVal keyEventArgs As KeyEventArgs)
            If (Not MenuConrtoller.MenuPool.IsAnyMenuOpen AndAlso (keyEventArgs.KeyCode = Me._inventoryKey)) Then
                Me._mainMenu.Visible = Not Me._mainMenu.Visible
            End If
        End Sub

        Private Sub OnLootedPed(ByVal ped As Ped)
            If ped.IsHuman Then
                Me.PickupLoot(ped, ItemType.Resource, 1, 3, 0.2!)
            Else
                Me.AnimalLoot(ped)
            End If
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me._inventory.ProcessKeys
            Me.GetWater
            Me.LootDeadPeds
        End Sub

        Public Function PickupItem(ByVal item As InventoryItemBase, ByVal type As ItemType) As Boolean
            Return ((Not item Is Nothing) AndAlso Me._inventory.AddItem(item, 1, type))
        End Function

        Public Sub PickupLoot(ByVal ped As Ped, ByVal Optional type As ItemType = 0, ByVal Optional amountPerItemMin As Integer = 1, ByVal Optional amountPerItemMax As Integer = 3, ByVal Optional successChance As Single = 0.2!)
            Dim source As List(Of InventoryItemBase) = If((type = ItemType.Resource), Me._inventory.Resources, Me._inventory.Items)
            If Enumerable.All(Of InventoryItemBase)(source, r => (r.Amount = r.MaxAmount)) Then
                UI.Notify($"Your {type}s are full!")
            Else
                Dim amount As Integer = 0
                source.ForEach(Function (ByVal i As InventoryItemBase) 
                    If ((i.Id <> "Cooked Meat") AndAlso (Database.Random.NextDouble <= successChance)) Then
                        Dim num As Integer = Database.Random.Next(amountPerItemMin, amountPerItemMax)
                        If ((i.Amount + num) > i.MaxAmount) Then
                            num = (i.MaxAmount - i.Amount)
                        End If
                        Me._inventory.AddItem(i, num, type)
                        amount = (amount + num)
                    End If
                End Function)
                UI.Notify($"{If((amount > 0), $"{type}s: +~g~{amount}", "Nothing found.")}", True)
                PlayerInventory.PlayerPed.get_Task.PlayAnimation("pickup_object", "pickup_low")
                If (Not ped Is Nothing) Then
                    Me._lootedPeds.Add(ped)
                End If
            End If
        End Sub


        ' Properties
        Property Instance As PlayerInventory
            <CompilerGenerated> _
            Public Shared Get
                Return PlayerInventory.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As PlayerInventory)
                PlayerInventory.<Instance>k__BackingField = value
            End Set
        End Property

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
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shared FoodUsed As OnUsedFoodEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shared WeaponUsed As OnUsedWeaponEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shared BuildableUsed As OnUsedBuildableEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shared LootedPed As OnLootedEvent
        Private ReadOnly _mainMenu As UIMenu = New UIMenu(String.Empty, "INVENTORY & RESOURCES", New Point(0, -105))
        Private ReadOnly _lootedPeds As List(Of Ped) = New List(Of Ped)
        Private _inventory As Inventory
        Private ReadOnly _inventoryKey As Keys = Keys.I

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub ctor>b__21_0(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                PlayerMap.Instance.EditMode = checked
            End Sub

            Friend Sub ctor>b__21_1(ByVal sender As UIMenu, ByVal item As UIMenuItem)
                MenuConrtoller.MenuPool.CloseAllMenus
                ModController.Instance.MainMenu.Visible = True
            End Sub

            Friend Sub ctor>b__21_3(ByVal i As InventoryItemBase)
                i.Amount = 0
            End Sub

            Friend Sub ctor>b__21_4(ByVal i As InventoryItemBase)
                i.Amount = 0
            End Sub

            Friend Function <GetWater>b__36_0(ByVal i As InventoryItemBase) As Boolean
                Return (i.Id = "Bottle")
            End Function

            Friend Function <PickupLoot>b__32_0(ByVal r As InventoryItemBase) As Boolean
                Return (r.Amount = r.MaxAmount)
            End Function


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__21_0 As ItemCheckboxEvent
            Public Shared <>9__21_1 As ItemActivatedEvent
            Public Shared <>9__21_3 As Action(Of InventoryItemBase)
            Public Shared <>9__21_4 As Action(Of InventoryItemBase)
            Public Shared <>9__32_0 As Func(Of InventoryItemBase, Boolean)
            Public Shared <>9__36_0 As Predicate(Of InventoryItemBase)
        End Class

        Public Delegate Sub OnLootedEvent(ByVal ped As Ped)

        Public Delegate Sub OnUsedBuildableEvent(ByVal item As BuildableInventoryItem, ByVal newProp As Prop)

        Public Delegate Sub OnUsedFoodEvent(ByVal item As FoodInventoryItem, ByVal foodType As FoodType)

        Public Delegate Sub OnUsedWeaponEvent(ByVal weapon As WeaponInventoryItem)
    End Class
End Namespace


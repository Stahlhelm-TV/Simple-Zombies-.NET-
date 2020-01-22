Imports GTA
Imports GTA.Native
Imports NativeUI
Imports System
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports ZombiesMod.Controllers
Imports ZombiesMod.Extensions
Imports ZombiesMod.PlayerManagement
Imports ZombiesMod.Scripts
Imports ZombiesMod.Static
Imports ZombiesMod.Zombies

Namespace ZombiesMod
    Public Class ModController
        Inherits Script
        ' Methods
        Public Sub New()
            ModController.Instance = Me
            Config.Check
            Relationships.SetRelationships
            Me.LoadSave
            Me.ConfigureMenu
            AddHandler MyBase.KeyUp, New KeyEventHandler(AddressOf Me.OnKeyUp)
        End Sub

        Private Sub ConfigureMenu()
            Me.MainMenu = New UIMenu("Simple Zombies", "SELECT AN OPTION")
            MenuConrtoller.MenuPool.Add(Me.MainMenu)
            Dim item As New UIMenuCheckboxItem("Infection Mode", False, "Enable/Disable zombies.")
            AddHandler item.CheckboxEvent, Function (ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean) 
                ZombieVehicleSpawner.Instance.Spawn = checked
                Loot247.Instance.Spawn = checked
                WorldController.Configure = checked
                AnimalSpawner.Instance.Spawn = checked
                If checked Then
                    WorldExtended.ClearAreaOfEverything(Database.PlayerPosition, 10000!)
                    Dim argumentArray1 As InputArgument() = New InputArgument() { "cs3_07_mpgates" }
                    Function.Call(DirectCast(Hash.REQUEST_IPL, Hash), argumentArray1)
                End If
            End Function
            Dim item2 As New UIMenuCheckboxItem("Fast Zombies", False, "Enable/Disable running zombies.")
            AddHandler item2.CheckboxEvent, (sender, checked) => ZombieCreator.Runners = checked
            Dim item3 As New UIMenuCheckboxItem("Electricity", True, "Enables/Disable blackout mode.")
            AddHandler item3.CheckboxEvent, (sender, checked) => World.SetBlackout(Not checked)
            Dim item4 As New UIMenuCheckboxItem("Survivors", False, "Enable/Disable survivors.")
            AddHandler item4.CheckboxEvent, (sender, checked) => SurvivorController.Instance.Spawn = checked
            Dim item5 As New UIMenuCheckboxItem("Stats", True, "Enable/Disable stats.")
            AddHandler item5.CheckboxEvent, (sender, checked) => PlayerStats.UseStats = checked
            Dim item6 As New UIMenuItem("Load", "Load the map, your vehicles and your bodyguards.")
            item6.SetLeftBadge(BadgeStyle.Heart)
            AddHandler item6.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                PlayerMap.Instance.Deserialize
                PlayerVehicles.Instance.Deserialize
                PlayerGroupManager.Instance.Deserialize
            End Function
            Dim item7 As New UIMenuItem("Save", "Saves the vehicle you are currently in.")
            item7.SetLeftBadge(BadgeStyle.Car)
            AddHandler item7.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                If ((Database.PlayerCurrentVehicle Is Nothing) OrElse ((Not Database.PlayerCurrentVehicle Is Nothing) AndAlso Not Database.PlayerCurrentVehicle.Exists)) Then
                    UI.Notify("You're not in a vehicle.")
                Else
                    PlayerVehicles.Instance.SaveVehicle(Database.PlayerCurrentVehicle)
                End If
            End Function
            Dim item8 As New UIMenuItem("Save All", "Saves all marked vehicles, and their positions.")
            item8.SetLeftBadge(BadgeStyle.Car)
            AddHandler item8.Activated, (sender, item) => PlayerVehicles.Instance.Serialize(True)
            Dim item9 As New UIMenuItem("Save All", "Saves the player ped group (guards).")
            item9.SetLeftBadge(BadgeStyle.Mask)
            AddHandler item9.Activated, (sender, item) => PlayerGroupManager.Instance.SavePeds
            Me.MainMenu.AddItem(item)
            Me.MainMenu.AddItem(item2)
            Me.MainMenu.AddItem(item3)
            Me.MainMenu.AddItem(item4)
            Me.MainMenu.AddItem(item5)
            Me.MainMenu.AddItem(item6)
            Me.MainMenu.AddItem(item7)
            Me.MainMenu.AddItem(item8)
            Me.MainMenu.AddItem(item9)
            Me.MainMenu.RefreshIndex
        End Sub

        Private Sub LoadSave()
            Me._menuKey = MyBase.get_Settings.GetValue(Of Keys)("keys", "zombies_menu_key", Me._menuKey)
            ZombiePed.ZombieDamage = MyBase.get_Settings.GetValue(Of Integer)("zombies", "zombie_damage", ZombiePed.ZombieDamage)
            MyBase.get_Settings.SetValue(Of Keys)("keys", "zombies_menu_key", Me._menuKey)
            MyBase.get_Settings.SetValue(Of Integer)("zombies", "zombie_damage", ZombiePed.ZombieDamage)
            MyBase.get_Settings.Save
        End Sub

        Private Sub OnKeyUp(ByVal sender As Object, ByVal keyEventArgs As KeyEventArgs)
            If (Not MenuConrtoller.MenuPool.IsAnyMenuOpen AndAlso (keyEventArgs.KeyCode = Me._menuKey)) Then
                Me.MainMenu.Visible = Not Me.MainMenu.Visible
            End If
        End Sub


        ' Properties
        Property MainMenu As UIMenu
            Public Get
            Private Set(ByVal value As UIMenu)
        End Property
        Property Instance As ModController
            <CompilerGenerated> _
            Public Shared Get
                Return ModController.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As ModController)
                ModController.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Private _menuKey As Keys = Keys.F10

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Sub <ConfigureMenu>b__12_0(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                ZombieVehicleSpawner.Instance.Spawn = checked
                Loot247.Instance.Spawn = checked
                WorldController.Configure = checked
                AnimalSpawner.Instance.Spawn = checked
                If checked Then
                    WorldExtended.ClearAreaOfEverything(Database.PlayerPosition, 10000!)
                    Dim argumentArray1 As InputArgument() = New InputArgument() { "cs3_07_mpgates" }
                    Function.Call(DirectCast(Hash.REQUEST_IPL, Hash), argumentArray1)
                End If
            End Sub

            Friend Sub <ConfigureMenu>b__12_1(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                ZombieCreator.Runners = checked
            End Sub

            Friend Sub <ConfigureMenu>b__12_2(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                World.SetBlackout(Not checked)
            End Sub

            Friend Sub <ConfigureMenu>b__12_3(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                SurvivorController.Instance.Spawn = checked
            End Sub

            Friend Sub <ConfigureMenu>b__12_4(ByVal sender As UIMenuCheckboxItem, ByVal checked As Boolean)
                PlayerStats.UseStats = checked
            End Sub

            Friend Sub <ConfigureMenu>b__12_5(ByVal sender As UIMenu, ByVal item As UIMenuItem)
                PlayerMap.Instance.Deserialize
                PlayerVehicles.Instance.Deserialize
                PlayerGroupManager.Instance.Deserialize
            End Sub

            Friend Sub <ConfigureMenu>b__12_6(ByVal sender As UIMenu, ByVal item As UIMenuItem)
                If ((Database.PlayerCurrentVehicle Is Nothing) OrElse ((Not Database.PlayerCurrentVehicle Is Nothing) AndAlso Not Database.PlayerCurrentVehicle.Exists)) Then
                    UI.Notify("You're not in a vehicle.")
                Else
                    PlayerVehicles.Instance.SaveVehicle(Database.PlayerCurrentVehicle)
                End If
            End Sub

            Friend Sub <ConfigureMenu>b__12_7(ByVal sender As UIMenu, ByVal item As UIMenuItem)
                PlayerVehicles.Instance.Serialize(True)
            End Sub

            Friend Sub <ConfigureMenu>b__12_8(ByVal sender As UIMenu, ByVal item As UIMenuItem)
                PlayerGroupManager.Instance.SavePeds
            End Sub


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__12_0 As ItemCheckboxEvent
            Public Shared <>9__12_1 As ItemCheckboxEvent
            Public Shared <>9__12_2 As ItemCheckboxEvent
            Public Shared <>9__12_3 As ItemCheckboxEvent
            Public Shared <>9__12_4 As ItemCheckboxEvent
            Public Shared <>9__12_5 As ItemActivatedEvent
            Public Shared <>9__12_6 As ItemActivatedEvent
            Public Shared <>9__12_7 As ItemActivatedEvent
            Public Shared <>9__12_8 As ItemActivatedEvent
        End Class
    End Class
End Namespace


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
Imports ZombiesMod.Static
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.PlayerManagement
    Public Class PlayerGroupManager
        Inherits Script
        ' Methods
        Public Sub New()
            Dim class_ As <>c__DisplayClass4_0
            Me._pedTasks = New Dictionary(Of Ped, PedTask)
            PlayerGroupManager.Instance = Me
            Me._pedMenu = New UIMenu("Guard", "SELECT AN OPTION")
            MenuConrtoller.MenuPool.Add(Me._pedMenu)
            AddHandler Me._pedMenu.OnMenuClose, sender => Me._selectedPed = Nothing
            Dim tasksItem As New UIMenuListItem("Tasks", Enumerable.ToList(Of Object)(Enumerable.Cast(Of Object)(Enum.GetNames(GetType(PedTask)))), 0, "Give peds a specific task to perform.")
            AddHandler tasksItem.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                If (Not Me._selectedPed Is Nothing) Then
                    Dim index As PedTask = DirectCast(tasksItem.Index, PedTask)
                    Me.SetTask(Me._selectedPed, index)
                End If
            End Function
            Dim item As New UIMenuItem("Apply To Nearby", "Apply the selected task to nearby peds within 50 meters.")
            AddHandler item.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                Dim task As PedTask = DirectCast(tasksItem.Index, PedTask)
                Enumerable.ToList(Of Ped)((From ped In DirectCast(Me.PlayerPed.get_CurrentPedGroup, IEnumerable(Of Ped))
                    Where (SystemExtended.VDist(ped.get_Position, class_.PlayerPosition) < 50!)
                    Select ped)).ForEach(ped => Me.SetTask(ped, task))
            End Function
            Dim item2 As New UIMenuItem("Give Weapon", "Give this ped your current weapon.")
            AddHandler item2.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                If (Not Me._selectedPed Is Nothing) Then
                    PlayerGroupManager.TradeWeapons(Me.PlayerPed, Me._selectedPed)
                End If
            End Function
            Dim item3 As New UIMenuItem("Take Weapon", "Take the ped's current weapon.")
            AddHandler item3.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                If (Not Me._selectedPed Is Nothing) Then
                    PlayerGroupManager.TradeWeapons(Me._selectedPed, Me.PlayerPed)
                End If
            End Function
            Dim globalTasks As New UIMenuListItem("Guard Tasks", Enumerable.ToList(Of Object)(Enumerable.Cast(Of Object)(Enum.GetNames(GetType(PedTask)))), 0, "Give all gurads a specific task to perform.")
            AddHandler globalTasks.Activated, Function (ByVal sender As UIMenu, ByVal item As UIMenuItem) 
                Dim task As PedTask = DirectCast(globalTasks.Index, PedTask)
                Enumerable.ToList(Of Ped)(DirectCast(Me.PlayerPed.get_CurrentPedGroup, IEnumerable(Of Ped))).ForEach(ped => Me.SetTask(ped, task))
            End Function
            Me._pedMenu.AddItem(tasksItem)
            Me._pedMenu.AddItem(item)
            Me._pedMenu.AddItem(item2)
            Me._pedMenu.AddItem(item3)
            ModController.Instance.MainMenu.AddItem(globalTasks)
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, New EventHandler(AddressOf Me.OnAborted)
        End Sub

        Public Sub Deserialize()
            If (Me._peds Is Nothing) Then
                Dim objA As PedCollection = Serializer.Deserialize(Of PedCollection)("./scripts/Guards.dat")
                If Object.ReferenceEquals(objA, Nothing) Then
                    objA = New PedCollection
                End If
                Me._peds = objA
                AddHandler Me._peds.ListChanged, count => Serializer.Serialize(Of PedCollection)("./scripts/Guards.dat", Me._peds)
                Enumerable.ToList(Of PedData)(Me._peds).ForEach(Function (ByVal data As PedData) 
                    Dim ped As Ped = World.CreatePed(DirectCast(data.Hash, Model), data.Position)
                    If (Not ped Is Nothing) Then
                        ped.set_Rotation(data.Rotation)
                        PedExtended.Recruit(ped, Me.PlayerPed)
                        data.Weapons.ForEach(w => ped.get_Weapons.Give(w.Hash, w.Ammo, True, True))
                        data.Handle = ped.Handle
                        Me.SetTask(ped, data.Task)
                    End If
                End Function)
            End If
        End Sub

        Private Sub OnAborted(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Dim group As PedGroup = Me.PlayerPed.get_CurrentPedGroup
            Dim list As List(Of Ped) = Enumerable.ToList(Of Ped)((From ped In DirectCast(group, IEnumerable(Of Ped))
                Where Not ped.IsPlayer
                Select ped))
            group.Dispose
            Do While (list.Count > 0)
                list(0).Delete
                list.RemoveAt(0)
            Loop
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If (Not Me.PlayerPed.IsInVehicle AndAlso (Not MenuConrtoller.MenuPool.IsAnyMenuOpen AndAlso (Me.PlayerPed.get_CurrentPedGroup.MemberCount > 0))) Then
                Dim nearbyPeds As Ped() = World.GetNearbyPeds(Me.PlayerPed, 1.5!)
                Dim closest As Ped = World.GetClosest(Of Ped)(Me.PlayerPosition, nearbyPeds)
                If ((Not closest Is Nothing) AndAlso (Not closest.IsInVehicle AndAlso (closest.get_CurrentPedGroup Is Me.PlayerPed.get_CurrentPedGroup))) Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Context, Control))
                    UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to configure this ped.", False)
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Context, Control)) Then
                        Me._selectedPed = closest
                        Me._pedMenu.Visible = Not Me._pedMenu.Visible
                    End If
                End If
            End If
        End Sub

        Public Sub SavePeds()
            If Object.ReferenceEquals(Me._peds, Nothing) Then
                Me.Deserialize
            End If
            Dim list As List(Of Ped) = Me.PlayerPed.get_CurrentPedGroup.ToList(False)
            If (list.Count <= 0) Then
                UI.Notify("You have no bodyguards.")
            Else
                Dim pedDatas As List(Of PedData) = Enumerable.ToList(Of PedData)(Me._peds)
                Enumerable.ToList(Of PedData)(list.ConvertAll(Of PedData)(ped => Me.UpdatePedData(ped, pedDatas.Find(pedData => (pedData.Handle = ped.Handle))))).ForEach(Function (ByVal data As PedData) 
                    If Not Me._peds.Contains(data) Then
                        Me._peds.Add(data)
                    End If
                End Function)
                Serializer.Serialize(Of PedCollection)("./scripts/Guards.dat", Me._peds)
                UI.Notify("~b~Guards~s~ saved!")
            End If
        End Sub

        Private Sub SetTask(ByVal ped As Ped, ByVal task As PedTask)
            If ((task <> Not PedTask.StandStill) AndAlso Not ped.IsPlayer) Then
                If Not Me._pedTasks.ContainsKey(ped) Then
                    Me._pedTasks.Add(ped, task)
                Else
                    Me._pedTasks(ped) = task
                End If
                ped.get_Task.ClearAll
                Select Case task
                    Case PedTask.StandStill
                        ped.get_Task.StandStill(-1)
                        Exit Select
                    Case PedTask.Guard
                        ped.get_Task.GuardCurrentPosition
                        Exit Select
                    Case PedTask.VehicleFollow
                        Dim closestVehicle As Vehicle = World.GetClosestVehicle(ped.get_Position, 100!)
                        If (Not closestVehicle Is Nothing) Then
                            Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(closestVehicle.Handle, InputArgument), DirectCast(Me.PlayerPed.Handle, InputArgument), &H400C0025, &H40000, 15 }
                            Function.Call(DirectCast(Hash.TASK_VEHICLE_FOLLOW, Hash), argumentArray2)
                        Else
                            UI.Notify("There's no vehicle near this ped.", True)
                            Return
                        End If
                        Exit Select
                    Case PedTask.Combat
                        ped.get_Task.FightAgainstHatedTargets(100!)
                        Exit Select
                    Case PedTask.Chill
                        Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(ped.get_Position.X, InputArgument), DirectCast(ped.get_Position.Y, InputArgument), DirectCast(ped.get_Position.Z, InputArgument), 100!, -1 }
                        Function.Call(DirectCast(Hash.TASK_USE_NEAREST_SCENARIO_TO_COORD, Hash), argumentArray1)
                        Exit Select
                    Case PedTask.Leave
                        ped.LeaveGroup
                        Dim blip1 As Blip = ped.get_CurrentBlip
                        If (blip1 Is Nothing) Then
                            Dim local1 As Blip = blip1
                        Else
                            blip1.Remove
                        End If
                        ped.MarkAsNoLongerNeeded
                        EntityEventWrapper.Dispose(DirectCast(ped, Entity))
                        Exit Select
                    Case Else
                        Exit Select
                End Select
                ped.set_BlockPermanentEvents((task = PedTask.Follow))
            End If
        End Sub

        Private Shared Sub TradeWeapons(ByVal trader As Ped, ByVal reviever As Ped)
            If Not Object.ReferenceEquals(trader.get_Weapons.get_Current, trader.get_Weapons.get_Item(-1569615261)) Then
                Dim weapon As Weapon = trader.get_Weapons.get_Current
                If Not reviever.get_Weapons.HasWeapon(weapon.get_Hash) Then
                    If Not reviever.IsPlayer Then
                        reviever.get_Weapons.Drop
                    End If
                    Dim weapon2 As Weapon = reviever.get_Weapons.Give(weapon.get_Hash, 0, True, True)
                    weapon2.set_Ammo(weapon.Ammo)
                    weapon2.set_InfiniteAmmo(False)
                    trader.get_Weapons.Remove(weapon)
                End If
            End If
        End Sub

        Private Function UpdatePedData(ByVal ped As Ped, ByVal data As PedData) As PedData
            Dim task As PedTask = If(Me._pedTasks.ContainsKey(ped), Me._pedTasks(ped), Not PedTask.StandStill)
            Dim source As IEnumerable(Of WeaponHash) = (From hash In DirectCast(Enum.GetValues(GetType(WeaponHash)), WeaponHash())
                Where ped.get_Weapons.HasWeapon(hash)
                Select hash)
            Dim componentHashes As WeaponComponent() = DirectCast(Enum.GetValues(GetType(WeaponComponent)), WeaponComponent())
            Dim weapons As List(Of Weapon) = Enumerable.ToList(Of Weapon)(Enumerable.ToList(Of WeaponHash)(source).ConvertAll(Of Weapon)(Function (ByVal hash As WeaponHash) 
                Dim weapon As Weapon = ped.get_Weapons.get_Item(hash)
                Return New Weapon(weapon.Ammo, weapon.get_Hash, Enumerable.ToArray(Of WeaponComponent)((From h In componentHashes
                    Where weapon.IsComponentActive(h)
                    Select h)))
            End Function))
            Dim flag As Boolean = Object.ReferenceEquals(data, Nothing)
            If flag Then
                If flag Then
                    data = New PedData(ped.Handle, ped.get_Model.Hash, ped.get_Rotation, ped.get_Position, task, weapons)
                End If
            Else
                data.Position = ped.get_Position
                data.Rotation = ped.get_Rotation
                data.Task = task
                data.Weapons = weapons
            End If
            Return data
        End Function


        ' Properties
        Public ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property

        Public ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property

        Property Instance As PlayerGroupManager
            <CompilerGenerated> _
            Public Shared Get
                Return PlayerGroupManager.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As PlayerGroupManager)
                PlayerGroupManager.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Private ReadOnly _pedMenu As UIMenu
        Private _selectedPed As Ped
        Private _peds As PedCollection
        Private ReadOnly _pedTasks As Dictionary(Of Ped, PedTask)

        ' Nested Types
        <Serializable, CompilerGenerated> _
        Private NotInheritable Class <>c
            ' Methods
            Friend Function <OnAborted>b__14_0(ByVal ped As Ped) As Boolean
                Return Not ped.IsPlayer
            End Function


            ' Fields
            Public Shared ReadOnly <>9 As <>c = New <>c
            Public Shared <>9__14_0 As Func(Of Ped, Boolean)
        End Class
    End Class
End Namespace


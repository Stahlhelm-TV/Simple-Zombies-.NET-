Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Diagnostics
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports ZombiesMod
Imports ZombiesMod.Extensions
Imports ZombiesMod.Scripts
Imports ZombiesMod.Static

Namespace ZombiesMod.PlayerManagement
    Public Class PlayerMap
        Inherits Script
        ' Events
        Public Shared Custom Event Interacted As InteractedEvent
            AddHandler(ByVal value As InteractedEvent)
                Dim interacted As InteractedEvent = PlayerMap.Interacted
                Do While True
                    Dim a As InteractedEvent = interacted
                    Dim event4 As InteractedEvent = DirectCast(Delegate.Combine(a, value), InteractedEvent)
                    interacted = Interlocked.CompareExchange(Of InteractedEvent)(PlayerMap.Interacted, event4, a)
                    If Object.ReferenceEquals(interacted, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As InteractedEvent)
                Dim interacted As InteractedEvent = PlayerMap.Interacted
                Do While True
                    Dim source As InteractedEvent = interacted
                    Dim event4 As InteractedEvent = DirectCast(Delegate.Remove(source, value), InteractedEvent)
                    interacted = Interlocked.CompareExchange(Of InteractedEvent)(PlayerMap.Interacted, event4, source)
                    If Object.ReferenceEquals(interacted, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub New()
            PlayerMap.Instance = Me
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, New EventHandler(AddressOf Me.OnAborted)
            AddHandler PlayerInventory.BuildableUsed, New OnUsedBuildableEvent(AddressOf Me.InventoryOnBuildableUsed)
        End Sub

        Public Sub Deserialize()
            If (Me._map Is Nothing) Then
                Dim objA As Map = Serializer.Deserialize(Of Map)("./scripts/Map.dat")
                If Object.ReferenceEquals(objA, Nothing) Then
                    objA = New Map
                End If
                Me._map = objA
                AddHandler Me._map.ListChanged, count => Serializer.Serialize(Of Map)("./scripts/Map.dat", Me._map)
                Me.LoadProps
            End If
        End Sub

        Private Shared Sub DisableAttackActions()
            Game.DisableControlThisFrame(2, DirectCast(Control.Attack2, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.Attack, Control))
            Game.DisableControlThisFrame(2, DirectCast(Control.Aim, Control))
        End Sub

        Public Function Find(ByVal prop As Prop) As Boolean
            Return ((Not Me._map Is Nothing) AndAlso Me._map.Contains(prop))
        End Function

        Private Sub InventoryOnBuildableUsed(ByVal item As BuildableInventoryItem, ByVal newProp As Prop)
            If Object.ReferenceEquals(Me._map, Nothing) Then
                Me.Deserialize
            End If
            Dim item2 As WeaponStorageInventoryItem = TryCast(item,WeaponStorageInventoryItem)
            Dim prop As New MapProp(item.Id, item.PropName, item.BlipSprite, item.BlipColor, item.GroundOffset, item.Interactable, item.IsDoor, item.CanBePickedUp, newProp.get_Rotation, newProp.get_Position, newProp.Handle, item2?.WeaponsList)
            Me._map.Add(prop)
            ZombieVehicleSpawner.Instance.SpawnBlocker.Add(prop.Position)
        End Sub

        Private Sub LoadProps()
            If (Me._map.Count > 0) Then
                Dim prop As MapProp
                For Each prop In Me._map
                    Dim propName As Model = DirectCast(prop.PropName, Model)
                    If Not propName.Request(&H3E8) Then
                        UI.Notify($"Tried to request ~y~{prop.PropName}~s~ but failed.")
                        Continue For
                    End If
                    Dim position As Vector3 = prop.Position
                    Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(propName.Hash, InputArgument), DirectCast(position.X, InputArgument), DirectCast(position.Y, InputArgument), DirectCast(position.Z, InputArgument), 1, 1, False }
                    Dim prop1 As New Prop(Function.Call(Of Integer)(DirectCast(Hash.CREATE_OBJECT_NO_OFFSET, Hash), argumentArray1))
                    prop1.set_FreezePosition(Not prop.IsDoor)
                    prop1.set_Rotation(prop.Rotation)
                    Dim prop2 As Prop = prop1
                    prop.Handle = prop2.Handle
                    If (prop.BlipSprite <> DirectCast(CInt(BlipSprite.Standard), BlipSprite)) Then
                        Dim blip As Blip = prop2.AddBlip
                        blip.set_Sprite(prop.BlipSprite)
                        blip.set_Color(prop.BlipColor)
                        blip.set_Name(prop.Id)
                        ZombieVehicleSpawner.Instance.SpawnBlocker.Add(prop.Position)
                    End If
                Next
            End If
        End Sub

        Public Sub NotifyListChanged()
            Me._map.NotifyListChanged
        End Sub

        Private Sub OnAborted(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me._map.Clear
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If (Not Object.ReferenceEquals(Me._map, Nothing) AndAlso (Enumerable.Any(Of MapProp)(Me._map) AndAlso Not MenuConrtoller.MenuPool.IsAnyMenuOpen)) Then
                Dim closest As MapProp = World.GetClosest(Of MapProp)(Me.PlayerPosition, Enumerable.ToArray(Of MapProp)(Me._map))
                If (Not Object.ReferenceEquals(closest, Nothing) AndAlso (closest.CanBePickedUp AndAlso (SystemExtended.VDist(closest.Position, Me.PlayerPosition) <= 3!))) Then
                    Me.TryUseMapProp(closest)
                End If
            End If
        End Sub

        Private Sub TryUseMapProp(ByVal mapProp As MapProp)
            Dim flag As Boolean = (mapProp.CanBePickedUp AndAlso Me.EditMode)
            If (flag OrElse mapProp.Interactable) Then
                If flag Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Context, Control))
                End If
                If mapProp.Interactable Then
                    PlayerMap.DisableAttackActions
                End If
                GameExtended.DisableWeaponWheel
                UiExtended.DisplayHelpTextThisFrame(($"{If(flag, $"Press ~INPUT_CONTEXT~ to pickup the {mapProp.Id}.
", If(Not Me.EditMode, "You're not in edit mode." & ChrW(10), ""))}" & $"{If(mapProp.Interactable, $"Press ~INPUT_ATTACK~ to {If(mapProp.IsDoor, "Lock/Unlock", "interact")}.", "")}"), False)
                If (Game.IsDisabledControlJustPressed(2, DirectCast(Control.Attack, Control)) AndAlso mapProp.Interactable) Then
                    If (PlayerMap.Interacted Is Nothing) Then
                        Dim interacted As InteractedEvent = PlayerMap.Interacted
                    Else
                        PlayerMap.Interacted.Invoke(mapProp, PlayerInventory.Instance.ItemFromName(mapProp.Id))
                    End If
                End If
                If ((Game.IsDisabledControlJustPressed(2, DirectCast(Control.Context, Control)) AndAlso mapProp.CanBePickedUp) AndAlso PlayerInventory.Instance.PickupItem(PlayerInventory.Instance.ItemFromName(mapProp.Id), ItemType.Item)) Then
                    mapProp.Delete
                    Me._map.Remove(mapProp)
                    ZombieVehicleSpawner.Instance.SpawnBlocker.Remove(mapProp.Position)
                End If
            End If
        End Sub


        ' Properties
        Property Instance As PlayerMap
            <CompilerGenerated> _
            Public Shared Get
                Return PlayerMap.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As PlayerMap)
                PlayerMap.<Instance>k__BackingField = value
            End Set
        End Property

        Public Property EditMode As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Public ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property


        ' Fields
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shared Interacted As InteractedEvent
        Public Const InteractDistance As Single = 3!
        Private _map As Map
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private <EditMode>k__BackingField As Boolean = True

        ' Nested Types
        Public Delegate Sub InteractedEvent(ByVal mapProp As MapProp, ByVal inventoryItem As InventoryItemBase)
    End Class
End Namespace


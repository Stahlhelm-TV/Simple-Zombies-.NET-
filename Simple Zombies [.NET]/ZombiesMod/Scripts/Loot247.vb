Imports GTA
Imports GTA.Math
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports ZombiesMod
Imports ZombiesMod.Extensions
Imports ZombiesMod.PlayerManagement
Imports ZombiesMod.Static

Namespace ZombiesMod.Scripts
    Public Class Loot247
        Inherits Script
        Implements ISpawner
        ' Methods
        Public Sub New()
            Loot247.Instance = Me
            Me._propHashes = New Integer() { Game.GenerateHash("v_ret_247shelves01"), Game.GenerateHash("v_ret_247shelves02"), Game.GenerateHash("v_ret_247shelves03"), Game.GenerateHash("v_ret_247shelves04"), Game.GenerateHash("v_ret_247shelves05") }
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            AddHandler MyBase.Aborted, (sender, args) => Me.Clear
        End Sub

        Public Sub Clear()
            Do While (Me._blips.Count > 0)
                Me._blips(0).Remove
                Me._blips.RemoveAt(0)
            Loop
        End Sub

        Private Sub ClearBlips()
            If Not Me.Spawn Then
                Me.Clear
            End If
        End Sub

        Private Function IsShelf(ByVal arg As Prop) As Boolean
            Return (Enumerable.Contains(Of Integer)(Me._propHashes, arg.get_Model.Hash) AndAlso Not Me._lootedShelfes.Contains(arg))
        End Function

        Private Sub LootShops()
            If (Me.Spawn AndAlso Not EntityExtended.IsPlayingAnim(DirectCast(Loot247.PlayerPed, Entity), "oddjobs@shop_robbery@rob_till", "loop")) Then
                Dim source As IEnumerable(Of Prop) = Enumerable.Where(Of Prop)(World.GetNearbyProps(Loot247.PlayerPosition, 15!), New Func(Of Prop, Boolean)(AddressOf Me.IsShelf))
                Dim closest As Prop = World.GetClosest(Of Prop)(Loot247.PlayerPosition, Enumerable.ToArray(Of Prop)(source))
                If ((Not closest Is Nothing) AndAlso (SystemExtended.VDist(closest.get_Position, Loot247.PlayerPosition) <= 1.5!)) Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Context, Control))
                    UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to loot the shelf.", False)
                    If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Context, Control)) Then
                        Me._lootedShelfes.Add(closest)
                        Dim flag As Boolean = (Database.Random.NextDouble > 0.30000001192092896)
                        PlayerInventory.Instance.PickupItem(If(flag, PlayerInventory.Instance.ItemFromName("Packaged Food"), PlayerInventory.Instance.ItemFromName("Clean Water")), ItemType.Item)
                        Loot247.PlayerPed.get_Task.PlayAnimation("oddjobs@shop_robbery@rob_till", "loop")
                        Loot247.PlayerPed.set_Heading((closest.get_Position - Loot247.PlayerPosition).ToHeading)
                    End If
                End If
            End If
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal e As EventArgs)
            Me.SpawnBlips
            Me.ClearBlips
            Me.LootShops
        End Sub

        Private Sub SpawnBlips()
            If (Me.Spawn AndAlso (Me._blips.Count < Database.Shops247Locations.Length)) Then
                Dim vector As Vector3
                For Each vector In Database.Shops247Locations
                    Dim item As Blip = World.CreateBlip(vector)
                    item.set_Sprite(DirectCast(BlipSprite.Store, BlipSprite))
                    item.set_Name("Store")
                    item.set_IsShortRange(True)
                    Me._blips.Add(item)
                Next
            End If
        End Sub


        ' Properties
        Property Instance As Loot247
            <CompilerGenerated> _
            Public Shared Get
                Return Loot247.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As Loot247)
                Loot247.<Instance>k__BackingField = value
            End Set
        End Property

        Public Property Spawn As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
        Private Shared ReadOnly Property PlayerPosition As Vector3
            Get
                Return Database.PlayerPosition
            End Get
        End Property

        Private Shared ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property


        ' Fields
        Public Const InteractDistance As Single = 1.5!
        Private ReadOnly _blips As List(Of Blip) = New List(Of Blip)
        Private ReadOnly _lootedShelfes As List(Of Prop) = New List(Of Prop)
        Private ReadOnly _propHashes As Integer()
    End Class
End Namespace


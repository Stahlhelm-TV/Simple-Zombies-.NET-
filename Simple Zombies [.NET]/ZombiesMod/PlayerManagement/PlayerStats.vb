Imports GTA
Imports NativeUI
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports ZombiesMod
Imports ZombiesMod.Static

Namespace ZombiesMod.PlayerManagement
    Public Class PlayerStats
        Inherits Script
        ' Methods
        Public Sub New()
            AddHandler PlayerInventory.FoodUsed, New OnUsedFoodEvent(AddressOf Me.PlayerInventoryOnFoodUsed)
            Me._sprintReductionMultiplier = MyBase.get_Settings.GetValue(Of Single)("stats", "sprint_reduction_multiplier", Me._sprintReductionMultiplier)
            Me._hungerReductionMultiplier = MyBase.get_Settings.GetValue(Of Single)("stats", "hunger_reduction_multiplier", Me._hungerReductionMultiplier)
            Me._thirstReductionMultiplier = MyBase.get_Settings.GetValue(Of Single)("stats", "thirst_reduction_multiplier", Me._thirstReductionMultiplier)
            Me._statDamageInterval = MyBase.get_Settings.GetValue(Of Single)("stats", "stat_damage_interaval", Me._statDamageInterval)
            Me._statSustainLength = MyBase.get_Settings.GetValue(Of Single)("stats", "stat_sustain_length", Me._statSustainLength)
            MyBase.get_Settings.SetValue(Of Boolean)("stats", "use_stats", PlayerStats.UseStats)
            MyBase.get_Settings.SetValue(Of Single)("stats", "sprint_reduction_multiplier", Me._sprintReductionMultiplier)
            MyBase.get_Settings.SetValue(Of Single)("stats", "hunger_reduction_multiplier", Me._hungerReductionMultiplier)
            MyBase.get_Settings.SetValue(Of Single)("stats", "thirst_reduction_multiplier", Me._thirstReductionMultiplier)
            MyBase.get_Settings.SetValue(Of Single)("stats", "stat_damage_interaval", Me._statDamageInterval)
            MyBase.get_Settings.SetValue(Of Single)("stats", "stat_sustain_length", Me._statSustainLength)
            MyBase.get_Settings.Save
            Me._statDisplay = New List(Of StatDisplayItem)
            Dim stat As Stat
            For Each stat In New Stats.StatList
                Dim item1 As New StatDisplayItem
                item1.Stat = stat
                Dim bar1 As New BarTimerBar(stat.Name.ToUpper)
                bar1.ForegroundColor = Color.White
                bar1.BackgroundColor = Color.Gray
                item1.Bar = bar1
                Dim item As StatDisplayItem = item1
                Me._statDisplay.Add(item)
                MenuConrtoller.BarPool.Add(item.Bar)
            Next
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
            MyBase.set_Interval(10)
        End Sub

        Private Sub HandleReductionStat(ByVal stat As Stat, ByVal targetName As String, ByVal notification As String, ByVal reductionMultiplier As Single, ByRef damageTimer As Single, ByRef sustainTimer As Single)
            If (stat.Name = targetName) Then
                If stat.Sustained Then
                    damageTimer = Me._statDamageInterval
                    sustainTimer = (sustainTimer + Game.LastFrameTime)
                    If (sustainTimer >= Me._statSustainLength) Then
                        sustainTimer = 0!
                        stat.Sustained = False
                    End If
                ElseIf (stat.Value > 0!) Then
                    stat.Value = (stat.Value - (Game.LastFrameTime * reductionMultiplier))
                    damageTimer = Me._statDamageInterval
                Else
                    UI.Notify(notification)
                    damageTimer = (damageTimer + Game.LastFrameTime)
                    If (damageTimer >= Me._statDamageInterval) Then
                        PlayerStats.PlayerPed.ApplyDamage(Database.Random.Next(3, 15))
                        damageTimer = 0!
                    End If
                    stat.Value = 0!
                End If
            End If
        End Sub

        Private Sub HandleStamina(ByVal stat As Stat)
            If (stat.Name = "Stamina") Then
                If Not stat.Sustained Then
                    Game.DisableControlThisFrame(2, DirectCast(Control.Sprint, Control))
                    stat.Value = (stat.Value + (Game.LastFrameTime * Me._sprintReductionMultiplier))
                    If (stat.Value >= (stat.MaxVal * 0.3!)) Then
                        stat.Sustained = True
                    End If
                ElseIf Not Database.PlayerIsSprinting Then
                    stat.Value = If((stat.Value >= stat.MaxVal), stat.MaxVal, (stat.Value + (Game.LastFrameTime * (Me._sprintReductionMultiplier * 10!))))
                ElseIf (stat.Value > 0!) Then
                    stat.Value = (stat.Value - (Game.LastFrameTime * Me._sprintReductionMultiplier))
                Else
                    stat.Sustained = False
                    stat.Value = 0!
                End If
            End If
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal e As EventArgs)
            If Database.PlayerIsDead Then
                Dim item As StatDisplayItem
                For Each item In Me._statDisplay
                    item.Stat.Value = item.Stat.MaxVal
                Next
            ElseIf Not PlayerStats.UseStats Then
                If Not Me._removedDisplay Then
                    Dim item2 As StatDisplayItem
                    For Each item2 In Me._statDisplay
                        MenuConrtoller.BarPool.Remove(item2.Bar)
                    Next
                    Me._removedDisplay = True
                End If
            Else
                If Me._removedDisplay Then
                    Dim item3 As StatDisplayItem
                    For Each item3 In Me._statDisplay
                        MenuConrtoller.BarPool.Add(item3.Bar)
                    Next
                    Me._removedDisplay = False
                End If
                Dim num As Integer = 0
                Dim count As Integer = Me._statDisplay.Count
                Do While True
                    If (num >= count) Then
                        Exit Do
                    End If
                    Dim item4 As StatDisplayItem = Me._statDisplay(num)
                    Dim stat As Stat = item4.Stat
                    item4.Bar.Percentage = stat.Value
                    Me.HandleReductionStat(stat, "Hunger", "You're ~r~starving~s~!", Me._hungerReductionMultiplier, Me._hungerDamageTimer, Me._hungerSustainTimer)
                    Me.HandleReductionStat(stat, "Thirst", "You're ~r~dehydrated~s~!", Me._thirstReductionMultiplier, Me._thirstDamageTimer, Me._thirstSustainTimer)
                    Me.HandleStamina(stat)
                    num += 1
                Loop
            End If
        End Sub

        Private Sub PlayerInventoryOnFoodUsed(ByVal item As FoodInventoryItem, ByVal foodType As FoodType)
            Select Case foodType
                Case FoodType.Water
                    Me.UpdateStat(item, "Thirst", "Thirst ~g~sustained~s~.", 0!)
                    Exit Select
                Case FoodType.Food
                    Me.UpdateStat(item, "Hunger", "Hunger ~g~sustained~s~.", 0!)
                    Exit Select
                Case FoodType.SpecialFood
                    Me.UpdateStat(item, "Hunger", "Hunger ~g~sustained~s~.", 0!)
                    Me.UpdateStat(item, "Thirst", "Thirst ~g~sustained~s~.", 0.15!)
                    Exit Select
                Case Else
                    Exit Select
            End Select
        End Sub

        Private Sub UpdateStat(ByVal item As IFood, ByVal name As String, ByVal notify As String, ByVal Optional valueOverride As Single = 0!)
            Dim item2 As StatDisplayItem = Me._statDisplay.Find(displayItem => (displayItem.Stat.Name = name))
            item2.Stat.Value = (item2.Stat.Value + If((valueOverride <= 0!), item.RestorationAmount, valueOverride))
            item2.Stat.Sustained = True
            UI.Notify(notify, True)
            If (item2.Stat.Value > item2.Stat.MaxVal) Then
                item2.Stat.Value = item2.Stat.MaxVal
            End If
        End Sub


        ' Properties
        Private Shared ReadOnly Property PlayerPed As Ped
            Get
                Return Database.PlayerPed
            End Get
        End Property


        ' Fields
        Public Shared UseStats As Boolean = True
        Private ReadOnly _statDamageInterval As Single = 5!
        Private ReadOnly _hungerReductionMultiplier As Single = 0.00045!
        Private ReadOnly _thirstReductionMultiplier As Single = 0.0007!
        Private ReadOnly _sprintReductionMultiplier As Single = 0.05!
        Private ReadOnly _statSustainLength As Single = 120!
        Private ReadOnly _statDisplay As List(Of StatDisplayItem)
        Private _hungerDamageTimer As Single
        Private _hungerSustainTimer As Single
        Private _thirstDamageTimer As Single
        Private _thirstSustainTimer As Single
        Private _removedDisplay As Boolean
    End Class
End Namespace


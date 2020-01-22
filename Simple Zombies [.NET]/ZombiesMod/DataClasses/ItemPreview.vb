Imports GTA
Imports GTA.Math
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices
Imports ZombiesMod.Extensions
Imports ZombiesMod.Scripts
Imports ZombiesMod.Static

Namespace ZombiesMod.DataClasses
    Public Class ItemPreview
        ' Methods
        Public Sub New()
            ScriptEventHandler.Instance.RegisterScript(New EventHandler(AddressOf Me.OnTick))
            AddHandler ScriptEventHandler.Instance.Aborted, (sender, args) => Me.Abort
        End Sub

        Public Sub Abort()
            If (Me._currentPreview Is Nothing) Then
                Dim local1 As Prop = Me._currentPreview
            Else
                Me._currentPreview.Delete
            End If
        End Sub

        Private Sub CreateItemPreview()
            If (Me._currentPreview Is Nothing) Then
                Me.PreviewComplete = False
                Me._currentOffset = Vector3.get_Zero
                Dim vector As New Vector3
                vector = New Vector3
                Dim prop As Prop = World.CreateProp(DirectCast(Me._currnetPropHash, Model), vector, vector, False, False)
                If (prop Is Nothing) Then
                    UI.Notify($"Failed to load prop, even after request.
Prop Name: {Me._currnetPropHash}")
                    Me._resultProp = Nothing
                    Me._preview = False
                    Me.PreviewComplete = True
                Else
                    prop.set_HasCollision(False)
                    Me._currentPreview = prop
                    Me._currentPreview.set_Alpha(150)
                    Database.PlayerPed.get_Weapons.Select(-1569615261, True)
                    Me._resultProp = Nothing
                End If
            Else
                UiExtended.DisplayHelpTextThisFrame("Press ~INPUT_AIM~ to cancel." & ChrW(10) & "Press ~INPUT_ATTACK~ to place the item.", True)
                Game.DisableControlThisFrame(2, DirectCast(Control.Aim, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.Attack, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.Attack2, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.ParachuteBrakeLeft, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.ParachuteBrakeRight, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.Cover, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.Phone, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.PhoneUp, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.PhoneDown, Control))
                Game.DisableControlThisFrame(2, DirectCast(Control.Sprint, Control))
                GameExtended.DisableWeaponWheel
                If Game.IsDisabledControlPressed(2, DirectCast(Control.Aim, Control)) Then
                    Dim prop2 As Prop
                    Me._currentPreview.Delete
                    Me._resultProp = DirectCast(prop2 = Nothing, Prop)
                    Me._currentPreview = prop2
                    Me._preview = False
                    Me.PreviewComplete = True
                    ScriptEventHandler.Instance.UnregisterScript(New EventHandler(AddressOf Me.OnTick))
                Else
                    Dim vector2 As Vector3 = GameplayCamera.get_Position
                    Dim vector4 As Vector3 = World.Raycast(vector2, DirectCast((vector2 + (GameplayCamera.get_Direction * 15!)), Vector3), -1, DirectCast(Database.PlayerPed, Entity)).get_HitCoords
                    If ((vector4 = Vector3.get_Zero) OrElse (vector4.DistanceTo(Database.PlayerPosition) <= 1.5!)) Then
                        Me._currentPreview.set_IsVisible(False)
                    Else
                        ItemPreview.DrawScaleForms
                        Dim num As Single = If(Game.IsControlPressed(2, DirectCast(Control.Sprint, Control)), 1.5!, 1!)
                        If Game.IsControlPressed(2, DirectCast(Control.ParachuteBrakeLeft, Control)) Then
                            Dim vector5 As Vector3 = Me._currentPreview.get_Rotation
                            Dim singlePtr1 As Single* = AddressOf vector5.Z
                            singlePtr1(0) = (singlePtr1(0) + ((Game.LastFrameTime * 50!) * num))
                            Me._currentPreview.set_Rotation(vector5)
                        ElseIf Game.IsControlPressed(2, DirectCast(Control.ParachuteBrakeRight, Control)) Then
                            Dim vector6 As Vector3 = Me._currentPreview.get_Rotation
                            Dim singlePtr2 As Single* = AddressOf vector6.Z
                            singlePtr2(0) = (singlePtr2(0) - ((Game.LastFrameTime * 50!) * num))
                            Me._currentPreview.set_Rotation(vector6)
                        End If
                        If Game.IsControlPressed(2, DirectCast(Control.PhoneUp, Control)) Then
                            Dim singlePtr3 As Single* = AddressOf Me._currentOffset.Z
                            singlePtr3(0) = (singlePtr3(0) + (Game.LastFrameTime * num))
                        ElseIf Game.IsControlPressed(2, DirectCast(Control.PhoneDown, Control)) Then
                            Dim singlePtr4 As Single* = AddressOf Me._currentOffset.Z
                            singlePtr4(0) = (singlePtr4(0) - (Game.LastFrameTime * num))
                        End If
                        Me._currentPreview.set_Position(DirectCast((vector4 + Me._currentOffset), Vector3))
                        Me._currentPreview.set_IsVisible(True)
                        If Game.IsDisabledControlJustPressed(2, DirectCast(Control.Attack, Control)) Then
                            Me._currentPreview.ResetAlpha
                            Me._resultProp = Me._currentPreview
                            Me._resultProp.set_HasCollision(True)
                            Me._resultProp.set_FreezePosition(Not Me._isDoor)
                            Me._preview = False
                            Me._currentPreview = Nothing
                            Me._currnetPropHash = String.Empty
                            Me.PreviewComplete = True
                            ScriptEventHandler.Instance.UnregisterScript(New EventHandler(AddressOf Me.OnTick))
                        End If
                    End If
                End If
            End If
        End Sub

        Private Shared Sub DrawScaleForms()
            Dim scaleform As New Scaleform("instructional_buttons")
            scaleform.CallFunction("CLEAR_ALL", New Object(0  - 1) {})
            Dim arguments As Object() = New Object() { 0 }
            scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", arguments)
            scaleform.CallFunction("CREATE_CONTAINER", New Object(0  - 1) {})
            Dim objArray2 As Object() = New Object(3  - 1) {}
            objArray2(0) = 0
            Dim argumentArray1 As InputArgument() = New InputArgument() { 2, &H98, 0 }
            objArray2(1) = Function.Call(Of String)(DirectCast(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTON, Hash), argumentArray1)
            objArray2(2) = String.Empty
            scaleform.CallFunction("SET_DATA_SLOT", objArray2)
            Dim objArray3 As Object() = New Object(3  - 1) {}
            objArray3(0) = 1
            Dim argumentArray2 As InputArgument() = New InputArgument() { 2, &H99, 0 }
            objArray3(1) = Function.Call(Of String)(DirectCast(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTON, Hash), argumentArray2)
            objArray3(2) = "Rotate"
            scaleform.CallFunction("SET_DATA_SLOT", objArray3)
            Dim objArray4 As Object() = New Object(3  - 1) {}
            objArray4(0) = 2
            Dim argumentArray3 As InputArgument() = New InputArgument() { 2, &HAC, 0 }
            objArray4(1) = Function.Call(Of String)(DirectCast(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTON, Hash), argumentArray3)
            objArray4(2) = String.Empty
            scaleform.CallFunction("SET_DATA_SLOT", objArray4)
            Dim objArray5 As Object() = New Object(3  - 1) {}
            objArray5(0) = 3
            Dim argumentArray4 As InputArgument() = New InputArgument() { 2, &HAD, 0 }
            objArray5(1) = Function.Call(Of String)(DirectCast(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTON, Hash), argumentArray4)
            objArray5(2) = "Lift/Lower"
            scaleform.CallFunction("SET_DATA_SLOT", objArray5)
            Dim objArray6 As Object() = New Object(3  - 1) {}
            objArray6(0) = 4
            Dim argumentArray5 As InputArgument() = New InputArgument() { 2, &H15, 0 }
            objArray6(1) = Function.Call(Of String)(DirectCast(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTON, Hash), argumentArray5)
            objArray6(2) = "Accelerate"
            scaleform.CallFunction("SET_DATA_SLOT", objArray6)
            Dim objArray7 As Object() = New Object() { -1 }
            scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", objArray7)
            scaleform.Render2D
        End Sub

        Public Function GetResult() As Prop
            Return Me._resultProp
        End Function

        Public Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If Me._preview Then
                Me.CreateItemPreview
            End If
        End Sub

        Public Sub StartPreview(ByVal propHash As String, ByVal offset As Vector3, ByVal isDoor As Boolean)
            If Not Me._preview Then
                Me._preview = True
                Me._currnetPropHash = propHash
                Me._isDoor = isDoor
            End If
        End Sub


        ' Properties
        Property PreviewComplete As Boolean
            Public Get
            Private Set(ByVal value As Boolean)
        End Property

        ' Fields
        Private _currentOffset As Vector3
        Private _currentPreview As Prop
        Private _resultProp As Prop
        Private _preview As Boolean
        Private _isDoor As Boolean
        Private _currnetPropHash As String
    End Class
End Namespace


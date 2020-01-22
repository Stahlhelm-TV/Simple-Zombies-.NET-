Imports GTA
Imports GTA.Native
Imports System
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports ZombiesMod.Wrappers

Namespace ZombiesMod.Extensions
    Public Class PedExtended
        ' Methods
        Shared Sub New()
            Dim textArray1 As String() = New String(&H25  - 1) {}
            textArray1(0) = "SPEECH_PARAMS_STANDARD"
            textArray1(1) = "SPEECH_PARAMS_ALLOW_REPEAT"
            textArray1(2) = "SPEECH_PARAMS_BEAT"
            textArray1(3) = "SPEECH_PARAMS_FORCE"
            textArray1(4) = "SPEECH_PARAMS_FORCE_FRONTEND"
            textArray1(5) = "SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND"
            textArray1(6) = "SPEECH_PARAMS_FORCE_NORMAL"
            textArray1(7) = "SPEECH_PARAMS_FORCE_NORMAL_CLEAR"
            textArray1(8) = "SPEECH_PARAMS_FORCE_NORMAL_CRITICAL"
            textArray1(9) = "SPEECH_PARAMS_FORCE_SHOUTED"
            textArray1(10) = "SPEECH_PARAMS_FORCE_SHOUTED_CLEAR"
            textArray1(11) = "SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL"
            textArray1(12) = "SPEECH_PARAMS_FORCE_PRELOAD_ONLY"
            textArray1(13) = "SPEECH_PARAMS_MEGAPHONE"
            textArray1(14) = "SPEECH_PARAMS_HELI"
            textArray1(15) = "SPEECH_PARAMS_FORCE_MEGAPHONE"
            textArray1(&H10) = "SPEECH_PARAMS_FORCE_HELI"
            textArray1(&H11) = "SPEECH_PARAMS_INTERRUPT"
            textArray1(&H12) = "SPEECH_PARAMS_INTERRUPT_SHOUTED"
            textArray1(&H13) = "SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR"
            textArray1(20) = "SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL"
            textArray1(&H15) = "SPEECH_PARAMS_INTERRUPT_NO_FORCE"
            textArray1(&H16) = "SPEECH_PARAMS_INTERRUPT_FRONTEND"
            textArray1(&H17) = "SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND"
            textArray1(&H18) = "SPEECH_PARAMS_ADD_BLIP"
            textArray1(&H19) = "SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT"
            textArray1(&H1A) = "SPEECH_PARAMS_ADD_BLIP_FORCE"
            textArray1(&H1B) = "SPEECH_PARAMS_ADD_BLIP_SHOUTED"
            textArray1(&H1C) = "SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE"
            textArray1(&H1D) = "SPEECH_PARAMS_ADD_BLIP_INTERRUPT"
            textArray1(30) = "SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE"
            textArray1(&H1F) = "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED"
            textArray1(&H20) = "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR"
            textArray1(&H21) = "SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL"
            textArray1(&H22) = "SPEECH_PARAMS_SHOUTED"
            textArray1(&H23) = "SPEECH_PARAMS_SHOUTED_CLEAR"
            textArray1(&H24) = "SPEECH_PARAMS_SHOUTED_CRITICAL"
            PedExtended.SpeechModifierNames = textArray1
        End Sub

        Public Shared Sub ApplyDamagePack(ByVal ped As Ped, ByVal damage As Single, ByVal multiplier As Single, ByVal damagePack As DamagePack)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(damagePack.ToString, InputArgument), DirectCast(damage, InputArgument), DirectCast(multiplier, InputArgument) }
            Function.Call(DirectCast(Hash.APPLY_PED_DAMAGE_PACK, Hash), argumentArray1)
        End Sub

        Public Shared Function CanHearPlayer(ByVal ped As Ped, ByVal player As Player) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(player.Handle, InputArgument), DirectCast(ped.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.CAN_PED_HEAR_PLAYER, Hash), argumentArray1)
        End Function

        Public Shared Sub ClearFleeAttributes(ByVal ped As Ped)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), 0, 0 }
            Function.Call(DirectCast(Hash.SET_PED_FLEE_ATTRIBUTES, Hash), argumentArray1)
        End Sub

        Public Shared Sub DisablePainAudio(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.DISABLE_PED_PAIN_AUDIO, Hash), argumentArray1)
        End Sub

        Public Shared Function GetDrawableVariation(ByVal ped As Ped, ByVal id As ComponentId) As Integer
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(id, InputArgument) }
            Return Function.Call(Of Integer)(DirectCast(Hash.GET_PED_DRAWABLE_VARIATION, Hash), argumentArray1)
        End Function

        Public Shared Function GetNumberOfDrawableVariations(ByVal ped As Ped, ByVal id As ComponentId) As Integer
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(id, InputArgument) }
            Return Function.Call(Of Integer)(DirectCast(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, Hash), argumentArray1)
        End Function

        Public Shared Function GetStealthMovement(ByVal ped As Ped) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.GET_PED_STEALTH_MOVEMENT, Hash), argumentArray1)
        End Function

        Public Shared Function HasBeenDamagedBy(ByVal ped As Ped, ByVal weapon As WeaponHash) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), weapon, 0 }
            Return Function.Call(Of Boolean)(DirectCast(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Hash), argumentArray1)
        End Function

        Public Shared Function HasBeenDamagedByMelee(ByVal ped As Ped) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), 0, 1 }
            Return Function.Call(Of Boolean)(DirectCast(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, Hash), argumentArray1)
        End Function

        Public Shared Function IsAmbientSpeechPlaying(ByVal ped As Ped) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.IS_AMBIENT_SPEECH_PLAYING, Hash), argumentArray1)
        End Function

        Public Shared Function IsCurrentWeaponSileced(ByVal ped As Ped) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.IS_PED_CURRENT_WEAPON_SILENCED, Hash), argumentArray1)
        End Function

        Public Shared Function IsDriving(ByVal ped As Ped) As Boolean
            Return (PedExtended.IsSubttaskActive(ped, Subtask.DrivingWandering) OrElse PedExtended.IsSubttaskActive(ped, Subtask.DrivingGoingToDestinationOrEscorting))
        End Function

        Public Shared Function IsSubttaskActive(ByVal ped As Ped, ByVal task As Subtask) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped, InputArgument), DirectCast(task, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.GET_IS_TASK_ACTIVE, Hash), argumentArray1)
        End Function

        Public Shared Function IsUsingAnyScenario(ByVal ped As Ped) As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.IS_PED_USING_ANY_SCENARIO, Hash), argumentArray1)
        End Function

        Public Shared Sub Jump(ByVal ped As Ped)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), True, 0, 0 }
            Function.Call(DirectCast(Hash.TASK_JUMP, Hash), argumentArray1)
        End Sub

        Public Shared Function LastDamagedBone(ByVal ped As Ped) As Bone
            Dim num As Integer
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(DirectCast(AddressOf num, IntPtr), InputArgument) }
            Return If(Not Function.Call(Of Boolean)(DirectCast(Hash.GET_PED_LAST_DAMAGE_BONE, Hash), argumentArray1), DirectCast(Bone.SkelRoot, Bone), DirectCast(num, Bone))
        End Function

        Public Shared Sub PlayAmbientSpeech(ByVal ped As Ped, ByVal speechName As String, ByVal Optional modifier As SpeechModifier = 0)
            If ((modifier < SpeechModifier.Standard) OrElse (modifier >= PedExtended.SpeechModifierNames.Length)) Then
                Throw New ArgumentOutOfRangeException("modifier")
            End If
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(speechName, InputArgument), DirectCast(PedExtended.SpeechModifierNames(CInt(modifier)), InputArgument) }
            Function.Call(DirectCast(Hash._PLAY_AMBIENT_SPEECH1, Hash), argumentArray1)
        End Sub

        Public Shared Sub PlayFacialAnim(ByVal ped As Ped, ByVal animSet As String, ByVal animName As String)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(animName, InputArgument), DirectCast(animSet, InputArgument) }
            Function.Call(DirectCast(Hash.PLAY_FACIAL_ANIM, Hash), argumentArray1)
        End Sub

        Public Shared Sub PlayPain(ByVal ped As Ped, ByVal type As Integer)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(type, InputArgument), 0, 0 }
            Function.Call(DirectCast(Hash.PLAY_PAIN, Hash), argumentArray1)
        End Sub

        Public Shared Sub Recruit(ByVal ped As Ped, ByVal leader As Ped)
            PedExtended.Recruit(ped, leader, True)
        End Sub

        Public Shared Sub Recruit(ByVal ped As Ped, ByVal leader As Ped, ByVal canBeTargetted As Boolean)
            PedExtended.Recruit(ped, leader, canBeTargetted, False, 100)
        End Sub

        Public Shared Sub Recruit(ByVal ped As Ped, ByVal leader As Ped, ByVal canBeTargeted As Boolean, ByVal invincible As Boolean, ByVal accuracy As Integer)
            If (Not leader Is Nothing) Then
                ped.LeaveGroup
                PedExtended.SetRagdollOnCollision(ped, False)
                ped.get_Task.ClearAll
                Dim group As PedGroup = leader.get_CurrentPedGroup
                group.set_SeparationRange(2.147484E+09!)
                If Not group.Contains(leader) Then
                    group.Add(leader, True)
                End If
                If Not group.Contains(ped) Then
                    group.Add(ped, False)
                End If
                ped.set_CanBeTargetted(canBeTargeted)
                ped.set_Accuracy(accuracy)
                ped.set_IsInvincible(invincible)
                ped.set_IsPersistent(True)
                ped.set_RelationshipGroup(leader.get_RelationshipGroup)
                ped.set_NeverLeavesGroup(True)
                Dim blip1 As Blip = ped.get_CurrentBlip
                If (blip1 Is Nothing) Then
                    Dim local1 As Blip = blip1
                Else
                    blip1.Remove
                End If
                Dim blip As Blip = ped.AddBlip
                blip.set_Color(DirectCast(BlipColor.Blue, BlipColor))
                blip.set_Scale(0.7!)
                blip.set_Name("Friend")
                Dim wrapper As New EntityEventWrapper(DirectCast(ped, Entity))
                AddHandler wrapper.Died, Function (ByVal sender As EntityEventWrapper, ByVal entity As Entity) 
                    Dim blip1 As Blip = entity.get_CurrentBlip
                    If (blip1 Is Nothing) Then
                        Dim local1 As Blip = blip1
                    Else
                        blip1.Remove
                    End If
                    wrapper.Dispose
                End Function
                PedExtended.PlayAmbientSpeech(ped, "GENERIC_HI", SpeechModifier.Standard)
            End If
        End Sub

        Public Shared Sub RemoveElegantly(ByVal ped As Ped)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
            Function.Call(DirectCast(Hash.REMOVE_PED_ELEGANTLY, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetAlertness(ByVal ped As Ped, ByVal alertness As Alertness)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(alertness, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_ALERTNESS, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCanAttackFriendlies(ByVal ped As Ped, ByVal type As FirendlyFireType)
            Dim type2 As FirendlyFireType = type
            If (type2 = FirendlyFireType.CantAttack) Then
                Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), False, False }
                Function.Call(DirectCast(Hash.SET_CAN_ATTACK_FRIENDLY, Hash), argumentArray2)
            ElseIf (type2 = FirendlyFireType.CanAttack) Then
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), True, False }
                Function.Call(DirectCast(Hash.SET_CAN_ATTACK_FRIENDLY, Hash), argumentArray1)
            End If
        End Sub

        Public Shared Sub SetCanEvasiveDive(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_CAN_EVASIVE_DIVE, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCanPlayAmbientAnims(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_CAN_PLAY_AMBIENT_ANIMS, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCombatAblility(ByVal ped As Ped, ByVal ability As CombatAbility)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(ability, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_COMBAT_ABILITY, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCombatAttributes(ByVal ped As Ped, ByVal attribute As CombatAttributes, ByVal enabled As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(attribute, InputArgument), DirectCast(enabled, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_COMBAT_ATTRIBUTES, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCombatMovement(ByVal ped As Ped, ByVal movement As CombatMovement)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(movement, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_COMBAT_MOVEMENT, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetCombatRange(ByVal ped As Ped, ByVal range As CombatRange)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(range, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_COMBAT_RANGE, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetComponentVariation(ByVal ped As Ped, ByVal id As ComponentId, ByVal drawableId As Integer, ByVal textureId As Integer, ByVal paletteId As Integer)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(id, InputArgument), DirectCast(drawableId, InputArgument), DirectCast(textureId, InputArgument), DirectCast(paletteId, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_COMPONENT_VARIATION, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetHearingRange(ByVal ped As Ped, ByVal hearingRange As Single)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(hearingRange, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_HEARING_RANGE, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetMovementAnimSet(ByVal ped As Ped, ByVal animation As String)
            If (Not ped Is Nothing) Then
                Do While True
                    Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(animation, InputArgument) }
                    If Function.Call(Of Boolean)(DirectCast(Hash.HAS_ANIM_SET_LOADED, Hash), argumentArray2) Then
                        Dim argumentArray3 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(animation, InputArgument), &H3E800000 }
                        Function.Call(DirectCast(Hash.SET_PED_MOVEMENT_CLIPSET, Hash), argumentArray3)
                        Exit Do
                    End If
                    Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(animation, InputArgument) }
                    Function.Call(DirectCast(Hash.REQUEST_ANIM_SET, Hash), argumentArray1)
                    Script.Yield
                Loop
            End If
        End Sub

        Public Shared Sub SetPathAvoidFires(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_PATH_AVOID_FIRE, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetPathAvoidWater(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_PATH_PREFER_TO_AVOID_WATER, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetPathCanClimb(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_PATH_CAN_USE_CLIMBOVERS, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetPathCanUseLadders(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.SET_PED_PATH_CAN_USE_LADDERS, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetRagdollOnCollision(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(toggle, InputArgument) }
            Function.Call(DirectCast(Hash.SET_PED_RAGDOLL_ON_COLLISION, Hash), argumentArray1)
        End Sub

        Public Shared Sub SetStealthMovement(ByVal ped As Ped, ByVal toggle As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument(2  - 1) {}
            Dim argumentArray2 As InputArgument() = New InputArgument(2  - 1) {}
            argumentArray2(0) = If(toggle, DirectCast(1, InputArgument), DirectCast(0, InputArgument))
            Dim local1 As InputArgument() = argumentArray2
            local1(1) = "DEFAULT_ACTION"
            Function.Call(DirectCast(Hash.SET_PED_STEALTH_MOVEMENT, Hash), local1)
        End Sub

        Public Shared Sub SetToRagdoll(ByVal ped As Ped, ByVal time As Integer)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), DirectCast(time, InputArgument), 0, 0, 0, 0, 0 }
            Function.Call(DirectCast(Hash.SET_PED_TO_RAGDOLL, Hash), argumentArray1)
        End Sub

        Public Shared Sub StopAmbientSpeechThisFrame(ByVal ped As Ped)
            If PedExtended.IsAmbientSpeechPlaying(ped) Then
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument) }
                Function.Call(DirectCast(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, Hash), argumentArray1)
            End If
        End Sub

        Public Shared Sub StopSpeaking(ByVal ped As Ped, ByVal shaking As Boolean)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(ped.Handle, InputArgument), If(shaking, DirectCast(1, InputArgument), DirectCast(0, InputArgument)) }
            Function.Call(DirectCast(Hash.STOP_PED_SPEAKING, Hash), argumentArray1)
        End Sub


        ' Fields
        Friend Shared ReadOnly SpeechModifierNames As String()
    End Class
End Namespace


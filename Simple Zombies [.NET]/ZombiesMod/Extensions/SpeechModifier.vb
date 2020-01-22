Imports System

Namespace ZombiesMod.Extensions
    Public Enum SpeechModifier
        ' Fields
        Standard = 0
        AllowRepeat = 1
        Beat = 2
        Force = 3
        ForceFrontend = 4
        ForceNoRepeatFrontend = 5
        ForceNormal = 6
        ForceNormalClear = 7
        ForceNormalCritical = 8
        ForceShouted = 9
        ForceShoutedClear = 10
        ForceShoutedCritical = 11
        ForcePreloadOnly = 12
        Megaphone = 13
        Helicopter = 14
        ForceMegaphone = 15
        ForceHelicopter = &H10
        Interrupt = &H11
        InterruptShouted = &H12
        InterruptShoutedClear = &H13
        InterruptShoutedCritical = 20
        InterruptNoForce = &H15
        InterruptFrontend = &H16
        InterruptNoForceFrontend = &H17
        AddBlip = &H18
        AddBlipAllowRepeat = &H19
        AddBlipForce = &H1A
        AddBlipShouted = &H1B
        AddBlipShoutedForce = &H1C
        AddBlipInterrupt = &H1D
        AddBlipInterruptForce = 30
        ForcePreloadOnlyShouted = &H1F
        ForcePreloadOnlyShoutedClear = &H20
        ForcePreloadOnlyShoutedCritical = &H21
        Shouted = &H22
        ShoutedClear = &H23
        ShoutedCritical = &H24
    End Enum
End Namespace


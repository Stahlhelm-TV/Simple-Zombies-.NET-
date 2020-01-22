Imports System

Namespace ZombiesMod.Extensions
    Public Enum Subtask
        ' Fields
        AimedShootingOnFoot = 4
        GettingUp = &H10
        MovingOnFootNoCombat = &H23
        MovingOnFootCombat = &H26
        UsingLadder = &H2F
        Climbing = 50
        GettingOffSomething = &H33
        SwappingWeapon = &H38
        RemovingHelmet = &H5C
        Dead = &H61
        MeleeCombat = 130
        HittingMelee = 130
        SittingInVehicle = 150
        DrivingWandering = &H97
        ExitingVehicle = &H98
        EnteringVehicleGeneral = 160
        EnteringVehicleBreakingWindow = &HA1
        EnteringVehicleOpeningDoor = &HA2
        EnteringVehicleEntering = &HA3
        EnteringVehicleClosingDoor = &HA4
        ExiingVehicleOpeningDoorExiting = &HA7
        ExitingVehicleClosingDoor = &HA8
        DrivingGoingToDestinationOrEscorting = &HA9
        UsingMountedWeapon = &HC7
        AimingThrowable = &H121
        AimingGun = 290
        AimingPreventedByObstacle = &H12B
        InCoverGeneral = &H11F
        InCoverFullyInCover = &H120
        Reloading = &H12A
        RunningToCover = 300
        InCoverTransitionToAimingFromCover = &H12E
        InCoverTransitionFromAimingFromCover = &H12F
        InCoverBlindFire = &H130
        Parachuting = &H14E
        PuttingOffParachute = &H150
        JumpingOrClimbingGeneral = 420
        JumpingAir = &H1A5
        JumpingFinishingJump = &H1A6
    End Enum
End Namespace


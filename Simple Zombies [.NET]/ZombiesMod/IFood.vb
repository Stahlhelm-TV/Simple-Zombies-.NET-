Imports System

Namespace ZombiesMod
    Public Interface IFood
        Inherits IAnimatable
        ' Properties
        Property FoodType As FoodType

        Property RestorationAmount As Single

    End Interface
End Namespace


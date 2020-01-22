Imports GTA
Imports GTA.Native
Imports System
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports ZombiesMod

Namespace ZombiesMod.DataClasses
    Public Class ParticleEffect
        Implements IHandleable, IDeletable
        ' Methods
        Friend Sub New(ByVal handle As Integer)
            Me.<Handle>k__BackingField = handle
        End Sub

        Public Sub Delete()
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(Me.Handle, InputArgument), 1 }
            Function.Call(DirectCast(Hash.REMOVE_PARTICLE_FX, Hash), argumentArray1)
        End Sub

        Public Function Exists() As Boolean
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(Me.Handle, InputArgument) }
            Return Function.Call(Of Boolean)(DirectCast(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, Hash), argumentArray1)
        End Function


        ' Properties
        Public ReadOnly Property Handle As Integer
            <CompilerGenerated> _
            Get
                Return Me.<Handle>k__BackingField
            End Get
        End Property

        Public WriteOnly Property Color As Color
            Set(ByVal value As Color)
                Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(Me.Handle, InputArgument), DirectCast(value.R, InputArgument), DirectCast(value.G, InputArgument), DirectCast(value.B, InputArgument), True }
                Function.Call(DirectCast(Hash.SET_PARTICLE_FX_LOOPED_COLOUR, Hash), argumentArray1)
            End Set
        End Property

    End Class
End Namespace


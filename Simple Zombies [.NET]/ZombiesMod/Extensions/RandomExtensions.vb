Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Extensions
    Friend Class RandomExtensions
        ' Methods
        Public Shared Sub Shuffle(Of T)(ByVal rng As Random, ByVal array As T())
            Dim length As Integer = array.Length
            Do While (length > 1)
                Dim index As Integer = rng.Next(length--)
                Dim local As T = array(length)
                array(length) = array(index)
                array(index) = local
            Loop
        End Sub

    End Class
End Namespace


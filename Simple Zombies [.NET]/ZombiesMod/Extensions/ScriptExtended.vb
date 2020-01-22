Imports GTA.Native
Imports System

Namespace ZombiesMod.Extensions
    Public Class ScriptExtended
        ' Methods
        Public Shared Sub TerminateScriptByName(ByVal name As String)
            Dim argumentArray1 As InputArgument() = New InputArgument() { DirectCast(name, InputArgument) }
            If Function.Call(Of Boolean)(DirectCast(Hash.DOES_SCRIPT_EXIST, Hash), argumentArray1) Then
                Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(name, InputArgument) }
                Function.Call(DirectCast(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, Hash), argumentArray2)
            End If
        End Sub

    End Class
End Namespace


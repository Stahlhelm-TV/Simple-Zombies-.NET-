Imports GTA.Native
Imports System
Imports System.Runtime.InteropServices
Imports ZombiesMod.Static

Namespace ZombiesMod.Extensions
    Public Class UiExtended
        ' Methods
        Public Shared Sub ClearAllHelpText()
            Function.Call(DirectCast(Hash.CLEAR_ALL_HELP_MESSAGES, Hash), New InputArgument(0  - 1) {})
        End Sub

        Public Shared Sub DisplayHelpTextThisFrame(ByVal helpText As String, ByVal Optional ignoreMenus As Boolean = False)
            If Not (Not ignoreMenus AndAlso MenuConrtoller.MenuPool.IsAnyMenuOpen) Then
                Dim argumentArray1 As InputArgument() = New InputArgument() { "CELL_EMAIL_BCON" }
                Function.Call(DirectCast(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, Hash), argumentArray1)
                Dim startIndex As Integer = 0
                Do While True
                    If (startIndex >= helpText.Length) Then
                        Dim argumentArray3 As InputArgument() = New InputArgument(4  - 1) {}
                        argumentArray3(0) = 0
                        argumentArray3(1) = 0
                        argumentArray3(2) = If(Function.Call(Of Boolean)(DirectCast(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED, Hash), New InputArgument(0  - 1) {}), DirectCast(0, InputArgument), DirectCast(1, InputArgument))
                        Dim local1 As InputArgument() = argumentArray3
                        local1(3) = -1
                        Function.Call(DirectCast(Hash.END_TEXT_COMMAND_DISPLAY_HELP, Hash), local1)
                        Exit Do
                    End If
                    Dim argumentArray2 As InputArgument() = New InputArgument() { DirectCast(helpText.Substring(startIndex, Math.Min(&H63, (helpText.Length - startIndex))), InputArgument) }
                    Function.Call(DirectCast(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Hash), argumentArray2)
                    startIndex = (startIndex + &H63)
                Loop
            End If
        End Sub

        Public Shared Function IsAnyHelpTextOnScreen() As Boolean
            Return Function.Call(Of Boolean)(DirectCast(Hash.IS_HELP_MESSAGE_BEING_DISPLAYED, Hash), New InputArgument(0  - 1) {})
        End Function

    End Class
End Namespace


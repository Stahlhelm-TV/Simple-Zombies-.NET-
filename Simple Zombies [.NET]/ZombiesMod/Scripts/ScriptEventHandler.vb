Imports GTA
Imports System
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices

Namespace ZombiesMod.Scripts
    Public Class ScriptEventHandler
        Inherits Script
        ' Methods
        Public Sub New()
            ScriptEventHandler.Instance = Me
            Me._wrapperEventHandlers = New List(Of EventHandler)
            Me._scriptEventHandlers = New List(Of EventHandler)
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
        End Sub

        Private Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.UpdateWrappers(sender, eventArgs)
            Me.UpdateScripts(sender, eventArgs)
        End Sub

        Public Sub RegisterScript(ByVal eventHandler As EventHandler)
            Me._scriptEventHandlers.Add(eventHandler)
        End Sub

        Public Sub RegisterWrapper(ByVal eventHandler As EventHandler)
            Me._wrapperEventHandlers.Add(eventHandler)
        End Sub

        Public Sub UnregisterScript(ByVal eventHandler As EventHandler)
            Me._scriptEventHandlers.Remove(eventHandler)
        End Sub

        Public Sub UnregisterWrapper(ByVal eventHandler As EventHandler)
            Me._wrapperEventHandlers.Remove(eventHandler)
        End Sub

        Private Sub UpdateScripts(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Dim i As Integer = (Me._scriptEventHandlers.Count - 1)
            Do While (i >= 0)
                Dim handler As EventHandler = Me._scriptEventHandlers(i)
                If (Not handler Is Nothing) Then
                    handler.Invoke(sender, eventArgs)
                End If
                i -= 1
            Loop
        End Sub

        Private Sub UpdateWrappers(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Dim num As Integer = Me._index
            Do While True
                If ((num >= (Me._index + 5)) OrElse (num >= Me._wrapperEventHandlers.Count)) Then
                    Me._index = (Me._index + 5)
                    If (Me._index >= Me._wrapperEventHandlers.Count) Then
                        Me._index = 0
                    End If
                    Return
                End If
                Dim handler As EventHandler = Me._wrapperEventHandlers(num)
                If (Not handler Is Nothing) Then
                    handler.Invoke(sender, eventArgs)
                End If
                num += 1
            Loop
        End Sub


        ' Properties
        Property Instance As ScriptEventHandler
            <CompilerGenerated> _
            Public Shared Get
                Return ScriptEventHandler.<Instance>k__BackingField
            End Get
            <CompilerGenerated> _
            Private Shared Set(ByVal value As ScriptEventHandler)
                ScriptEventHandler.<Instance>k__BackingField = value
            End Set
        End Property


        ' Fields
        Private ReadOnly _wrapperEventHandlers As List(Of EventHandler)
        Private ReadOnly _scriptEventHandlers As List(Of EventHandler)
        Private _index As Integer
    End Class
End Namespace


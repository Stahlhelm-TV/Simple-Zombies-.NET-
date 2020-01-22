Imports GTA
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports ZombiesMod.Scripts

Namespace ZombiesMod.Wrappers
    Public Class EntityEventWrapper
        ' Events
        Public Custom Event Aborted As OnWrapperAbortedEvent
            AddHandler(ByVal value As OnWrapperAbortedEvent)
                Dim aborted As OnWrapperAbortedEvent = Me.Aborted
                Do While True
                    Dim a As OnWrapperAbortedEvent = aborted
                    Dim event4 As OnWrapperAbortedEvent = DirectCast(Delegate.Combine(a, value), OnWrapperAbortedEvent)
                    aborted = Interlocked.CompareExchange(Of OnWrapperAbortedEvent)(Me.Aborted, event4, a)
                    If Object.ReferenceEquals(aborted, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnWrapperAbortedEvent)
                Dim aborted As OnWrapperAbortedEvent = Me.Aborted
                Do While True
                    Dim source As OnWrapperAbortedEvent = aborted
                    Dim event4 As OnWrapperAbortedEvent = DirectCast(Delegate.Remove(source, value), OnWrapperAbortedEvent)
                    aborted = Interlocked.CompareExchange(Of OnWrapperAbortedEvent)(Me.Aborted, event4, source)
                    If Object.ReferenceEquals(aborted, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Custom Event Died As OnDeathEvent
            AddHandler(ByVal value As OnDeathEvent)
                Dim died As OnDeathEvent = Me.Died
                Do While True
                    Dim a As OnDeathEvent = died
                    Dim event4 As OnDeathEvent = DirectCast(Delegate.Combine(a, value), OnDeathEvent)
                    died = Interlocked.CompareExchange(Of OnDeathEvent)(Me.Died, event4, a)
                    If Object.ReferenceEquals(died, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnDeathEvent)
                Dim died As OnDeathEvent = Me.Died
                Do While True
                    Dim source As OnDeathEvent = died
                    Dim event4 As OnDeathEvent = DirectCast(Delegate.Remove(source, value), OnDeathEvent)
                    died = Interlocked.CompareExchange(Of OnDeathEvent)(Me.Died, event4, source)
                    If Object.ReferenceEquals(died, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Custom Event Disposed As OnWrapperDisposedEvent
            AddHandler(ByVal value As OnWrapperDisposedEvent)
                Dim disposed As OnWrapperDisposedEvent = Me.Disposed
                Do While True
                    Dim a As OnWrapperDisposedEvent = disposed
                    Dim event4 As OnWrapperDisposedEvent = DirectCast(Delegate.Combine(a, value), OnWrapperDisposedEvent)
                    disposed = Interlocked.CompareExchange(Of OnWrapperDisposedEvent)(Me.Disposed, event4, a)
                    If Object.ReferenceEquals(disposed, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnWrapperDisposedEvent)
                Dim disposed As OnWrapperDisposedEvent = Me.Disposed
                Do While True
                    Dim source As OnWrapperDisposedEvent = disposed
                    Dim event4 As OnWrapperDisposedEvent = DirectCast(Delegate.Remove(source, value), OnWrapperDisposedEvent)
                    disposed = Interlocked.CompareExchange(Of OnWrapperDisposedEvent)(Me.Disposed, event4, source)
                    If Object.ReferenceEquals(disposed, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event
        Public Custom Event Updated As OnWrapperUpdateEvent
            AddHandler(ByVal value As OnWrapperUpdateEvent)
                Dim updated As OnWrapperUpdateEvent = Me.Updated
                Do While True
                    Dim a As OnWrapperUpdateEvent = updated
                    Dim event4 As OnWrapperUpdateEvent = DirectCast(Delegate.Combine(a, value), OnWrapperUpdateEvent)
                    updated = Interlocked.CompareExchange(Of OnWrapperUpdateEvent)(Me.Updated, event4, a)
                    If Object.ReferenceEquals(updated, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnWrapperUpdateEvent)
                Dim updated As OnWrapperUpdateEvent = Me.Updated
                Do While True
                    Dim source As OnWrapperUpdateEvent = updated
                    Dim event4 As OnWrapperUpdateEvent = DirectCast(Delegate.Remove(source, value), OnWrapperUpdateEvent)
                    updated = Interlocked.CompareExchange(Of OnWrapperUpdateEvent)(Me.Updated, event4, source)
                    If Object.ReferenceEquals(updated, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub New(ByVal ent As Entity)
            Me.<Entity>k__BackingField = ent
            ScriptEventHandler.Instance.RegisterWrapper(New EventHandler(AddressOf Me.OnTick))
            AddHandler ScriptEventHandler.Instance.Aborted, (sender, args) => Me.Abort
            EntityEventWrapper.Wrappers.Add(Me)
        End Sub

        Public Sub Abort()
            If (Me.Aborted Is Nothing) Then
                Dim aborted As OnWrapperAbortedEvent = Me.Aborted
            Else
                Me.Aborted.Invoke(Me, Me.Entity)
            End If
        End Sub

        Public Sub Dispose()
            ScriptEventHandler.Instance.UnregisterWrapper(New EventHandler(AddressOf Me.OnTick))
            EntityEventWrapper.Wrappers.Remove(Me)
            If (Me.Disposed Is Nothing) Then
                Dim disposed As OnWrapperDisposedEvent = Me.Disposed
            Else
                Me.Disposed.Invoke(Me, Me.Entity)
            End If
        End Sub

        Public Shared Sub Dispose(ByVal entity As Entity)
            Dim item As EntityEventWrapper = EntityEventWrapper.Wrappers.Find(w => (w.Entity Is entity))
            If (Not item Is Nothing) Then
                item.Dispose
            End If
            EntityEventWrapper.Wrappers.Remove(item)
        End Sub

        Public Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If ((Me.Entity Is Nothing) OrElse Not Me.Entity.Exists) Then
                Me.Dispose
            Else
                Me.IsDead = Me.Entity.IsDead
                If (Me.Updated Is Nothing) Then
                    Dim updated As OnWrapperUpdateEvent = Me.Updated
                Else
                    Me.Updated.Invoke(Me, Me.Entity)
                End If
            End If
        End Sub


        ' Properties
        Public ReadOnly Property Entity As Entity
            <CompilerGenerated> _
            Get
                Return Me.<Entity>k__BackingField
            End Get
        End Property

        Property IsDead As Boolean
            Public Get
                Return Me.Entity.IsDead
            End Get
            Private Set(ByVal value As Boolean)
                If (value AndAlso Not Me._isDead) Then
                    If (Me.Died Is Nothing) Then
                        Dim died As OnDeathEvent = Me.Died
                    Else
                        Me.Died.Invoke(Me, Me.Entity)
                    End If
                End If
                Me._isDead = value
            End Set
        End Property


        ' Fields
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Died As OnDeathEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Aborted As OnWrapperAbortedEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Updated As OnWrapperUpdateEvent
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Disposed As OnWrapperDisposedEvent
        Private Shared ReadOnly Wrappers As List(Of EntityEventWrapper) = New List(Of EntityEventWrapper)
        Private _isDead As Boolean

        ' Nested Types
        Public Delegate Sub OnDeathEvent(ByVal sender As EntityEventWrapper, ByVal entity As Entity)

        Public Delegate Sub OnWrapperAbortedEvent(ByVal sender As EntityEventWrapper, ByVal entity As Entity)

        Public Delegate Sub OnWrapperDisposedEvent(ByVal sender As EntityEventWrapper, ByVal entity As Entity)

        Public Delegate Sub OnWrapperUpdateEvent(ByVal sender As EntityEventWrapper, ByVal entity As Entity)
    End Class
End Namespace


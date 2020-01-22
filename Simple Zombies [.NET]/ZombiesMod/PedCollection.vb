Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Threading

Namespace ZombiesMod
    <Serializable, DefaultMember("Item")> _
    Public Class PedCollection
        Implements IList(Of PedData), ICollection(Of PedData), IEnumerable(Of PedData), IEnumerable
        ' Events
        Public Custom Event ListChanged As ListChangedEvent
            AddHandler(ByVal value As ListChangedEvent)
                Dim listChanged As ListChangedEvent = Me.ListChanged
                Do While True
                    Dim a As ListChangedEvent = listChanged
                    Dim event4 As ListChangedEvent = DirectCast(Delegate.Combine(a, value), ListChangedEvent)
                    listChanged = Interlocked.CompareExchange(Of ListChangedEvent)(Me.ListChanged, event4, a)
                    If Object.ReferenceEquals(listChanged, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As ListChangedEvent)
                Dim listChanged As ListChangedEvent = Me.ListChanged
                Do While True
                    Dim source As ListChangedEvent = listChanged
                    Dim event4 As ListChangedEvent = DirectCast(Delegate.Remove(source, value), ListChangedEvent)
                    listChanged = Interlocked.CompareExchange(Of ListChangedEvent)(Me.ListChanged, event4, source)
                    If Object.ReferenceEquals(listChanged, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub Add(ByVal item As PedData)
            Me._peds.Add(item)
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
        End Sub

        Public Sub Clear()
            Me._peds.Clear
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
        End Sub

        Public Function Contains(ByVal item As PedData) As Boolean
            Return Me._peds.Contains(item)
        End Function

        Public Sub CopyTo(ByVal array As PedData(), ByVal arrayIndex As Integer)
            Me._peds.CopyTo(array, arrayIndex)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of PedData)
            Return Me._peds.GetEnumerator
        End Function

        Public Function IndexOf(ByVal item As PedData) As Integer
            Return Me._peds.IndexOf(item)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal item As PedData)
            Me._peds.Insert(index, item)
        End Sub

        Public Function Remove(ByVal item As PedData) As Boolean
            Dim flag As Boolean = Me._peds.Remove(item)
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
            Return flag
        End Function

        Public Sub RemoveAt(ByVal index As Integer)
            Me._peds.RemoveAt(index)
        End Sub

        Private Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function


        ' Properties
        Public ReadOnly Property Count As Integer
            Get
                Return Me._peds.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean
            Get
                Return Me._peds.IsReadOnly
            End Get
        End Property

        Public Default Property Item(ByVal index As Integer) As PedData
            Get
                Return Me._peds(index)
            End Get
            Set(ByVal value As PedData)
                Me._peds(index) = value
            End Set
        End Property


        ' Fields
        <NonSerialized, CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private ListChanged As ListChangedEvent
        Private ReadOnly _peds As List(Of PedData) = New List(Of PedData)

        ' Nested Types
        Public Delegate Sub ListChangedEvent(ByVal sender As PedCollection)
    End Class
End Namespace


Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Threading

Namespace ZombiesMod
    <Serializable, DefaultMember("Item")> _
    Public Class VehicleCollection
        Implements IList(Of VehicleData), ICollection(Of VehicleData), IEnumerable(Of VehicleData), IEnumerable
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
        Public Sub Add(ByVal item As VehicleData)
            Me._vehicles.Add(item)
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
        End Sub

        Public Sub Clear()
            Me._vehicles.Clear
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
        End Sub

        Public Function Contains(ByVal item As VehicleData) As Boolean
            Return Me._vehicles.Contains(item)
        End Function

        Public Sub CopyTo(ByVal array As VehicleData(), ByVal arrayIndex As Integer)
            Me._vehicles.CopyTo(array, arrayIndex)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of VehicleData)
            Return Me._vehicles.GetEnumerator
        End Function

        Public Function IndexOf(ByVal item As VehicleData) As Integer
            Return Me._vehicles.IndexOf(item)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal item As VehicleData)
            Me._vehicles.Insert(index, item)
        End Sub

        Public Function Remove(ByVal item As VehicleData) As Boolean
            Dim flag As Boolean = Me._vehicles.Remove(item)
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As ListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me)
            End If
            Return flag
        End Function

        Public Sub RemoveAt(ByVal index As Integer)
            Me._vehicles.RemoveAt(index)
        End Sub

        Private Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function


        ' Properties
        Public ReadOnly Property Count As Integer
            Get
                Return Me._vehicles.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean
            Get
                Return Me._vehicles.IsReadOnly
            End Get
        End Property

        Public Default Property Item(ByVal index As Integer) As VehicleData
            Get
                Return Me._vehicles(index)
            End Get
            Set(ByVal value As VehicleData)
                Me._vehicles(index) = value
            End Set
        End Property


        ' Fields
        <NonSerialized, CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private ListChanged As ListChangedEvent
        Private ReadOnly _vehicles As List(Of VehicleData) = New List(Of VehicleData)

        ' Nested Types
        Public Delegate Sub ListChangedEvent(ByVal sender As VehicleCollection)
    End Class
End Namespace


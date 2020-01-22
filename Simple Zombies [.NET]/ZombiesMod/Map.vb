Imports GTA
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Threading

Namespace ZombiesMod
    <Serializable> _
    Public Class Map
        Implements ICollection(Of MapProp), IEnumerable(Of MapProp), IEnumerable
        ' Events
        Public Custom Event ListChanged As OnListChangedEvent
            AddHandler(ByVal value As OnListChangedEvent)
                Dim listChanged As OnListChangedEvent = Me.ListChanged
                Do While True
                    Dim a As OnListChangedEvent = listChanged
                    Dim event4 As OnListChangedEvent = DirectCast(Delegate.Combine(a, value), OnListChangedEvent)
                    listChanged = Interlocked.CompareExchange(Of OnListChangedEvent)(Me.ListChanged, event4, a)
                    If Object.ReferenceEquals(listChanged, a) Then
                        Return
                    End If
                Loop
            End AddHandler
            RemoveHandler(ByVal value As OnListChangedEvent)
                Dim listChanged As OnListChangedEvent = Me.ListChanged
                Do While True
                    Dim source As OnListChangedEvent = listChanged
                    Dim event4 As OnListChangedEvent = DirectCast(Delegate.Remove(source, value), OnListChangedEvent)
                    listChanged = Interlocked.CompareExchange(Of OnListChangedEvent)(Me.ListChanged, event4, source)
                    If Object.ReferenceEquals(listChanged, source) Then
                        Return
                    End If
                Loop
            End RemoveHandler
        End Event

        ' Methods
        Public Sub Add(ByVal item As MapProp)
            Me.Props.Add(item)
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As OnListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me.Props.Count)
            End If
        End Sub

        Public Sub Clear()
            Do While (Me.Props.Count > 0)
                Dim item As MapProp = Me.Props(0)
                item.Delete
                Me.Props.Remove(item)
            Loop
        End Sub

        Public Function Contains(ByVal prop As Prop) As Boolean
            Return (Not Me.Props.Find(m => (m.Handle = prop.Handle)) Is Nothing)
        End Function

        Public Function Contains(ByVal item As MapProp) As Boolean
            Return Me.Props.Contains(item)
        End Function

        Public Sub CopyTo(ByVal array As MapProp(), ByVal arrayIndex As Integer)
            Me.Props.CopyTo(array, arrayIndex)
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of MapProp)
            Return Me.Props.GetEnumerator
        End Function

        Public Sub NotifyListChanged()
            If (Me.ListChanged Is Nothing) Then
                Dim listChanged As OnListChangedEvent = Me.ListChanged
            Else
                Me.ListChanged.Invoke(Me.Count)
            End If
        End Sub

        Public Function Remove(ByVal item As MapProp) As Boolean
            Dim flag2 As Boolean
            If Not Me.Props.Remove(item) Then
                flag2 = False
            Else
                If (Me.ListChanged Is Nothing) Then
                    Dim listChanged As OnListChangedEvent = Me.ListChanged
                Else
                    Me.ListChanged.Invoke(Me.Props.Count)
                End If
                flag2 = True
            End If
            Return flag2
        End Function

        Private Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator
        End Function


        ' Properties
        Public ReadOnly Property Count As Integer
            Get
                Return Me.Props.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean
            Get
        End Property

        ' Fields
        <NonSerialized, CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private ListChanged As OnListChangedEvent
        Public Props As List(Of MapProp) = New List(Of MapProp)
        <CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private ReadOnly <IsReadOnly>k__BackingField As Boolean = False

        ' Nested Types
        Public Delegate Sub OnListChangedEvent(ByVal count As Integer)
    End Class
End Namespace


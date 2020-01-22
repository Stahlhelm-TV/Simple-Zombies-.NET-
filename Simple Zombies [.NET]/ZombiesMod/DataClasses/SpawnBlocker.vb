Imports GTA.Math
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Reflection

Namespace ZombiesMod.DataClasses
    <DefaultMember("Item")> _
    Public Class SpawnBlocker
        Implements IList(Of Vector3), ICollection(Of Vector3), IEnumerable(Of Vector3), IEnumerable
        ' Methods
        Public Sub Add(ByVal item As Vector3)
            Me._blockers.Add(item)
        End Sub

        Public Sub Clear()
            Me._blockers.Clear
        End Sub

        Public Function Contains(ByVal item As Vector3) As Boolean
            Return Me._blockers.Contains(item)
        End Function

        Public Sub CopyTo(ByVal array As Vector3(), ByVal arrayIndex As Integer)
            Me._blockers.CopyTo(array, arrayIndex)
        End Sub

        Public Function FindIndex(ByVal match As Predicate(Of Vector3)) As Integer
            Dim num As Integer
            If Object.ReferenceEquals(match, Nothing) Then
                num = -1
            Else
                Dim num2 As Integer = 0
                Do While True
                    If (num2 >= Me.Count) Then
                        num = -1
                    Else
                        If Not match.Invoke(Me(num2)) Then
                            num2 += 1
                            Continue Do
                        End If
                        num = num2
                    End If
                    Exit Do
                Loop
            End If
            Return num
        End Function

        Public Function GetEnumerator() As IEnumerator(Of Vector3)
            Return Me._blockers.GetEnumerator
        End Function

        Public Function IndexOf(ByVal item As Vector3) As Integer
            Return Me._blockers.IndexOf(item)
        End Function

        Public Sub Insert(ByVal index As Integer, ByVal item As Vector3)
            Me._blockers.Insert(index, item)
        End Sub

        Public Function Remove(ByVal item As Vector3) As Boolean
            Return Me._blockers.Remove(item)
        End Function

        Public Sub RemoveAt(ByVal index As Integer)
            Me._blockers.RemoveAt(index)
        End Sub

        Private Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return Me._blockers.GetEnumerator
        End Function


        ' Properties
        Public ReadOnly Property Count As Integer
            Get
                Return Me._blockers.Count
            End Get
        End Property

        Public ReadOnly Property IsReadOnly As Boolean
            Get
                Return Me._blockers.IsReadOnly
            End Get
        End Property

        Public Default Property Item(ByVal index As Integer) As Vector3
            Get
                Return Me._blockers(index)
            End Get
            Set(ByVal value As Vector3)
                Me._blockers(index) = value
            End Set
        End Property


        ' Fields
        Private ReadOnly _blockers As List(Of Vector3) = New List(Of Vector3)
    End Class
End Namespace


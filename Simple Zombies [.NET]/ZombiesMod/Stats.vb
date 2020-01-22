Imports System
Imports System.Collections.Generic

Namespace ZombiesMod
    <Serializable> _
    Public Class Stats
        ' Methods
        Public Sub New()
            Dim item As New Stat
            item.Name = "Hunger"
            item.Value = 1!
            item.MaxVal = 1!
            Dim list1 As New List(Of Stat)
            list1.Add(item)
            Dim stat2 As New Stat
            stat2.Name = "Thirst"
            stat2.Value = 1!
            stat2.MaxVal = 1!
            list1.Add(stat2)
            Dim stat3 As New Stat
            stat3.Name = "Stamina"
            stat3.Value = 1!
            stat3.MaxVal = 1!
            list1.Add(stat3)
            Me.StatList = list1
        End Sub


        ' Fields
        Public StatList As List(Of Stat)
    End Class
End Namespace


Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class Stat
        ' Properties
        Public Property Name As String
            Get
            Set(ByVal value As String)
        End Property
        Public Property Value As Single
            Get
            Set(ByVal value As Single)
        End Property
        Public Property MaxVal As Single
            Get
            Set(ByVal value As Single)
        End Property
        Public Property Sustained As Boolean
            Get
            Set(ByVal value As Boolean)
        End Property
    End Class
End Namespace


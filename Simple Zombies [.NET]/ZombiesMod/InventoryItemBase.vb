Imports NativeUI
Imports System
Imports System.Runtime.CompilerServices

Namespace ZombiesMod
    <Serializable> _
    Public Class InventoryItemBase
        Implements IIdentifier
        ' Methods
        Public Sub New(ByVal amount As Integer, ByVal maxAmount As Integer, ByVal id As String, ByVal description As String)
            Me.Amount = amount
            Me.MaxAmount = maxAmount
            Me.Id = id
            Me.Description = description
        End Sub

        Public Sub CreateMenuItem()
            Me.MenuItem = New UIMenuItem(Me.Id, Me.Description)
        End Sub


        ' Properties
        Public Property Amount As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property MaxAmount As Integer
            Get
            Set(ByVal value As Integer)
        End Property
        Public Property Id As String
            Get
            Set(ByVal value As String)
        End Property
        Public Property Description As String
            Get
            Set(ByVal value As String)
        End Property

        ' Fields
        <NonSerialized> _
        Public MenuItem As UIMenuItem
    End Class
End Namespace


Imports GTA
Imports NativeUI
Imports System

Namespace ZombiesMod.Static
    Public Class MenuConrtoller
        Inherits Script
        ' Methods
        Public Sub New()
            AddHandler MyBase.Tick, New EventHandler(AddressOf Me.OnTick)
        End Sub

        Public Sub OnTick(ByVal sender As Object, ByVal eventArgs As EventArgs)
            If ((Not MenuConrtoller._barPool Is Nothing) AndAlso ((MenuConrtoller._menuPool Is Nothing) OrElse ((Not MenuConrtoller._menuPool Is Nothing) AndAlso Not MenuConrtoller._menuPool.IsAnyMenuOpen))) Then
                MenuConrtoller._barPool.Draw
            End If
            If (MenuConrtoller._menuPool Is Nothing) Then
                Dim local1 As MenuPool = MenuConrtoller._menuPool
            Else
                MenuConrtoller._menuPool.ProcessMenus
            End If
        End Sub


        ' Properties
        Public Shared ReadOnly Property MenuPool As MenuPool
            Get
                Return If(MenuConrtoller._menuPool <>  Nothing , MenuConrtoller._menuPool, MenuConrtoller._menuPool = New MenuPool)
            End Get
        End Property

        Public Shared ReadOnly Property BarPool As TimerBarPool
            Get
                Return If(MenuConrtoller._barPool <>  Nothing , MenuConrtoller._barPool, MenuConrtoller._barPool = New TimerBarPool)
            End Get
        End Property


        ' Fields
        Private Shared _menuPool As MenuPool
        Private Shared _barPool As TimerBarPool
    End Class
End Namespace


Imports GTA
Imports System
Imports System.IO

Namespace ZombiesMod.Static
    Public Class Config
        ' Methods
        Public Shared Sub Check()
            Dim settings As ScriptSettings = ScriptSettings.Load("./scripts/ZombiesMod.ini")
            If (settings.GetValue("mod", "version_id", "0") <> Config.VersionId) Then
                If File.Exists("./scripts/ZombiesMod.ini") Then
                    File.Delete("./scripts/ZombiesMod.ini")
                End If
                If File.Exists("./scripts/Inventory.dat") Then
                    File.Delete("./scripts/Inventory.dat")
                End If
                UI.Notify(($"Updating Simple Zombies to version ~g~{Config.VersionId}~s~. Overwritting the " & "inventory file since there are new items."))
                settings.SetValue("mod", "version_id", Config.VersionId)
                settings.Save
            End If
        End Sub


        ' Fields
        Public Shared VersionId As String = "1.0.2d"
        Public Const ScriptFilePath As String = "./scripts/"
        Public Const IniFilePath As String = "./scripts/ZombiesMod.ini"
        Public Const InventoryFilePath As String = "./scripts/Inventory.dat"
        Public Const MapFilePath As String = "./scripts/Map.dat"
        Public Const VehicleFilePath As String = "./scripts/Vehicles.dat"
        Public Const GuardsFilePath As String = "./scripts/Guards.dat"
    End Class
End Namespace


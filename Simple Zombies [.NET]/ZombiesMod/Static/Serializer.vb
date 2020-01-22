Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace ZombiesMod.Static
    Public Class Serializer
        ' Methods
        Public Shared Function Deserialize(Of T)(ByVal path As String) As T
            Dim local2 As T
            Dim local As T = CType(Nothing, T)
            If Not File.Exists(path) Then
                local2 = local
            Else
                Try 
                    Dim serializationStream As New FileStream(path, FileMode.Open)
                    local = DirectCast(New BinaryFormatter().Deserialize(serializationStream), T)
                    serializationStream.Close
                Catch exception As Exception
                    File.WriteAllText("./scripts/ZombiesModCrashLog.txt", $"
[{DateTime.UtcNow.ToShortDateString}] {exception.Message}")
                End Try
                local2 = local
            End If
            Return local2
        End Function

        Public Shared Sub Serialize(Of T)(ByVal path As String, ByVal obj As T)
            Try 
                Dim serializationStream As New FileStream(path, FileMode.Create)
                New BinaryFormatter().Serialize(serializationStream, obj)
                serializationStream.Close
            Catch exception As Exception
                File.WriteAllText("./scripts/ZombiesModCrashLog.txt", $"
[{DateTime.UtcNow.ToShortDateString}] {exception.Message}")
            End Try
        End Sub

    End Class
End Namespace


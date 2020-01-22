Imports GTA
Imports System

Namespace ZombiesMod.Static
    Public Class Relationships
        ' Methods
        Public Shared Sub SetRelationshipBothWays(ByVal rel As Relationship, ByVal group1 As Integer, ByVal group2 As Integer)
            World.SetRelationshipBetweenGroups(rel, group1, group2)
            World.SetRelationshipBetweenGroups(rel, group2, group1)
        End Sub

        Public Shared Sub SetRelationships()
            Relationships.InfectedRelationship = World.AddRelationshipGroup("Zombie")
            Relationships.FriendlyRelationship = World.AddRelationshipGroup("Friendly")
            Relationships.MilitiaRelationship = World.AddRelationshipGroup("Private_Militia")
            Relationships.HostileRelationship = World.AddRelationshipGroup("Hostile")
            Relationships.PlayerRelationship = Database.PlayerPed.get_RelationshipGroup
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.InfectedRelationship, Relationships.FriendlyRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.InfectedRelationship, Relationships.MilitiaRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.InfectedRelationship, Relationships.HostileRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.InfectedRelationship, Relationships.PlayerRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.FriendlyRelationship, Relationships.MilitiaRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.FriendlyRelationship, Relationships.HostileRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.HostileRelationship, Relationships.MilitiaRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.HostileRelationship, Relationships.PlayerRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Hate, Relationship), Relationships.PlayerRelationship, Relationships.MilitiaRelationship)
            Relationships.SetRelationshipBothWays(DirectCast(Relationship.Like, Relationship), Relationships.PlayerRelationship, Relationships.FriendlyRelationship)
            Database.PlayerPed.set_IsPriorityTargetForEnemies(True)
        End Sub


        ' Fields
        Public Shared InfectedRelationship As Integer
        Public Shared FriendlyRelationship As Integer
        Public Shared MilitiaRelationship As Integer
        Public Shared HostileRelationship As Integer
        Public Shared PlayerRelationship As Integer
    End Class
End Namespace


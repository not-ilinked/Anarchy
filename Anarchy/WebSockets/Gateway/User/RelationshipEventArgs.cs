namespace Discord.Gateway
{
    public class RelationshipEventArgs
    {
        public DiscordRelationship Relationship { get; private set; }

        public RelationshipEventArgs(DiscordRelationship relationship)
        {
            Relationship = relationship;
        }

        public override string ToString()
        {
            return Relationship.ToString();
        }
    }
}

namespace Discord
{
    public class RelationshipEventArgs
    {
        public Relationship Relationship { get; private set; }


        public RelationshipEventArgs(Relationship relationship)
        {
            Relationship = relationship;
        }


        public override string ToString()
        {
            return Relationship.ToString();
        }
    }
}

namespace DayTwenty
{
    public class PortalEntity : Entity
    {
        public PortalEntity(string identifier, PortalType portalType) : base(identifier, EntityType.Portal)
        {
            PortalType = portalType;
        }

        public PortalType PortalType { get; }
    }
}

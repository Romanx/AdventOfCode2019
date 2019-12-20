using System.Diagnostics;

namespace DayTwenty
{
    [DebuggerDisplay("{Identifier} {Type}")]
    public class Entity
    {
        public static readonly Entity Wall = new Entity("#", EntityType.Wall);
        public static readonly Entity Empty = new Entity(".", EntityType.Empty);

        protected Entity(string identifier, EntityType type)
        {
            Identifier = identifier;
            Type = type;
        }

        public string Identifier { get; }
        public EntityType Type { get; }
    }
}

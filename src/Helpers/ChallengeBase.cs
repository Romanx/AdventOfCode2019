namespace Helpers
{
    public abstract class ChallengeBase
    {
        protected ChallengeBase(int day)
        {
            Day = day;
        }

        public abstract string Name { get; }

        public int Day { get; }
    }
}

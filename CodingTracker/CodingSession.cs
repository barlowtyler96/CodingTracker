namespace CodingTracker
{
    internal class CodingSession
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }
        public String? DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan Duration { get; set; }

    }
}

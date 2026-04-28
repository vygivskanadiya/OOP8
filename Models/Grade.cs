namespace StudentJournal.Models
{
    public class Grade
    {
        private double _value;

        public Subject Subject { get; set; } = null!;

        public double Value
        {
            get => _value;
            set => _value = value is < 0 or > 100
                ? throw new ArgumentOutOfRangeException(nameof(value), "Оцінка має бути від 0 до 100")
                : value;
        }

        public bool IsPassing => Value >= 60;
        public string SubjectName => Subject.Name;

        public override string ToString() => $"{SubjectName}: {Value}";
    }
}

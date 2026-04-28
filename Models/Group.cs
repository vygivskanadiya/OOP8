using System.Collections.ObjectModel;

namespace StudentJournal.Models
{
    public class Group : IEntity
    {
        private string _name = "";

        public int    Id   { get; set; }
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Назва групи не може бути порожньою") : value.Trim();
        }

        public ObservableCollection<Student> Students { get; set; } = new();

        public int    StudentCount  => Students.Count;
        public double GroupAverage  => Students.Count == 0 ? 0
            : Math.Round(Students.Average(s => s.AverageGrade), 2);

        public string GetInfo() =>
            $"Група: {Name} | Студентів: {StudentCount} | Середній бал: {GroupAverage}";

        public override string ToString() => Name;
    }
}

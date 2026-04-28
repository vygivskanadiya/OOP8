using System.Collections.ObjectModel;

namespace StudentJournal.Models
{
    /// <summary>
    /// Клас, що представляє студента.
    /// </summary>
    /// <remarks>
    /// Демонструє такі принципи ООП:
    /// - Успадкування: <c>Student</c> успадковує клас <see cref="Person"/>.
    /// - Композиція: студент "містить" <see cref="Grades"/> (оцінки не існують без студента, видаляються разом з ним).
    /// - Агрегація: містить посилання на <see cref="Group"/> (студент може існувати без групи).
    /// - Поліморфізм: перевизначення абстрактного методу <see cref="GetInfo"/>.
    /// </remarks>
    public class Student : Person
    {
        public Group? Group { get; set; }

        public ObservableCollection<Grade> Grades { get; set; } = new();

        public double AverageGrade
        {
            get
            {
                if (Grades.Count == 0) return 0;
                return Math.Round(Grades.Average(g => g.Value), 2);
            }
        }

        public bool IsPassing => AverageGrade >= 60;

        public override string GetInfo() =>
            $"Студент: {FullName} | Група: {Group?.Name ?? "—"} | Середній бал: {AverageGrade} | {(IsPassing ? "Успішний" : "Неуспішний")}";

        internal bool IsPassingSubject(Subject filterSubject)
        {
            var grade = Grades.FirstOrDefault(g => g.Subject == filterSubject);

            return grade != null && grade.Value >= 60;
        }
    }
}

namespace StudentJournal.Models
{
    /// <summary>
    /// Базовий абстрактний клас, що представляє людину.
    /// </summary>
    /// <remarks>
    /// Демонструє такі принципи ООП:
    /// - Абстракція: реалізовано як абстрактний клас.
    /// - Реалізація: реалізує інтерфейс <see cref="IEntity"/>.
    /// - Інкапсуляція: використовує приватні поля та публічні властивості з валідацією даних.
    /// </remarks>
    public abstract class Person : IEntity
    {
        private string _firstName = "";
        private string _lastName  = "";

        public int Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Ім'я не може бути порожнім") : value.Trim();
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Прізвище не може бути порожнім") : value.Trim();
        }

        public string FullName => $"{LastName} {FirstName}";

        public abstract string GetInfo();

        public override string ToString() => FullName;
    }
}

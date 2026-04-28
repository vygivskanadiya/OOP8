namespace StudentJournal.Models
{
    public class Subject : IEntity
    {
        private string _name = "";

        public int Id  { get; set; }
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Назва предмету не може бути порожньою") : value.Trim();
        }

        public Teacher? Teacher { get; set; }

        public string GetInfo() =>
            $"Предмет: {Name} | Викладач: {Teacher?.FullName ?? "—"}";

        public override string ToString() => Name;
    }
}

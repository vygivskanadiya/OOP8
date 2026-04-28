namespace StudentJournal.Models
{
    /// <summary>
    /// Клас, що представляє викладача.
    /// </summary>
    /// <remarks>
    /// Демонструє такі принципи ООП:
    /// - Успадкування: <c>Teacher</c> успадковує клас <see cref="Person"/>.
    /// - Асоціація: зв'язок між <c>Teacher</c> та <see cref="Subject"/> (викладач веде предмет; обидва існують незалежно — слабкий двосторонній зв'язок).
    /// - Поліморфізм: перевизначення абстрактного методу <see cref="GetInfo"/>.
    /// </remarks>
    public class Teacher : Person
    {
        public Subject? Subject { get; set; }

        public override string GetInfo() =>
            $"Викладач: {FullName} | Предмет: {Subject?.Name ?? "—"}";
    }
}

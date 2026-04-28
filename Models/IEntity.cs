namespace StudentJournal.Models
{
    public interface IEntity
    {
        int Id { get; set; }

        string GetInfo();
    }
}
                                                                            
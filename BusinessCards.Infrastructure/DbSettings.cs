namespace BusinessCards.Infrastructure;

public class DbSettings
{
    public const string Section = "Database";

    public string ConnectionString { get; set; }

    public string DatabaseName { get; set; }

    public string CardsCollectionName { get; set; }
}

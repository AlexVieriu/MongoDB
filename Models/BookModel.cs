namespace Models;

public class BookModel{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public int FrequencyInDays { get; set; }
    public UserModel AssignedTo { get; set; }
    public DateTime? LastCompleted { get; set; }
}
namespace Models;

public class BookHistoryModel{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string BookId { get; set; }
    public string Title { get; set; }
    public DateTime DateCompleted { get; set; }
    public UserModel WhoCompleted { get; set; }

    public BookHistoryModel()
    {

    }

    public BookHistoryModel(BookModel book)
    {
        BookId = book.Id;
        DateCompleted = book.LastCompleted ?? DateTime.Now;
        WhoCompleted = book.AssignedTo;
        Title = book.Title;
    }
}
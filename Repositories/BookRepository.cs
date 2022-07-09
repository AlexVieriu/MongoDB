namespace Repositories;

public class BookRepository
{
    private const string BookCollection = "book";
    private const string BookHistoryCollection = "book_history";

    private readonly NoSql _noSql;

    public BookRepository()
    {
        _noSql = new();
    }

    public async Task<List<BookModel>> GetBooksAsync()
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        var results = await choresCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<BookModel>> GetBooksForUserAsync(UserModel user)
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        var results = await choresCollection.FindAsync(c => c.AssignedTo.Id == user.Id);
        return results.ToList();
    }

    public async Task CreateAsync(BookModel book)
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        await choresCollection.InsertOneAsync(book);
    }

    public async Task UpdateAsync(BookModel book)
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        var filter = Builders<BookModel>.Filter.Eq("Id", book.Id);
        await choresCollection.ReplaceOneAsync(filter, book, new ReplaceOptions { IsUpsert = true });
        // IsUpsert - update, if don't find it then will insert it
    }

    public async Task DeleteAsync(BookModel book)
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        await choresCollection.DeleteOneAsync(c => c.Id == book.Id);
    }

    public async Task CompleteBookAsync(BookModel book)
    {
        var choresCollection = _noSql.ConnectToMongo<BookModel>(BookCollection);
        var filter = Builders<BookModel>.Filter.Eq("Id", book.Id);
        await choresCollection.ReplaceOneAsync(filter, book);

        var choreHistoryCollection = _noSql.ConnectToMongo<BookHistoryModel>(BookHistoryCollection);
        await choreHistoryCollection.InsertOneAsync(new BookHistoryModel(book));
    }

    public async Task CompleteBookWithTransactionAsync(BookModel book)
    {

        var client = new MongoClient("mongodb+srv://admin21:cfzVM4xVB5wsrEqQ@atlasmongodb.ereac1d.mongodb.net/local?retryWrites=true&w=majority");

        using var session = await client.StartSessionAsync();

        session.StartTransaction();
        try
        {            
            var db = client.GetDatabase("bookdb");
            
            var choresCollection = db.GetCollection<BookModel>(BookCollection);
            var filter = Builders<BookModel>.Filter.Eq("Id", book.Id);
            await choresCollection.ReplaceOneAsync(filter, book);

            var choreHistoryCollection = db.GetCollection<BookHistoryModel>(BookHistoryCollection);
            await choreHistoryCollection.InsertOneAsync(new BookHistoryModel(book));

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            System.Console.WriteLine(ex.Message);
        }
    }
}

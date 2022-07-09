namespace Data;

public class ChoreDataAccess
{
    // Connect to Atlas : https://cloud.mongodb.com/v2/62c5e14d4cba3542add2e0d6#clusters
    private const string ConnectionString = "mongodb://localhost:27017";
    // pass = cfzVM4xVB5wsrEqQ
    // private const string ConnectionString = "mongodb+srv://admin21:cfzVM4xVB5wsrEqQ@atlasmongodb.ereac1d.mongodb.net/AtlasMongoDB?retryWrites=true&w=majority";
    private const string DatabaseName = "choredb";
    private const string ChoreCollection = "chore_Chart";
    private const string UserCollection = "users";
    private const string ChoreHistoryCollection = "chore_history";

    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection);
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        var results = await usersCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChoresAsync()
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choresCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChoresForAUserAsync(UserModel user)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choresCollection.FindAsync(c => c.AssignedTo.Id == user.Id);
        return results.ToList();
    }

    public async Task CreateUserAsync(UserModel user)
    {
        var userCollection = ConnectToMongo<UserModel>(UserCollection);
        await userCollection.InsertOneAsync(user);
    }

    public async Task CreateChoreAsync(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        await choresCollection.InsertOneAsync(chore);
    }

    public async Task UpdateChoreAsync(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);
        await choresCollection.ReplaceOneAsync(filter, chore, new ReplaceOptions { IsUpsert = true });
        // IsUpsert - update, if don't find it then will insert it
    }

    public async Task DeleteChoreAsync(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        await choresCollection.DeleteOneAsync(c => c.Id == chore.Id);
    }

    public async Task CompleteChoreAsync(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);
        await choresCollection.ReplaceOneAsync(filter, chore);

        var choreHistoryCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistoryCollection);
        await choreHistoryCollection.InsertOneAsync(new ChoreHistoryModel(chore));
    }
}

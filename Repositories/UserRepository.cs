namespace Repositories;

public class UserRepository
{
    private const string UserCollection = "users";
    private readonly NoSql _noSql;

    public UserRepository()
    {
        _noSql = new();
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        var usersCollection = _noSql.ConnectToMongo<UserModel>(UserCollection);
        var results = await usersCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task CreateUserAsync(UserModel user)
    {
        var userCollection = _noSql.ConnectToMongo<UserModel>(UserCollection);
        await userCollection.InsertOneAsync(user);
    }
}
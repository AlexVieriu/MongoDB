namespace Data;

public class NoSql
{
    // Connect to Atlas : https://cloud.mongodb.com/v2/62c5e14d4cba3542add2e0d6#clusters

    // we need to add a default database(ex:local,admin,config), it not the connection will not work
    private const string ConnectionString = "mongodb+srv://admin21:cfzVM4xVB5wsrEqQ@atlasmongodb.ereac1d.mongodb.net/local?retryWrites=true&w=majority";
    // private const string ConnectionString = "mongodb://localhost:27017";            
    private const string DatabaseName = "choredb";

    public IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection);
    }
}



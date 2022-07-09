// -- Example 2 --
ChoreDataAccess db = new ChoreDataAccess();
await db.CreateUserAsync(new UserModel() { FirstName = "Vieriu", LastName = "Alexandru" });

var users = await db.GetAllUsersAsync();
var chore = new ChoreModel()
{
    AssignedTo = users.First(),
    ChoreText = "Mow the Lawn",
    FrequencyInDays = 7
};

await db.CreateChoreAsync(chore);


// --- Example 1 ---
// string connectionString = "mongodb://localhost:27017";
// string databaseName = "simple_db";
// string collectionName = "people";

// var client = new MongoClient(connectionString);
// var db = client.GetDatabase(databaseName);
// var collection = db.GetCollection<PersonModel>(collectionName);

// var person = new PersonModel { FirstName = "Vieriu", LastName = "Alexandru" };

// await collection.InsertOneAsync(person);

// var results = await collection.FindAsync(_ => true);
// foreach (var result in results.ToList())
//     System.Console.WriteLine($"{result.Id}: {result.FirstName} {result.LastName}");


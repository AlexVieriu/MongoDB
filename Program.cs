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


// -- Example 2 --

UserRepository userRepo = new();
BookRepository bookRepo = new();

await userRepo.CreateUserAsync(new UserModel() { FirstName = "Vieriu", LastName = "Alexandru" });
var users = await userRepo.GetUsersAsync();

var book = new BookModel()
{
    AssignedTo = users.First(),
    Title = "Mow the Lawn",
    FrequencyInDays = 7
};

await bookRepo.CreateAsync(book);

var books = await bookRepo.GetBooksAsync();
var newBook = books.First();
newBook.LastCompleted = DateTime.UtcNow;
await bookRepo.CompleteBookWithTransactionAsync(newBook);

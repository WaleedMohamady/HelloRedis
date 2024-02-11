using HelloRedis;
using StackExchange.Redis;
using System.Text.Json;

Console.WriteLine("Run this command to start redis in docker:");
Console.WriteLine("docker run --name my-redis -p 6379:6379 -d redis");

// Connection to Redis server - make sure it's running in Docker!
var redis = ConnectionMultiplexer.Connect("localhost");
var db = redis.GetDatabase();

var firstUser = new User { Id = 1, Name = "Waleed Ahmed", Email = "WaleedAhmed@WAM.com" };
var secondUser = new User { Id = 2, Name = "Hesham Mostafa", Email = "HeshamMostafa@WAM.com" };


// Serialize the User object to a JSON string.
string firstUserJson = JsonSerializer.Serialize(firstUser);
string secondUserJson = JsonSerializer.Serialize(secondUser);

// Store the serialized User object in Redis with the key "user1, user2".
await db.StringSetAsync("user1", firstUserJson);
await db.StringSetAsync("user2", secondUserJson);


Console.WriteLine("Users object stored in Redis.");

// Fetch the serialized User object from Redis.
string? fetchedFirstUserJson = await db.StringGetAsync("user1");
string? fetchedSecondUserJson = await db.StringGetAsync("user2");

// If not found end program
if (fetchedFirstUserJson is null || fetchedSecondUserJson is null)
{
    Console.WriteLine("User object not found in Redis.");
    return;
}

// Deserialize the JSON string back to a User object.
var fetchedFirstUser = JsonSerializer.Deserialize<User>(fetchedFirstUserJson);
var fetchedSecondUser = JsonSerializer.Deserialize<User>(fetchedSecondUserJson);

// If not found end program
if (fetchedFirstUser is null || fetchedSecondUser is null)
{
    Console.WriteLine($"User object failed to deserialize from json:\n {fetchedFirstUser} & {fetchedSecondUser}.");
    return;
}

Console.WriteLine($"Fetched User: {fetchedFirstUser.Name}, Email: {fetchedFirstUser.Email}");
Console.WriteLine($"Fetched User: {fetchedSecondUser.Name}, Email: {fetchedSecondUser.Email}");


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("If you got this far and you see the user name and email above, it worked!");
Console.ForegroundColor = ConsoleColor.White;
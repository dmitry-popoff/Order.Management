using DbUp;
using System.Reflection;

var connectionString =
       args.FirstOrDefault()
       ?? "Server=localhost;Port=3306;Database=orders_db;User ID=root;Password=password;";

EnsureDatabase.For.MySqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .MySqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;

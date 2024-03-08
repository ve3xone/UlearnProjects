private static string GetGreetingMessage(string name, double salary)
{
    // возвращает "Hello, <name>, your salary is <salary>"
    return $"Hello, {name}, your salary is {Math.Ceiling(salary)}";
}
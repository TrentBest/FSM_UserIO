using TheSingularityWorkshop.FSM_UserIO;

Console.WriteLine("FSM_UserIO Alpha 1 — keyboard adapter and host policy");
Console.WriteLine("Press [S] for settings, [D] for delete-account, or [Q] to quit.");

while (true)
{
    var key = Console.ReadKey(intercept: true).Key;
    Console.WriteLine();

    if (key == ConsoleKey.Q)
        break;

    // This switch is application-owned adapter code, not a UserIO API.
    var intent = key switch
    {
        ConsoleKey.S => new SemanticIntent("open-settings"),
        ConsoleKey.D => new SemanticIntent("delete-account"),
        _ => null
    };

    if (intent is null)
    {
        Console.WriteLine("Ignored unsupported key.");
        continue;
    }

    // A valid intent is still subject to independent host policy.
    if (!HostAccepts(intent))
    {
        Console.WriteLine($"Host denied: {intent.Name}");
        continue;
    }

    Console.WriteLine($"Host accepted: {intent.Name}");
}

static bool HostAccepts(SemanticIntent intent) =>
    intent.Name == "open-settings";

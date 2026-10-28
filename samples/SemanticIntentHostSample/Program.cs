using TheSingularityWorkshop.FSM_UserIO;

if (args.Length == 1 && args[0] == "--self-test")
{
    ProcessKey(ConsoleKey.S, Console.WriteLine);
    ProcessKey(ConsoleKey.D, Console.WriteLine);
    return;
}

if (args.Length != 0)
{
    Console.Error.WriteLine("Usage: SemanticIntentHostSample [--self-test]");
    Environment.ExitCode = 64;
    return;
}

Console.WriteLine("FSM_UserIO Alpha 1 — keyboard adapter and host policy");
Console.WriteLine("Press [S] for settings, [D] for delete-account, or [Q] to quit.");

while (true)
{
    var key = Console.ReadKey(intercept: true).Key;
    Console.WriteLine();
    if (!ProcessKey(key, Console.WriteLine))
        break;
}

static bool ProcessKey(ConsoleKey key, Action<string> write)
{
    if (key == ConsoleKey.Q)
        return false;

    // This switch is application-owned adapter code, not a UserIO API.
    var intent = key switch
    {
        ConsoleKey.S => new SemanticIntent("open-settings"),
        ConsoleKey.D => new SemanticIntent("delete-account"),
        _ => null
    };

    if (intent is null)
    {
        write("Ignored unsupported key.");
        return true;
    }

    // A valid intent is still subject to independent host policy.
    if (!HostAccepts(intent))
    {
        write($"Host denied: {intent.Name}");
        return true;
    }

    write($"Host accepted: {intent.Name}");
    return true;
}

static bool HostAccepts(SemanticIntent intent) =>
    intent.Name == "open-settings";

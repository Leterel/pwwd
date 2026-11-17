using System.Security.Cryptography;

const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*-_?";
const string version = "0.1.0";

int length = 24;

if (args.Length > 0)
{
    if (args.Length == 1 && args[0] is "--help" or "-h")
    {
        Console.WriteLine("pwwd - Secure password generator");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  pwwd [length]");
        Console.WriteLine("  pwwd --length <number>");
        Console.WriteLine("  pwwd --help");
        Console.WriteLine("  pwwd --version");
        return;
    }

    if (args.Length == 1 && args[0] == "--version")
    {
        Console.WriteLine(version);
        return;
    }

    string? value = args.Length switch
    {
        1 => args[0],
        2 when args[0] is "--length" or "-l" => args[1],
        _ => null
    };

    if (!int.TryParse(value, out length) || length < 1 || length > 1024)
    {
        Console.Error.WriteLine("Error: Password length must be between 1 and 1024.");
        Environment.ExitCode = 1;
        return;
    }
}

char[] password = new char[length];

for (int i = 0; i < length; i++)
{
    int index = RandomNumberGenerator.GetInt32(chars.Length);
    password[i] = chars[index];
}

Console.WriteLine(password);

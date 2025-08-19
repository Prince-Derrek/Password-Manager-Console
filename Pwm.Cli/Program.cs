using Pwm;
using System.Linq;

string vaultPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    ".mypwm.vault.json");

var vaultFile = new VaultFile(vaultPath);

while (true)
{
    Console.WriteLine("\nMy Password Manager");
    Console.WriteLine("-------------------");
    Console.WriteLine("1) Init vault");
    Console.WriteLine("2) Add item");
    Console.WriteLine("3) List items");
    Console.WriteLine("4) Get item");
    Console.WriteLine("5) Generate password");
    Console.WriteLine("0) Exit");
    Console.Write("Choose: ");
    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                if (vaultFile.Exists()) { Console.WriteLine("Vault already exists."); break; }
                var mp1 = ReadHidden("Create master password");
                var mp2 = ReadHidden("Confirm master password");
                if (mp1 != mp2) { Console.WriteLine("Mismatch."); break; }
                vaultFile.Init(mp1);
                Console.WriteLine($"Vault created at {vaultPath}");
                break;

            case "2":
                RequireVault();
                {
                    var mp = ReadHidden("Master password");
                    var vault = vaultFile.Load(mp);

                    Console.Write("Title: "); var title = Console.ReadLine() ?? "";
                    Console.Write("URL: "); var url = Console.ReadLine() ?? "";
                    Console.Write("Username: "); var user = Console.ReadLine() ?? "";
                    Console.Write("Generate password? (y/N): ");
                    var gen = (Console.ReadLine() ?? "").Trim().ToLower() == "y";

                    string pass = gen ? PasswordGenerator.Generate(20) : ReadHidden("Password");

                    vault.Items.Add(new VaultItem { Title = title, Url = url, Username = user, Password = pass });
                    vaultFile.Save(mp, vault);
                    Console.WriteLine("Saved.");
                }
                break;

            case "3":
                RequireVault();
                {
                    var mp = ReadHidden("Master password");
                    var vault = vaultFile.Load(mp);
                    foreach (var it in vault.Items.OrderBy(i => i.Title))
                        Console.WriteLine($"- {it.Title} ({it.Url}) as {it.Username}");
                }
                break;

            case "4":
                RequireVault();
                {
                    Console.Write("Title to fetch: ");
                    var wanted = Console.ReadLine() ?? "";

                    var mp = ReadHidden("Master password");
                    var vault = vaultFile.Load(mp);

                    var it = vault.Items.FirstOrDefault(i =>
                        string.Equals(i.Title, wanted, StringComparison.OrdinalIgnoreCase));

                    if (it == null) { Console.WriteLine("Not found."); break; }

                    Console.WriteLine($"Username: {it.Username}");
                    Console.WriteLine($"Password: {it.Password}  (consider copying manually)");
                }
                break;

            case "5":
                Console.Write("Length (default 20): ");
                var txt = Console.ReadLine();
                int length = int.TryParse(txt, out var L) ? L : 20;
                Console.WriteLine(PasswordGenerator.Generate(length));
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Unknown choice.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static string ReadHidden(string label)
{
    Console.Write(label + ": ");
    var sb = new System.Text.StringBuilder();
    ConsoleKeyInfo k;
    while ((k = Console.ReadKey(true)).Key != ConsoleKey.Enter)
    {
        if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
        {
            sb.Remove(sb.Length - 1, 1);
            Console.Write("\b \b");
        }
        else if (!char.IsControl(k.KeyChar))
        {
            sb.Append(k.KeyChar);
            Console.Write("*");
        }
    }
    Console.WriteLine();
    return sb.ToString();
}

void RequireVault()
{
    if (!vaultFile.Exists())
        throw new InvalidOperationException("No vault found. Choose option 1 to init.");
}

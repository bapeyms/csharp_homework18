namespace csharp_homework18;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("--- HOMEWORK MENU ---");
            Console.WriteLine("1) Prime numbers and Fibonacci");
            Console.WriteLine("2) Find and replace a word in a file");
            Console.WriteLine("3) Moderator");
            Console.WriteLine("4) Reverse a file");
            Console.WriteLine("5) Statistics for 100 000 numbers");
            Console.WriteLine("0) Exit");
            Console.Write("Option: ");
            string option = Console.ReadLine() ?? string.Empty;
            
            if (option == "0") break;

            try
            {
                switch (option)
                {
                    case "1": Task1(); break;
                    case "2": Task2(); break;
                    case "3": Task3(); break;
                    case "4": Task4(); break;
                    case "5": Task5(); break;
                    default: Console.WriteLine("Invalid option!"); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }

    static void Task1()
    {
        Random random = new Random();
        List<int> numbers = new List<int>();
        for (int i = 0; i < 100; i++)
            numbers.Add(random.Next(1, 101));
        
        List<int> fibs = new List<int>();
        int a = 1;
        int b = 1;
        while (a <= 100)
        {
            fibs.Add(a);
            int next = a + b;
            a = b;
            b = next;
        }

        List<string> primeLines = new List<string>();
        List<string> fibsLines = new List<string>();

        foreach (int n in numbers)
        {
            if (IsPrime(n))
                primeLines.Add(n.ToString());
            if (fibs.Contains(n))
                fibsLines.Add(n.ToString());
        }
        File.WriteAllLines("prime.txt", primeLines);
        File.WriteAllLines("fibs.txt", fibsLines);
        
        Console.WriteLine($"Generated numbers: {numbers.Count}");
        Console.WriteLine($"Prime numbers: {primeLines.Count}");
        Console.WriteLine($"Fibonacci numbers: {fibsLines.Count}");
    }

    static bool IsPrime(int n)
    {
        if (n < 2)
            return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }

    static void Task2()
    {
        Console.Write("Path to the file: ");
        string path = Console.ReadLine() ?? string.Empty;
        Console.Write("A word to search for: ");
        string search = Console.ReadLine() ?? string.Empty;
        Console.Write("A word to change to: ");
        string change = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrEmpty(search))
        {
            Console.WriteLine("Word cannot be empty!");
            return;
        }
        
        string text = File.ReadAllText(path);
        int count = 0;
        int position = text.IndexOf(search);
        while (position != -1)
        {
            count++;
            position = text.IndexOf(search, position + search.Length);
        }
        string newText = text.Replace(search, change);
        File.WriteAllText(path, newText);
        
        Console.WriteLine($"Search: {search}");
        Console.WriteLine($"Changed: {change}");
        Console.WriteLine($"Changes' count: {count}");
    }

    static void Task3()
    {
        Console.Write("Path to the file with text: ");
        string textPath = Console.ReadLine() ?? string.Empty;
        Console.Write("Path to the file with words for moderation: ");
        string badPath = Console.ReadLine() ?? string.Empty;
        
        string badText = File.ReadAllText(badPath);
        string[] badWords = badText.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        string[] lines =  File.ReadAllLines(textPath);
        int replacesCount = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] words = lines[i].Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                foreach (string bad in badWords)
                {
                    if (words[j] == bad)
                    {
                        words[j] = new string('*', words[j].Length);
                        replacesCount++;
                        break;
                    }
                }
            }
            lines[i] = string.Join(" ", words);
        }
        File.WriteAllLines(textPath, lines);
        Console.WriteLine($"Moderation is over! Replaced words: {replacesCount}");
    }

    static void Task4()
    {
        Console.Write("Path to the file: ");
        string path = Console.ReadLine() ?? string.Empty;
        string text = File.ReadAllText(path);
        char[] chars = text.ToCharArray();
        Array.Reverse(chars);
        string reversed = new string(chars);
        
        string folder = Path.GetDirectoryName(path) ?? string.Empty;
        string name =  Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        string newPath = Path.Combine(folder, name + "_reversed" + extension);
        
        File.WriteAllText(newPath, reversed);
        Console.WriteLine($"Text is reversed! Path to the new file: {newPath}");
    }

    static void Task5()
    {
        string path = "task5.txt";
        if (!File.Exists(path))
        {
            Random random = new Random();
            List<string> gen = new List<string>();
            for (int i = 0; i < 100000; i++)
                gen.Add(random.Next(-99999, 100000).ToString());
            File.WriteAllLines(path, gen);
            Console.WriteLine($"New file was created due to non-existence of task5.txt");
        }

        List<string> positive = new List<string>();
        List<string> negavite = new List<string>();
        List<string> twoDigit = new List<string>();
        List<string> fiveDigit = new List<string>();
        
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            int n = int.Parse(line);
            if (n > 0)
                positive.Add(line);
            if (n < 0)
                negavite.Add(line);
            
            int abs = Math.Abs(n);
            if (abs >= 10 && abs <= 99)
                twoDigit.Add(line);
            if (abs >= 10000 && abs <= 99999)
                fiveDigit.Add(line);
        }
        
        File.WriteAllLines("positive.txt", positive);
        File.WriteAllLines("negavite.txt", negavite);
        File.WriteAllLines("twoDigit.txt", twoDigit);
        File.WriteAllLines("fiveDigit.txt", fiveDigit);
        
        Console.WriteLine($"Amount of numbers: {lines.Length}");
        Console.WriteLine($"Positive numbers: {positive.Count}");
        Console.WriteLine($"Negavite numbers: {negavite.Count}");
        Console.WriteLine($"Two Digits: {twoDigit.Count}");
        Console.WriteLine($"Five Digits: {fiveDigit.Count}");
    }
}
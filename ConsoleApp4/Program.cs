using System;

class Program
{
    static void Main()
    {
        string[] names = { "Rock", "Paper", "Scissors", "Lizard", "Spock" };
        Random rng = new Random();
        int playerScore = 0, agentScore = 0;

        Console.WriteLine("=== Rock Paper Scissors Spock Lizard ===");
        Console.WriteLine("Der spilles 3 runder. Skriv q for at afslutte.\n");

        for (int round = 1; round <= 3; round++)
        {
            Console.Write("Runde {0}: Vælg (0)Rock, (1)Paper, (2)Scissors, (3)Lizard, (4)Spock: ", round);
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "q") return;

            if (!int.TryParse(input, out int p) || p < 0 || p > 4)
            {
                Console.WriteLine("Ugyldigt valg.\n");
                round--; // prøv samme runde igen
                continue;
            }

            int a = rng.Next(0, 5);
            Console.WriteLine($"Du: {names[p]}  |  Agent: {names[a]}");

            if (p == a)
            {
                Console.WriteLine("Uafgjort.\n");
            }
            else if (
                (p == 0 && (a == 2 || a == 3)) || // Rock slår Scissors/Lizard
                (p == 1 && (a == 0 || a == 4)) || // Paper slår Rock/Spock
                (p == 2 && (a == 1 || a == 3)) || // Scissors slår Paper/Lizard
                (p == 3 && (a == 1 || a == 4)) || // Lizard slår Paper/Spock
                (p == 4 && (a == 0 || a == 2))    // Spock slår Rock/Scissors
            )
            {
                playerScore++;
                Console.WriteLine("Du vinder runden!\n");
            }
            else
            {
                agentScore++;
                Console.WriteLine("Agenten vinder runden!\n");
            }

            Console.WriteLine($"Score — Du: {playerScore} | Agent: {agentScore}\n");
        }

        if (playerScore > agentScore) Console.WriteLine("🎉 Du vandt spillet!");
        else if (agentScore > playerScore) Console.WriteLine("🤖 Agenten vandt spillet.");
        else Console.WriteLine("😮 Spillet endte uafgjort!");
    }
}


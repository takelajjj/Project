using System;

public class Class1
{
    public static class LeaderboardData
    {
        // Храним пары Никнейм + Кол-во попыток
        public static List<(string Nick, int Attempts)> Results { get; } = new List<(string, int)>();

        public static void AddResult(string nick, int attempts)
        {
            Results.Add((nick, attempts));
            // Сортировка: меньше попыток — выше
            Results.Sort((a, b) => a.Attempts.CompareTo(b.Attempts));

            // Оставляем только топ-6
            if (Results.Count > 6)
                Results.RemoveRange(6, Results.Count - 6);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public partial class LB
    {
        public static class LeaderboardData
        {
            //ники и попытки
            public static List<(string Nick, int Attempts)> Results { get; } = new List<(string, int)>();

            public static void AddResult(string nick, int attempts)
            {
                Results.Add((nick, attempts));
                //сортировка по кол-ву попыток
                Results.Sort((a, b) => a.Attempts.CompareTo(b.Attempts));

                //топ 6 первых
                if (Results.Count > 6)
                    Results.RemoveRange(6, Results.Count - 6);
            }
        }
    }
}

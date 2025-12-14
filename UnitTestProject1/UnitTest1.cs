using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Setup()
        {
            LB.LeaderboardData.Results.Clear();
        }

        [TestMethod]
        public void AddResult_AddsPlayerToLeaderboard()
        {
            LB.LeaderboardData.AddResult("Player1", 3);

            Assert.AreEqual(1, LB.LeaderboardData.Results.Count);
            Assert.AreEqual("Player1", LB.LeaderboardData.Results[0].Nick);
            Assert.AreEqual(3, LB.LeaderboardData.Results[0].Attempts);
        }

        [TestMethod]
        public void AddResult_SortsAndLimitsToTop6()
        {
            LB.LeaderboardData.AddResult("P1", 5);
            LB.LeaderboardData.AddResult("P2", 3);
            LB.LeaderboardData.AddResult("P3", 6);
            LB.LeaderboardData.AddResult("P4", 2);
            LB.LeaderboardData.AddResult("P5", 4);
            LB.LeaderboardData.AddResult("P6", 1);
            LB.LeaderboardData.AddResult("P7", 7);

            Assert.AreEqual(6, LB.LeaderboardData.Results.Count);

            var attempts = LB.LeaderboardData.Results.Select(r => r.Attempts).ToArray();
            var sortedAttempts = attempts.OrderBy(a => a).ToArray();
            CollectionAssert.AreEqual(sortedAttempts, attempts);

            Assert.IsFalse(LB.LeaderboardData.Results.Any(r => r.Nick == "P7"));
        }
    }   
        
}


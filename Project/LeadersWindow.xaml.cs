using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Project.LB;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для LeadersWindow.xaml
    /// </summary>
    public partial class LeadersWindow : Window
    {
        private List<TextBlock> NickBlocks;
        private List<TextBlock> AttemptsBlocks;

        public LeadersWindow()
        {
            InitializeComponent();

            NickBlocks = new List<TextBlock> { Nick1, Nick2, Nick3, Nick4, Nick5, Nick6 };
            AttemptsBlocks = new List<TextBlock> { Attempt1, Attempt2, Attempt3, Attempt4, Attempt5, Attempt6 };

            UpdateLeaderboard();
        }

        private void UpdateLeaderboard()
        {
            var results = LeaderboardData.Results;

            for (int i = 0; i < 6; i++)
            {
                if (i < results.Count)
                {
                    NickBlocks[i].Text = results[i].Nick;
                    AttemptsBlocks[i].Text = results[i].Attempts.ToString();
                }
                else
                {
                    NickBlocks[i].Text = "";
                    AttemptsBlocks[i].Text = "";
                }
            }
        }

        private void ToMenuFromLeaders(object sender, EventArgs e)
        {
            var menuWin = new MainWindow();
            menuWin.Show();

            menuWin.Left = this.Left;
            menuWin.Top = this.Top;

            this.Hide();
        }

        private void Nick4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

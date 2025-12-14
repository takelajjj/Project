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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GameStart(object sender, EventArgs e)
        {
            string nickname = NicknameTextBox.Text.Trim();
            if(string.IsNullOrEmpty(nickname) )
            {
                MessageBox.Show("Введите свой никнейм!");
                return;
            }


            var gameWin = new GameWindow();
            gameWin.PlayerNick=nickname;
            gameWin.Left = this.Left;
            gameWin.Top = this.Top;
            

            gameWin.Show();

            this.Hide();
        }
        private void GameLeave(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}

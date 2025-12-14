using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Project.LB;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        private TextBox[,] cells = new TextBox[6, 4];
        private int currentRow = 0;
        private int currentColumn = 0;
        private bool _isVirtualKeyboardInput = false;
        private Dictionary<char, Button > keybuttons = new Dictionary<char, Button>();
       

        public string PlayerNick {  get; set; }

        public int Attempts => currentRow ;


        public GameWindow()
        {
            InitializeComponent();
            
            RegisterCells();
            RowAccess();
            RegisterKeyboard();
            FocusCurrentCell();
            

            var rand = new Random();
            targetWord = WordList[rand.Next(WordList.Length)];
            targetWord = targetWord.ToUpper();
            Console.WriteLine($"Debug: загаданное слово: {targetWord}");
        }
        private void RegisterKeyboard()
        {
            foreach (var child in KeyboardGrid.Children)
            {
                if (child is Button btn && btn.Content is string s && s.Length == 1)
                {
                    char letter = s[0];
                    keybuttons[letter] = btn;
                }
            }
        }

        private readonly string[] WordList = new string[]
{
"ДЕНЬ", "ТРЮК", "БИНТ", "ГЛАЗ", "КРЮК", "КЛЮЧ", "СТОЛ", "ВРАЧ", "ВИНТ", "НОЧЬ",
"ГРОМ", "КНУТ", "ГУСЬ", "ПУСК", "КРОТ", "ГРУЗ", "БЛОК", "БОРТ", "СТУК", "БАРС",
"КЛЫК", "СТУЛ", "ГРАЧ", "КРЕМ", "ГРИБ", "ПАРК", "ВЬЮН", "КОНЬ", "ПИРС", "КАРП",
"ЦИРК", "ЛЕНЬ", "БОКС", "ДОЧЬ", "БАНТ", "КЛЕЙ", "ЛИСТ", "СЛЕД", "РИСК", "ВКУС",
"ГНОМ", "ГРАД", "СОЛЬ", "БОЛЬ", "СВЕТ", "ЗМЕЙ", "ЛИФТ", "МРАК", "КЛЁН", "СЛИВ",
"ТИГР", "БАНК", "КЛЮВ", "ВОЛК", "БОРЩ", "КРУГ", "МАТЬ", "МАРС", "ПОСТ", "ПЛОД",
"ПЛАЩ", "РИСК", "ВРУН", "ПЛОД", "РИТМ", "ВРАГ",
"СТЫД", "ФЛОТ", "РЫСЬ", "СЕМЬ", "ФЛАГ", "ПЛОТ", "ПЛОВ", "ХЛЕБ", "СТОН", "СЛОН",
"ЦЕПЬ", "ПЫЛЬ", "ПЯТЬ", "ЦЕЛЬ", "МОРЖ", "ТЕНЬ", "ПЛЕН", "САЙТ", "ТРЮК", "ШАРФ",
"СЕТЬ", "КОНЬ", "СТОГ", "ШРАМ", "МОРС", "ШКАФ", "ЗУБР", "ТОРТ", "ЩЕЛЬ", "СПОР",
"ТЬМА", "ДРУГ", "ПЛЮС", "ПЕЧЬ", "РОСТ", "ПЛЯЖ", "МОСТ", "МЕТР", "МАЗЬ", "ЛОСЬ",
"ЛОЖЬ", "БЛОК", "ЛАНЬ", "КОРМ", "ЛЕНЬ", "КРИК", "КУСТ"
};

        private string targetWord; //слово для отгадывания
        private int maxAttempts = 6;
        private int wordLength = 4;

        private void BackToMenu(object sender, EventArgs e)
        {
            var menuWin = new MainWindow();

            menuWin.Left = this.Left;
            menuWin.Top = this.Top;

            menuWin.Show();

            this.Hide();
        }

      

        private void EnterWord(object sender, EventArgs e) //ввод слова
        {
            string attempt = "";
            for (int c = 0; c < wordLength; c++)
                attempt += cells[currentRow, c].Text.ToUpper();

            if (attempt.Length < wordLength)
            {
                MessageBox.Show("Введите 4 буквы");
                return;
            }

            char[] targetChars = targetWord.ToCharArray();
            char[] attemptChars = attempt.ToCharArray();

            for (int i = 0; i < wordLength; i++)
            {
                var tb = cells[currentRow, i];
                if (attemptChars[i] == targetChars[i])
                {
                    tb.Background = Brushes.LightGreen;
                    targetChars[i] = '*';

                    ColorKeyboard(attemptChars[i], Brushes.LightGreen); //отметка что буква есть
                }
            }

            for (int i = 0; i < wordLength; i++)
            {
                var tb = cells[currentRow, i];
                if (tb.Background == Brushes.LightGreen) continue; //уже обработано



                if (targetChars.Contains(attemptChars[i]))
                {
                    //буква есть но не на месте
                    tb.Background = Brushes.LightBlue;
                    int index = Array.IndexOf(targetChars, attemptChars[i]);
                    ColorKeyboard(attemptChars[i], Brushes.LightBlue);
                    targetChars[index] = '*';
                }
                else
                {
                    //буквы нет в слове
                    tb.Background = Brushes.Gray;
                    ColorKeyboard(attemptChars[i], Brushes.Gray);
                }

            }
            if (attempt == targetWord)
            {
               
                EndGame();
            }

            //переход на следующую строку
            currentRow++;
            currentColumn = 0;

            if (currentRow >= maxAttempts)
            {
                EndGame();
                return;
            }

            FocusCurrentCell();
        }

        private void ColorKeyboard(char letter, Brush color) //покраска клавы
        {
            if (!keybuttons.ContainsKey(letter))
                return;
            var btn = keybuttons[letter];

            
            if (btn.Background == Brushes.LightGreen)
                return;

            if (btn.Background == Brushes.LightBlue && color == Brushes.Gray)
                return;

            btn.Background = color;
        }

        private void RegisterCells() //корды клеток
        {
            cells[0, 0] = Cell_0_0;
            cells[0, 1] = Cell_0_1;
            cells[0, 2] = Cell_0_2;
            cells[0, 3] = Cell_0_3;

            cells[1, 0] = Cell_1_0;
            cells[1, 1] = Cell_1_1;
            cells[1, 2] = Cell_1_2;
            cells[1, 3] = Cell_1_3;

            cells[2, 0] = Cell_2_0;
            cells[2, 1] = Cell_2_1;
            cells[2, 2] = Cell_2_2;
            cells[2, 3] = Cell_2_3;

            cells[3, 0] = Cell_3_0;
            cells[3, 1] = Cell_3_1;
            cells[3, 2] = Cell_3_2;
            cells[3, 3] = Cell_3_3;

            cells[4, 0] = Cell_4_0;
            cells[4, 1] = Cell_4_1;
            cells[4, 2] = Cell_4_2;
            cells[4, 3] = Cell_4_3;

            cells[5, 0] = Cell_5_0;
            cells[5, 1] = Cell_5_1;
            cells[5, 2] = Cell_5_2;
            cells[5, 3] = Cell_5_3;


            foreach (var tb in cells)
            {
                tb.TextChanged += Cell_TextChanged;
                tb.PreviewKeyDown += Cell_PreviewKeyDown;
            }

            FocusCurrentCell();
        }

        private void Cell_TextChanged(object sender, TextChangedEventArgs e) //удаление букв
        {
            if (_isVirtualKeyboardInput) return; 

            if (!(sender is TextBox tb)) return;
            if (tb.Text.Length == 0) return;

            if (currentColumn < wordLength)
            {
                currentColumn++;
                FocusCurrentCell();
            }
        }
        private void EndGame() //конец игры и вывод о выигрыше
        {
            MessageBox.Show($"Игра окончена. Загаданное слово: {targetWord}");
            DisableAllCells();

            LeaderboardData.AddResult(PlayerNick, Attempts);

            var leadersBoardWin = new LeadersWindow();
           
            leadersBoardWin.Show();

            leadersBoardWin.Left = this.Left;
            leadersBoardWin.Top = this.Top;

            this.Close();
        }


        private void Cell_PreviewKeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Back)
            {
                var tb = (TextBox)sender;

                if (tb.Text.Length == 0 && currentColumn > 0)
                {
                    currentColumn--;
                    FocusCurrentCell();
                    e.Handled = true;
                }
            }
        }

        private void FocusCurrentCell()
        {
            if (currentRow >= maxAttempts || currentColumn >= wordLength) return;

            cells[currentRow, currentColumn].Focus();
            cells[currentRow, currentColumn].SelectAll();
        }

        private void DisableAllCells()
        {
            for (int r = 0; r < cells.GetLength(0); r++)
            {
                for (int c = 0; c < cells.GetLength(1); c++)
                {
                    if (cells[r, c] != null)
                        cells[r, c].IsReadOnly = true;

                }
            }
        }

        private void RowAccess()
        {
            for (int r = 0; r < cells.GetLength(0); r++)
            {
                for (int c = 0; c < cells.GetLength(1); c++)
                {
                    if (cells[r, c] != null)
                    {
                        cells[r, c].IsReadOnly = r != currentRow;
                    }
                }
            }
        }

        private void keyboard(object sender, RoutedEventArgs e) //клава
        {
            var btn = sender as Button;
            if (btn == null) return;

            string content = btn.Content.ToString();

            if ((string)btn.Tag == "ENTER")
            {
                EnterWord(this, EventArgs.Empty);
                return;
            }

            if ((string)btn.Tag == "BACK")
            {
                if (currentColumn > 0)
                {
                    currentColumn--;
                    cells[currentRow, currentColumn].Text = "";
                    FocusCurrentCell();
                }
                return;
            }

            if (currentColumn >= wordLength) return;

            _isVirtualKeyboardInput = true;
            cells[currentRow, currentColumn].Text = content;
            _isVirtualKeyboardInput = false;

            currentColumn++;
            FocusCurrentCell();
        }

       
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class GameForm : Form
    {
        private Timer _gameTimer;
        private Snake _snake;
        private Food _food;
        private readonly int _gridSize = 10;

        public GameForm()
        {
            InitializeComponent();
            // Inicializace herního časovače a nastavení intervalu na 100 ms
            _gameTimer = new Timer();
            _gameTimer.Interval = 100;
            _gameTimer.Tick += GameLoop;

            // Vytvoření nového hada a nastavení směru
            _snake = new Snake(new Point(20, 20), new Point(_gridSize, 0));

            // Vytvoření prvního jídla na náhodné pozici
            Random random = new Random();
            _food = new Food(new Point(random.Next(0, pbCanvas.Width / _gridSize) * _gridSize, random.Next(0, pbCanvas.Height / _gridSize) * _gridSize));
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Pohyb hada
            _snake.Move();

            // Kontrola kolize hada s jídlem
            if (_snake.CollidesWith(_food.Position))
            {
                // Přidání nového těla hadovi a vytvoření nového jídla
                _snake.Body.Add(_snake.Body[_snake.Body.Count - 1]);
                Random random = new Random();
                _food.Position = new Point(random.Next(0, pbCanvas.Width / _gridSize) * _gridSize, random.Next(0, pbCanvas.Height / _gridSize) * _gridSize);
            }

            // Kontrola kolize hada s okraji hracího pole nebo sám se sebou
            if (_snake.IsOutOfBounds(0, 0, pbCanvas.Width, pbCanvas.Height) || CollidesWithSelf())
            {
                // Hra končí, zastaví se časovač a zobrazí se výzva k restartu
                _gameTimer.Stop();
                MessageBox.Show("Konec hry, klikni OK pro restart");
                _snake = new Snake(new Point(20, 20), new Point(_gridSize, 0));
            }

            // Redraw herní pole a hada
            pbCanvas.Invalidate();
        }

        private bool CollidesWithSelf()
        {
            // Kontroluje, zda se had sám sebou koliduje
            for (int i = 1; i < _snake.Body.Count; i++)
            {
                if (_snake.Body[i] == _snake.Body[0])
                {
                    return true;
                }
            }

            return false;
        }

        private void pbCanvas_Paint_1(object sender, PaintEventArgs e)
        {
            // Vykreslení hada a jídla
            for (int i = 0; i < _snake.Body.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Black, new Rectangle(_snake.Body[i].X, _snake.Body[i].Y, _gridSize, _gridSize));
            }
            e.Graphics.FillRectangle(Brushes.Red, new Rectangle(_food.Position.X, _food.Position.Y, _gridSize, _gridSize));
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Ovládání hada pomocí klávesnicových šipek
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (_snake.Direction.X != _gridSize)
                    {
                        _snake.Direction = new Point(-_gridSize, 0);
                    }
                    break;
                case Keys.Right:
                    if (_snake.Direction.X != -_gridSize)
                    {
                        _snake.Direction = new Point(_gridSize, 0);
                    }
                    break;
                case Keys.Up:
                    if (_snake.Direction.Y != _gridSize)
                    {
                        _snake.Direction = new Point(0, -_gridSize);
                    }
                    break;
                case Keys.Down:
                    if (_snake.Direction.Y != -_gridSize)
                    {
                        _snake.Direction = new Point(0, _gridSize);
                    }
                    break;
            }
        }
    }
}
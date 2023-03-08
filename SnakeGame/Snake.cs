using System.Collections.Generic;
using System.Drawing;

namespace SnakeGame
{
    public class Snake
    {
        public List<Point> Body = new List<Point>();
        public Point Direction;

        public Snake(Point head, Point direction)
        {
            Body.Add(head);
            Direction = direction;
        }

        public void Move()
        {
            // Přidá novou hlavu na správnou pozici
            Body.Insert(0, new Point(Body[0].X + Direction.X, Body[0].Y + Direction.Y));

            // Smaže poslední část hada (ocas)
            Body.RemoveAt(Body.Count - 1);
        }

        public bool CollidesWith(Point point)
        {
            // Kontroluje, zda had koliduje s určitým bodem (jídlem)
            foreach (var bodyPart in Body)
            {
                if (bodyPart == point)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOutOfBounds(int minX, int minY, int maxX, int maxY)
        {
            // Kontroluje, zda had nenarazil do okrajů hracího pole
            return Body[0].X < minX || Body[0].Y < minY || Body[0].X >= maxX || Body[0].Y >= maxY;
        }
    }

    public class Food
    {
        public Point Position;

        public Food(Point position)
        {
            Position = position;
        }
    }


}
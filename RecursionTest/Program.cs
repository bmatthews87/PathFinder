using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecursionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //set window properties
            Console.CursorVisible = false;
            Console.WindowHeight = 20;
            Console.WindowWidth = 20;

            while (true)
            {

                //define start and end points
                point Start = new point(2, 3);
                point End = new point(15, 6);

                //genereate grid
                point[,] someGrid = GenerateGrid(20, 20);

                //make walls
                for (int i = 1; i < 10; i++)
                {
                    someGrid[10, i].isWall = true;
                }

                //find shortest path
                List<point> path = FindShortestPath(someGrid, Start, End);

                //draw grid
                DrawGrid(someGrid);

                //draw destination
                Console.SetCursorPosition(End.x, End.y);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("*");

                //draw path
                int StepCounter = 0;

                foreach (point p in path)
                {
                    Console.SetCursorPosition(p.x, p.y);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("*");

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, 0);
                    StepCounter++;
                    Console.Write("Steps:{0}", StepCounter);

                    Thread.Sleep(200);
                }
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
            }
        }

        /// <summary>
        /// Struct to define point
        /// </summary>
        public struct point
        {
            public int x;
            public int y;
            public bool visited;
            public bool isWall;

            public point(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.visited = false;
                this.isWall = false;
            }

            public point(int x, int y, bool visited, bool isWall)
            {
                this.x = x;
                this.y = y;
                this.visited = visited;
                this.isWall = isWall;
            }
        }

        /// <summary>
        /// Method to return shortest path
        /// </summary>
        /// <param name="Grid"></param>
        /// <param name="StartLocation"></param>
        /// <param name="EndLocation"></param>
        public static List<point> FindShortestPath(point[,] Grid, point StartLocation, point EndLocation)
        {
            /***********Calculate G/H/F values*******************************************************************************
            G = Movement cost to move from Current Position to Given Square (Diagonal = 14, Horizontal/Vertical = 10)
            H = Estimated Movement cost to move from Given Square to Destination (using Distance Formula) c = sqrt((y1-y2)^2 + (x1-x2)^2)
            F = G + H
            ****************************************************************************************************************/
            const int ADJACENT_COST = 10;
            const int DIAGONAL_COST = 14;


            bool ReachedDestination = false;
            List<point> Path = new List<point>(); //use this to hold selected nodes that had the lowest F
            point CurrentPosition = StartLocation; //init current position to start location and mark it as visited
            Grid[CurrentPosition.x, CurrentPosition.y].visited = true;

            //Keep walking nodes until we get to the end position
            while (ReachedDestination == false)
            {
                //Calculate F values for adjacent nodes if: 1) They exist 2) Are not walls 3) Are not visited

                Dictionary<point, double> AdjacentCells = new Dictionary<point, double>();
                //top left: (x-1, y+1)
                if (CurrentPosition.y + 1 >= 0 && CurrentPosition.x - 1 >= 0 && Grid[CurrentPosition.x - 1, CurrentPosition.y + 1].isWall == false && Grid[CurrentPosition.x - 1, CurrentPosition.y + 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y + 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x - 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x - 1, CurrentPosition.y + 1), DIAGONAL_COST + distance);
                }
                //top middle: (x-0, y+1)
                if (CurrentPosition.y + 1 >= 0 && CurrentPosition.x - 0 >= 0 && Grid[CurrentPosition.x - 0, CurrentPosition.y + 1].isWall == false && Grid[CurrentPosition.x - 0, CurrentPosition.y + 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y + 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x - 0 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x - 0, CurrentPosition.y + 1), ADJACENT_COST + distance);
                }
                //top right: (x+1, y+1)
                if (CurrentPosition.y + 1 >= 0 && CurrentPosition.x + 1 >= 0 && Grid[CurrentPosition.x + 1, CurrentPosition.y + 1].isWall == false && Grid[CurrentPosition.x + 1, CurrentPosition.y + 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y + 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x + 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x + 1, CurrentPosition.y + 1), DIAGONAL_COST + distance);
                }
                //middle left: (x-1, y+0)
                if (CurrentPosition.y + 0 >= 0 && CurrentPosition.x - 1 >= 0 && Grid[CurrentPosition.x - 1, CurrentPosition.y + 0].isWall == false && Grid[CurrentPosition.x - 1, CurrentPosition.y + 0].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y + 0 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x - 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x - 1, CurrentPosition.y + 0), ADJACENT_COST + distance);
                }
                //middle right: (x+1, y+0)
                if (CurrentPosition.y + 0 >= 0 && CurrentPosition.x + 1 >= 0 && Grid[CurrentPosition.x + 1, CurrentPosition.y + 0].isWall == false && Grid[CurrentPosition.x + 1, CurrentPosition.y + 0].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y + 0 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x + 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x + 1, CurrentPosition.y + 0), ADJACENT_COST + distance);
                }
                //bottom left: (x-1, y-1)
                if (CurrentPosition.y - 1 >= 0 && CurrentPosition.x - 1 >= 0 && Grid[CurrentPosition.x - 1, CurrentPosition.y - 1].isWall == false && Grid[CurrentPosition.x - 1, CurrentPosition.y - 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y - 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x - 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x - 1, CurrentPosition.y - 1), DIAGONAL_COST + distance);
                }
                //bottom middle: (x+0, y-1)
                if (CurrentPosition.y - 1 >= 0 && CurrentPosition.x - 0 >= 0 && Grid[CurrentPosition.x + 0, CurrentPosition.y - 1].isWall == false && Grid[CurrentPosition.x + 0, CurrentPosition.y - 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y - 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x + 0 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x + 0, CurrentPosition.y - 1), ADJACENT_COST + distance);
                }
                //bottom right: (x+1, y-1)
                if (CurrentPosition.y - 1 >= 0 && CurrentPosition.x + 1 >= 0 && Grid[CurrentPosition.x + 1, CurrentPosition.y - 1].isWall == false && Grid[CurrentPosition.x + 1, CurrentPosition.y - 1].visited == false)
                {
                    double distance = Math.Sqrt((Math.Pow((CurrentPosition.y - 1 - EndLocation.y), 2) + Math.Pow((CurrentPosition.x + 1 - EndLocation.x), 2)));
                    AdjacentCells.Add(new point(CurrentPosition.x + 1, CurrentPosition.y - 1), DIAGONAL_COST + distance);
                }

                //after evaluating all points, set current position to point with lowest F and add to path
                CurrentPosition = AdjacentCells.OrderBy(kvp => kvp.Value).First().Key;
                Grid[CurrentPosition.x, CurrentPosition.y].visited = true;
                Path.Add(CurrentPosition);

                //clear out entries for next pass
                AdjacentCells.Clear();

                if (CurrentPosition.x == EndLocation.x && CurrentPosition.y == EndLocation.y)
                {
                    ReachedDestination = true;
                }
            }
            return Path;
        }

        /// <summary>
        /// Generate 2D Grid
        /// </summary>
        /// <param name="GridSizeX"></param>
        /// <param name="GridSizeY"></param>
        /// <returns></returns>
        public static point[,] GenerateGrid(int GridSizeX, int GridSizeY)
        {
            //Instance to handle randomization of walls
            Random rand = new Random();

            //Generate Grid Points
            point[,] GeneratedGrid = new point[GridSizeX, GridSizeY];
            for (int i = 0; i < GridSizeX; i++)
            {
                for (int j = 0; j < GridSizeY; j++)
                {
                    GeneratedGrid[i, j].x = i;
                    GeneratedGrid[i, j].y = j;
                    GeneratedGrid[i, j].visited = false;
                    //GeneratedGrid[i, j].isWall = RandomWall(rand);
                }
            }
            return GeneratedGrid;
        }

        /// <summary>
        /// Draws 2D Grid of Points
        /// </summary>
        /// <param name="Grid"></param>
        public static void DrawGrid(point[,] Grid)
        {
            foreach (point p in Grid)
            {
                Console.SetCursorPosition(p.x, p.y);
                if (p.isWall == true)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("*");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("|");
                }
            }
        }

        /// <summary>
        /// Returns Random Wall Bool Value
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static bool RandomWall(Random rand)
        {
            int roll = rand.Next(1, 100);
            //if even number, then retrun true for a wall, otherwise return false
            if ((roll % 2) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

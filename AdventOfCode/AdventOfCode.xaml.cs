using System;
using System.Windows;

namespace AdventOfCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ISolver Solver;
                ISolverFactory SolverFactory;
                switch (day.SelectedIndex)
                {
                    case 0:
                        SolverFactory = new Day1.SolverFactory();
                        break;
                    case 1:
                        SolverFactory = new Day2.SolverFactory();
                        break;
                    default:
                        throw new Exception("Choose day you want to get solution for.");
                }

                int selectedPart;
                switch (part.SelectedIndex)
                {
                    case 0:
                        selectedPart = 1;
                        break;
                    case 1:
                        selectedPart = 2;
                        break;
                    default:
                        throw new Exception("Choose part you want to get solution of day X for.");
                }

                if (inputData.Text == "")
                {
                    throw new Exception("Enter input data you want to get solution for.");
                }

                solution.Text = SolverFactory.getSolver(selectedPart).getSolution(inputData.Text).ToString();
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
        }
    }

    public interface ISolver
    {
        int getSolution(string inputData);
    }

    public interface ISolverFactory
    {
        ISolver getSolver(int part);
    }

    namespace Day1
    {
        public class SolverFactory : ISolverFactory
        {
            public ISolver getSolver(int part)
            {
                ISolver Solver;
                switch (part)
                {
                    case 1:
                        Solver = new Part1();
                        break;
                    case 2:
                        Solver = new Part2();
                        break;
                    default:
                        throw new Exception("Wrong part.");
                }

                return Solver;
            }
        }

        abstract public class Solver : ISolver
        {
            protected const char FLOOR_UP = '(';
            protected const char FLOOR_DOWN = ')';

            abstract public int getSolution(string inputData);
        }

        public class Part1 : Solver
        {
            public override int getSolution(string inputData)
            {
                int floor = 0;
                foreach (char c in inputData)
                {
                    switch (c)
                    {
                        case FLOOR_UP:
                            floor++;
                            break;
                        case FLOOR_DOWN:
                            floor--;
                            break;
                    }
                }

                return floor;
            }
        }

        public class Part2 : Solver
        {
            public override int getSolution(string inputData)
            {
                int floor = 0;
                int step = 0;
                bool inBasement = false;

                foreach (char c in inputData)
                {
                    switch (c)
                    {
                        case FLOOR_UP:
                            floor++;
                            break;
                        case FLOOR_DOWN:
                            floor--;
                            break;
                    }

                    step++;

                    if (floor < 0)
                    {
                        inBasement = true;
                        break;
                    }
                }

                if (!inBasement)
                {
                    step = -1;
                }

                return step;
            }
        }
    }

    namespace Day2
    {
        public class SolverFactory : ISolverFactory
        {
            public ISolver getSolver(int part)
            {
                ISolver Solver;
                switch (part)
                {
                    case 1:
                        Solver = new Part1();
                        break;
                    case 2:
                        Solver = new Part2();
                        break;
                    default:
                        throw new Exception("Wrong part.");
                }

                return Solver;
            }
        }

        abstract public class Solver : ISolver
        {
            public int getSolution(string inputData)
            {
                var amount = 0;

                // each line contains one box definition
                string[] boxes = inputData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string boxDimensions in boxes)
                {
                    var box = new Box(boxDimensions);
                    amount += getAmountForBox(box);
                }

                return amount;
            }

            protected int getAmountForBox(Box box)
            {
                var wrap = getWrapAmount(box);
                var extra = getExtraAmount(box);

                return wrap + extra;
            }

            abstract protected int getWrapAmount(Box box);
            abstract protected int getExtraAmount(Box box);
        }

        public class Part1 : Solver
        {
            protected override int getWrapAmount(Box box)
            {
                return (2 * box.width * box.length) + (2 * box.width * box.height) + (2 * box.length * box.height);
            }

            protected override int getExtraAmount(Box box)
            {
                return box.width * box.length;
            }
        }

        public class Part2 : Solver
        {
            protected override int getWrapAmount(Box box)
            {
                return (2 * box.width) + (2 * box.length);
            }

            protected override int getExtraAmount(Box box)
            {
                return box.width * box.length * box.height;
            }
        }

        public class Box
        {
            private const char DIMENSIONS_SEPARATOR = 'x';

            public int length { get; }
            public int width { get; }
            public int height { get; }

            public Box(string sizes)
            {
                // split string in form of LENxWIDxHEI into string sizes
                string[] parts = sizes.Split(new char[] { DIMENSIONS_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                {
                    throw new ArgumentException("Invalid box sizes: '" + sizes + "'");
                }

                // create array of integers
                var dimensions = new int[3];
                for (var i = 0; i < 3; i++)
                {
                    dimensions[i] = int.Parse(parts[i]);
                }
                Array.Sort(dimensions);

                width = dimensions[0];
                length = dimensions[1];
                height = dimensions[2];
            }
        }
    }
}

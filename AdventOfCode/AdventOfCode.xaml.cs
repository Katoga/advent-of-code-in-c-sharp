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
                SolverInterface Solver;
                SolverFactoryInterface SolverFactory;
                switch (day.SelectedIndex)
                {
                    case 0:
                        SolverFactory = new Day1.SolverFactory();
                        break;
                    default:
                        throw new System.Exception("Choose day you want to get solution for.");
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
                        throw new System.Exception("Choose part you want to get solution of day X for.");
                }

                if (inputData.Text == "")
                {
                    throw new System.Exception("Enter input data you want to get solution for.");
                }

                solution.Text = SolverFactory.getSolver(selectedPart).getSolution(inputData.Text).ToString();
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message, "info");
            }
        }
    }

    public interface SolverInterface
    {
        int getSolution(string inputData);
    }

    public interface SolverFactoryInterface
    {
        SolverInterface getSolver(int part);
    }

    namespace Day1
    {
        public class SolverFactory : SolverFactoryInterface
        {
            public SolverInterface getSolver(int part)
            {
                SolverInterface Solver;
                switch (part)
                {
                    case 1:
                        Solver = new Part1();
                        break;
                    case 2:
                        Solver = new Part2();
                        break;
                    default:
                        throw new System.Exception("Wrong part.");
                }

                return Solver;
            }
        }

        abstract public class Solver : AdventOfCode.SolverInterface
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
}

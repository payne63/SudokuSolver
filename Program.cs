using Raylib_cs;
using System.Diagnostics;

namespace SudokuSolver;

class Program {
    private static readonly Grid Grid = new Grid();
    private static readonly RaylibDrawer RaylibDrawer = new RaylibDrawer();
    private static readonly Stopwatch Stopwatch = new Stopwatch();
    private static float _elapsedTime = 0f;
    private static List<float>? _elapsedTimes = [];
    private static List<Cell> _cellsToFind = [];
    private static int difficulty = 0;
    private static int [,] originalGrid = new int [9, 9];

    [STAThread]
    private static void Main(string[] args) {
        Raylib.InitWindow(1280, 1024, "SudokuSolver");
        
        while (!Raylib.WindowShouldClose()) {
            RaylibDrawer.Draw((() => {
                // var index = 0;
                // foreach (var cell in _cellsToFind) {
                //     Raylib.DrawText(cell.ToString(),RaylibDrawer.TextSize*9,index,15, Color.White);
                //     index+=30;
                // }
                Raylib.DrawText($"difficulty: {difficulty.ToString()}",RaylibDrawer.TextSize*9,10,20, Color.Black);
                RaylibDrawer.DrawGridWithStats(originalGrid,Color.LightGray);
                RaylibDrawer.DrawGridWithStats(Grid.GridValues,Color.Black,_elapsedTime,_elapsedTimes,_cellsToFind);
                ;}));
            
            if (Raylib.IsKeyPressed(KeyboardKey.B)) Benchmark();
            if (Raylib.IsKeyPressed(KeyboardKey.Enter)) {
                difficulty = 0;
                Grid.DeleteValue();
                _cellsToFind.Clear();
                Grid.FillAllBlocs();
                Array.Copy(Grid.GridValues, originalGrid, Grid.GridValues.Length);
                Grid.RemoveNumbers(45);
                _cellsToFind = Grid.GetFreeCells();
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.Down)) {
                if (!SolveCells()) {
                    Console.WriteLine("No solution found");
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Up)) {
                if (_cellsToFind.Count==0) return;
                var isNoneOneSolution = false;
                while (!isNoneOneSolution) {
                    var actualCellToFind = _cellsToFind.First();
                    if (actualCellToFind.PossibleValue.Count == 0) throw new Exception("no solution????");
                    if (actualCellToFind.PossibleValue.Count == 1) {
                        Grid.GridValues[actualCellToFind.Position.x, actualCellToFind.Position.y] = actualCellToFind.PossibleValue[0];
                        _cellsToFind.Remove(actualCellToFind);
                        if (_cellsToFind.Count == 0) {
                            Console.WriteLine("sudoku solved");
                            break;
                        }
                    }
                    else {
                        isNoneOneSolution = true;
                    }
                }
            }
        }
        Raylib.CloseWindow();
    }

    private static bool SolveCells()
    {
        if (_cellsToFind.Count==0) return true;
        // primary search
        foreach (var cell in _cellsToFind) {
            for (var i = 0; i < 9; i++) {
                // horizontal
                var valueHorizontal = Grid.GridValues[i, cell.Position.y];
                difficulty += cell.PossibleValue.Remove(valueHorizontal)?1:0 ;
                // vertical
                var valueVertical = Grid.GridValues[cell.Position.x, i];
                difficulty += cell.PossibleValue.Remove(valueVertical)?1:0;
                // in a bloc
                foreach (var valueTuple in Grid.GetLocalBloc((cell.Position.x, cell.Position.y))) { 
                    difficulty += cell.PossibleValue.Remove(valueTuple.value)?1:0;
                }
            }
        }
        //complex search
        var groupByBloc = new List<((int, int), Cell)>();
        foreach (var cell in _cellsToFind) {
            var groups = _cellsToFind.GroupBy(c => c.GetLocalGridPosition()
            );
            foreach (var group in groups) {
                for (var nb = 1; nb < 10; nb++) {
                    var verifyNumber = new List<Cell>();
                    foreach (var LocalCell in group) {
                        if (LocalCell.PossibleValue.Contains(nb)) {
                            verifyNumber.Add(LocalCell);
                        }
                    }

                    if (verifyNumber.Count == 1) {
                        verifyNumber[0].PossibleValue.Clear();
                        verifyNumber[0].PossibleValue.Add(nb);
                        difficulty += 3;
                    }
                }

            }
        
        }
            
        _cellsToFind = _cellsToFind.OrderBy(x => x.PossibleValue.Count).ToList();
        return _cellsToFind.First().PossibleValue.Count <= 1;
    }

    private static void Benchmark() {
        if (_elapsedTimes != null) {
            _elapsedTimes = new List<float>();
        }
        for (int i = 0; i < 1000; i++) {
            Grid.DeleteValue();
            Stopwatch.Start();
            Grid.FillAllBlocs();
            Stopwatch.Stop();
            _elapsedTime = Stopwatch.ElapsedMilliseconds;
            _elapsedTimes.Add(_elapsedTime);
            Stopwatch.Reset();
        }
    }
}
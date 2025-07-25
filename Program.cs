using Raylib_cs;
using System.Diagnostics;

namespace SudokuSolver;

class Program {
    private static readonly Grid Grid = new Grid();
    private static readonly RaylibDrawer RaylibDrawer = new RaylibDrawer();
    private static readonly Stopwatch Stopwatch = new Stopwatch();
    private static float _elapsedTime = 0f;
    private static List<float>? _elapsedTimes = [];
    private static List<Cell> _cells = [];

    [STAThread]
    private static void Main(string[] args) {
        Raylib.InitWindow(1280, 1024, "SudokuSolver");
        
        while (!Raylib.WindowShouldClose()) {
            RaylibDrawer.Draw((() => {
                var index = 0;
                foreach (var cell in _cells) {
                    Raylib.DrawText(cell.ToString(),400,index,20, Color.White);
                    index+=30;
                }
                RaylibDrawer.DrawGridWithStats(Grid.GridValues,_elapsedTime,_elapsedTimes);
                ;}));
            
            if (Raylib.IsKeyPressed(KeyboardKey.B)) Benchmark();
            if (Raylib.IsKeyPressed(KeyboardKey.Enter)) { 
                Grid.DeleteValue();
                _cells.Clear();
                Grid.FillAllBlocs();
                Grid.RemoveNumbers(20);
                _cells = Grid.GetFreeCells();
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
                if (_cells.Count==0) return;
                foreach (var cell in _cells) {
                    for (var i = 0; i < 9; i++) {
                        var valueHorizontal = Grid.GridValues[i, cell.Position.y];
                        cell.PossibleValue.Remove(valueHorizontal);
                        var valueVertical = Grid.GridValues[cell.Position.x, i];
                        cell.PossibleValue.Remove(valueVertical);
                        foreach (var valueTuple in Grid.GetLocalBloc((cell.Position.x, cell.Position.y))) { 
                            cell.PossibleValue.Remove(valueTuple.value);
                        }
                    }
                }
            }
        }
        Raylib.CloseWindow();
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
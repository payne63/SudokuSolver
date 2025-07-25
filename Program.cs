using Raylib_cs;
using System.Diagnostics;

namespace SudokuSolver;

class Program {
    private static readonly Grid Grid = new Grid();
    private static readonly RaylibDrawer RaylibDrawer = new RaylibDrawer();
    private static readonly Stopwatch Stopwatch = new Stopwatch();
    private static float _elapsedTime = 0f;
    private static readonly List<float> ElapsedTimes = [];

    [STAThread]
    private static void Main(string[] args) {
        Raylib.InitWindow(1280, 1024, "SudokuSolver");
        
        Grid.FillAllBlocs();
        Grid.Modification(10);
        var cells = Grid.GetFreeCells();
        
        while (!Raylib.WindowShouldClose()) {
            RaylibDrawer.Draw((() => {
                var index = 0;
                foreach (var cell in cells) {
                    Raylib.DrawText(cell.ToString(),400,index,30, Color.White);
                    index+=50;
                }
                RaylibDrawer.DrawGridWithStats(Grid.GridValues,_elapsedTime,ElapsedTimes);

                ;}));
            if (Raylib.IsKeyPressed(KeyboardKey.Enter)) {
                for (int i = 0; i < 1000; i++) {
                    Grid.DeleteGrid();
                    Stopwatch.Start();
                    Grid.FillAllBlocs();
                    Stopwatch.Stop();
                    _elapsedTime = Stopwatch.ElapsedMilliseconds;
                    ElapsedTimes.Add(_elapsedTime);
                    Stopwatch.Reset();
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
                var instantGrid = Grid.GetGrid();
            }
        }
        Raylib.CloseWindow();
    }
    

}
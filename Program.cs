using Raylib_cs;
using System.Diagnostics;

namespace SudokuSolver;

class Program {
    private static Grid grid = new Grid();
    private static GridDrawer gridDrawer = new GridDrawer();
    private static Stopwatch stopwatch = new Stopwatch();
    private static float ElapsedTime = 0f;
    private static readonly List<float> ElapsedTimes = [];

    [STAThread]
    private static void Main(string[] args) {
        Raylib.InitWindow(800, 800, "SudokuSolver");
        
        grid.FillAllBlocs();
        grid.Modification(10);
        
        while (!Raylib.WindowShouldClose()) {
            gridDrawer.Draw(grid.GridValues,ElapsedTime,ElapsedTimes);
            if (Raylib.IsKeyPressed(KeyboardKey.Enter)) {
                for (int i = 0; i < 1000; i++) {
                    grid.DeleteGrid();
                    stopwatch.Start();
                    grid.FillAllBlocs();
                    stopwatch.Stop();
                    ElapsedTime = stopwatch.ElapsedMilliseconds;
                    ElapsedTimes.Add(ElapsedTime);
                    stopwatch.Reset();
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Space)) {
                var instantGrid = Grid.GetGrid();
            }
        }
        Raylib.CloseWindow();
    }

}
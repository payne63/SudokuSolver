using System.Numerics;
using Raylib_cs;

namespace SudokuSolver;

public class GridDrawer {
    private Camera2D camera;
    public static int Size;

    public GridDrawer(int size = 40, int positionX = 100,int positionY = 100) {
        camera = new Camera2D();
        camera.Rotation = 0;
        camera.Target = Vector2.Zero;
        camera.Offset = new Vector2(positionX,positionY );
        camera.Zoom = 1;
        Size = size;
    }
    public void Draw( int[,] grid,float? ElapsedTime = null, List<float> ElapsedTimes = null ) {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(camera);
        Raylib.ClearBackground(Color.Blue);
        DrawGrid(grid);
        if (ElapsedTime != null) {
            Raylib.DrawText($"Elapsed Time= {ElapsedTime.ToString()}", 0, -40, Size, Color.Orange);
        }
        if (ElapsedTimes != null && ElapsedTimes.Count() > 0) {
            Raylib.DrawText($"Elapsed Time Average= {ElapsedTimes.Sum() / ElapsedTimes.Count}", 0, -80, Size, Color.Orange);
        }    
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }

    private static void DrawGrid(int[,] grid) {
        for (int y = 0; y < 9; y++) {
            for (int x = 0; x < 9; x++) {
                var val = grid[y, x] == 0 ? " ": grid[y, x].ToString();
                Raylib.DrawText(val, x * Size, y * Size, Size, Color.Black);
            }
        }

        for (int i = 0; i < 10; i = i + 3) {
            Raylib.DrawLine(Size * i, 0, Size * i, Size * 9, Color.Green);
            Raylib.DrawLine(0, Size * i, Size * 9, Size * i, Color.Green);
        }
    }
    
}
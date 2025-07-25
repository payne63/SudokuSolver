using System.Numerics;
using Raylib_cs;

namespace SudokuSolver;

public class RaylibDrawer {
    private Camera2D camera;
    public static int TextSize;

    public RaylibDrawer(int textSize = 40, int positionX = 100,int positionY = 100) {
        camera = new Camera2D();
        camera.Rotation = 0;
        camera.Target = Vector2.Zero;
        camera.Offset = new Vector2(positionX,positionY );
        camera.Zoom = 1;
        TextSize = textSize;
    }
    public void DrawGridWithStats( int[,] grid,float? ElapsedTime = null, List<float> ElapsedTimes = null ) {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(camera);
        // Raylib.ClearBackground(Color.Blue);
        DrawGrid(grid);
        if (ElapsedTime != null) {
            Raylib.DrawText($"Elapsed Time= {ElapsedTime.ToString()}", 0, -40, TextSize, Color.Orange);
        }
        if (ElapsedTimes != null && ElapsedTimes.Count() > 0) {
            Raylib.DrawText($"Elapsed Time Average= {ElapsedTimes.Sum() / ElapsedTimes.Count}", 0, -80, TextSize, Color.Orange);
        }    
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }

    public void Draw(Action drawAction) {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(camera); 
        Raylib.ClearBackground(Color.Blue);
        drawAction();
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }
    
    public void Clear() {Draw(()=> { Raylib.ClearBackground(Color.Blue);});}

    private static void DrawGrid(int[,] grid) {
        for (int y = 0; y < 9; y++) {
            for (int x = 0; x < 9; x++) {
                var val = grid[y, x] == 0 ? " ": grid[y, x].ToString();
                Raylib.DrawText(val, x * TextSize, y * TextSize, TextSize, Color.Black);
            }
        }

        for (int i = 0; i < 10; i = i + 3) {
            Raylib.DrawLine(TextSize * i, 0, TextSize * i, TextSize * 9, Color.Green);
            Raylib.DrawLine(0, TextSize * i, TextSize * 9, TextSize * i, Color.Green);
        }
    }
    
}
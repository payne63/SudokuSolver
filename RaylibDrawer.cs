using System.Numerics;
using Raylib_cs;

namespace SudokuSolver;

public class RaylibDrawer {
    private Camera2D camera;
    private static int _textSize = 40;

    public RaylibDrawer(int textSize = 40, int positionX = 10,int positionY = 10) {
        camera = new Camera2D();
        camera.Rotation = 0;
        camera.Target = Vector2.Zero;
        camera.Offset = new Vector2(positionX,positionY );
        camera.Zoom = 1;
        _textSize = textSize;
    }
    
    public void Draw(Action drawAction) {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(camera); 
        Raylib.ClearBackground(Color.Blue);
        drawAction();
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }
    
    public void DrawGridWithStats( int[,] grid,float elapsedTime = 0, List<float>? elapsedTimes = null ) {
        DrawGrid(grid);
        if (elapsedTime != 0) {
            Raylib.DrawText($"Elapsed Time= {elapsedTime.ToString()}", 0, -40, _textSize, Color.Orange);
        }
        if (elapsedTimes?.Count > 0) {
            Raylib.DrawText($"Elapsed Time Average= {elapsedTimes.Sum() / elapsedTimes.Count}", 0, -80, _textSize, Color.Orange);
        }    
    }
    
    private static void DrawGrid(int[,] grid) {
        for (int y = 0; y < 9; y++) {
            for (int x = 0; x < 9; x++) {
                var val = grid[x, y] == 0 ? " ": grid[x, y].ToString();
                Raylib.DrawText(val, x * _textSize, y * _textSize, _textSize, Color.Black);
            }
        }

        for (int i = 0; i < 10; i = i + 3) {
            Raylib.DrawLine(_textSize * i, 0, _textSize * i, _textSize * 9, Color.Green);
            Raylib.DrawLine(0, _textSize * i, _textSize * 9, _textSize * i, Color.Green);
        }
    }
    
}
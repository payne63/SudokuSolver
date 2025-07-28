using System.Numerics;
using Raylib_cs;

namespace SudokuSolver;

public class RaylibDrawer {
    private Camera2D _camera;
    public static int TextSize;
    public static readonly (int x, int y ) offset = (16, 4);
    public static readonly (int x, int y ) offsetsmall = (5, 2);
    

    public RaylibDrawer(int textSize = 80, int positionX = 10,int positionY = 10) {
        _camera = new Camera2D();
        _camera.Rotation = 0;
        _camera.Target = Vector2.Zero;
        _camera.Offset = new Vector2(positionX,positionY );
        _camera.Zoom = 1;
        TextSize = textSize;
    }
    
    public void Draw(Action drawAction) {
        Raylib.BeginDrawing();
        Raylib.BeginMode2D(_camera); 
        Raylib.ClearBackground(Color.RayWhite);
        drawAction();
        Raylib.EndMode2D();
        Raylib.EndDrawing();
    }
    
    public void DrawGridWithStats( int[,] grid ,Color color ,float elapsedTime = 0, List<float>? elapsedTimes = null , List<Cell> cells = null) {
        DrawGrid(grid,color);
        if (cells?.Count != 0) {
            DrawPossibleCell(cells);
        }
        if (elapsedTime != 0) {
            Raylib.DrawText($"Elapsed Time= {elapsedTime.ToString()}", 0, -40, TextSize, Color.Orange);
        }
        if (elapsedTimes?.Count > 0) {
            Raylib.DrawText($"Elapsed Time Average= {elapsedTimes.Sum() / elapsedTimes.Count}", 0, -80, TextSize, Color.Orange);
        }    
    }

    private void DrawPossibleCell(List<Cell>? cells) {
        if (cells == null) return;
        foreach (var cell in cells) {
            for (var i = 0; i < 9; i++) {
                if (cell.PossibleValue.Contains(i+1)) {
                    var decalX = i switch {
                        0 or 3 or 6 => 0,
                        1 or 4 or 7 => 1,
                        2 or 5 or 8 => 2,
                    };
                    var decalY = i switch {
                        0 or 1 or 2 => 0,
                        3 or 4 or 5 => 1,
                        6 or 7 or 8 => 2,
                    };
                    decalX *= TextSize / 3;
                    decalY *= TextSize / 3;
                    Raylib.DrawText((i+1).ToString(),
                        cell.Position.x*TextSize+decalX+ offsetsmall.x,
                        cell.Position.y*TextSize+decalY +offsetsmall.y,
                        TextSize/3,Color.DarkGray);
                }
            }
        }
    }

    private static void DrawGrid(int[,] grid,Color color) {
        for (int y = 0; y < 9; y++) {
            for (int x = 0; x < 9; x++) {
                var val = grid[x, y] == 0 ? " ": grid[x, y].ToString();
                Raylib.DrawText(val, x * TextSize+ offset.x, y * TextSize+ offset.y, TextSize, color);
            }
        }

        var thickness = 0f;
        for (int i = 0; i < 10;  i++ ) {
            thickness = i switch {
                0 or 3 or 6 or 9 => 3,
                _ => 1
            };
            Raylib.DrawLineEx(new Vector2(TextSize * i, 0),new Vector2(TextSize * i, TextSize * 9),thickness,color);
            Raylib.DrawLineEx(new Vector2(0,TextSize * i),new Vector2(TextSize * 9, TextSize * i),thickness,color);
        }
    }
    
}
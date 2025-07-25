namespace SudokuSolver;

public class Cell {
    public int MaxRange = 9;
    public Cell((int x,int y) position) {
        Position = position;
        for (var i = 0; i < MaxRange; i++) {
            PossibleValue[i] = true;
        }
    }
    public bool[] PossibleValue = new bool[9];
    public (int x, int y) Position;

    public override string ToString() {
        return $"cell position {Position.x},{Position.y}";
    }
}
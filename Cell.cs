using System.Text;

namespace SudokuSolver;

public class Cell {
    public readonly int MaxRange = 9;

    public Cell((int x, int y) position) {
        Position = position;
    }

    public List<int> PossibleValue = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public (int x, int y) Position;

    public override string ToString() {
        return $"cell position {Position.x},{Position.y} nb possible {PrintPossibleValue()}";
    }

    private string PrintPossibleValue() {
        var sb = new StringBuilder();
        foreach (var value in PossibleValue) {
            sb.Append($"{value} ");
        }

        return sb.ToString();
    }

    public int GetLocalGridPosition() {
        var localX = Position.x switch {
            < 3 => 0,
            < 6 => 1,
            < 9 => 2,
        };
        var localY = Position.y switch {
            < 3 => 0,
            < 6 => 1,
            < 9 => 2,
        };
        return (localX, localY) switch {
            (0, 0) => 1,
            (1, 0) => 2,
            (2, 0) => 3,
            (0, 1) => 4,
            (1, 1) => 5,
            (2, 1) => 6,
            (0, 2) => 7,
            (1, 2) => 8,
            (2, 2) => 9,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Cell GetClone() {
        var pv = new List<int>(PossibleValue);
        var cell = new Cell((Position.x, Position.y));
        cell.PossibleValue = pv;
        return cell;
    }
}
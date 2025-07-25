using System.Text;

namespace SudokuSolver;

public class Cell {
    public int MaxRange = 9;
    public Cell((int x,int y) position) {
        Position = position;
    }
    public readonly List<int> PossibleValue = new List<int> (){1,2,3,4,5,6,7,8,9};
    public (int x, int y) Position;

    public override string ToString() {
        return $"cell position {Position.x},{Position.y} nb possible {PrintPossibleValue()}";
    }

    private string PrintPossibleValue (){
        var sb = new StringBuilder();
        foreach (var value in PossibleValue) {
            sb.Append($"{value} ");
        }
        return sb.ToString();
    }
}
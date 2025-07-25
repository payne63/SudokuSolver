using Raylib_cs;

namespace SudokuSolver;

public class Grid {
    private int RetryCountSettings;
    private bool GridIsFilled = false;
    public int[,] GridValues = new int[9, 9];

    public static readonly List<(int, int)> PositionsBlocs =
        [(0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2)];

    private static int _retryCount = 0;

    public Grid(int retryCountSettings = 10) {
        RetryCountSettings =  retryCountSettings;
    }

    public static int[,] GetGrid() {
        Grid grid = new Grid();
        grid.FillAllBlocs();
        return grid.GridValues;
    }
    
    public void FillAllBlocs() {
        var indexBloc = 0;
        _retryCount = 0;
        while (true) {
            var resultFillOK = FillSmallBlocs(PositionsBlocs[indexBloc].Multiply(3));
            if (resultFillOK) {
                indexBloc++;
                if (indexBloc >= PositionsBlocs.Count) break;
            }
            else { 
                if (_retryCount > RetryCountSettings && indexBloc > 2) {
                    DeleteLocalBloc(PositionsBlocs[indexBloc].Multiply(3));
                    indexBloc--;
                    DeleteLocalBloc(PositionsBlocs[indexBloc].Multiply(3));
                    indexBloc--;
                    DeleteLocalBloc(PositionsBlocs[indexBloc].Multiply(3));
                    _retryCount = 0;
                }
                else {
                    DeleteLocalBloc(PositionsBlocs[indexBloc].Multiply(3));
                }
            }

            // Program.DrawGrid();
            if (Raylib.WindowShouldClose()) return;
        }
        GridIsFilled = true;
    }

    private bool FillSmallBlocs((int x, int y) blocPos) {
        var listPositions = new Queue<(int, int)>(PositionsBlocs);
        var listNumbers = new Queue<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var pos = listPositions.Dequeue().Add(blocPos);
        var escapeCounter = 0;
        while (listNumbers.Count > 0) {
            // Program.DrawGrid();
            escapeCounter++;
            listNumbers = listNumbers.Randomize();
            var number = listNumbers.Peek();
            if (CanBePlace(pos, number)) {
                GridValues[pos.x, pos.y] = listNumbers.Dequeue();
                if (listPositions.Count == 0) return true;
                pos = listPositions.Dequeue().Add(blocPos);
                escapeCounter = 0;
            }
            if (escapeCounter > RetryCountSettings) {
                _retryCount++;
                return false;
            }
        }
        return true;
    }

    public List<Cell> GetFreeCells() {
        List<Cell> cells = new List<Cell>();
        foreach (var valueTuple in GetGridValues()) {
            if (valueTuple.val == 0) {
                cells.Add(new Cell(valueTuple.pos));
            }
        }
        return cells;
    }

    private bool CanBePlace((int x, int y) selectedPosition, int nb) {
        for (int x = 0; x < 9; x++) {
            if (GridValues[selectedPosition.x, x] == nb || GridValues[x, selectedPosition.y] == nb) {
                return false;
            }
        }
        return true;
    }

    public void DeleteValue() {
        foreach (var positionsBloc in PositionsBlocs) {
            DeleteLocalBloc(positionsBloc.Multiply(3));
        }
    }

    private void DeleteLocalBloc((int x, int y) pos) {
        for (var i = 0; i < 3; i++) {
            for (var j = 0; j < 3; j++) {
                GridValues[i + pos.x, j + pos.y] = 0;
            }
        }
    }


    public IEnumerable<(int val, (int X, int Y) pos)> GetGridValues() {
        for (var y = 0; y < 9; y++) {
            for (var x = 0; x < 9; x++) {
                yield return new(GridValues[x, y], (x, y));
            }
        }
    }

    public IEnumerable<(int value, (int x, int y) pos)> GetLocalBloc((int x, int y) pos) {
        var localX = pos.x switch {
            < 3 => 0,
            < 6 => 3,
            < 9 => 6,
        };
        var localY = pos.y switch {
            < 3 => 0,
            < 6 => 3,
            < 9 => 6,
        };
        for (var i = 0; i < 3; i++) {
            for (var j = 0; j < 3; j++) {
                yield return (GridValues[i + localX, j + localY],(i + localX, j + localY ));
            }
        }
    }

    public void RemoveNumbers(int countRemove) {
        if (!GridIsFilled) throw new Exception("Grid is not filled");
        var rnd = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < countRemove; i++) {
            (int x,int y) pos = (rnd.Next(0, 9), rnd.Next(0, 9));
            while (GridValues[pos.x, pos.y] == 0) {
                pos = (rnd.Next(0, 9), rnd.Next(0, 9));
            }
            GridValues[pos.x,pos.y] = 0;
        }
    }
}
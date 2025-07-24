namespace SudokuSolver;

public static class Extension {
    public static Random rnd = new Random(DateTime.Now.Millisecond);

    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source) {
        return source.OrderBy<T, int>((item) => rnd.Next());
    }

    public static Queue<T> Randomize<T>(this Queue<T> source) {
        return new Queue<T>(source.OrderBy<T, int>((item) => rnd.Next()));
    }

    public static (int x, int y) Add(this (int x, int y) a, (int x, int y) b) {
        return (a.x + b.x, a.y + b.y);
    }

    public static (int x, int y) Multiply(this (int x, int y) a, int b) {
        return (a.x * b, a.y * b);
    }
}
namespace Inertia.PathFinding
{
    public class Grid
    {
        public readonly bool UseDiagonal;
        public int MaxSize { get; private set; }
        
        private Cell[,] _cells;
        
        public Grid(Cell[,] cells, bool diagonal)
        {
            _cells = cells;
            UseDiagonal = diagonal;
            MaxSize = _cells.GetLength(0) * _cells.GetLength(1);

            foreach (var cell in _cells)
                cell.LoadNeighbours(this);
        }

        public Cell GetCellByIndexes(int x, int y)
        {
            if (x < 0 || x >= _cells.GetLength(0) || y < 0 || y >= _cells.GetLength(1))
                return null;

            return _cells[x, y];
        }
    }
}

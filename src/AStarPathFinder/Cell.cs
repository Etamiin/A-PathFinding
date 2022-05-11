using System.Collections.Generic;

namespace Inertia.Tools
{
    public class Cell
    {
        public readonly int GridX;
        public readonly int GridY;
        public bool Walkable { get; set; }
        
        private Cell[] _neighbours;

        public Cell(int x, int y, bool walkable)
        {
            GridX = x;
            GridY = y;
            Walkable = walkable;
        }

        public Cell[] GetNeighbours()
        {
            return _neighbours;
        }

        internal void LoadNeighbours<T>(MapGrid<T> grid) where T : Cell
        {
            var n = new List<T>();

            if (grid.UseDiagonal)
            {
                AddNeighbour(GridX - 1, GridY - 1);
                AddNeighbour(GridX - 1, GridY + 1);
                AddNeighbour(GridX + 1, GridY - 1);
                AddNeighbour(GridX + 1, GridY + 1);
            }

            AddNeighbour(GridX - 1, GridY);
            AddNeighbour(GridX + 1, GridY);
            AddNeighbour(GridX, GridY - 1);
            AddNeighbour(GridX, GridY + 1);

            _neighbours = n.ToArray();

            void AddNeighbour(int x, int y)
            {
                var cell = grid.GetCellByIndexes(x, y);
                if (cell != null) n.Add(cell);
            }
        }
    }

    internal class CellMeta : IHeapItem<CellMeta>
    {
        public int HeapIndex { get; set; }

        internal readonly Cell Cell;
        internal CellMeta Parent;
        internal int Gcost;
        internal int Hcost;
        internal int Fcost => Gcost + Hcost;

        internal CellMeta(Cell cell)
        {
            Cell = cell;
        }

        public int CompareTo(CellMeta meta)
        {
            var compare = Fcost.CompareTo(meta.Fcost);
            if (compare == 0)
            {
                compare = Hcost.CompareTo(meta.Hcost);
            }

            return -compare;
        }
    }
}

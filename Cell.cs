using System.Collections.Generic;

namespace Inertia.PathFinding
{
    public class Cell
    {
        public int GridX { get; set; }
        public int GridY { get; set; }
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

        internal void LoadNeighbours(Grid grid)
        {
            var n = new List<Cell>();

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
                if (cell != null)
                    n.Add(cell);
            }
        }
    }

    public class CellMeta : IHeapItem<CellMeta>
    {
        public int HeapIndex { get; set; }

        public readonly Cell Cell;
        public CellMeta Parent;
        public int Gcost;
        public int Hcost;
        public int Fcost => Gcost + Hcost;

        public CellMeta(Cell cell)
        {
            Cell = cell;
        }

        public int CompareTo(CellMeta meta)
        {
            var compare = Fcost.CompareTo(meta.Fcost);
            if (compare == 0)
                compare = Hcost.CompareTo(meta.Hcost);

            return -compare;
        }
    }
}

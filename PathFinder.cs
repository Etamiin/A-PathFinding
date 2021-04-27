using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Inertia.PathFinding
{
    public class PathFinder
    {
        private Grid _grid;

        public PathFinder(Grid grid)
        {
            _grid = grid;
        }

        public void ThreadFind(Cell startCell, Cell endCell, Action<Cell[]> onPath)
        {
            new Thread(() => onPath(Find(startCell, endCell))).Start();
        }
        public async Task<Cell[]> TaskFind(Cell startCell, Cell endCell)
        {
            return await Task.Factory.StartNew(() => Find(startCell, endCell));
        }
        public Cell[] Find(Cell startCell, Cell endCell)
        {
            if (!startCell.Walkable || !endCell.Walkable)
                return new Cell[0];

            var openSet = new HeapCollection<CellMeta>(_grid.MaxSize);
            var closedSet = new List<CellMeta>();

            var startMeta = new CellMeta(startCell);
            var endMeta = new CellMeta(endCell);

            openSet.Add(startMeta);

            var metas = new Dictionary<Cell, CellMeta>() { { startCell, startMeta }, { endCell, endMeta } };

            while (openSet.Count > 0)
            {
                var node = openSet.RemoveFirstItem();
                closedSet.Add(node);

                if (node.Cell == endCell)
                    break;

                foreach (var neighbour in node.Cell.GetNeighbours())
                {
                    var nMeta = TryGetMeta(neighbour);
                    if (!neighbour.Walkable || closedSet.Contains(nMeta))
                        continue;

                    var newCost = node.Gcost + GetDistanceCost(node, nMeta);
                    var dontContains = !openSet.Contains(nMeta);
                    if (newCost <= nMeta.Gcost || dontContains)
                    {
                        nMeta.Gcost = newCost;
                        nMeta.Hcost = GetDistanceCost(nMeta, endMeta);
                        nMeta.Parent = node;

                        if (dontContains)
                            openSet.Add(nMeta);
                    }
                }
            }

            var currentMeta = endMeta;
            if (endMeta == null || endMeta.Parent == null)
                return new Cell[0];

            var path = new List<Cell>();
            while (currentMeta != startMeta)
            {
                path.Add(currentMeta.Cell);
                currentMeta = currentMeta.Parent;
            }

            return path.ToArray();

            CellMeta TryGetMeta(Cell cell)
            {
                if (metas.TryGetValue(cell, out CellMeta meta))
                    return meta;

                var m = new CellMeta(cell);
                metas.Add(cell, m);

                return m;
            }
            int GetDistanceCost(CellMeta meta0, CellMeta meta1)
            {
                var dx = Math.Abs(meta0.Cell.GridX - meta1.Cell.GridX);
                var dy = Math.Abs(meta0.Cell.GridY - meta1.Cell.GridY);

                if (dx > dy)
                    return 14 * dy + 10 * (dx - dy);

                return 14 * dx + 10 * (dy - dx);
            }
        }
    }
}

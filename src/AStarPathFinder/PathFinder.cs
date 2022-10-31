using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Inertia.Tools
{
    public sealed class PathFinder<T> where T : Cell
    {
        private MapGrid<T> _grid;

        public PathFinder(MapGrid<T> grid)
        {
            _grid = grid;
        }

        public PathFinderResult<T> Find(T startCell, T endCell)
        {
            if (startCell == endCell) return null;
            if (!startCell.Walkable || !endCell.Walkable) return null;

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

                if (node.Cell == endCell) break;

                foreach (var neighbour in node.Cell.GetNeighbours())
                {
                    var nMeta = TryGetMeta(neighbour as T);
                    if (!neighbour.Walkable || closedSet.Contains(nMeta)) continue;

                    var newCost = node.Gcost + GetDistanceCost(node, nMeta);
                    var dontContains = !openSet.Contains(nMeta);
                    if (newCost < nMeta.Gcost || dontContains)
                    {
                        nMeta.Gcost = newCost;
                        nMeta.Hcost = GetDistanceCost(nMeta, endMeta);
                        nMeta.Parent = node;

                        if (dontContains) openSet.Add(nMeta);
                    }
                }
            }

            var currentMeta = endMeta;
            if (endMeta == null || endMeta.Parent == null) return null;

            var path = new List<T>();
            while (currentMeta != startMeta)
            {
                path.Add((T)currentMeta.Cell);
                currentMeta = currentMeta.Parent;
            }

            return new PathFinderResult<T>(path.ToArray());

            CellMeta TryGetMeta(T cell)
            {
                if (metas.TryGetValue(cell, out CellMeta meta)) return meta;

                var m = new CellMeta(cell);
                metas.Add(cell, m);

                return m;
            }
            int GetDistanceCost(CellMeta meta0, CellMeta meta1)
            {
                var dx = Math.Abs(meta0.Cell.GridX - meta1.Cell.GridX);
                var dy = Math.Abs(meta0.Cell.GridY - meta1.Cell.GridY);

                if (dx > dy) return (14 * dy) + (10 * (dx - dy));

                return (14 * dx) + (10 * (dy - dx));
            }
        }
        public void FindAsync(T startCell, T endCell, Action<PathFinderResult<T>> onResult)
        {
            Task.Factory.StartNew(async () => {
                var result = Find(startCell, endCell);
                onResult(result);

                await Task.CompletedTask;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inertia.PathFinding
{
    public class PathFindingExample
    {
        //Example
        public PathFindingExample()
        {
            //Load cells

            var gridSize = 100; //100x100
            var includeDiagonal = true; //diagonal path
            var cells = new Cell[gridSize, gridSize];

            for (var x = 0; x < gridSize; x++)
            {
                for (var y = 0; y < gridSize; y++)
                {
                    var cellWalkable = true; //set cell waklkable state

                    cells[x, y] = new Cell(x, y, cellWalkable);
                }
            }

            //Initialize grid
            var grid = new Grid(cells, includeDiagonal);
            
            //initialize finder(s)
            var finder = new PathFinder(grid);
            //var finder2 = new PathFinder(grid);

            var r = new Random();

            //select start and end point
            var start = grid.GetCellByIndexes(0, 0);
            var end = (Cell)null;

            //get random end cell
            while (end == null)
                end = grid.GetCellByIndexes(r.Next(1, grid.Width), r.Next(1, grid.Height));

            //get path result and use it
            var result = finder.Find(start, end);

            /*
             * Thread finder
             
            finder.ThreadFind(start, end, (result) => { use result here });

            */

            /*
             * Async finder
            
            finder.FindASync(start, end, (result) => { 
                //use result
            });

            */

            //check if selected end cell or start arn't walkable or if the finder failed finding a path
            if (result == null)
                return;

            //loop on result path
            while (!result.IsEndOfPath)
            {
                var cell = result.GetNextCell();
                //use cell
            }

        }
    }
}

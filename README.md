### Usage example

```cs
//Initialize cells and grid

var setDiagonalPath = true;
var cells = new Cell[100, 100];

for (var x = 0; x < cells.GetLength(0); x++)
{
  for (var y = 0; y < cells.GetLength(1); y++)
  {
    var isWalkable = true;
    cells[x, y] = new Cell(x, y, isWalkable);
  }
}

var grid = new Grid(cells, setDiagonalPath);

//Find path
var finder = new PathFinder(grid);
var start = grid.GetCellByIndexes(0, 0); //set start cell
var end = grid.GetCellByIndexes(21, 21); //set end cell

var result = finder.Find(start, end);
if (result != null)
{
  //loop on result path
  while (!result.IsEndOfPath)
  {
    var currentCell = result.GetNextCell();
    //
  }
}
else 
{
  //no path found
}

//Find path async
finder.FindAsync(start, end, (result) => {
  //
});

```
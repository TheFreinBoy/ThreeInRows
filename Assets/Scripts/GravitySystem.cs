using UnityEngine;
using StaticData;

/// <summary>
/// Manages gravity on the board
/// Handles cell falling and filling empty spaces with new cells
/// </summary>
public class GravitySystem
{
    private readonly BoardService _boardService;
    private readonly CellFactory _cellFactory;
    private readonly CellMover _cellMover;
    private readonly int[] _fillingCellsCountByColumn;
    private readonly System.Collections.Generic.List<Cell> _deadCells;
    private readonly System.Collections.Generic.List<Cell> _updatingCells;

    public GravitySystem(
        BoardService boardService,
        CellFactory cellFactory,
        CellMover cellMover,
        int[] fillingCellsCountByColumn,
        System.Collections.Generic.List<Cell> deadCells,
        System.Collections.Generic.List<Cell> updatingCells)
    {
        _boardService = boardService;
        _cellFactory = cellFactory;
        _cellMover = cellMover;
        _fillingCellsCountByColumn = fillingCellsCountByColumn;
        _deadCells = deadCells;
        _updatingCells = updatingCells;
    }

    /// <summary>
    /// Apply gravity to the board (make cells fall down)
    /// </summary>
    public void ApplyGravity()
    {
        for (int x = 0; x < _boardService.BoardWidth; x++)
        {
            for (int y = _boardService.BoardHeight - 1; y >= 0; y--)
            {
                ApplyGravityToCell(x, y);
            }
        }
    }

    /// <summary>
    /// Apply gravity to a specific cell
    /// </summary>
    private void ApplyGravityToCell(int x, int y)
    {
        var point = new Point(x, y);
        var cellTypeAtPoint = _boardService.GetCellTypeAtPoint(point);
        
        if (cellTypeAtPoint != 0)
            return;
        
        for (int newY = y - 1; newY >= -1; newY--)
        {
            var nextPoint = new Point(x, newY);
            var nextCellType = _boardService.GetCellTypeAtPoint(nextPoint);

            if (nextCellType == 0)
                continue;

            if (nextCellType != CellData.CellType.Hole)
            {
                MoveCell(nextPoint, point);
            }
            else
            {
                CreateNewCell(x, point);
            }

            break;
        }
    }

    /// <summary>
    /// Move a cell from one position to another
    /// </summary>
    private void MoveCell(Point fromPoint, Point toPoint)
    {
        var fromCellData = _boardService.GetCellAtPoint(fromPoint);
        var toCellData = _boardService.GetCellAtPoint(toPoint);
        var cell = fromCellData.GetCell();

        toCellData.SetCell(cell);
        _updatingCells.Add(cell);
        fromCellData.SetCell(null);
    }

    /// <summary>
    /// Create a new cell in the hole
    /// </summary>
    private void CreateNewCell(int x, Point holePoint)
    {
        var cellType = GetRandomCellType();
        var fallPoint = new Point(x, -1 - _fillingCellsCountByColumn[x]);

        Cell cell;
        
        if (_deadCells.Count > 0)
        {
            var revivedCell = _deadCells[0];
            revivedCell.gameObject.SetActive(true);
            cell = revivedCell;
            _deadCells.RemoveAt(0);
        }
        else
        {
            cell = _cellFactory.InstantiateCell();
        }

        cell.Initialize(
            new CellData(cellType, holePoint),
            _boardService.GetSpriteForCellType(cellType),
            _cellMover,
            _boardService);

        cell.rect.anchoredPosition = _boardService.GetBoardPositionFromPoint(fallPoint);

        var holeCellData = _boardService.GetCellAtPoint(holePoint);
        holeCellData.SetCell(cell);
        
        _boardService.ResetCell(cell);
        _fillingCellsCountByColumn[x]++;
    }

    /// <summary>
    /// Get a random cell type (with possible bonuses)
    /// </summary>
    private CellData.CellType GetRandomCellType()
    {
        float randomValue = Random.value;
        const float bonusSpawnChance = 0.025f;

        if (randomValue < bonusSpawnChance)
        {
            return Random.value < 0.3f ? CellData.CellType.VerticalBonus : CellData.CellType.Bomb;
        }

        var cellSprites = _boardService.CellSprites;
        return (CellData.CellType)(Random.Range(1, cellSprites.Length + 1));
    }
}

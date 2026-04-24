using System.Collections.Generic;
using StaticData;

/// <summary>
/// Manages cell updates in the game loop
/// Handles cell movement, match detection, and destruction
/// </summary>
public class CellUpdateHandler
{
    private readonly BoardService _boardService;
    private readonly MatchMachine _matchMachine;
    private readonly CellDestructionHandler _destructionHandler;
    private readonly List<Cell> _updatingCells;
    private readonly List<CellFlip> _flippedCells;
    private readonly int[] _fillingCellsCountByColumn;

    public CellUpdateHandler(
        BoardService boardService,
        MatchMachine matchMachine,
        CellDestructionHandler destructionHandler,
        List<Cell> updatingCells,
        List<CellFlip> flippedCells,
        int[] fillingCellsCountByColumn)
    {
        _boardService = boardService;
        _matchMachine = matchMachine;
        _destructionHandler = destructionHandler;
        _updatingCells = updatingCells;
        _flippedCells = flippedCells;
        _fillingCellsCountByColumn = fillingCellsCountByColumn;
    }

    /// <summary>
    /// Update all moving cells and process completed ones
    /// </summary>
    public void UpdateCells()
    {
        var finishedUpdating = new List<Cell>();

        foreach (var cell in _updatingCells)
        {
            if (!cell.UpdateCell())
                finishedUpdating.Add(cell);
        }

        foreach (var cell in finishedUpdating)
        {
            ProcessFinishedCell(cell);
        }
    }

    /// <summary>
    /// Process a cell that completed its movement/update
    /// </summary>
    private void ProcessFinishedCell(Cell cell)
    {
        var x = cell.Point.x;
        _fillingCellsCountByColumn[x] = UnityEngine.Mathf.Clamp(_fillingCellsCountByColumn[x] - 1, 0, _boardService.BoardWidth);

        var flip = GetFlip(cell);
        var connectedPoints = _matchMachine.GetMatchedPoints(cell.Point, true);

        Cell flippedCell = null;

        if (flip != null)
        {
            flippedCell = flip.GetOtherCell(cell);
            MatchMachine.AddPoints(ref connectedPoints, _matchMachine.GetMatchedPoints(flippedCell.Point, true));
        }

        if (connectedPoints.Count == 0)
        {
            if (flippedCell != null)
                _boardService.FlipCells(cell.Point, flippedCell.Point, false);
        }
        else
        {
            _destructionHandler.DestroyCells(connectedPoints);
        }

        _flippedCells.Remove(flip);
        _updatingCells.Remove(cell);
    }

    /// <summary>
    /// Get flip information for a cell
    /// </summary>
    private CellFlip GetFlip(Cell cell)
    {
        foreach (var flip in _flippedCells)
        {
            if (flip.GetOtherCell(cell) != null)
                return flip;
        }
        return null;
    }
}

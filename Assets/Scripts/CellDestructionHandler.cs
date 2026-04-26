using System.Collections.Generic;

/// <summary>
/// Manages cell destruction on the board
/// Handles bomb explosions, vertical line destruction, and regular match logic
/// </summary>
public class CellDestructionHandler
{
    private readonly BoardService _boardService;
    private readonly ParticleEffectPool _matchFxPool;
    private readonly ParticleEffectPool _explosionFxPool;
    private readonly ScoreService _scoreService;
    private readonly TimerService _timerService;
    private readonly List<Cell> _deadCells;

    public CellDestructionHandler(
        BoardService boardService,
        ParticleEffectPool matchFxPool,
        ParticleEffectPool explosionFxPool,
        ScoreService scoreService,
        TimerService timerService,
        List<Cell> deadCells)
    {
        _boardService = boardService;
        _matchFxPool = matchFxPool;
        _explosionFxPool = explosionFxPool;
        _scoreService = scoreService;
        _timerService = timerService;
        _deadCells = deadCells;
    }

    /// <summary>
    /// Destroy cells from a list of points (for regular matches)
    /// </summary>
    public void DestroyCells(List<Point> points)
    {
        int destroyedCount = 0;

        foreach (var point in points)
        {
            if (DestroyCellAtPoint(point, _matchFxPool))
            {
                destroyedCount++;
            }
        }

        ApplyScore(destroyedCount);
    }

    /// <summary>
    /// Explode a bomb and destroy surrounding cells
    /// </summary>
    public void ExplodeBomb(Point bombPoint)
    {
        var bombCellData = _boardService.GetCellAtPoint(bombPoint);
        var bombCell = bombCellData.GetCell();
        
        if (bombCell != null && _explosionFxPool != null)
        {
            _explosionFxPool.PlayEffectAt(bombCell.rect.transform.position);
        }

        int destroyedCount = 0;
        
        for (int x = bombPoint.x - 1; x <= bombPoint.x + 1; x++)
        {
            for (int y = bombPoint.y - 1; y <= bombPoint.y + 1; y++)
            {
                var point = new Point(x, y);
                var cellType = _boardService.GetCellTypeAtPoint(point);

                if (cellType == CellData.CellType.Hole)
                    continue;

                if (DestroyCellAtPoint(point, _matchFxPool))
                {
                    destroyedCount++;
                }
            }
        }

        ApplyScore(destroyedCount);
    }

    /// <summary>
    /// Destroy a vertical line (bonus effect)
    /// </summary>
    public void DestroyVerticalLine(Point bonusPoint)
    {
        int destroyedCount = 0;

        for (int y = 0; y < _boardService.BoardHeight; y++)
        {
            var point = new Point(bonusPoint.x, y);
            var cellType = _boardService.GetCellTypeAtPoint(point);

            if (cellType == CellData.CellType.Hole)
                continue;

            var cellData = _boardService.GetCellAtPoint(point);
            var cell = cellData.GetCell();

            if (cell != null)
            {
                if (_matchFxPool != null)
                    _matchFxPool.PlayEffectAt(cell.rect.transform.position);

                cell.gameObject.SetActive(false);
                _deadCells.Add(cell);
                destroyedCount++;
            }

            cellData.SetCell(null);
        }

        ApplyScore(destroyedCount);
    }

    /// <summary>
    /// Destroy a single cell at the specified point
    /// </summary>
    private bool DestroyCellAtPoint(Point point, ParticleEffectPool fxPool)
    {
        var cellType = _boardService.GetCellTypeAtPoint(point);

        if (cellType == CellData.CellType.Hole)
            return false;

        var cellData = _boardService.GetCellAtPoint(point);
        var cell = cellData.GetCell();

        if (cell != null)
        {
            if (fxPool != null)
                fxPool.PlayEffectAt(cell.rect.transform.position);

            cell.gameObject.SetActive(false);
            _deadCells.Add(cell);
        }

        cellData.SetCell(null);
        return true;
    }

    /// <summary>
    /// Apply score and time bonus for destruction
    /// </summary>
    private void ApplyScore(int destroyedCount)
    {
        if (destroyedCount <= 0)
            return;

        _scoreService.AddScore(destroyedCount);

        if (_timerService != null)
            _timerService.AddTimeOnMatch(destroyedCount);
    }

    /// <summary>
    /// Update effect pools (move completed effects back to the pool)
    /// </summary>
    public void UpdateEffectPools()
    {
        if (_matchFxPool != null)
            _matchFxPool.UpdatePool();
        
        if (_explosionFxPool != null)
            _explosionFxPool.UpdatePool();
    }
}

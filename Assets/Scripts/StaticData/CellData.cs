public class CellData
{
    public enum CellType
    {
        Hole = -1,
        Blank = 0,
        Blue = 1,
        Green = 2,
        Purple = 3,
        Red = 4,
        White = 5,
        Yellow = 6,
        Bomb = 7,
        VerticalBonus = 8,
    }

    public CellType cellType;
    public Point point;
    private Cell _cell;
    
    private BoardService _boardService;
    public CellData(CellType cellType, Point point, BoardService boardService)
    {
        this.cellType = cellType;
        this.point = point;
        this._boardService = boardService;
    }
    public Cell GetCell()
    {
        return _cell;
    }
    public void SetCell(Cell newCell)
    {
        _cell = newCell;
        if (newCell == null)
        {
            cellType = CellType.Blank;
        }
        else
        {
            cellType = newCell.CellType;
            _cell.SetCellPoint(point);
        }
        
        if (_boardService != null)
        {
            _boardService.SendCellToNetwork(point.x, point.y, cellType);
        }
    }

}
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

    }

    public CellType cellType;
    public Point point;
    private Cell _cell;
    public CellData(CellType cellType, Point point)
    {
        this.cellType = cellType;
        this.point = point;
    }

}
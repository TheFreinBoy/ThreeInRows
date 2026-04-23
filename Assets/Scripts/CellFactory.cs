using UnityEngine;
using StaticData;
using Match3;

public class CellFactory: MonoBehaviour
{
    private BoardService _boardService;


    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private Cell _cellPrefab;

    public void InstantiateBoard(BoardService boardService, CellMover cellMover)
    {
         for (int y = 0; y < Config.BoardHeight; y++)
        {
            for (int x = 0; x < Config.BoardWidth; x++)
            {       
                var point = new Point(x, y);
                var cellData = boardService.GetCellAtPoint(point);    
                var cellType = cellData.cellType;

                if (cellType == CellData.CellType.Blank || cellType == CellData.CellType.Hole)          
                    continue;
                
                var cell = InstantiateCell();
                cell.rect.anchoredPosition = BoardService.GetBoardPositionFromPoint(point);
                var sprite = boardService.GetSpriteForCellType(cellType);
                cell.Initialize(new CellData(cellType, new Point(x,y)), sprite, cellMover, boardService);
                cellData.SetCell(cell);
            }
        }
    }
    

    public Cell InstantiateCell()
    {
        return Instantiate(_cellPrefab, _boardRect);
    }
}
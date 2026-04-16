using System;
using System.Collections.Generic;
using StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CellFactory))]
public class BoardService : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [SerializeField] private Sprite[] _cellSprites;
    private CellData[,] _board;
    private CellFactory _cellFactory;
    private MatchMachine _matchMachine;
    private CellMover _cellMover;
    public Sprite[] CellSprites => _cellSprites;

    private readonly List <Cell> _updatingCells = new List<Cell>();
    private void Awake()
    {
        _cellFactory = GetComponent<CellFactory>();
        _matchMachine = new MatchMachine(this);
        _cellMover = new CellMover(this);

    }
    void Start()
    {
       InitializeBoard();
       VerifyBoardOnMathes();
       _cellFactory.InstantiateBoard(this, _cellMover);
    }
    private void Update()
    {
        _cellMover.Update();

        var finishedUpdating = new List<Cell>();
        foreach (var cell in _updatingCells)
        {
            if (!cell.UpdateCell())
                finishedUpdating.Add(cell);
        }
        foreach (var cell in finishedUpdating)
        {
            _updatingCells.Remove(cell);
        }
        
    }
    public void ResetCell(Cell cell)
    {
        cell.ResetPosition();
        _updatingCells.Add(cell);
    }
    private void VerifyBoardOnMathes()
    {
        for (int y = 0; y < Config.BoardHeight; y++)
        {
            for (int x = 0; x < Config.BoardWidth; x++)
            {
                var point = new Point(x, y);
                var cellTypeAtPoint = GetCellTypeAtPoint(point);

                if(cellTypeAtPoint <= 0)
                    continue;

                var removeCellTypes = new List<CellData.CellType>();
                int maxAttempts = 100;
                int attempts = 0;
                while (_matchMachine.GetMatchedPoints(point,true).Count > 0 && attempts < maxAttempts)
                {
                    if (removeCellTypes.Contains(cellTypeAtPoint) == false)
                        removeCellTypes.Add(cellTypeAtPoint);
                    SetCellTypeAtPoint(point,GetNewCellType(ref removeCellTypes));
                    attempts++;
                }    
            }
        }
    }
    private void SetCellTypeAtPoint(Point point, CellData.CellType newCellType)
    {
        _board[point.x, point.y].cellType = newCellType;
    }
    private CellData.CellType GetNewCellType(ref List<CellData.CellType> removeCellTypes)
    {
        var availableCellTypes = new List<CellData.CellType>();
        for (int i = 0; i < CellSprites.Length; i++)
            availableCellTypes.Add((CellData.CellType)(i + 1));
        foreach (var removeCellType in removeCellTypes)
            availableCellTypes.Remove(removeCellType);
  
        return availableCellTypes.Count <= 0
            ? CellData.CellType.Blank
            : availableCellTypes[Random.Range(0, availableCellTypes.Count)];
    }
    public CellData.CellType GetCellTypeAtPoint(Point point)
    {
        if (point.x < 0 || point.x >= Config.BoardWidth || point.y < 0 || point.y >= Config.BoardHeight)   
            return CellData.CellType.Hole;   
        return _board[point.x, point.y].cellType;
    }
    
    private void InitializeBoard()
    {
        _board = new CellData[Config.BoardWidth, Config.BoardHeight];
        for (int y = 0; y < Config.BoardHeight; y++)
        {
            for (int x = 0; x < Config.BoardWidth; x++)
            {
                _board[x, y] = new CellData(
                    boardLayout.rows[y].row[x] ? CellData.CellType.Hole : GetRandomCellType(), 
                    new Point(x, y)
                );
                
            }
        }
    }
    public CellData GetCellAtPoint(Point point)
    {
        return _board[point.x, point.y];
    }

    private CellData.CellType GetRandomCellType()
    {
        return (CellData.CellType)(Random.Range(0, _cellSprites.Length) + 1);
    }
   
    public static Vector2 GetBoardPositionFromPoint(Point point)
    {
        return new Vector2(
            Config.PieceSize/2 + Config.PieceSize * point.x, 
            -Config.PieceSize/2 - Config.PieceSize * point.y);
    }
}

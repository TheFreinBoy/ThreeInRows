using System.Collections.Generic;
using StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CellFactory))]
public class BoardService : MonoBehaviour
{
    [SerializeField] private LevelData _currentLevel;

    [SerializeField] private Sprite[] _cellSprites;
    [SerializeField] private Sprite _bombSprite;
    [SerializeField] private Sprite _verticalBonusSprite;
    [SerializeField] private ParticleSystem _matchFxPrefab;
    [SerializeField] private ParticleSystem _bombExplosionFxPrefab;
    [SerializeField] private ScoreService _scoreService;
    [SerializeField] private TimerService _timerService;
    [SerializeField] private GameStateService _gameStateService;

    private CellData[,] _board;
    private CellFactory _cellFactory;
    private MatchMachine _matchMachine;
    private CellMover _cellMover;
    private int[] _fillingCellsCountByColumn;
    
    public Sprite[] CellSprites => _cellSprites;
    public int BoardWidth => _currentLevel.boardWidth;
    public int BoardHeight => _currentLevel.boardHeight;
    
    private ParticleEffectPool _matchFxPool;
    private ParticleEffectPool _explosionFxPool;
    private CellDestructionHandler _destructionHandler;
    private GravitySystem _gravitySystem;
    private CellUpdateHandler _updateHandler;

    private readonly List<Cell> _updatingCells = new List<Cell>();
    private readonly List<Cell> _deadCells = new List<Cell>();
    private readonly List<CellFlip> _flippedCells = new List<CellFlip>();
    private void Awake()
    {
        if (GameContext.SelectedLevel != null)
        {
            _currentLevel = GameContext.SelectedLevel;
        }
        
        if (_currentLevel == null)
        {
            Debug.LogError("Уровень не выбран! Назначьте его в Инспекторе или запустите из меню.");
            return;
        }
        
        _cellFactory = GetComponent<CellFactory>();
        if (_cellFactory == null)
        {
            Debug.LogError("CellFactory component not found on BoardService GameObject!");
            return;
        }
        _fillingCellsCountByColumn = new int[_currentLevel.boardWidth];
        _matchMachine = new MatchMachine(this);
        _cellMover = new CellMover(this);
        
        if (_matchFxPrefab != null)
            _matchFxPool = new ParticleEffectPool(_matchFxPrefab, transform);
        else
            Debug.LogWarning("Match FX Prefab not assigned in Inspector!");

        if (_bombExplosionFxPrefab != null)
            _explosionFxPool = new ParticleEffectPool(_bombExplosionFxPrefab, transform);
        else
            Debug.LogWarning("Bomb Explosion FX Prefab not assigned in Inspector!");
        
        _destructionHandler = new CellDestructionHandler(
            this, _matchFxPool, _explosionFxPool, _scoreService, _timerService, _deadCells);

        _gravitySystem = new GravitySystem(
            this, _cellFactory, _cellMover, _fillingCellsCountByColumn, _deadCells, _updatingCells);

        _updateHandler = new CellUpdateHandler(
            this, _matchMachine, _destructionHandler, _updatingCells, _flippedCells, _fillingCellsCountByColumn);
    }
    void Start()
    {
       InitializeBoard();
       VerifyBoardOnMathes();
       _cellFactory.InstantiateBoard(this, _cellMover);
       
       if (_gameStateService != null)
           _gameStateService.Initialize();
       
       if (_timerService != null)
           _timerService.OnTimeExpired += OnGameOver;
    }
    private void Update()
    {
        _cellMover.Update();
        _updateHandler.UpdateCells();
        _destructionHandler.UpdateEffectPools();
        _gravitySystem.ApplyGravity();
    }
    public void FlipCells(Point firstPoint, Point secondPoint, bool main)
    {
        if(GetCellTypeAtPoint(firstPoint) < 0 )
            return;
        
        var firstCellData = GetCellAtPoint(firstPoint);
        var firstCell = firstCellData.GetCell();
        
        if (GetCellTypeAtPoint(firstPoint) == CellData.CellType.Bomb || 
            GetCellTypeAtPoint(secondPoint) == CellData.CellType.Bomb ||
            GetCellTypeAtPoint(firstPoint) == CellData.CellType.VerticalBonus ||
            GetCellTypeAtPoint(secondPoint) == CellData.CellType.VerticalBonus)
        {
            ResetCell(firstCell);
            return;
        }

        if(GetCellTypeAtPoint(secondPoint) > 0)
        {
            var secondCellData = GetCellAtPoint(secondPoint);
            var secondCell = secondCellData.GetCell();
            firstCellData.SetCell(secondCell);
            secondCellData.SetCell(firstCell);

            if(main)
                _flippedCells.Add(new CellFlip(firstCell, secondCell));
            
            _updatingCells.Add(firstCell);
            _updatingCells.Add(secondCell);
        }
        else
        {
            ResetCell(firstCell);
        }
    }
    public void ResetCell(Cell cell)
    {
        cell.ResetPosition();
        _updatingCells.Add(cell);
    }

    public void ExplodeBomb(Point bombPoint)
    {
        _destructionHandler.ExplodeBomb(bombPoint);
    }
    
    public void DestroyVerticalLine(Point bonusPoint)
    {
        _destructionHandler.DestroyVerticalLine(bonusPoint);
    }
    
    private void VerifyBoardOnMathes()
    {
        for (int y = 0; y < _currentLevel.boardHeight; y++)
        {
            for (int x = 0; x < _currentLevel.boardWidth; x++)
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
        if (point.x < 0 || point.x >= _currentLevel.boardWidth || point.y < 0 || point.y >= _currentLevel.boardHeight)   
            return CellData.CellType.Hole;   
        return _board[point.x, point.y].cellType;
    }
    
    private void InitializeBoard()
    {
        
        _board = new CellData[_currentLevel.boardWidth, _currentLevel.boardHeight];
        
        for (int y = 0; y < _currentLevel.boardHeight; y++)
        {
            for (int x = 0; x < _currentLevel.boardWidth; x++)
            {
                var cellType = _currentLevel.boardLayout.rows[y].row[x] 
                    ? CellData.CellType.Hole 
                    : (CellData.CellType)(Random.Range(1, _currentLevel.availableColors + 1));
                
                _board[x, y] = new CellData(cellType, new Point(x, y));
            }
        }
    }
    public CellData GetCellAtPoint(Point point)
    {
        return _board[point.x, point.y];
    }
    
    public Vector2 GetBoardPositionFromPoint(Point point)
    {
        float startX = -(_currentLevel.boardWidth * Config.PieceSize) / 2f + Config.PieceSize / 2f;
        float startY = (_currentLevel.boardHeight * Config.PieceSize) / 2f - Config.PieceSize / 2f;

        return new Vector2(
            startX + (point.x * Config.PieceSize),
            startY - (point.y * Config.PieceSize)
        );
    }

    private void OnGameOver()
    {
        if (_gameStateService != null)
            _gameStateService.EndGame();
    }
    public Sprite GetSpriteForCellType(CellData.CellType cellType)
    {
        if (cellType == CellData.CellType.Hole)
            return null;
        if (cellType == CellData.CellType.Bomb)
            return _bombSprite;
        if (cellType == CellData.CellType.VerticalBonus)
            return _verticalBonusSprite;
        if (cellType <= 0)
            return null;
        return _cellSprites[(int)(cellType - 1)];
    }

    public bool CanMakeMove()
    {
        if (_gameStateService != null && _gameStateService.IsGameOver)
            return false;
        
        return _flippedCells.Count == 0 && _updatingCells.Count == 0;
    }
}

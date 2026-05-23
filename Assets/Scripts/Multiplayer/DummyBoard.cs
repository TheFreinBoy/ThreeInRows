using Fusion;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class DummyBoard : MonoBehaviour
    {
        [SerializeField] private BoardService _mainBoard; 
        [SerializeField] private GameObject _dummyCellPrefab; 
        
        private NetworkBoardState _enemyState;
        private GameObject[] _dummyCells;
        private int[] _previousBoardData; 
        private bool _isInitialized = false;

        void Update()
        {
            if (_enemyState == null)
            {
                foreach (var state in FindObjectsByType<NetworkBoardState>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                {
                    if (!state.HasStateAuthority) 
                    {
                        _enemyState = state;
                        break;
                    }
                }
                return; 
            }

            int width = _enemyState.ActualWidth;
            int height = _enemyState.ActualHeight;

            if (width <= 0 || height <= 0) return;

            if (!_isInitialized)
            {
                CreateGrid(width, height); 
            }

            if (_isInitialized)
            {
                SyncVisualsWithAnimation(width, height);
            }
        }

        private void CreateGrid(int width, int height)
        {
            _dummyCells = new GameObject[width * height];
            _previousBoardData = new int[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject cellObj = Instantiate(_dummyCellPrefab, transform);
                    
                    Vector2 localPos = _mainBoard.GetBoardPositionFromPoint(new Point(x, y));
                    cellObj.transform.localPosition = localPos;
                    
                    var cellScript = cellObj.GetComponent<Cell>();
                    if (cellScript != null) Destroy(cellScript);

                    var button = cellObj.GetComponent<UnityEngine.UI.Button>();
                    if (button != null) Destroy(button);

                    var collider = cellObj.GetComponent<Collider2D>();
                    if (collider != null) Destroy(collider);

                    _dummyCells[y * width + x] = cellObj;
                }
            }
            _isInitialized = true;
        }

        private void SyncVisualsWithAnimation(int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    
                    int currentCellValue = _enemyState.BoardData.Get(index);
                    
                    if (currentCellValue != _previousBoardData[index])
                    {
                        AnimateCellChange(x, y, width, currentCellValue, _previousBoardData[index]);
                        
                        _previousBoardData[index] = currentCellValue;
                    }
                }
            }
        }

        private void AnimateCellChange(int x, int y, int width, int newValue, int oldValue)
        {
            int index = y * width + x;
            GameObject cellObj = _dummyCells[index];
            Sprite newSprite = _mainBoard.GetSpriteForCellType((CellData.CellType)newValue);
            
            SpriteRenderer spriteRenderer = cellObj.GetComponent<SpriteRenderer>();
            Image image = cellObj.GetComponent<Image>();
            
            if (newValue <= 0)
            {
                cellObj.transform.DOScale(Vector3.zero, 0.15f).OnComplete(() => 
                {
                    SetComponentSprite(spriteRenderer, image, null);
                    cellObj.transform.localScale = Vector3.one;
                });
            }
            else
            {
                SetComponentSprite(spriteRenderer, image, newSprite);
                
                Vector3 targetLocalPos = _mainBoard.GetBoardPositionFromPoint(new Point(x, y));
                
                float offset = 0.6f; 
                cellObj.transform.localPosition = new Vector3(targetLocalPos.x, targetLocalPos.y + offset, targetLocalPos.z);
                
                cellObj.transform.DOLocalMove(targetLocalPos, 0.25f).SetEase(Ease.OutQuad);
                
                cellObj.transform.localScale = Vector3.zero;
                cellObj.transform.DOScale(Vector3.one, 0.15f);
            }
        }

        private void SetComponentSprite(SpriteRenderer sr, Image img, Sprite sprite)
        {
            if (sr != null)
            {
                sr.sprite = sprite;
                sr.color = sprite == null ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            }
            if (img != null)
            {
                img.sprite = sprite;
                img.color = sprite == null ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            }
        }
    }
}
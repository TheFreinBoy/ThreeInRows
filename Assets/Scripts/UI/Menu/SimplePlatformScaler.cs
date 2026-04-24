using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasScaler))]
    public class SimplePlatformScaler : MonoBehaviour
    {
        private CanvasScaler _scaler;

        private void Awake()
        {
            _scaler = GetComponent<CanvasScaler>();
            UpdateScale();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) 
            {
                UpdateScale();
            }
#endif
        }

        private void UpdateScale()
        {
            if (_scaler == null) return;
            
            _scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

#if UNITY_EDITOR
            if (Screen.dpi > 200f || Screen.height > 1080)
            {
                _scaler.scaleFactor = 3f; 
            }
            else
            {
                _scaler.scaleFactor = 1f; 
            }
#else
            
            if (Application.isMobilePlatform)
            {
                _scaler.scaleFactor = 3f; 
            }
            else
            {
                _scaler.scaleFactor = 1f; 
            }
#endif
        }
    }
}
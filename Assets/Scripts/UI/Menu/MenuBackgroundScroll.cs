using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundScroll : MonoBehaviour
{
    [SerializeField] private RawImage _backgroundImage;
    [SerializeField] private float _speedX = 0.05f;
    [SerializeField] private float _speedY = 0.05f;

    private void Update()
    {
        if (_backgroundImage != null)
        {
            Rect uvRect = _backgroundImage.uvRect;
            
            uvRect.x += _speedX * Time.deltaTime;
            uvRect.y += _speedY * Time.deltaTime;
            
            _backgroundImage.uvRect = uvRect;
        }
    }
}
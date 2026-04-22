using UnityEngine;
using DG.Tweening; 

public class ButtonPulse : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private float _targetScale = 1.1f; 
    [SerializeField] private float _duration = 1f;     

    private void Start()
    {

        transform.DOScale(_targetScale, _duration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
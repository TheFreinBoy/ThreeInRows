using UnityEngine;

// Використовуємо alias щоб пофіксити конфлікт між неймспейсами
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        void Start()
        {
            float speed = Random.value;
            Debug.Log("Швидкість: " + speed);
        }
    }
}

namespace EditorTools.MapCreation
{
    public class Drawing
    {
        
    }
}
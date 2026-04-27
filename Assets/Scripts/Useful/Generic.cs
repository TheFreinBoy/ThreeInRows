using UnityEngine;
namespace Useful
{

    // T - це тип менеджера, який ми хочемо зробити Singleton-ом
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Шукаємо об'єкт цього типу на сцені 
                    // (У нових версіях Unity краще використовувати FindFirstObjectByType)
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError($"Об'єкт типу {typeof(T)} не знайдено на сцені!");
                    }
                }
                return _instance;
            }
        }
    }
    
// Тепер створення менеджера займає один рядок. 
// Ми передаємо сам GameManager як тип T.
    public class GameManager : Singleton<GameManager>
    {
        public int score = 0;

        public void AddScore(int points)
        {
            score += points;
            Debug.Log("Рахунок: " + score);
        }
    }

    public class PlayerLogic : MonoBehaviour
    {
        void Start()
        {
            // Викликаємо метод менеджера з будь-якого місця
            GameManager.Instance.AddScore(10); 
        }
    }
    public static class GameObjectExtensions
    {
        // Метод працює з будь-яким типом T, який є компонентом Unity
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            // Спробуємо отримати компонент
            T component = gameObject.GetComponent<T>();
        
            // Якщо його немає, додаємо новий
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
        
            return component;
        }
    }
    

    public class GenericMethodPracticalExample : MonoBehaviour
    {
        void Start()
        {
            // Якщо на цьому об'єкті немає Rigidbody, він буде автоматично створений!
            Rigidbody rb = gameObject.GetOrAddComponent<Rigidbody>();
        
            // Якщо BoxCollider вже є, ми просто отримаємо посилання на нього
            BoxCollider col = gameObject.GetOrAddComponent<BoxCollider>();
        
            rb.mass = 5f;
            Debug.Log("Компоненти успішно знайдені або додані.");
        }
    }
}
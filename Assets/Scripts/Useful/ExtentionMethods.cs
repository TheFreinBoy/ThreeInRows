using UnityEngine;

// Клас для методів розширення завжди має бути статичним.
// Зручно називати такі класи за типом, який вони розширюють.
public static class TransformExtensions
{
    // Метод має бути статичним.
    // Ключове слово 'this' перед типом Transform вказує, що 
    // цей метод "прикріплюється" до всіх об'єктів типу Transform.
    public static void DestroyAllChildren(this Transform parent)
    {
        // Проходимося по всіх дочірніх об'єктах у циклі
        // (Unity дозволяє ітерувати Transform безпосередньо)
        foreach (Transform child in parent)
        {
            // Знищуємо ігровий об'єкт, до якого прив'язаний цей Transform
            GameObject.Destroy(child.gameObject);
        }
    }
}

public class ExtensionMethodsExample : MonoBehaviour
{
    void Start()
    {
        // Тепер ви можете викликати DestroyAllChildren() безпосередньо 
        // на будь-якому компоненті Transform у вашому проєкті.
        transform.DestroyAllChildren();

        Debug.Log("Всі дочірні об'єкти були успішно знищені!");
    }
}


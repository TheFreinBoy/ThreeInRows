namespace Useful
{
    using UnityEngine;

    // Глобальний клас для зберігання рахунку гравця
    public class ScoreManager : MonoBehaviour
    {
        // Статична змінна для рахунку
        public static int Score = 0;

        // Статичний метод, який будь-хто може викликати
        public static void AddPoints(int points)
        {
            Score += points;
            Debug.Log($"Отримано {points} очок! Загальний рахунок: {Score}");
        }

        // Важливий крок для Unity: скидаємо рахунок, якщо сцена перезавантажується
        private void OnDestroy()
        {
            Score = 0; 
        }
    }

    // Тепер, наприклад, у скрипті монетки:
    public class Coin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Монетці не треба шукати ScoreManager на сцені.
                // Вона просто звертається до нього напряму, бо він static!
                ScoreManager.AddPoints(10);
            
                Destroy(gameObject); // Знищуємо монетку
            }
        }
    }
}
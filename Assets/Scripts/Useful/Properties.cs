using UnityEngine;
using UnityEngine.Events;

namespace Useful
{
    public class HealthSystem : MonoBehaviour
    {
        public int maxHealth = 100;
    
        // Подія, на яку може підписатися UI, щоб малювати смужку здоров'я
        public UnityEvent<int> OnHealthChanged;

        private int _currentHealth;

        // Створюємо розумну властивість
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                // 1. Обмежуємо значення (Mathf.Clamp не дасть здоров'ю впасти нижче 0 або перевищити maxHealth)
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);

                // 2. Автоматично повідомляємо гру, що здоров'я змінилося!
                // Тепер нам не треба вручну оновлювати UI щоразу, коли ми отримуємо шкоду.
                OnHealthChanged?.Invoke(_currentHealth);

                // 3. Перевірка на смерть
                if (_currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        void Start()
        {
            // Присвоюємо значення. Автоматично спрацює 'set', обмежить його і оновить UI.
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            // Ми просто віднімаємо здоров'я. Уся складна логіка сама відпрацює у властивості!
            CurrentHealth -= damage;
        }

        private void Die()
        {
            Debug.Log("Гравець помер!");
        }
    }
}
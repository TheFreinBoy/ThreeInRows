using UnityEngine;

namespace Useful
{
    
    // Базовий клас для всіх ворогів
    public class Enemy : MonoBehaviour
    {
        public string enemyName = "Невідомий ворог";
        public int health = 100;

        /* Ключове слово "virtual" означає, що цей метод можна 
        змінити (перевизначити) у дочірніх класах.*/
        public virtual void TakeDamage(int amount)
        {
            health -= amount;
            Debug.Log($"{enemyName} отримав {amount} шкоди. Залишилось ХП: {health}");

            if (health <= 0)
            {
                Die();
            }
        }

        // "protected" означає, що метод доступний лише в цьому класі та дочірніх
        protected virtual void Die()
        {
            Debug.Log($"{enemyName} помер.");
            Destroy(gameObject);
        }
        
        // Zombie наслідує все від Enemy
        public class Zombie : Enemy
        {
            private void Start()
            {
                enemyName = "Зомбі"; // Змінюємо змінну батьківського класу
                health = 150;
            }

            // "override" дозволяє нам змінити віртуальний метод батька
            public override void TakeDamage(int amount)
            {
                // base.TakeDamage викликає оригінальний код віднімання ХП з класу Enemy
                base.TakeDamage(amount); 
        
                // Додаємо специфічну поведінку ТІЛЬКИ для зомбі (наприклад, звук)
                Debug.Log("Зомбі ричить");
            }
        }
    }
    
                    // Приклад 2
    // Базовий клас для всього, з чим можна взаємодіяти
    public class Interactable : MonoBehaviour
    {
        public string promptText = "Натисніть E для взаємодії";

        // Базовий метод нічого не робить, він створений для того, 
        // щоб його перевизначали дочірні класи.
        public virtual void Interact()
        {
            Debug.Log("Взаємодія з базовим об'єктом");
        }
    }

// Дочірній клас - Скриня з лутом
    public class LootChest : Interactable
    {
        private void Start()
        {
            promptText = "Відкрити скриню";
        }

        public override void Interact()
        {
            Debug.Log("Скриня відкрилась! Ви отримали 50 золота.");
            // Тут логіка анімації та додавання предметів в інвентар
        }
    }

// Дочірній клас - Двері
    public class Door : Interactable
    {
        private bool isOpen = false;

        private void Start()
        {
            promptText = "Відкрити двері";
        }

        public override void Interact()
        {
            isOpen = !isOpen;
            Debug.Log(isOpen ? "Двері зі скрипом відчинились." : "Двері зачинились.");
            // Тут логіка анімації обертання дверей
        }
    }
    
}
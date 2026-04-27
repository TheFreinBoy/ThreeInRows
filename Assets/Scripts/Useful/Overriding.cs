using UnityEngine;

namespace Useful
{

    // Базовий клас для всіх здібностей
    public class BaseAbility : MonoBehaviour
    {
        public string abilityName;
        public int manaCost;

        // Спільна логіка для всіх здібностей
        public virtual void CastAbility(int currentMana)
        {
            if (currentMana < manaCost)
            {
                Debug.Log($"Недостатньо мани для {abilityName}!");
                return;
            }

            Debug.Log($"- Віднято {manaCost} мани.");
            Debug.Log($"- Програється стандартна анімація використання магії.");
        }
    }

    // Специфічне заклинання - Лікування
    public class HealAbility : BaseAbility
    {
        public int healAmount = 50;

        public override void CastAbility(int currentMana)
        {
            // 1. Спочатку виконуємо базову логіку (перевірка та витрата мани)
            base.CastAbility(currentMana);

            // Якщо базовий метод пройшов успішно, додаємо унікальний ефект:
            if (currentMana >= manaCost) 
            {
                Debug.Log($"[ЕФЕКТ] Гравець вилікувався на {healAmount} ХП!");
                // Логіка додавання здоров'я гравцю...
            }
        }
    }

    // Специфічне заклинання - Вогняна куля
    public class FireballAbility : BaseAbility
    {
        public override void CastAbility(int currentMana)
        {
            // 1. Спочатку виконуємо базову логіку
            base.CastAbility(currentMana);

            // 2. Унікальний ефект вогняної кулі
            if (currentMana >= manaCost)
            {
                Debug.Log($"[ЕФЕКТ] Вогняна куля вилетіла у ворога!");
                // Логіка створення префабу (Instantiate) вогняної кулі...
            }
        }
    }
}
using UnityEngine;
 
namespace Useful
{
    public interface IDamageable
    {
        void Damage(float damage); 
    }
    
    public class RaycastShooter : MonoBehaviour
    {
        public float damage = 20f;
        public float range = 100f;

        void Update()
        {
            // Стріляємо по кліку лівої кнопки миші
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        void Shoot()
        {
            // Пускаємо промінь з центру екрана (камери)
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                // Намагаємося отримати компонент, який реалізує IDamageable
                // Нам неважливо, чи це стіна, чи ворог!
                IDamageable target = hit.collider.GetComponent<IDamageable>();
            
                if (target != null)
                {
                    target.Damage(damage);
                    Debug.Log($"Влучили! Нанесли {damage} шкоди об'єкту {hit.collider.name}");
                }
                else
                {
                    Debug.Log("Влучили у щось невразливе (наприклад, звичайну стіну).");
                }
            }
        }
    }
}
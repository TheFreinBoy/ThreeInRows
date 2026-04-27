using UnityEngine;

namespace Useful
{
    public class Item { }

    public class Weapon : Item 
    {
        public void Equip() { Debug.Log("Зброю взято в руки!"); }
    }

    public class Potion : Item 
    {
        public void Drink() { Debug.Log("Зілля випито, здоров'я відновлено!"); }
    }

    public class InventorySystem : MonoBehaviour
    {
        // Апкастинг: масив містить різні предмети
        public Item[] inventory = new Item[] { new Weapon(), new Potion(), new Weapon() };

        public void UseItem(int index)
        {
            Item selectedItem = inventory[index];

            // Використовуємо сучасний Pattern Matching для Даункастингу
            if (selectedItem is Weapon weapon)
            {
                weapon.Equip();
            }
            else if (selectedItem is Potion potion)
            {
                potion.Drink();
            }
            else
            {
                Debug.Log("Цей предмет не можна використати.");
            }
        }
    }
}
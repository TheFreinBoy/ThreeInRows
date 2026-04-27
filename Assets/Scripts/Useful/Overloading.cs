using UnityEngine;
using System.Collections;

namespace Useful
{
    public class SomeClass
    {
        // Перша версія методу Add (Додати): працює з цілими числами.
        // Сигнатура методу: Add(int, int)
        public int Add(int num1, int num2)
        {
            // Тут оператор '+' виконує математичне додавання
            return num1 + num2;
        }

        // Друга версія методу Add: працює з текстом (рядками).
        // Сигнатура методу: Add(string, string)
        public string Add(string str1, string str2)
        {
            // Тут оператор '+' виконує конкатенацію (зшивання) тексту
            return str1 + str2;
        }
    }

    public class OverloadingExample : MonoBehaviour
    {
        void Start()
        {
            SomeClass myClass = new SomeClass();

            // Компілятор бачить, що ми передаємо два числа (1 та 2).
            // Тому він автоматично обирає ПЕРШУ версію методу.
            int sum = myClass.Add(1, 2);
            Debug.Log("Сума чисел: " + sum); // Виведе: Сума чисел: 3

            // Компілятор бачить, що ми передаємо два текстові рядки.
            // Тому він автоматично обирає ДРУГУ версію методу.
            string fullText = myClass.Add("Hello ", "World");
            Debug.Log("Повний текст: " + fullText); // Виведе: Повний текст: Hello World
        }
    }
}
using UnityEngine;
namespace Useful
{
    
    // Базовий клас для всього транспорту
    public class Vehicle
    {
        // Базова версія методу SoundHorn (Подати звуковий сигнал)
        // Тут немає 'virtual', отже метод не готовий до стандартного поліморфізму.
        public void SoundHorn()
        {
            Debug.Log("Звук невідомого транспорту: Бі-біп!");
        }
    }

    // Клас Car (Легковик) наслідує Vehicle
    public class Car : Vehicle
    {
        // Використовуємо 'new', щоб приховати батьківський метод.
        // Ми ігноруємо стандартний "Бі-біп" і робимо свій власний сигнал для легковика.
        new public void SoundHorn()
        {
            Debug.Log("Звук легковика: Фа-фа!");
        }
    }

    // Клас Truck (Вантажівка) наслідує Car
    public class Truck : Car
    {
        // Знову приховуємо метод, тепер уже метод класу Car, використовуючи 'new'.
        new public void SoundHorn()
        {
            Debug.Log("Звук вантажівки: ТУУУУУ-ТУУУУУ!");
        }
    }

    // Скрипт для тестування в Unity
    public class VehicleHidingExample : MonoBehaviour
    {
        void Start()
        {

            Vehicle someVehicle = new Vehicle();
            Vehicle myCar = new Car();
            Vehicle myTruck = new Truck();


            someVehicle.SoundHorn(); 
            myTruck.SoundHorn();     


            Debug.Log("--- Тепер використовуємо конкретні типи ---");

            Truck realTruck = new Truck();
            

            realTruck.SoundHorn();


            Car realCar = new Car();
            realCar.SoundHorn();   
        }
    }
}
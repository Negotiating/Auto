using System;
using System.Collections.Generic;

namespace Test
{
    internal class Program
    {
        
        enum AutoType//типы автомобилей, при необходимости может быть дополнен
        {
            B,
            C
        }
        class Auto
        {
            public AutoType Type {get; protected set;} //тип тс
            public float ConsumptionFuel { get; protected set; } //средний расход топлива
            public float FullFuel { get; protected set; } //объем полного бака
            public float Speed { get; protected set; } //скорость
            public float CurrentFuel { get; protected set; } //текущее кол-во топлива

            public Auto(AutoType type,float consumptionFuel, float fullFuel, float speed, float currenFuel)
            {
                Type = type;
                ConsumptionFuel = consumptionFuel;
                FullFuel = fullFuel;
                Speed = speed;
                CurrentFuel = currenFuel;
            }
            protected virtual float CalculateFuelRange(float fuel) => fuel / ConsumptionFuel;
            public override string ToString()
            {
                string res = "";
                switch (Type)
                {
                    case AutoType.B: res = "Легковой автомобиль"; break;
                    case AutoType.C: res = "Грузовой автомобиль"; break;
                }
                return res;
            }

            public void ShowCurrentFuelRange() => Console.WriteLine($"Текущий запас хода при текущем кол-ве топлива в баке: {RemainderWithCurrentTank()}");
            public void ShowFullFuelRange() => Console.WriteLine($"Текущий запас хода при полном баке: {RemainderWithFullTank()}");
            /// <summary>
            /// Посчет запаса хода при полном баке
            /// </summary>
            /// <returns></returns>
            public float RemainderWithFullTank() => CalculateFuelRange(FullFuel);
            /// <summary>
            /// Посчет запаса хода при текущем кол-ве топлива в баке
            /// </summary>
            /// <returns></returns>
            public float RemainderWithCurrentTank() => CalculateFuelRange(CurrentFuel);
            /// <summary>
            /// Подсчет времени для прохождения заданой дистанции
            /// </summary>
            /// <param name="distance">Дистанция</param>
            /// <returns></returns>
            public float CalculateTimeForDistance(float distance) => distance / Speed; 

        }
        class PassengerCar:Auto
        {
            public int MaxPassengers { get; protected set; } //Максимальное кол-во пассажиров
            public int CurrentPassengers { get; protected set; } //Текущее кол-во пассажиров

            private const float PowerReserve = 0.06F; //Уменьшение запаса хода на каждого пассажира
            public PassengerCar(float consumptionFuel, float fullFuel, float speed, float currenFuel, int maxPas, int currentPas)
                          :base(AutoType.B, consumptionFuel, fullFuel, speed, currenFuel)
            {
                MaxPassengers = maxPas;
                CurrentPassengers = currentPas;
                if (CurrentPassengers > MaxPassengers)
                    throw new ArgumentException($"Число пассажиров не может быть больше {MaxPassengers}");
            }
            protected override float CalculateFuelRange(float fuel) => base.CalculateFuelRange(fuel) * (1 - PowerReserve * CurrentPassengers);
        }
        class Truck : Auto
        {
            public float MaxCapasity { get; protected set; } //максимальная вместимоть грузовика
            public float CurrentLoad { get; protected set; } //текущая загруженность грузовика

            private const float PowerReserve = 0.04F; //Уменьшение запаса хода
            public Truck(float consumptionFuel, float fullFuel, float speed, float currenFuel, float maxCapasity, float currentLoad)
                          : base(AutoType.C, consumptionFuel, fullFuel, speed, currenFuel)
            {
                MaxCapasity = maxCapasity;
                CurrentLoad = currentLoad;
                if (CurrentLoad > MaxCapasity)
                    throw new ArgumentException($"Загрузка не может быть больше {MaxCapasity}");
            }
            protected override float CalculateFuelRange(float fuel) => base.CalculateFuelRange(fuel) * (1 - PowerReserve * (CurrentLoad / 200));
        }
        static void Main(string[] args)
        {
            try
            {
                List<Auto> AutoList = new List<Auto>()
            {
                new PassengerCar(5, 100, 60, 50, 4, 3),
                new Truck(10, 100, 30, 40, 300, 200)
            };
                foreach (Auto auto in AutoList)
                {
                    Console.WriteLine(auto.ToString());
                    auto.ShowCurrentFuelRange();
                    auto.ShowFullFuelRange();
                    Console.WriteLine($"Время для дистанции 30км: {auto.CalculateTimeForDistance(30)} ч.");
                    Console.WriteLine();
                }
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}

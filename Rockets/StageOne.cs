using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rockets
{
    class StageOne : Stages
    {
        public StageOne()
        {
            T = 120.0; // Время работы первой ступени, с
            Fmax = 4000*1000+941*1000; // Суммарная тяга двигателей первой ступени в ваккууме (4 + 1), Н
            Fmin = 810*4*1000+745*1000; // Суммарная тяга двигателей на старте (4 + 1), Н
            M = 267000.0; // Стартовая масса ракеты с космическим кораблём, кг
            angle = 73; // Конечный угол поворота ракеты относительно вертикальной оси за время работы первой ступени, град
            k = (43000-3400)/120*4+(94000-7500)/310; // Суммарный расход массы первой и второй ступеней, кг/с

            Calculate_TimeValues();
        }

        public void Calculate_All()
        {
            for (int i = 0; i < TimeValues.Count(); i++)
            {
                //Вычисление ускорения по имеющемуся дифф. уравнению
                Acc_XValues.Add(Calculate_AccX(TimeValues[i]));
                Acc_YValues.Add(Calculate_AccY(TimeValues[i], i));
                //Вычисление скорости с помощью ускорения методом Эйлера
                Speed_XValues.Add(Euler(SetStartValues(Speed_XValues, null, i), Acc_XValues[i])); 
                Speed_YValues.Add(Euler(SetStartValues(Speed_YValues, null, i), Acc_YValues[i]));
                //Вычисление перемещения с помощью скорости методом Эйлера
                Move_XValues.Add(Euler(SetStartValues(Move_XValues, null, i), Speed_XValues[i])); 
                Move_YValues.Add(Euler(SetStartValues(Move_YValues, null, i), Speed_YValues[i]));
                GValues.Add(Calculate_GValues(Move_YValues[Move_YValues.Count() - 1], 
                    Move_XValues[Move_YValues.Count() - 1]));
            }

            printParameters();
        }
        private double Calculate_AccX(double arg) // Функция ускорения по х
        {
            return ((Fmin + engineForceIncrease() * arg) * Math.Sin(GetAngleFunction() * arg)) / (M - k * arg);
        }
        private double Calculate_AccY(double arg, int i) // Функция ускорения по y
        {
            double g = 9.81;
            if (i != 0) g = GValues[i - 1];
            return ((Fmin + engineForceIncrease() * arg) * Math.Cos(GetAngleFunction() * arg)) / (M - k * arg) - g;
        }
        private double engineForceIncrease() // Коэффициент тяги
        {
            return (Fmax - Fmin) / T;
        }
    }
}

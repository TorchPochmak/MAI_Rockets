using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Rockets
{
    class StageTwo : Stages
    {
        public StageOne stageOne;

        public StageTwo()
        {
            T = 175.0; // Время работы второй ступени после отделения первой, с
            Fmin = 745000;  // Тяга двигателя второй ступени (Блок А) на уровне моря
            Fmax = 941000; // Тяга двигателя второй ступени в вакууме, Н
            angle = 24.5; // Конечный угол поворота ракеты относительно вертикальной оси за время работы второй ступени, град
            k = (94000-7500) / 310; // Расход массы второй ступени, кг/с
            M = 94000-120*k; // Масса ракеты с космическим кораблём после отделения первой ступени, кг
            Calculate_TimeValues();
        }

        public void Calculate_All() // Вычисление всех параметров полета итеративно
        {
            for (int i = 0; i < TimeValues.Count(); i++)
            {
                //Вычисление ускорения по имеющемуся дифф. уравнению
                Acc_XValues.Add(Calculate_AccX(TimeValues[i]));
                Acc_YValues.Add(Calculate_AccY(TimeValues[i], i));
                //Вычисление скорости с помощью ускорения методом Эйлера
                Speed_XValues.Add(Euler(SetStartValues(Speed_XValues, stageOne.Speed_XValues, i), Acc_XValues[i]));
                Speed_YValues.Add(Euler(SetStartValues(Speed_YValues, stageOne.Speed_YValues, i), Acc_YValues[i]));
                //Вычисление перемещения с помощью скорости методом Эйлера
                Move_XValues.Add(Euler(SetStartValues(Move_XValues, stageOne.Move_XValues, i), Speed_XValues[i]));
                Move_YValues.Add(Euler(SetStartValues(Move_YValues, stageOne.Move_YValues, i), Speed_YValues[i]));

                GValues.Add(Calculate_GValues(Move_YValues[Move_YValues.Count() - 1],
                    Move_XValues[Move_YValues.Count() - 1]));
            }

            printParameters();
        }
        private double Calculate_AccX(double arg) // Функция ускорения по х
        {
            return (Fmax * Math.Sin(stageOne.GetAngleRad() + GetAngleFunction() * arg)) / (M - k * arg);
        }
        private double Calculate_AccY(double arg, int i) // Функция ускорения по y
        {
            double g = 9.81;
            if (i != 0) g = GValues[i - 1];
            return (Fmax  * Math.Cos(stageOne.GetAngleRad() + GetAngleFunction() * arg)) / (M - k * arg) - g;
        }
    }
}

using System;
namespace Rockets
{
    class Stages
    {
        protected double T;//время работы ступени, с
        protected double Fmax;//Тяга двигателя в ваккуме, Н
        protected double Fmin;//Минимальная тяга двигателя, Н
        protected double M;//Масса ступени, кг
        protected double angle;//Конечный угол поворота ракеты относительно ОУ, град
        protected double k; //Расход массы ступени, кг/с
        protected double time_step = 0.01;// Шаг перерасчета всех параметров, с

        //Списки вычисленных параметров
        public List<double> GValues { get; protected set; } = new List<double> { };//Ускорение свободного падения
        public List<double> TimeValues { get; protected set; } = new List<double>{ };//Время полета ступени
        public List<double> Move_XValues { get; protected set; } = new List<double> { };//Координаты
        public List<double> Move_YValues { get; protected set; } = new List<double> { };
        public List<double> Speed_XValues { get; protected set; } = new List<double> { };//Скорость
        public List<double> Speed_YValues { get; protected set; } = new List<double> { };
        public List<double> Acc_XValues { get; protected set; } = new List<double> { };//Ускорение
        public List<double> Acc_YValues { get; protected set; } = new List<double> { };
        protected void Calculate_TimeValues() //Заполнить список секунд на каждом шаге
        {
            for(double i = 0; i < T; i += time_step)
            {
                TimeValues.Add(i);
            }
        }
        protected double Calculate_GValues(double yarg, double xarg)
        {
            yarg += Calculator.R / (Math.Cos(Math.Atan(xarg / Calculator.R)));
            double res = 6.67 * 6 * 1e13 / Math.Pow(yarg, 2.0);
            return res;
        }
        protected double GetAngleFunction()// рад/с
        {
            return angle * (Math.PI / 180) / T;
        }
        protected double SetStartValues(List<double> values, List<double> previousValues, int i)//get y[i]
        {
            if (i >= 1)
            {
                return values[i - 1];
            }
            else if (previousValues != null)
            {
                return previousValues[previousValues.Count() - 1];
            }
            else return 0.0;
        }
        protected void printParameters()
        {
            Console.WriteLine("Величины в конце работы ступени: " + this.GetType());
            Console.WriteLine("X = " + String.Format("{0:0.######}", Move_XValues[Move_XValues.Count() - 1]) + " м");
            Console.WriteLine("Y = " + String.Format("{0:0.######}", Move_YValues[Move_YValues.Count() - 1]) + " м");

            Console.WriteLine("Скорость по X: = " + String.Format("{0:0.######}", Speed_XValues[Speed_XValues.Count() - 1]) + " м/с");
            Console.WriteLine("Скорость по Y: = " + String.Format("{0:0.######}", Speed_YValues[Speed_YValues.Count() - 1]) + " м/с");

            double sp = Math.Sqrt(Math.Pow(Speed_XValues[Speed_XValues.Count() - 1], 2.0) + Math.Pow(Speed_YValues[Speed_YValues.Count() - 1], 2.0));

            Console.WriteLine("Полная скорость: = " + String.Format("{0:0.######}", sp) + " м/с");

            Console.WriteLine("Ускорение по X: = " + String.Format("{0:0.######}", Acc_XValues[Acc_XValues.Count() - 1]) + " м/с^2");
            Console.WriteLine("Ускорение по Y: = " + String.Format("{0:0.######}", Acc_YValues[Acc_YValues.Count() - 1]) + " м/с^2");

            double acc = Math.Sqrt(Math.Pow(Acc_XValues[Acc_XValues.Count() - 1], 2.0) + Math.Pow(Acc_YValues[Acc_YValues.Count() - 1], 2.0));

            Console.WriteLine("Полное ускорение: = " + String.Format("{0:0.######}", acc) + " м/с^2");

            Console.WriteLine();
        }

        //Метод Эйлера
        //Получается вместо кривой ломаная из касательных,  пересчитываемых каждый time_step
        //Сделаем так для скорости и перемещения, исходя из ускорения, ведь оно является первой и второй производной
        //Y[i+1] = Y[i] + h * f(x,y);, где f(x,y) - производная
        public double Euler(double arg1, double arg2)
        {
            return arg1 + time_step * arg2;
        }
        public double GetAngleRad()
        {
            return GetAngleFunction() * T;
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Calculator calc = new Calculator();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Rockets
{
    class Calculator
    {
        public const double R = 600000.0; // Радиус Земли
        public StageOne stageOne = new StageOne();
        public StageTwo stageTwo = new StageTwo();
        public Calculator()
        {
            evaluateParameters();
            writeResults();
        }
        private void writeResults()
        {
            using (StreamWriter sw = new StreamWriter("C:\\Users\\Никита\\out.txt"))
            {
                //Код для кривизны
                var xAxisValues = new List<double> { };
                var yAxisValues = new List<double> { };

                xAxisValues.AddRange(stageOne.Move_XValues);
                xAxisValues.AddRange(stageTwo.Move_XValues);

                yAxisValues.AddRange(stageOne.Move_YValues);
                yAxisValues.AddRange(stageTwo.Move_YValues);

                yAxisValues = curve(yAxisValues, xAxisValues);

                Console.WriteLine("Высота орбиты с поправкой кривизны Земли: = " +
                    String.Format("{0:0.######}", yAxisValues[stageOne.Move_XValues.Count() - 1]) + " м");

                Console.WriteLine("Высота орбиты с поправкой кривизны Земли: = " + 
                    String.Format("{0:0.######}", yAxisValues[yAxisValues.Count() - 1]) + " м");
                //Запись в файл
                sw.WriteLine("t H G Sx Sy Vx Vy Ax Ay");
                for (int i = 0; i < stageOne.Move_XValues.Count(); i++)
                {
                    sw.WriteLine($"{stageOne.TimeValues[i]} {yAxisValues[i]} {stageOne.GValues[i]} {stageOne.Move_XValues[i]} " +
                        $"{stageOne.Move_YValues[i]} {stageOne.Speed_XValues[i]} {stageOne.Speed_YValues[i]} " +
                        $"{stageOne.Acc_XValues[i]} {stageOne.Acc_YValues[i]}");

                }
                for (int i = 0; i < stageTwo.Move_XValues.Count(); i++)
                {

                    sw.WriteLine($"{stageOne.TimeValues[stageOne.TimeValues.Count()-1] + stageTwo.TimeValues[i]} {yAxisValues[i + stageOne.Move_XValues.Count()]} " +
                        $"{stageTwo.GValues[i]} {stageTwo.Move_XValues[i]} " +
                        $"{stageTwo.Move_YValues[i]} {stageTwo.Speed_XValues[i]} {stageTwo.Speed_YValues[i]} " +
                        $"{stageTwo.Acc_XValues[i]} {stageTwo.Acc_YValues[i]}");
                }
            }
        }
        private void evaluateParameters()
        {
            stageTwo.stageOne = stageOne;

            stageOne.Calculate_All();
            stageTwo.Calculate_All();
        }
        // Расчёт поправки кривизны
        public List<double> curve(List<double> Yvals, List<double> xVals)
        {
            List<double> res = new List<double> { };
            for(int i = 0; i < Yvals.Count(); i++)
            {
                res.Add(Yvals[i] + (R / (Math.Cos(Math.Atan(xVals[i] / R))) - R));
            }
            return res;
        }
    }
}

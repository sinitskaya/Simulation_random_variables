using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_Моделирование_случайных_величин
{
    class RandomValue
    {
        public void RandomFunc(double veroatnost, int kol_experiment, Dictionary<int, int> map)
        {
            Random rnd = new Random(); //Инициализирует новый экземпляр Random класс с помощью начального значения по умолчанию, зависящего от времени.
            //при =100 при каждом запуске пррограммы будут значения одни и те же, т е с 2 до Nго запуска программы значения будут те же, что и при первом запуске. 
            for (int i = 0; i < kol_experiment; i++)
            {
                int index = -1;
                int Value = 0;///число полученных ответов, студент ответил
                while (index != 0)//пока студент не ответит
                {
                    double rndvalue = Convert.ToDouble(rnd.Next(1000) )/ 1000;
                    //Возвращает неотрицательное случайное целое число, которое меньше максимально допустимого значения.
                    //которое больше или равно 0 и меньше чем maxValue; то есть диапазон возвращаемых значений включает 0, но не maxValue. Однако если maxValue равно 0, maxValue возвращается.
                    if (rndvalue < veroatnost)
                    {
                        Value = Value + 1;
                        index = 1;
                    }
                    else index = 0;
                }
                ///////////////////////
                if (!map.ContainsKey(Value))//ContainsKey(TKey)
                {
                    map.Add(Value, 0);
                }
                map[Value] = map[Value]+1;//Add(TKey key, TValue value);TryGetValue(TKey, TValue)
            }
        }
        public double RandomAverange(Dictionary<int, int> map, int kol_experiment)//выборочное среднее
        {//по форм из методички
            double x, sumx=0;
            for (int i=0; i<map.Count; i++)
                sumx = map.ElementAt(i).Value * map.ElementAt(i).Key + sumx;
            x = ((Double)1/kol_experiment)*sumx;
            return x;
        }

        public double SelectiveVariable(Dictionary<int,int> map, int kol_experiment)//выборочная дисперсия
        {//по форм из методички
            double s = 0;
            for (int i = 0; i < map.Count; i++)
                s = map.ElementAt(i).Value * Math.Pow((map.ElementAt(i).Key - RandomAverange(map, kol_experiment)), 2)+s;
            return s = ((Double)1 / kol_experiment) * s;
        }

        public int RangeSelection(Dictionary<int,int> map)//размах выборки
        {//по форм из методички
            int R;
            R = map.Last().Key - map.First().Key;
            return R;
        }

        public double MathExpectation(double veroatnost)//матиматическое ожидание
        {
            double MO = veroatnost / (1.0 - veroatnost);
            return MO;
        }

        public double Dispersion(double veroatnost)//дисперсия 
        {
            double D = (Double)veroatnost / ( (1 - veroatnost)*(1.0 - veroatnost)) ;
            return D;
        }

        public double Mediana(Dictionary<int,int> map, int kol_experiment)//выборочная медиана
        {//по форм из методички             
            double Me, Xk, Xk1;       
            int sum = 0, i = -1, k = kol_experiment / 2;
            if (kol_experiment % 2 == 1)//нечетн
                k = k + 1;

            if (kol_experiment != 1)
            {
                while ((sum < k) && (sum != k))
                {
                    i++;
                    sum += map.ElementAt(i).Value;
                    
                }
                Xk = map.ElementAt(i).Key;
                if (kol_experiment % 2 == 1)//нечетн
                    return Me = Xk;
                //если четн продолжаем
                if (k - sum == 0)
                {
                    Xk1 = map.ElementAt(i + 1).Key;
                    return Me = (Double)(Xk + Xk1) / 2;
                }
                Xk1 = map.ElementAt(i).Key;
                return Me = (Double)(Xk + Xk1) / 2;             
            }
            else return Me = map.Last().Key;
        }

        public double Veroatnost(double veroatnost, int n)
        {//по формуле геометр распред
            double ver = Math.Pow(veroatnost, n) * (1 - veroatnost);//p*q^n - до наблюдения первой удачи
            return ver;                                           //p^n*q - до наблюдения первой неудачи - мой вар
        }

        public double MeasureOfDiscrepancy(Dictionary<int, int> map, double veroatnost, int kol_experiment)//мера расхождения
        {
            double maxD = 0, value1 = 0, value2 = 0, raz = 0;
            for (int i = 0; i < map.Count; i++)
            {
                value1 = value1 + Veroatnost(veroatnost, map.ElementAt(i).Key);
                value2 = value2 + (Double)map.ElementAt(i).Value / kol_experiment;//частота
                raz = Math.Abs(value1 - value2);
                if (maxD < raz)
                    maxD = raz; 
            }
            return maxD;
        }

        public double MaxOtclonenie(Dictionary<int,int> map, int kol_experiment, double veroatnost)
        {
            double frequency, probability, max = -1;
            for(int i=0; i<map.Count; i++)
            {
                frequency = (Double)map.ElementAt(i).Value / kol_experiment;//частота
                probability = Veroatnost(veroatnost, map.ElementAt(i).Key);//по формуле геом распр
                double m = Math.Abs(frequency - probability);
                if (max < m)
                    max = m;
            }
            return max;
        }
        ///////////////////часть 3
        public int[] N_J(Dictionary<int, int> map, double[] zi)// число наблюдений попавших в delta(j)
        {
            int[] nj = new int[zi.Length + 1];//число наблюдений попавших в delta(j)
            for (int k = 0; k < zi.Length; k++)
            {
                for (int i = 0; i < map.Count; i++)
                {
                    if (k == 0)//если первый
                    {
                        if (zi[k] > map.ElementAt(i).Key)
                            nj[k] = nj[k] + map.ElementAt(i).Value;
                    }
                    else
                    {
                        if (zi[k - 1] <= map.ElementAt(i).Key && map.ElementAt(i).Key < zi[k])
                            nj[k] = nj[k] + map.ElementAt(i).Value;//(k != 0 || k != zi.Length) если средний
                    }
                }
            }
            int k1 = zi.Length - 1;//если последний
            for (int i = 0; i < map.Count; i++)
            {
                if (map.ElementAt(i).Key > zi[k1])
                    nj[k1+1] = nj[k1+1] + map.ElementAt(i).Value;
            }
            return nj;
        }

        public double[] QJ(int kol_intervalov, double[] zi, Dictionary<int,int> map, double veroatnost)
        {//qj
            double[] qj = new double[kol_intervalov];
            int i = 0;
            for (int k = 0; k < kol_intervalov-1; k++)//без последнего интервала
            {
                if (k == 0)//если первый
                {
                    while (i < zi[k])
                    {
                        qj[k] = qj[k] + Veroatnost(veroatnost, i);
                        i++;
                    }
                }
                else {
                    while (zi[k - 1] <= i && i < zi[k])
                    {
                        qj[k] = qj[k] + Veroatnost(veroatnost, i);
                        i++;
                    }
                }
            }
            int k1 = kol_intervalov-1;//если последний
            qj[k1] = 1;
            for (int j = 0; j < k1; j++)
                qj[k1] = qj[k1] - qj[j]; 
            return qj;
        }

        public double R0(int [] nj, double [] qj, int kol_intervalov, int kol_experiment)
        {// vмера расхождения между наблюдавшимися частотами и ожидаемым числом попаданий в интервал при нулевой гипотезы - в кач статистики критерия
            double R0 = 0;
            for (int j = 0; j < kol_intervalov; j++)
                R0 = R0 + (Double)Math.Pow((nj[j] - kol_experiment*qj[j]), 2) / qj[j];
            R0 = (Double)R0 / kol_experiment;
            return R0;
        }

        public double L(double a)
        {
            if (a == 1)
                return 1;
            else if (a == (Double)1/2)
                return 1.77245;
            else
                return (a-1)*L(a-1);
        }

        public double G(double x, int kol_intervalov)
        {
            int r = kol_intervalov - 1;
            if (x == 0)
                return 0;
            return Math.Pow( x, (Double) r / 2 - 1)* Math.Exp((Double)(-x) / 2);
        }

        public double FR0(int[] nj, double[] qj, int kol_intervalov, int kol_experiment)
        {
            int n = 1000;
            int r = kol_intervalov - 1;
            double r0 = R0(nj, qj, kol_intervalov, kol_experiment);
            double sum = 0;
            for (int i = 1; i <= n; i++)
                sum += G(r0 * (Double)(i - 1) / n, kol_intervalov) + G(r0 * (Double)i / n, kol_intervalov);
            sum = sum * (Double)r0 / (2 * n);
            sum = sum * (Double)Math.Pow(2, (Double)(-r) / 2) / L( (Double)r/2 );
            return 1 - sum;
        }
    }
}
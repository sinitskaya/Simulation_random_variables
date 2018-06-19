using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Numerics;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

/*using System.ComponentModel;
using System.Data;

using System.Text;
using System.Threading.Tasks;
*/

namespace lab1_Моделирование_случайных_величин
{
    public partial class Form1 : Form
    {
        Dictionary<int, int> map_ = new Dictionary<int, int>();
        RandomValue RNDValue_ = new RandomValue();//чтобы доступ был везде и мог обновляться, иначе все суммируется

        public Form1()
        {
            InitializeComponent();
        }
        double veroatnost;
        int kol_experiment;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                veroatnost = double.Parse(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Некорректные данные");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)//кол
        {
            try
            {
                kol_experiment = int.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Некорректные данные");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<int, int> map = new Dictionary<int, int>();
                map_ = map;
                RandomValue RNDValue = new RandomValue();
                RNDValue_ = RNDValue;
                //dataGridView.ClearSelection();
                RNDValue.RandomFunc(veroatnost, kol_experiment, map);
                map = map.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                //Сортирует элементы последовательности в возрастающем порядке по ключу. 
                //OrderBy<KeyValuePair<TKey, TValue>, TKey>(Func<KeyValuePair<TKey, TValue>, TKey>)

                Random d1 = new Random();

                dataGridView1.Size = new Size(680, 130);
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.RowCount = 4;
                dataGridView1.ColumnCount = map.Count; //случайная величина (row-строка,column-столбец)

                dataGridView1.Rows[0].HeaderCell.Value = "Различные значения случайной величины";
                dataGridView1.Rows[1].HeaderCell.Value = "Кол эспериментов";
                dataGridView1.Rows[2].HeaderCell.Value = "Частота";
                dataGridView1.Rows[3].HeaderCell.Value = "Вероятность";

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Rows[0].Cells[i].Value = map.ElementAt(i).Key.ToString();  //вывод случ величин
                    dataGridView1.Rows[1].Cells[i].Value = map.ElementAt(i).Value.ToString();
                    dataGridView1.Rows[2].Cells[i].Value = Math.Round(((Double)map.ElementAt(i).Value / kol_experiment), 7).ToString();
                    //dataGridView1.Rows[3].Cells[i].Value = Math.Round((Math.Pow(veroatnost, i) * Math.Pow(1 - veroatnost, i + 1 - i) * (Double)(Factorial(i + 1) / (Factorial(i + 1 - i) * Factorial(i)))), 4).ToString();
                    //dataGridView1.Rows[3].Cells[i].Value = Math.Round( (1-veroatnost) * Math.Pow(veroatnost,i), 4).ToString();
                    dataGridView1.Rows[3].Cells[i].Value = Math.Round(RNDValue.Veroatnost(veroatnost, map.ElementAt(i).Key), 7).ToString();

                }
                TextBox_maxOtclonenie.Text = RNDValue.MaxOtclonenie(map, kol_experiment, veroatnost).ToString();
                //////////////////////////////////////////////
                //числовые характеристики
                dataGridView2.RowCount = 1;
                dataGridView2.ColumnCount = 9;
                dataGridView2.Columns[0].HeaderText = "E(η)";//мат ожидание
                dataGridView2.Columns[1].HeaderText = "x";//выборочное среднее
                dataGridView2.Columns[2].HeaderText = "| E(η) - x |";
                dataGridView2.Columns[3].HeaderText = "D(η)";//дисперсия
                dataGridView2.Columns[4].HeaderText = "S^2";//выборочная дисперсия
                dataGridView2.Columns[5].HeaderText = "| D(η) - S^2 |";
                dataGridView2.Columns[6].HeaderText = "Me";//медиана
                dataGridView2.Columns[7].HeaderText = "R";//размах выборки
                dataGridView2.Columns[8].HeaderText = "D-мера расхождения";//мера расхождения

                dataGridView2.Rows[0].Cells[0].Value = Math.Round( RNDValue.MathExpectation(veroatnost), 4 ).ToString();
                //( (Double)((1-veroatnost)/(veroatnost*veroatnost)) ).ToString();  
                dataGridView2.Rows[0].Cells[1].Value = Math.Round(RNDValue.RandomAverange(map, kol_experiment), 4).ToString(); //выборочное среднее
                dataGridView2.Rows[0].Cells[2].Value = Math.Round( Math.Abs( RNDValue.MathExpectation(veroatnost)-RNDValue.RandomAverange(map, kol_experiment) ), 4 ).ToString();
                dataGridView2.Rows[0].Cells[3].Value = Math.Round(RNDValue.Dispersion(veroatnost), 4).ToString(); //дисперсия
                dataGridView2.Rows[0].Cells[4].Value = Math.Round(RNDValue.SelectiveVariable(map, kol_experiment), 4).ToString(); //выборочное среднее
                dataGridView2.Rows[0].Cells[5].Value = Math.Round(Math.Abs(RNDValue.Dispersion(veroatnost) - RNDValue.SelectiveVariable(map, kol_experiment)), 4).ToString();
                dataGridView2.Rows[0].Cells[6].Value = Math.Round(RNDValue.Mediana(map, kol_experiment), 4).ToString(); //медиана
                dataGridView2.Rows[0].Cells[7].Value = RNDValue.RangeSelection(map).ToString(); //размах выборки int
                dataGridView2.Rows[0].Cells[8].Value = RNDValue.MeasureOfDiscrepancy(map, veroatnost, kol_experiment).ToString();//мера расхождения
                ////////////////////////////////////////////////////

                chart1.Series.Clear();
                Series SeriesOfPoints_Selective = new Series("ИНФ выборочная");
                SeriesOfPoints_Selective.ChartType = SeriesChartType.Line;
                SeriesOfPoints_Selective.Points.AddXY(-1, 0);
                SeriesOfPoints_Selective.Color = Color.Green;
                SeriesOfPoints_Selective.BorderWidth = 3;

                double value = 0;
                for (int i = 0; i < map.Count; i++)
                {
                    SeriesOfPoints_Selective.Points.AddXY(map.ElementAt(i).Key, value);
                    value = value + (Double)map.ElementAt(i).Value / kol_experiment;//частота
                    SeriesOfPoints_Selective.Points.AddXY(map.ElementAt(i).Key, value);
                }
                SeriesOfPoints_Selective.Points.AddXY(map.Last().Key + 1, value);
                chart1.Series.Add(SeriesOfPoints_Selective);
                ///////////////////////////////////////////////////////////////////////////////////////////////

                Series SeriesOfPoints_Teoretical = new Series("ИНФ теоретическая");
                SeriesOfPoints_Teoretical.ChartType = SeriesChartType.Line;
                SeriesOfPoints_Teoretical.Points.AddXY(0, 0);
                SeriesOfPoints_Teoretical.Color = Color.Black;
                SeriesOfPoints_Teoretical.BorderWidth = 2;
                value = 0; //значение функции
                for (int i = 0; i < map.Count; i++)
                {
                    SeriesOfPoints_Teoretical.Points.AddXY(map.ElementAt(i).Key, value);
                    value = value + RNDValue.Veroatnost(veroatnost, map.ElementAt(i).Key);
                    SeriesOfPoints_Teoretical.Points.AddXY(map.ElementAt(i).Key, value);
                }
                SeriesOfPoints_Teoretical.Points.AddXY(map.Last().Key + 1, value);
                chart1.Series.Add(SeriesOfPoints_Teoretical);
                //MeasureOfDiscrepancy(Dictionary<int, int> map, double veroatnost, int kol_experiment)
            }
            catch
            {
                MessageBox.Show("error1");
            }

        }
    ///////////////////////////////////////
        BigInteger Factorial(int value)
        {
            BigInteger result = new BigInteger(1);
            for (int i = 0; i < value; i++)
            {
                result += result * i;
            }
            return result;
        }

        //часть3
        int kol_intervalov = 0;
        private void textBox_k_TextChanged(object sender, EventArgs e)
        {
            try
            {
                kol_intervalov = int.Parse(textBox_k.Text);

                dataGridView3.RowCount = 1;
                dataGridView3.ColumnCount = kol_intervalov-1;

                dataGridView4.RowCount = 1;
                dataGridView4.ColumnCount = kol_intervalov;
                for (int i = 0; i < kol_intervalov - 1; i++)
                {
                    string s = "z" + (i+1).ToString();
                    dataGridView3.Columns[i].HeaderText = s;
                }

                for (int i = 0; i < kol_intervalov; i++)
                {
                    string s1 = "q" + (i + 1).ToString();
                    dataGridView4.Columns[i].HeaderText = s1;
                }

            }
            catch
            {
                MessageBox.Show("Некорректные данные1");
            }
        }

        double alpha;
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                alpha = double.Parse(textBox3_alpha.Text);
            }
            catch
            {
                MessageBox.Show("Некорректные данные3");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {// работает только с учетом нажатия первой кнопки
            try
            {
                double [] zi = new double[kol_intervalov-1];//границы интервалов
                double[] qj = new double[kol_intervalov];
                int[] nj = new int[kol_intervalov];//число наблюдений попавших в delta(j)
                //ввод границ интервалов
                for (int i = 0; i < kol_intervalov-1; i++)
                {
                    zi[i] = double.Parse(dataGridView3.Rows[0].Cells[i].Value.ToString());//dataGridView1[0, i].Value.ToString();0-строка
                    // TextBox_maxOtclonenie.Text = zi[i].ToString();
                }
                qj = RNDValue_.QJ(kol_intervalov, zi, map_, veroatnost);
                nj = RNDValue_.N_J(map_, zi);

                for (int i = 0; i < kol_intervalov; i++)
                {
                    dataGridView4.Rows[0].Cells[i].Value = qj[i].ToString();
                }

                //double r0 = RNDValue.R0(nj,qj,kol_intervalov, kol_experiment);
                double fr0 = Math.Round(RNDValue_.FR0(nj, qj, kol_intervalov, kol_experiment), 5);
                textBox4_FR0.Text = fr0.ToString();

                if (alpha != 0)
                {
                    if (fr0 > alpha)
                        textBox4_Hypothesis.Text = "Гипотеза принята";
                    else
                        textBox4_Hypothesis.Text = "Гипотеза отклонена";
                }
            }
            catch
            {
                //MessageBox.Show("Некорректные данные2");
            }
        }        
        ////////////////////////////////////////
    }
}

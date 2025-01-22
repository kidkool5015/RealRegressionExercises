using System.Text;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;
using ScottPlot.WPF;
using OfficeOpenXml;
using System.IO;
using ScottPlot.Plottables;
using System.Diagnostics;
using ScottPlot.Colormaps;
using OpenTK.Graphics.OpenGL;


namespace RealRegressionExercises
{

    public partial class MainWindow : Window
    {
        int Col1;
        int Col2;
        Calcs Maths = new Calcs();
        Plotting Plotting = new Plotting();
        string file_path = "C:\\Users\\lucky\\OneDrive\\Desktop\\ShoppingData.xlsx";


        public MainWindow()
        {

            


        }

        /// <summary>
        /// Performs the Calculation for the Linear Regression Rate and displays it.
        /// </summary>
        /// <param name="x"> The x values for the input data </param>
        /// <param name="y"> The x values for the input data </param>
        /// <returns> Nothing. </returns>

        public void Regression(double[] x, double[] y, double intercept)
        {
            double Coeff = Maths.GetCoeff(x, y);
            Trace.WriteLine(Coeff);
            double[] yFit = Plotting.GetFit(Coeff, x, intercept);

            double xMin = Maths.GetMin(x);
            double xMax = Maths.GetMax(x);

            double yMin = Maths.GetMin(y);
            double yFitMax = Maths.GetMax(yFit);



            MyWpfPlot.Plot.Add.ScatterPoints(x,yFit);
            


        }
        
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Click");
            Program();

        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            Col1 = int.Parse(text);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //Introduce some catches for wrong input
            string text2 = ((TextBox)sender).Text;
            Col2 = int.Parse(text2);
        }


        /// <summary>
        /// Puts into action the program, calculates the relavant data, plots it, and displays information.
        /// </summary>

        public void Program()
        {
            double[] xData = Plotting.GetData(file_path, Col1);
            double[] yData = Plotting.GetData(file_path, Col2);
            double yIntercept = Plotting.Intercept(Maths.GetMean(xData), Maths.GetMean(yData), Maths.GetCoeff(xData, yData));
            double[] yFit = Plotting.GetFit(Maths.GetCoeff(xData, yData), xData, yIntercept);



            InitializeComponent();

            MyWpfPlot.Plot.Title("Example Plot");
            MyWpfPlot.Plot.XLabel("X-Axis");
            MyWpfPlot.Plot.YLabel("Y-Axis");


            MyWpfPlot.Plot.Add.ScatterPoints(xData, yData);
            Regression(xData, yData, yIntercept);

            BottomBox.Text = "Slope = " + Math.Round(Maths.GetCoeff(xData, yData), 2) + "\n" +
                "your R2 value for the fit is: " + Math.Round(Maths.doubleR(yData, yFit), 3);

            MyWpfPlot.Plot.Axes.AutoScale();
            MyWpfPlot.Refresh();
        }

    }

    public class Plotting {

        /// <summary>
        /// Gets the data from an input excel file.
        /// </summary>
        /// <param name="x"> The x values for the input data </param>
        /// <param name="y"> The x values for the input data </param>
        /// <returns> Nothing. </returns>
        public double[] GetData(string file_path, int col = 2 )
        {


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(file_path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                double[] data = new double[rowCount];
                Trace.WriteLine(col);
                for (int i = 2; i <= rowCount / 30; i++)
                {

                    data[i - 2] = (double)worksheet.Cells[i, col].Value;

                }

                data = data.Where(x => x != 0).ToArray();




                return data;
            }


        }

        public double[] GetFit(double Coeff, double[] data, double intercept)
        {
            double[] yFit = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                yFit[i] = (Coeff * data[i]) + intercept;
            }
            return yFit;


        }

        public double Intercept(double xMean, double yMean, double Coeff)
        {
            return yMean - Coeff * xMean;
        }

        

    }
    
    public class Calcs
    {

        public double GetCoeff(double[] xData, double[] yData)
        {
            double CoVar = 0.0;
            double StDevX = 0.0;
            double MeanX = GetMean(xData);
            double MeanY = GetMean(yData);
            double Coeff = 0.0;

            int dataSize = xData.Length;

            for (int i = 0; i < dataSize; i++)
            {
                CoVar += (xData[i] - MeanX) * (yData[i] - MeanY);
                StDevX += Math.Pow(xData[i] - MeanX, 2);
            }

            Coeff = CoVar / StDevX;

            return Coeff;
        }

        public double GetMin(double[] data)
        {
            double min = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
            }
            return min;
        }
        public double GetMax(double[] data)
        {
            double max = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > max)
                {
                    max = data[i];
                }
            }
            return max;
        }
        public double GetMean(double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / data.Length;
        }

        public double ResSum(double[] actual, double[] fit)
        {
            double ResSum = 0.0;
            for (int i = 0; i < actual.Length; i++)
            {
                ResSum += Math.Pow(actual[i] - fit[i], 2);
            }
            return ResSum;
        }

        public double doubleR(double[] actual, double[] fit)
        {

            double RestSum = 0.0;
            double TotalSum = 0.0;
            double adjR = 0.0;

            for (int i = 0; i < actual.Length; i++)
            {
                RestSum = this.ResSum(actual, fit);
                TotalSum += Math.Pow(actual[i] - this.GetMean(actual), 2);
            }
            Trace.WriteLine("RestSum:" + RestSum / TotalSum);

            adjR = 1 - (RestSum / TotalSum);

            return 1 - ((1 - adjR) * (actual.Length - 1) / (actual.Length - 3));
        }

    }
}

//Make Classes for Data, Math, Plotting (Main Program with button presses/word boxes)
//Function to modify text box to include various columns


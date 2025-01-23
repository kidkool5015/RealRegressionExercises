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
        string file_path;


        public MainWindow()
        {

            


        }

        public class OpenFileDialog
        {
            public string file_path;
            public OpenFileDialog()
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".xlsx";
                dlg.Filter = "Excel Files (*.xlsx)|*.xlsx";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    file_path = dlg.FileName;
                }
            }
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

        /// <summary>
        /// Handles the button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Click");
            Program();

        }

        /// <summary>
        /// Handles the text changed event for the first text box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            Col1 = int.Parse(text);
        }

        /// <summary>
        /// Handles the text changed event for the second text box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //Introduce some catches for wrong input
            string text2 = ((TextBox)sender).Text;
            Col2 = int.Parse(text2);
        }


        /// <summary>
        /// Executes the main program logic, calculates relevant data, plots it, and displays information.
        /// </summary>

        public void Program()
        {
            //double[] xData = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //double[] yData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 25 };
            double[] xData = Plotting.GetData(file_path, 1);
            double[] yData = Plotting.GetData(file_path, 2);
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

        /// <summary>
        /// Handles the button click event to open a file dialog.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file_path = file.file_path;
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

        /// <summary>
        /// Calculates the fitted values for the given data using the coefficient and intercept.
        /// </summary>
        /// <param name="Coeff">The coefficient of the linear regression.</param>
        /// <param name="data">The input data array.</param>
        /// <param name="intercept">The intercept of the linear regression.</param>
        /// <returns> An array of doubles containing the fitted values.</returns>
        public double[] GetFit(double Coeff, double[] data, double intercept)
        {
            double[] yFit = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                yFit[i] = (Coeff * data[i]) + intercept;
            }
            return yFit;


        }

        /// <summary>
        /// Calculates the intercept for the linear regression.
        /// </summary>
        /// <param name="xMean">The mean of the x values.</param>
        /// <param name="yMean">The mean of the y values.</param>
        /// <param name="Coeff">The coefficient of the linear regression.</param>
        /// <returns>The intercept of the linear regression model .</returns>

        public double Intercept(double xMean, double yMean, double Coeff)
        {
            return yMean - Coeff * xMean;
        }

        

    }
    
    public class Calcs
    {
        /// <summary>
        /// Calculates the coefficient for the linear regression.
        /// </summary>
        /// <param name="xData">The x values for the input data.</param>
        /// <param name="yData">The y values for the input data.</param>
        /// <returns>The coefficient of the linear regression model.</returns>
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

        /// <summary>
        /// Gets the minimum value from the data array.
        /// </summary>
        /// <param name="data">The input data array.</param>
        /// <returns>The minimum value in the data set as a double.</returns>
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

        /// <summary>
        /// Gets the maximum value from the data array.
        /// </summary>
        /// <param name="data">The input data array.</param>
        /// <returns>The maximum value in the data array as a double.</returns>
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

        /// <summary>
        /// Calculates the mean value of the data array.
        /// </summary>
        /// <param name="data">The input data array.</param>
        /// <returns>The mean value of the data array as a double.</returns>
        public double GetMean(double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / data.Length;
        }

        /// <summary>
        /// Calculates the residual sum of squares between the actual and fitted values.
        /// </summary>
        /// <param name="actual">The actual data values.</param>
        /// <param name="fit">The fitted data values.</param>
        /// <returns>The residual sum of squares.</returns>
        public double ResSum(double[] actual, double[] fit)
        {
            double ResSum = 0.0;
            for (int i = 0; i < actual.Length; i++)
            {
                ResSum += Math.Pow(actual[i] - fit[i], 2);
            }
            return ResSum;
        }

        /// <summary>
        /// Calculates the adjusted R-squared value for the fit.
        /// </summary>
        /// <param name="actual">The actual data values.</param>
        /// <param name="fit">The fitted data values.</param>
        /// <returns>The adjusted R-squared value as a double.</returns>
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

            return Math.Max(1 - ((1 - adjR) * (actual.Length - 1) / (actual.Length - 3)), 0);
        }

    }
}

//Function to modify text box to include various columns
//If more than 2 total columns of numerical data, allows user to choose which two from list and inputs
//Function to optimize fit
//Maybe non-linear regression?


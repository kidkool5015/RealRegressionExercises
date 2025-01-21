using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using ScottPlot;
using ScottPlot.WPF;
using OfficeOpenXml;
using System.IO;
using ScottPlot.Plottables;
using OpenTK.Graphics.OpenGL;
using System.Reflection.Metadata.Ecma335;
using System.Drawing.Imaging;
using System.Diagnostics;
using ScottPlot.Colormaps;


namespace RealRegressionExercises
{

    public partial class MainWindow : Window
    {
        int Col1;
        int Col2;

        public MainWindow()
        {



        }



        public double[] GetData(int col = 2)
        {
            string file_path = "C:\\Users\\lucky\\OneDrive\\Desktop\\ShoppingData.xlsx";
            

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(file_path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                double[] data = new double[rowCount];
                Trace.WriteLine(col);
                for (int i = 2; i <= rowCount/30 ; i++)
                {
                    
                    data[i - 2] = (double)worksheet.Cells[i, col].Value;
                    
                }
                
                data = data.Where(x => x != 0).ToArray();


                return data;
            }
           

        }

        public double GetCoeff(double[] xData, double[] yData)
        {
            double CoVar = 0.0;
            double StDevX = 0.0;
            double MeanX = 0.0;
            double MeanY = 0.0;
            double Coeff = 0.0;

            int dataSize = xData.Length;
            for (int i = 0; i < dataSize; i++)
            {
                MeanX += xData[i] / dataSize;
                MeanY += yData[i] / dataSize;
            }

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

        public double Regression(double[] x, double[] y)
        {
            double Coeff = GetCoeff(x, y);
            Trace.WriteLine(Coeff);
            double[] yFit = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                yFit[i] = Coeff * x[i];
            }
            int sizeData = x.Length;
            double xMin = GetMin(x);
            double xMax = GetMax(x);

            double yFitMin = GetMin(yFit);
            double yFitMax = GetMax(yFit);

            double yAdjust = yFitMax - yFitMin;

            //Find a way to remove the + 60 and have the line plot in the middle of the plot
            LinePlot line = MyWpfPlot.Plot.Add.Line(xMin, yFitMin + 60, xMax, yFitMax + 60);
            //Set the line color and width (Make it thicker/More Dark)

            line.LineColor = Generate.RandomColor();
            line.LineWidth = 4;

            return Coeff;


        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Click");
            BottomBox.Text = "Slope = " + Math.Round(GetCoeff(GetData(Col1), GetData(Col2)),2);

            double[] xData = GetData(Col1);
            double[] yData = GetData(Col2);
         

            InitializeComponent();

            MyWpfPlot.Plot.Title("Example Plot");
            MyWpfPlot.Plot.XLabel("X-Axis");
            MyWpfPlot.Plot.YLabel("Y-Axis");


            MyWpfPlot.Plot.Add.ScatterPoints(xData, yData);
            Regression(xData, yData);

            MyWpfPlot.Plot.Axes.AutoScale();
            MyWpfPlot.Refresh();
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            Col1 = int.Parse(text);
            Trace.WriteLine(Col1);
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            //Introduce some catches for wrong input
            string text2 = ((TextBox)sender).Text;
            Col2 = int.Parse(text2);
            Trace.WriteLine(Col2);
        }
    }
}

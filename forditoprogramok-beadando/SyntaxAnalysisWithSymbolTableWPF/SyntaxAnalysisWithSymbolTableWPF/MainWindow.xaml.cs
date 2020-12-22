using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExcelDataReader;
using Microsoft.Win32;

namespace SyntaxAnalysisWithSymbolTableWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SyntaxAnalyzerAutomat automat;
        private static char csvSeparator = ';';
        private DataSet result;
        private string[][] table;
        private string path;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void original_button_Click(object sender, RoutedEventArgs e)
        {
            string original = input_original_tb.Text;
            automat = new SyntaxAnalyzerAutomat(original);
            input_converted_tb.Text = automat.Converted;
            changeMessagesLabelContent("Input read and converted successfully", AlertType.SUCCESS);

        }

        private void converted_button_Click(object sender, RoutedEventArgs e)
        {
            string converted = input_converted_tb.Text;
            input_original_tb.Clear();
            automat.Converted = converted;
            changeMessagesLabelContent("Converted text changed successfully", AlertType.SUCCESS);
        }

        private void read_file_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.csv", ValidateNames = true };
            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
                symbol_table_data_grid.ItemsSource = CreateDataSource(path);
                disableDataGridColumnsSorting(symbol_table_data_grid);
                filepath_tb.Text = path;

                if (messages_label.Content.Equals("CSV file read successfully")) changeMessagesLabelContent("CSV file changed successfully", AlertType.SUCCESS);
                else changeMessagesLabelContent("CSV file read successfully", AlertType.SUCCESS);
            }
            table = readTable(path);

            // EXCEL STUFF
            /*
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx;*.xls", ValidateNames = true };
            if(ofd.ShowDialog() == true)
            {
                FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(fs);
                result = reader.AsDataSet();
                sheetCbox.Items.Clear();
                foreach (DataTable dt in result.Tables)
                {
                    sheetCbox.Items.Add(dt.TableName);
                }
                reader.Close();

                changeMessagesLabelContent("Excel workbook opened", AlertType.SUCCESS);
            } else
            {
                changeMessagesLabelContent("Can't open Excel workbook", AlertType.DANGER);
            }
            */

        }

        /* // For Excel
        private void sheetCbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            symbol_table_data_grid.DataContext = result.Tables[sheetCbox.SelectedIndex].DefaultView;
            disableDataGridColumnsSorting(symbol_table_data_grid);
            changeMessagesLabelContent("Sheet changed", AlertType.SUCCESS);
        }
        */

        private void disableDataGridColumnsSorting(DataGrid dataGridId)
        {
            foreach (DataGridColumn column in dataGridId.Columns)
            {
                column.CanUserSort = false;
            }
        }

        private void changeMessagesLabelContent(string message, AlertType alertType = AlertType.DEFAULT)
        {
            if (alertType == AlertType.DEFAULT)
            {
                messages_label.Foreground = Brushes.Black;
            } else if (alertType == AlertType.SUCCESS)
            {
                messages_label.Foreground = Brushes.Green;
            } else if (alertType == AlertType.DANGER)
            {
                messages_label.Foreground = Brushes.Red;
            }
            messages_label.Content = message;
        }

        private ICollection CreateDataSource(string filepath)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            string[] lines = File.ReadAllLines(filepath);
            if(lines.Length > 0)
            {
                // headers
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(csvSeparator);
                /*
                foreach(string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }*/

                for (int i = 0; i < headerLabels.Length; i++)
                {
                    dt.Columns.Add(new DataColumn("Column " + (i + 1)));
                }

                //data
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(';');
                    dr = dt.NewRow();
                    for (int j = 0; j < dataWords.Length; j++)
                    {
                        dr[j] = dataWords[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            return new DataView(dt);
        }

        private string[][] readTable(String path)
        {
            StreamReader sr = new StreamReader(path);
            var lines = new List<string[]>();
            int Row = 0;
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(',');
                lines.Add(Line);
                Row++;
                Console.WriteLine(Row);
            }

            var data = lines.ToArray();
            return data;
        }
    }
}

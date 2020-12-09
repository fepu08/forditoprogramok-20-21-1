using System;
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
        DataSet result;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void original_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void converted_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void read_file_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx;*.xls", ValidateNames = true };
            if(ofd.ShowDialog() == true)
            {
                FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(fs);
                result = reader.AsDataSet();
                sheetCbox.Items.Clear();
                foreach(DataTable dt in result.Tables)
                {
                    sheetCbox.Items.Add(dt.TableName);
                }
                reader.Close();

                changeMessagesLabelContent("Excel workbook opened", AlertType.SUCCESS);
            } else
            {
                changeMessagesLabelContent("Can't open Excel workbook", AlertType.DANGER);
            }
            
        }

        private void sheetCbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            symbol_table_data_grid.DataContext = result.Tables[sheetCbox.SelectedIndex].DefaultView;
            disableDataGridColumnsSorting(symbol_table_data_grid);
            changeMessagesLabelContent("Sheet changed", AlertType.SUCCESS);
        }

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
    }
}

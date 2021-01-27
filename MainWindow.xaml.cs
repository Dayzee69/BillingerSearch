using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace BillingerSearch
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchWindow searchWindow = new SearchWindow();
                searchWindow.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SearchFunc(int param, string startDate, string endDate, string name, string inn, string amount, string phone, string card) 
        {
            try
            {
                payeesDataGrid.ItemsSource = null;
                transactionsDataGrid.ItemsSource = null;
                payoutsDataGrid.ItemsSource = null;

                Thread payeesThread = new Thread(() => PayeesSearch(name, phone, inn));
                Thread transactionsThread = new Thread(() => TransactionsSearch(startDate, endDate, phone, card, name, amount));
                Thread payoutsThread = new Thread(() => PayoutsSearch(name, phone, card, startDate, endDate, amount, inn));

                switch (param)
                {

 

                    case 1:
                        payeesLabel.Content = "Получатели Выполняется...";
                        payeesThread.Start();

                        transactionsLabel.Content = "Пополнения Выполняется...";
                        transactionsThread.Start();

                        payoutsLabel.Content = "Выплаты Выполняется...";
                        payoutsThread.Start();

                        break;
                    case 2:
                        transactionsLabel.Content = "Пополнения Выполняется...";
                        transactionsThread.Start();

                        payoutsLabel.Content = "Выплаты Выполняется...";
                        payoutsThread.Start();

                        break;
                    case 3:
                        payeesLabel.Content = "Получатели Выполняется...";
                        payeesThread.Start();

                        payoutsLabel.Content = "Выплаты Выполняется...";
                        payoutsThread.Start();

                        break;
                    case 4:
                        payeesLabel.Content = "Получатели Выполняется...";
                        payeesThread.Start();

                        transactionsLabel.Content = "Пополнения Выполняется...";
                        transactionsThread.Start();

                        break;
                    case 5:
                        payeesLabel.Content = "Получатели Выполняется...";
                        payeesThread.Start();

                        break;
                    case 6:
                        transactionsLabel.Content = "Пополнения Выполняется...";
                        transactionsThread.Start();

                        break;
                    case 7:
                        payoutsLabel.Content = "Выплаты Выполняется...";
                        payoutsThread.Start();

                        break;
                    default:
                        throw new Exception("Нужно выбрать хотя бы одну таблицу для поиска: Получатели, Пополнения, Выплаты");
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void PayeesSearch(string name, string phone, string inn)
        {

            if (phone != "")
                phone = "%" + phone + "%";
            else phone = "NOTNULL";
            if (name != "")
                name = "%" + name + "%";
            else name = "NOTNULL";
            if (inn != "")
                inn = "%" + inn + "%";
            else inn = "NOTNULL";

            DBConnect conn = new DBConnect();
            MySqlConnection mysqlConnection = new MySqlConnection("Server=" + conn.host + ";Port=" + conn.port
                + ";Database=" + conn.database + ";Uid=" + conn.user + ";Pwd=" + conn.password + ";Charset=utf8;");

            try
            {

                mysqlConnection.Open();

                MySqlCommand sqlCommand = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", mysqlConnection);

                sqlCommand.CommandText = "SELECT id, (SELECT title FROM aggregators AS t2 WHERE t2.id = t1.aggregator) AS aggregator, `type`, " +
                    "DATE_FORMAT(registered_datetime, '%d/%m/%Y %T') AS registered_datetime, status , current_balance, aggregator_payee_id, account_number, name, company_title, " +
                    "company_orgn, company_inn, company_kpp, company_director_name, company_bank_subaccount_number, company_agreement_number, email, phone, " +
                    "last_name, third_name, passport_series, passport_number, registration_address, additional_document_type, additional_document_number " +
                    "FROM payees_all_data AS t1 WHERE name LIKE ? OR phone LIKE ? OR company_inn LIKE ?;";

                sqlCommand.Parameters.AddWithValue("", name);
                sqlCommand.Parameters.AddWithValue("", phone);
                sqlCommand.Parameters.AddWithValue("", inn);

                sqlCommand.CommandTimeout = 99999;

                payeesDataGrid.Dispatcher.Invoke(DispatcherPriority.Background, new 
                    Action(() =>
                    {
                        PayeesDataGridCreate(sqlCommand);
                    }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                mysqlConnection.Close();
                mysqlConnection.Dispose();
            }
        }

        private void TransactionsSearch(string startDate, string endDate, string phone, string card, string name, string amount)
        {

            if (phone != "")
                phone = "%" + phone + "%";
            else phone = "NOTNULL";
            if (card != "" && !card.Contains("%"))
                card = "%" + card + "%";
            else if (card != "" && card.Contains("%"))
            { }
            else card = "NOTNULL";
            if (name != "")
                name = "%" + name + "%";
            else name = "NOTNULL";

            startDate = startDate + " 00:00:00";
            endDate = endDate + " 23:59:59";

            DBConnect conn = new DBConnect();
            MySqlConnection mysqlConnection = new MySqlConnection("Server=" + conn.host + ";Port=" + conn.port
                + ";Database=" + conn.database + ";Uid=" + conn.user + ";Pwd=" + conn.password + ";Charset=utf8;");

            try
            {

                mysqlConnection.Open();

                MySqlCommand sqlCommand = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", mysqlConnection);

                if (amount != "") 
                {

                    sqlCommand.CommandText = "SELECT id, DATE_FORMAT(added_datetime, '%d/%m/%Y %T') AS added_datetime, DATE_FORMAT(`datetime`, '%d/%m/%Y %T') AS `datetime`, order_id, (SELECT title FROM aggregators AS t2 WHERE " +
                        "t2.id = t1.aggregator) AS aggregator, (SELECT title FROM  payment_systems AS t2 WHERE t2.id = t1.payment_system ) AS payment_system, " +
                        "(SELECT name FROM payees_all_data AS t2 WHERE t2.id = t1.payee) AS payee, payer_name, payer_email, payer_phone, amount, commission, " +
                        "ps_commission, bank_commission, aggregator_commission, payer_commission, ps_in_commission, auth_code, payer_purse, doс_number, " +
                        "payee_credit_doc_number  FROM transactions AS t1 WHERE (datetime >= ? AND datetime <= ?) AND (payer_phone LIKE ? OR  payer_purse  " +
                        "LIKE ? OR payer_purse  LIKE ? OR payer_name LIKE ? OR amount = ?);";

                    sqlCommand.Parameters.AddWithValue("", startDate);
                    sqlCommand.Parameters.AddWithValue("", endDate);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", card);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", name);
                    sqlCommand.Parameters.AddWithValue("", amount);
                }
                else
                {
                    sqlCommand.CommandText = "SELECT id, DATE_FORMAT(added_datetime, '%d/%m/%Y %T') AS added_datetime, DATE_FORMAT(`datetime`, '%d/%m/%Y %T') AS `datetime`, order_id, (SELECT title FROM aggregators AS t2 WHERE " +
                        "t2.id = t1.aggregator) AS aggregator, (SELECT title FROM  payment_systems AS t2 WHERE t2.id = t1.payment_system ) AS payment_system, " +
                        "(SELECT name FROM payees_all_data AS t2 WHERE t2.id = t1.payee) AS payee, payer_name, payer_email, payer_phone, amount, commission, " +
                        "ps_commission, bank_commission, aggregator_commission, payer_commission, ps_in_commission, auth_code, payer_purse, doс_number, " +
                        "payee_credit_doc_number  FROM transactions AS t1 WHERE (datetime >= ? AND datetime <= ?) AND (payer_phone LIKE ? OR  payer_purse  LIKE ? " +
                        "OR payer_purse  LIKE ? OR payer_name LIKE ?);";

                    sqlCommand.Parameters.AddWithValue("", startDate);
                    sqlCommand.Parameters.AddWithValue("", endDate);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", card);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", name);
                }
               

                sqlCommand.CommandTimeout = 99999;

                transactionsDataGrid.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() =>
                    {
                        TransactionsDataGridCreate(sqlCommand);
                    }));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                mysqlConnection.Close();
                mysqlConnection.Dispose();
            }
        }

        private void PayoutsSearch(string name, string phone, string card, string startDate, string endDate, string amount, string inn)
        {

            

            if (phone != "")
                phone = "%" + phone + "%";
            else phone = "NOTNULL";
            if (card != "" && !card.Contains("%"))
                card = "%" + card + "%";
            else if (card != "" && card.Contains("%"))
            { }
            else card = "NOTNULL";
            if (name != "")
                name = "%" + name + "%";
            else name = "NOTNULL";
            if (inn != "")
                inn = "%" + inn + "%";
            else inn = "NOTNULL";

            startDate = startDate + " 00:00:00";
            endDate = endDate + " 23:59:59";

            DBConnect conn = new DBConnect();
            MySqlConnection mysqlConnection = new MySqlConnection("Server=" + conn.host + ";Port=" + conn.port
                + ";Database=" + conn.database + ";Uid=" + conn.user + ";Pwd=" + conn.password + ";Charset=utf8;");

            try
            {

                mysqlConnection.Open();                

                MySqlCommand sqlCommand = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", mysqlConnection);

                if (amount != "")
                {
                    //DATE_FORMAT(added_datetime, '%d/%m/%Y %T') AS added_datetime, DATE_FORMAT(`datetime`, '%d/%m/%Y %T') AS `datetime`
                    sqlCommand.CommandText = "SELECT id, DATE_FORMAT(added_datetime, '%d/%m/%Y %T') AS added_datetime, (SELECT title FROM aggregators AS t2 WHERE t2.id = t1.aggregator) AS aggregator, " +
                        "(SELECT name FROM payees_all_data AS t2 WHERE t2.id = t1.payee) AS payee, destination_type, amount, commission, chredentials, " +
                        "DATE_FORMAT(`datetime`, '%d/%m/%Y %T') AS `datetime`, order_id, bank_doc_num, status, operation_details, gate_commission, bank_commission , operator_commission , " +
                        "transport_registry_id , TAX_info FROM payouts AS t1 WHERE (datetime >= ? AND datetime <= ?) AND (chredentials LIKE ? OR chredentials " +
                        "LIKE ? OR chredentials LIKE ? OR chredentials LIKE ? OR amount = ?);";

                    sqlCommand.Parameters.AddWithValue("", startDate);
                    sqlCommand.Parameters.AddWithValue("", endDate);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", name);
                    sqlCommand.Parameters.AddWithValue("", card);
                    sqlCommand.Parameters.AddWithValue("", inn);
                    sqlCommand.Parameters.AddWithValue("", amount);
                }
                else
                {
                    sqlCommand.CommandText = "SELECT id, DATE_FORMAT(added_datetime, '%d/%m/%Y %T') AS added_datetime, (SELECT title FROM aggregators AS t2 WHERE t2.id = t1.aggregator) AS aggregator, " +
                        "(SELECT name FROM payees_all_data AS t2 WHERE t2.id = t1.payee) AS payee, destination_type, amount, commission, chredentials, " +
                        "DATE_FORMAT(`datetime`, '%d/%m/%Y %T') AS `datetime`, order_id, bank_doc_num, status, operation_details, gate_commission, bank_commission , operator_commission , " +
                        "transport_registry_id , TAX_info FROM payouts AS t1 WHERE (datetime >= ? AND datetime <= ?) AND (chredentials LIKE ? OR chredentials " +
                        "LIKE ? OR chredentials LIKE ? OR chredentials LIKE ?);";

                    sqlCommand.Parameters.AddWithValue("", startDate);
                    sqlCommand.Parameters.AddWithValue("", endDate);
                    sqlCommand.Parameters.AddWithValue("", phone);
                    sqlCommand.Parameters.AddWithValue("", name);
                    sqlCommand.Parameters.AddWithValue("", card);
                    sqlCommand.Parameters.AddWithValue("", inn);
                }

                sqlCommand.CommandTimeout = 99999;

                payoutsDataGrid.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() =>
                    {
                        PayoutsDataGridCreate(sqlCommand);
                    }));

            }
            catch (Exception ex)
            {
                payoutsLabel.Content = "Выплаты";
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                mysqlConnection.Close();
                mysqlConnection.Dispose();
            }

        }

        private void CreateXML_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string strDate = DateTime.UtcNow.ToString();
                strDate = strDate.Replace(".", "");
                strDate = strDate.Replace(" ", "");
                strDate = strDate.Replace(":", "");

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel 2010|*.xlsx";
                saveFileDialog.FileName = "billsearch" + strDate;
                if (saveFileDialog.ShowDialog() == true)
                {
                    FileInfo excelFile = new FileInfo(saveFileDialog.FileName);

                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("Payees");
                        ExcelWorksheet worksheetPayees = excel.Workbook.Worksheets["Payees"];

                        if (payeesDataGrid.SelectedIndex > -1)
                        {
                            DataView viewPayees = (DataView)payeesDataGrid.ItemsSource;
                            DataTable dtPayees = viewPayees.Table.Clone();
                            foreach (DataRowView dataRowView in viewPayees)
                            {
                                dtPayees.ImportRow(dataRowView.Row);
                            }

                            DataTable dataTablePayees = dtPayees;
                            worksheetPayees.Cells["A1"].LoadFromDataTable(dataTablePayees, true);
                        }


                        excel.Workbook.Worksheets.Add("Transactions");
                        ExcelWorksheet worksheetTransactions = excel.Workbook.Worksheets["Transactions"];

                        
                        if (transactionsDataGrid.SelectedIndex > -1)
                        {
                            DataView viewTransactions = (DataView)transactionsDataGrid.ItemsSource;
                            DataTable dtTransactions = viewTransactions.Table.Clone();
                            foreach (DataRowView dataRowView in viewTransactions)
                            {
                                dtTransactions.ImportRow(dataRowView.Row);
                            }

                            DataTable dataTableTransactions = dtTransactions;
                            worksheetTransactions.Cells["A1"].LoadFromDataTable(dataTableTransactions, true);
                        }


                        excel.Workbook.Worksheets.Add("Payouts");
                        ExcelWorksheet worksheetPayouts = excel.Workbook.Worksheets["Payouts"];

                        if (payoutsDataGrid.SelectedIndex > -1)
                        {
                            DataView viewPayouts = (DataView)payoutsDataGrid.ItemsSource;
                            DataTable dtPayouts = viewPayouts.Table.Clone();
                            foreach (DataRowView dataRowView in viewPayouts)
                            {
                                dtPayouts.ImportRow(dataRowView.Row);
                            }

                            DataTable dataTablePayouts = dtPayouts;
                            worksheetPayouts.Cells["A1"].LoadFromDataTable(dataTablePayouts, true);
                        }

                        excel.SaveAs(excelFile);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void PayeesDataGridCreate(MySqlCommand sqlCommand) 
        {
            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                payeesDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                payeesLabel.Content = "Получатели";
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally 
            {
                payeesLabel.Content = "Получатели";
            }
        }

        private void TransactionsDataGridCreate(MySqlCommand sqlCommand)
        {
            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                transactionsDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                transactionsLabel.Content = "Пополнения";
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                transactionsLabel.Content = "Пополнения";
            }
        }

        private void PayoutsDataGridCreate(MySqlCommand sqlCommand)
        {
            try
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                payoutsDataGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                payoutsLabel.Content = "Выплаты";
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                payoutsLabel.Content = "Выплаты";
            }
        }
    }
}

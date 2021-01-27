using System;
using System.Windows;

namespace BillingerSearch
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            startDatePicker.SelectedDate = DateTime.Now;
            endDatePicker.SelectedDate = DateTime.Now;
        }

        private void seacrhButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (payeesCb.IsChecked == true && transactionsCb.IsChecked == true && payoutsCb.IsChecked == true)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(1,
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text,
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, ИНН, Сумма, Номер карты, Номер телефона");
                }
                else if (payeesCb.IsChecked == false && transactionsCb.IsChecked == true && payoutsCb.IsChecked == true)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(2,
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text,
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, ИНН, Сумма, Номер карты, Номер телефона");

                }
                else if (payeesCb.IsChecked == true && transactionsCb.IsChecked == false && payoutsCb.IsChecked == true)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "" || innTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(3,
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text,
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, ИНН, Сумма, Номер карты, номер телефона");
                }
                else if (payeesCb.IsChecked == true && transactionsCb.IsChecked == true && payoutsCb.IsChecked == false)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(4,
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text,
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, Сумма, Номер карты, Номер телефона");
                }
                else if (payeesCb.IsChecked == true && transactionsCb.IsChecked == false && payoutsCb.IsChecked == false)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || innTb.Text != "")
                    {

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(5, "", "",
                            nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text, cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, ИНН, Номер телефона");
                }
                else if (payeesCb.IsChecked == false && transactionsCb.IsChecked == true && payoutsCb.IsChecked == false)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(6, 
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text, 
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, Сумма, Номер карты, Номер телефона");
                }
                else if (payeesCb.IsChecked == false && transactionsCb.IsChecked == false && payoutsCb.IsChecked == true)
                {
                    if (phoneTb.Text != "" || nameTb.Text != "" || cardTb.Text != "" || amountTb.Text != "" || innTb.Text != "")
                    {
                        DateTime stratDate = (DateTime)startDatePicker.SelectedDate;
                        DateTime endDate = (DateTime)endDatePicker.SelectedDate;

                        Dispatcher.Invoke(() => ((MainWindow)Application.Current.MainWindow).SearchFunc(7,
                            stratDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), nameTb.Text, innTb.Text, amountTb.Text, phoneTb.Text,
                            cardTb.Text));

                        Close();
                    }
                    else
                        throw new Exception("Нужно заполнить хотя бы одно поле для поиска: ФИО, ИНН, Сумма, Номер карты, номер телефона");
                }
                else
                    throw new Exception("Нужно выбрать хотя бы одну таблицу для поиска: Получатели, Пополнения, Выплаты");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

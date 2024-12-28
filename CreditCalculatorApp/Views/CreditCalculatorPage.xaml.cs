using CreditCalculatorApp.Models;
using CreditCalculatorApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CreditCalculatorApp.Views
{
    public partial class CreditCalculatorPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private readonly User _currentUser;

        // Конструктор без параметров для использования в XAML
        public CreditCalculatorPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        // Конструктор с параметром User
        public CreditCalculatorPage(User user) : this()
        {
            _currentUser = user;
            LoadCredits();
        }

        // Обработчик изменения значения слайдера
        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            interestRateLabel.Text = $"{e.NewValue:0}%";
        }

        // Обработчик нажатия на кнопку "Рассчитать"
        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            if (double.TryParse(loanAmountEntry.Text, out double loanAmount))
            {
                double interestRate = creditSlider.Value; // Получаем процентную ставку с слайдера
                int loanTerm = 12; // Пример: срок кредита 12 месяцев (можно добавить поле для ввода)

                // Преобразуем процентную ставку в месячную
                double monthlyRate = (interestRate / 100) / 12;

                // Расчет ежемесячного платежа
                double monthlyPayment = loanAmount * (monthlyRate * Math.Pow(1 + monthlyRate, loanTerm)) / (Math.Pow(1 + monthlyRate, loanTerm) - 1);

                // Выводим результат
                await DisplayAlert("Результат",
                    $"Сумма кредита: {loanAmount}\nПроцентная ставка: {interestRate}%\nЕжемесячный платеж: {monthlyPayment:C}",
                    "OK");

                // Сохраняем кредит в базе данных
                var credit = new Credit
                {
                    UserId = _currentUser.Id,
                    LoanAmount = loanAmount,
                    InterestRate = interestRate,
                    MonthlyPayment = monthlyPayment // Добавляем ежемесячный платеж в модель
                };
                await _databaseService.AddCreditAsync(credit);
                LoadCredits(); // Обновляем список кредитов
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверный формат суммы кредита", "OK");
            }
        }


        // Загрузка всех кредитов пользователя
        private async void LoadCredits()
        {
            var credits = await _databaseService.GetCreditsByUserIdAsync(_currentUser.Id);
            creditsListView.ItemsSource = credits;
        }
    }
}
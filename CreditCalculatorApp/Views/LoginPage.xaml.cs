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
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public LoginPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entryFirstName.Text) && !string.IsNullOrEmpty(entryPassword.Text))
            {
                // Получаем пользователя из базы данных
                var user = await _databaseService.GetUserAsync(entryFirstName.Text, entryPassword.Text);

                if (user != null)
                {
                    // Если пользователь найден, переходим на страницу калькулятора кредита
                    await DisplayAlert("Успех", $"Добро пожаловать, {user.FirstName}!", "ОК");

                    // Переход на страницу калькулятора кредита, передавая объект пользователя
                    await Navigation.PushAsync(new CreditCalculatorPage(user));
                }
                else
                {
                    await DisplayAlert("Ошибка", "Неверное имя или пароль.", "ОК");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля.", "ОК");
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            // Переход на страницу регистрации
            await Navigation.PushAsync(new RegistrationPage());
        }
    }
}
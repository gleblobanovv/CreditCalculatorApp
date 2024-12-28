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
    public partial class RegistrationPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public RegistrationPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entryFirstName.Text) &&
                !string.IsNullOrEmpty(entryLastName.Text) &&
                !string.IsNullOrEmpty(entryEmail.Text) &&
                !string.IsNullOrEmpty(entryPhoneNumber.Text) &&
                !string.IsNullOrEmpty(entryPassword.Text))
            {
                // Проверка на существующего пользователя с таким именем и почтой
                var existingUser = await _databaseService.GetUserByNameAndEmailAsync(entryFirstName.Text, entryEmail.Text);

                if (existingUser != null)
                {
                    await DisplayAlert("Ошибка", "Пользователь с таким именем и почтой уже существует.", "ОК");
                    return;
                }

                var user = new User
                {
                    FirstName = entryFirstName.Text,
                    LastName = entryLastName.Text,
                    Email = entryEmail.Text,
                    PhoneNumber = entryPhoneNumber.Text,
                    Password = entryPassword.Text 
                };

                await _databaseService.AddUserAsync(user);

                await DisplayAlert("Успех", "Вы успешно зарегистрировались!", "ОК");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля.", "ОК");
            }
        }
    }
}
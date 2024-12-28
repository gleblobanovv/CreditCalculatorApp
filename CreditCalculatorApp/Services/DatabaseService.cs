using CreditCalculatorApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CreditCalculatorApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Credit>().Wait(); // Создание таблицы для кредитов
        }

        // Метод для добавления пользователя
        public async Task AddUserAsync(User user)
        {
            if (user != null)
            {
                await _database.InsertAsync(user);
            }
        }

        // Метод для получения пользователя по имени и паролю
        public async Task<User> GetUserAsync(string firstName, string password)
        {
            return await _database.Table<User>()
                                  .FirstOrDefaultAsync(u => u.FirstName == firstName && u.Password == password);
        }

        // Метод для сохранения кредита
        public async Task AddCreditAsync(Credit credit)
        {
            if (credit != null)
            {
                await _database.InsertAsync(credit);
            }
        }

        // Получение всех кредитов для пользователя
        public async Task<List<Credit>> GetCreditsByUserIdAsync(int userId)
        {
            return await _database.Table<Credit>()
                                  .Where(c => c.UserId == userId)
                                  .ToListAsync();
        }

        public async Task<User> GetUserByNameAndEmailAsync(string firstName, string email)
        {
            return await _database.Table<User>()
                                  .FirstOrDefaultAsync(u => u.FirstName == firstName && u.Email == email);
        }

    }
}

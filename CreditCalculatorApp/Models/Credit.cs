using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditCalculatorApp.Models
{
    public class Credit
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; } // ID пользователя, к которому относится кредит
        public double LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public double MonthlyPayment { get; set; }
    }
}

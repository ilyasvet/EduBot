using Simulator.Services;
using System;

namespace Simulator.Models
{
    public class User
    {
        private long _userID;
        public long UserID
        {
            get => _userID;
            private set
            {
                if(value < 0)
                    throw new ArgumentException("TelegramID должен быть положительным числом.");
                _userID = value;
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            private set
            {
                if (!UserHandler.IsCorrectName(value))
                    throw new ArgumentException("Неверный формат имени.");
                _name = value;
            }
        }
        private string _surname;
        public string Surname
        {
            get => _surname;
            private set
            {
                if (!UserHandler.IsCorrectName(value))
                    throw new ArgumentException("Неверный формат фамилии.");
                _surname = value;
            }
        }
        public string GroupNumber { get; set; }
        public bool IsAdmin { get; set; }

        public User(long userId, string name, string surname)
        {
            UserID = userId;
            Name = name;
            Surname = surname;
            IsAdmin = false;
        }
        public override string ToString()
        {
            string result = Name + " " + Surname;
            if(IsAdmin)
            {
                result += " (Администратор)";
            }
            return result;
        }
    }
}

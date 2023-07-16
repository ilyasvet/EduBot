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
        public UserType UserType { get; set; }

        public User(long userId, string name, string surname)
        {
            UserID = userId;
            Name = name;
            Surname = surname;
        }
        public override string ToString()
        {
            string result = Name + " " + Surname;
            switch (UserType)
            {
                case UserType.Admin:
                    result += " (А)";
                    break;
                case UserType.ClassLeader:
                    result += " (C)";
                    break;
            }
            return result;
        }
    }
}

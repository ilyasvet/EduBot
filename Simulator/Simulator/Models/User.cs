using System;
using System.Linq;

namespace Simulator.Models
{
    public class User
    {
        private long userID;
        public long UserID
        {
            get { return userID; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private string surname;
        public string Surname
        {
            get { return surname; }
        }

        private string password = "Undefined";
        public string Password
        {
            get
            {
                return password;    
            }
            set
            {
                password = value;   
            }
        }

        private bool isAdmin = false;
        public bool IsAdmin
        {
            get
            {
                return isAdmin;   
            }
            set
            {
               isAdmin = value;   
            }
        }

        private bool isOnline = false;
        public bool IsOnline
        {
            get
            {
                return isOnline;   
            }
            set
            {
                isOnline = value;  
            }
        }

        private object firstAttempt = null; //AttemptDescription
        public object FirstAttempt
        {
            get
            {
                return firstAttempt;    
            }
            set
            {
                firstAttempt = value;   
            }
        }

        private object secondAttempt = null; //AttemptDescription
        public object SecondAttempt
        {
            get
            {
                return secondAttempt;   
            }
            set
            {
                secondAttempt = value;   
            }
        }

        public User(long id, string name, string surname)
        {
            this.userID = id;
            this.name = name;
            this.surname = surname;
        }

        public void SetPassword(out string password)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()0123456789";
            password=new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void SetPassword(string password)
        {
            Password = password;
        }

        public override string ToString() { return name +" "+ surname; }
    }
}

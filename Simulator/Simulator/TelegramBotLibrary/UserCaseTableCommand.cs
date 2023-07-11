using System;
using System.Data.SqlClient;

namespace Simulator.TelegramBotLibrary
{
    public static class UserCaseTableCommand
    {
        private static SqlCommand command = new SqlCommand();

        static UserCaseTableCommand()
        {
            command.Connection = LocalSqlDbConnection.Connection;
        }

        public static void Dispose()
        {
            command.Dispose();
        }

        public static void AddUser(long userId)
        {
            string commandText = $"insert into UserCase (UserId, OnCourse, Rate, Point, HealthPoints)" +
                $" values ('{userId}','{false}','{0}','{0}','{3}')";
            //Добавлять пользователя (если его нет в базе)
            ExecuteNonQueryCommand(commandText);
        }
        public static bool IsOnCourse(long userId)
        {
            string commandText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (bool)reader["OnCourse"];
                }
            }
            throw new ArgumentException();
        }

        public static int GetPoint(long userId)
        {
            string commandText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (int)reader["Point"];
                }
            }
            throw new ArgumentException();
        }

        public static void SetPoint(long userId, int number)
        {
            string commandText = $"update UserCase set Point = {number} where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        public static double GetRate(long userId)
        {
            string commantText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commantText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (double)reader["Rate"];
                }
            }
            throw new ArgumentException();
        }

        public static void SetRate(long userId, double currentUserRate)
        {
            string commandText = $"update UserCase set Rate = {currentUserRate} where userId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        public static void SetOnCourse(long userId, bool boolOnCourse)
        {
            string commandText = $"update UserCase set OnCourse = '{boolOnCourse}' where userId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        public static void SetHealthPoints(long userId, int healthPoints = 3)
        {
            string commandText = $"update UserCase set HealthPoints = '{healthPoints}' where userId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        public static int GetHealthPoints(long userId)
        {
            string commantText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commantText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (int)reader["HealthPoints"];
                }
            }
            throw new ArgumentException();
        }
        public static DateTime GetStartTime(long userId)
        {
            string commandText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if(reader.Read())
                {
                    return (DateTime)reader["StartTime"];
                }
            }
            throw new ArgumentException();
        }
        public static void SetStartTime(long userId, DateTime time)
        {
            string commandText = $"update UserCase set StartTime = '{time}' where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public static DateTime GetStartCaseTime(long userId)
        {
            string commandText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (DateTime)reader["StartCaseTime"];
                }
            }
            throw new ArgumentException();
        }
        public static void SetStartCaseTime(long userId, DateTime time)
        {
            string commandText = $"update UserCase set StartCaseTime = '{time}' where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }
        public static DateTime GetEndCaseTime(long userId)
        {
            string commandText = $"select * from UserCase where UserId = {userId}";
            command.CommandText = commandText;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (DateTime)reader["EndCaseTime"];
                }
            }
            throw new ArgumentException();
        }
        public static void SetEndCaseTime(long userId, DateTime time)
        {
            string commandText = $"update UserCase set EndCaseTime = '{time}' where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        public static void DeleteUser(long userId)
        {
            string commandText = $"delete from UserCase where UserId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        private static void ExecuteNonQueryCommand(string commandText)
        {
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}

using Simulator.BotControl.State;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            string commandText = $"update UserCase set OnCourse = {boolOnCourse} where userId = {userId}";
            ExecuteNonQueryCommand(commandText);
        }

        private static void ExecuteNonQueryCommand(string commandText)
        {
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}

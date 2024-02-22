using System.Text.RegularExpressions;

namespace EduBot.Services
{
    public static class UserHandler
    {
        public static bool IsCorrectName(string inputName)
        {
            Regex regex = new Regex("[А-Я][а-я]+");
            return regex.IsMatch(inputName);
        }
        
    }
}

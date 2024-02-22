using System.Text.RegularExpressions;

namespace Simulator.Services
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

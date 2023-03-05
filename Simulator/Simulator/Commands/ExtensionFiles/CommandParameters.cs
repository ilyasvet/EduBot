using System.Collections.Generic;
using System.Linq;

namespace Simulator.Commands
{
    public struct CommandParameters
    {
        public long UserID { get; private set; }
        public bool IsAdmin { get; private set; }
        public List<string> Parameters { get; private set; }
        public CommandParameters(long userID, bool isAdmin, string parameters)
        {
            UserID = userID;
            IsAdmin = isAdmin;
            Parameters = new List<string>();
            Parameters = parameters.Split(' ').ToList();
        }
    }
}

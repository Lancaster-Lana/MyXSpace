using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class LoggingTryout : Entity<string>
    {
        public string UserLogin { get; set; }
        public int Tryout { get; set; }
        public DateTime LastTry { get; set; }
        public string UnlockCode { get; set; }
    }
}

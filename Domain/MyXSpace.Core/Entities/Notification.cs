using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class Notification : Entity<string>
    {
        public string ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public DateTime ExecutionDate { get; set; }
        public int Mode { get; set; }
        public int Receiver { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public bool NotifyUnregisteredUsers { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AgencyFileCode { get; set; }
        //public string PayId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class MockSms : Entity<string>
    {
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
    }
}

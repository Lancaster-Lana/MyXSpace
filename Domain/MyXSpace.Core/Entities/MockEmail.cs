using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class MockEmail : Entity<string>
    {
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string MailHtml { get; set; }
    }
}

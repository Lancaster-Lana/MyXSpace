using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class Flow : Entity<string>
    {
        public int Source { get; set; }
        public int Type { get; set; }
        public DateTime? DataUpdated { get; set; }
    }
}

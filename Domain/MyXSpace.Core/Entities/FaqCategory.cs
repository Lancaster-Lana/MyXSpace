using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class FaqCategory : Entity<string>
    {
        public FaqCategory()
        {
            FaqQuestion = new HashSet<FaqQuestion>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int UserType { get; set; }

        public virtual ICollection<FaqQuestion> FaqQuestion { get; set; }
    }
}

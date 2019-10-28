using System;
using System.Collections.Generic;

namespace MyXSpace.Core.Entities
{
    public class FaqQuestion : Entity<string>
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string FaqCategoryId { get; set; }

        public virtual FaqCategory FaqCategory { get; set; }
    }
}

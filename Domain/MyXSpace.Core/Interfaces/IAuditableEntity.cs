using System;

namespace MyXSpace.Core.Interfaces
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        DateTime DateCreated { get; set; }

        string UpdatedBy { get; set; }
        DateTime? DateUpdated { get; set; }
    }
}

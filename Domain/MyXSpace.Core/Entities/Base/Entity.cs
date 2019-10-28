
using MyXSpace.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyXSpace.Core.Entities
{
    public class Entity<TKey> : IAuditableEntity
    {
        [Key]
        public TKey Id { get; set; }
        
        public DateTime DateCreated { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}

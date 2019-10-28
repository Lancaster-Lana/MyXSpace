using System;
using System.Collections.Generic;
using System.Text;

namespace MyXSpace.Core.Permissions
{
    /// <summary>
    /// This is an application permission 
    /// It can be assigned to some Permission Group 
    /// </summary>
    public class ApplicationPermission
    {
        public string Name { get; set; }
        public string Value { get; set; }

       public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }
 
        public ApplicationPermission()
        { }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }
        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}

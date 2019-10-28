using System;

namespace MyXSpace.Core.Entities
{
    public class UserProfile : Entity<string>
    {     
        /// <summary>
        /// TODO: FK
        /// </summary>
        public string UserId { get; set; }
   
        public AppUser User { get; set; }

        //public bool Invited { get; set; }
        //public bool Registered { get; set; }

        //public string Index { get; set; }
        //public string Civility { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Nationality { get; set; }
        public string Ssnumber { get; set; }
    }
}

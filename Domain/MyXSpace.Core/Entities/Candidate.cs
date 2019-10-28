
using Core.Enums;

namespace MyXSpace.Core.Entities
{
    /// <summary>
    /// Candidate entity (worker) for external user 
    /// </summary>
    public class Candidate : Entity<int>
    {
        public bool Invited { get; set; }

        public string JobTitle { get; set; }

        /// <summary>
        /// TW, PU, Contractor
        /// TODO: may have several statuses ?
        /// </summary>
        public WorkerStatus Status { get; set; } = WorkerStatus.None;


        /// <summary>
        ///Candidate contact information : FIO, Email, etc 
        /// </summary>
        public UserProfile Profile { get; set; }

        //TODO: maybe in profile
        public string BirthPlace { get; set; }
        public string Nationality { get; set; } //TODO:
        public string Ssnumber { get; set; }

    }
}

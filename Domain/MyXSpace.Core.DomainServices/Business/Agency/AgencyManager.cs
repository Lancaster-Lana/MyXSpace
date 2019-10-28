
using MyXSpace.Core.Entities;
using MyXSpace.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyXSpace.Core.Services
{
    /// <summary>
    /// TODO:
    /// </summary>
    public class AgencyManager : IAgencyManager
    {
        private readonly IRepository<Agency> _agencies;
        //private readonly IRepository<AgencyCompany> _agencyCompanies;

        public AgencyManager(IRepository<Agency> agencies
            //,IRepository<IAgencyCompany> agencyCompanies
           )
        {
            _agencies = agencies;
            //_agencyCompanies = agencyCompanies;
        }


    }
}
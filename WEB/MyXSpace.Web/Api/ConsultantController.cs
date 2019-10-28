using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using MyXSpace.AppServices.Sessions;
using MyXSpace.WebSPA.Model;

namespace MyXSpace.WebSPA.Api
{
    /// <summary>
    /// Consultant - internal user, who can invite candiate\client
    /// </summary>
    [Route("api/[controller]")] //TODO: tenantt check :   [Route("api/{tenant}/[controller]")]
    [AutoValidateAntiforgeryToken]
    //[Authorize("consultant")]
    public class ConsultantController : Controller
    {
        private readonly ISessionAppService _session;
        public ConsultantController(ISessionAppService sessionAppService)
        {
            _session = sessionAppService;
            //_session.GetCurrentUserPrincipal();// NOTE: curren user 
        }

        #region Candidates

        [HttpGet]
        //[Route("~/api/{tenantName}/Consultant/{consultantID}/Candidates")]
        [Route("{consultantID}/Candidates")]
        public List<CandidateModel> GetCandidatesByConsultantId(string consultantID)//, string tenantName)
        {
            var candidates = GetCandidatesFromHub();// tenantID - BUT from that tenant only !

            //TODO: select only candidates, belonging to the Tenant (not Consultant!)
            //Guid consultantGuid;
            //if (!string.IsNullOrWhiteSpace(consultantID) && Guid.TryParse(consultantID, out consultantGuid))
            //    candidates = candidates.Where(c => c.TenantID == tenantID); //candidates.Where(c => c.ConsultantsIDs.Contains(consultantID));

            return candidates.ToList();
        }

        [HttpGet]
        [Route("AllCandidates")]
        [Route("~/api/{tenantName}/AllCandidates")]
        public IList<CandidateModel> GetAllCandidates(string tenantName = "")
        {
            return GetCandidatesFromHub(tenantName).ToList(); //TODO: get candidates by tenantID from HUB
        }

        /// <summary>
        /// TODO: get candidates (from fake Hub), belonging to some tenant  
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        private IQueryable<CandidateModel> GetCandidatesFromHub(string tenantName ="")
        {
            //TODO: filter by "tenantID" !
            var rng = new Random();
            return Enumerable.Range(1, 25).Select(index => new CandidateModel
            {
                ID = Guid.NewGuid().ToString(),
                Status = WorkerStatus.TW, //TODO:
                Invited = false,
                ConsultantsIDs = new List<string> { index.ToString() /*, TOOD: */ },

                //Profile info
                FirstName = "F" + index,
                LastName = "L" + index,
                Email = "E" + index,
                JobTitle = "SDE",
                BirthDate = DateTime.Parse("12.09.1980"),//DateTime.Now,
                BirthPlace = "Birtch Placce" + index,
                Nationality = "Nat" + index,
                Ssnumber = Guid.NewGuid().ToString()
            }).AsQueryable();
        }

        #endregion

        #region Clients 

        [HttpGet]
        [Route("{consultantID}/Clients")] //[HttpGet("GetClientsByConsultantId/{id}")]
        public IList<ClientModel> GetClientsByConsultantId(string consultantID)//string tenantID)
        {
            var clients = GetClientsFromHub();

            //TODO: select only clients, that registered in the Tenant ( Consultant - no matter!)
            //Guid tenantGuid;
            //if (!string.IsNullOrWhiteSpace(tenantID) && Guid.TryParse(tenantID, out tenantGuid))
            //     clients = clients.Where(c => c.Tenants.Contains(tenantGuid)); 
            return clients.ToList();
        }

        [HttpGet]
        [Route("AllClients")]
        [Route("~/api/{tenantName}/AllClients")]
        public IList<ClientModel> GetAllClients(string tenantName = "")
        {
            return GetClientsFromHub().ToList();
        }

        /// <summary>
        /// TODO: stub method with fake clients
        /// </summary>
        /// <returns></returns>
        private IQueryable<ClientModel> GetClientsFromHub()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new ClientModel
            {
                ID = Guid.NewGuid().ToString(),               
                //Index = index.ToString(),
                //AgencyFileCode = "AgencyFileCode " + index, //ApeCode = "ApeCode" + index, //ZipCode = "ZipCode" + index,
                ClientName = "Client " + index,
                ClientCode = "ClientCode " + index,
                Invited = true,
                Status = ClientType.Finance, //TODO:
                //Profile info
                Email = "Email " + index,
                PhoneNumber = index + " 777 888 "               
            }).AsQueryable();
        }

        #endregion

        #region Contracts 

        [HttpGet]
        [Route("~/api/{tenantName}/Consultant/{consultantID}/Contracts")]
        [Route("{consultantID}/Contracts")]         
        public IEnumerable<ContractModel> GetContractsByConsultantId(string consultantID, string tenantName)
        {
            var contracts = GetAllContracts(tenantName);

            //TODO: get contract only of this consultant
            //Guid consultantGuid;
            //if (!string.IsNullOrWhiteSpace(consultantID) && Guid.TryParse(consultantID, out consultantGuid))
            //    contracts = contracts.Where(c => c.InternalUserSignOrigin == consultantID);

            return contracts;
        }

        /// <summary>
        /// Fake contracts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllContracts")] //api/consultant/allcontracts
        [Route("~/api/{tenantName}/Consultant/AllContracts")]//BETTER: 
        public IQueryable<ContractModel> GetAllContracts(string tenantName)  
        {
            return GetFakeContracts(tenantName);
        }

        private static IQueryable<ContractModel> GetFakeContracts(string tenantName)
        { 
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new ContractModel
            {
                ID = Guid.NewGuid().ToString(),
                ClientSigned = true, //TODO:
                ClientSignOrigin = Guid.NewGuid().ToString(), //TODO:
                CreatedOn = DateTime.Now,
                CreatedBy = "CreatedBy" + index,
                ExternalUserSigned = false,
                ExternalUserSignOrigin = Guid.NewGuid().ToString(), //TODO:
                FullySigned = false,
                InternalUserSigned = false,
                InternalUserSignOrigin = "Consultant " + index //todo:

            }).AsQueryable();
        }

        #endregion

    }
}

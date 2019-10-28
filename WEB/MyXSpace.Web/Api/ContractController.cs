using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyXSpace.WebSPA.Model;

namespace MyXSpace.WebSPA.Api
{
    /// <summary>
    /// TODO: api conteoller\service to mage contracts
    /// </summary>
    [Route("api/[controller]")]
    [AutoValidateAntiforgeryToken]
    public class ContractController : Controller
    {

        //[HttpGet]
        //[Route("GetConsultantContracts/{id}")]
        //public IEnumerable<ContractModel> GetConsultantContracts(string id)
        //{
        //    return GetAllContracts().Where( c => c.ID == id);
        //}
 
    }
}

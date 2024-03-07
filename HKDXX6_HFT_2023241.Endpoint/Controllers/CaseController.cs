using HKDXX6_HFT_2023241.Endpoint.Services;
using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Models.NonCrudModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        ICaseLogic logic;
        readonly IHubContext<SignalRHub> hub;

        public CaseController(ICaseLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        // GET: <CaseController>
        [HttpGet]
        public IEnumerable<Case> ReadAll()
        {
            return logic.ReadAll();
        }

        // GET <CaseController>/5
        [HttpGet("{id}")]
        public Case Read(int id)
        {
            return logic.Read(id);
        }

        // POST <CaseController>
        [HttpPost]
        public void Create([FromBody] Case value)
        {
            logic.Create(value);
            hub.Clients.All.SendAsync("CaseCreated", value);
        }

        // PUT <CaseController>/5
        [HttpPut]
        public void Put([FromBody] Case value)
        {
            logic.Update(value);
            hub.Clients.All.SendAsync("CaseUpdated", value);
        }

        // DELETE <CaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var c = logic.Read(id);
            logic.Delete(id);
            hub.Clients.All.SendAsync("CaseDeleted", c);
        }

        [HttpPost("AutoAssign")]
        public void AutoAssignCase([FromBody]AutoAssignData assignData)
        {
            var c = logic.Read(assignData.CaseID);
            logic.AutoAssignCase(assignData.CaseID, assignData.PrecinctID);
            hub.Clients.All.SendAsync("CaseUpdated", c);
        }
    }
}

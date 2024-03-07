using HKDXX6_HFT_2023241.Endpoint.Services;
using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OfficerController : ControllerBase
    {
        IOfficerLogic logic;
        readonly IHubContext<SignalRHub> hub;
        public OfficerController(IOfficerLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        // GET: <OfficerController>
        [HttpGet]
        public IEnumerable<Officer> ReadAll()
        {
            return logic.ReadAll();
        }

        // GET <OfficerController>/5
        [HttpGet("{id}")]
        public Officer Read(int id)
        {
            return logic.Read(id);
        }

        // POST <OfficerController>
        [HttpPost]
        public void Create([FromBody] Officer value)
        {
            logic.Create(value);
            hub.Clients.All.SendAsync("OfficerCreated", value);
        }

        // PUT <OfficerController>/5
        [HttpPut]
        public void Put([FromBody] Officer value)
        {
            logic.Update(value);
            hub.Clients.All.SendAsync("OfficerUpdated", value);
        }

        // DELETE <OfficerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var o = this.logic.Read(id);
            logic.Delete(id);
            this.hub.Clients.All.SendAsync("OfficerDeleted", o);
        }
    }
}

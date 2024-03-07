using HKDXX6_HFT_2023241.Endpoint.Services;
using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrecinctController : ControllerBase
    {
        IPrecinctLogic logic;
        readonly IHubContext<SignalRHub> hub;
        public PrecinctController(IPrecinctLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        // GET: <PrecinctController>
        [HttpGet]
        public IEnumerable<Precinct> ReadAll()
        {
            return logic.ReadAll();
        }

        // GET <PrecinctController>/5
        [HttpGet("{id}")]
        public Precinct Read(int id)
        {
            return logic.Read(id);
        }

        // POST <PrecinctController>
        [HttpPost]
        public void Create([FromBody] Precinct value)
        {
            logic.Create(value);
            hub.Clients.All.SendAsync("PrecinctCreated", value);
        }

        // PUT <PrecinctController>/5
        [HttpPut]
        public void Put([FromBody] Precinct value)
        {
            logic.Update(value);
            hub.Clients.All.SendAsync("PrecinctUpdated", value);
        }

        // DELETE <PrecinctController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var p = logic.Read(id);
            logic.Delete(id);
            hub.Clients.All.SendAsync("PrecinctDeleted", p);
        }

        // GET <PrecinctController>/GetCaptain/5
        [HttpGet("GetCaptain/{id}")]
        public Officer GetCaptain(int id)
        {
            return logic.GetCaptain(id);
        }

    }
}

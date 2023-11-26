using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PrecinctController : ControllerBase
    {
        IPrecinctLogic logic;

        public PrecinctController(IPrecinctLogic logic)
        {
            this.logic = logic;
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
        }

        // PUT <PrecinctController>/5
        [HttpPut]
        public void Put([FromBody] Precinct value)
        {
            logic.Update(value);
        }

        // DELETE <PrecinctController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            logic.Delete(id);
        }

        // GET <PrecinctController>/GetCaptain/5
        [HttpGet("GetCaptain/{id}")]
        public Officer GetCaptain(int id)
        {
            return logic.GetCaptain(id);
        }

    }
}

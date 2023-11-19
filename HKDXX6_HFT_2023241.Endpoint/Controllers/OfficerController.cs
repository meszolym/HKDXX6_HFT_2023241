using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OfficerController : ControllerBase
    {
        IOfficerLogic logic;

        public OfficerController(IOfficerLogic logic)
        {
            this.logic = logic;
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
        }

        // PUT <OfficerController>/5
        [HttpPut]
        public void Put([FromBody] Officer value)
        {
            logic.Update(value);
        }

        // DELETE <OfficerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            logic.Delete(id);
        }
    }
}

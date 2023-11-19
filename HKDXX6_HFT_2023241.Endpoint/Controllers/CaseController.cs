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
    public class CaseController : ControllerBase
    {
        ICaseLogic logic;

        public CaseController(ICaseLogic logic)
        {
            this.logic = logic;
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
        }

        // PUT <CaseController>/5
        [HttpPut]
        public void Put([FromBody] Case value)
        {
            logic.Update(value);
        }

        // DELETE <CaseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            logic.Delete(id);
        }
    }
}

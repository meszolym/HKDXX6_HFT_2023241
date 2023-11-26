using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Models.NonCrudModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using static HKDXX6_HFT_2023241.Logic.CaseLogic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HKDXX6_HFT_2023241.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        ICaseLogic logic;

        public StatisticsController(ICaseLogic logic)
        {
            this.logic = logic;
        }


        [HttpGet]
        public IEnumerable<CasesPerOfficerStatistic> casesPerOfficerStatistics()
        {
            return logic.casesPerOfficerStatistics();
        }

        [HttpGet]
        public IEnumerable<CasesPerPrecinctStatistic> casesPerPrecinctStatistics()
        {
            return logic.casesPerPrecinctStatistics();
        }

        [HttpGet]
        public IEnumerable<OfficerCaseAverageOpenTimeItem> CaseAverageOpenTimePerOfficer()
        {
            return logic.OfficerCaseAverageOpenTime();
        }

        [HttpGet]
        public IEnumerable<PrecinctCaseAverageOpenTimeItem> CaseAverageOpenTimePerPrecinct()
        {
            return logic.PrecinctCaseAverageOpenTime();
        }

        [HttpGet("{id}")]
        public IEnumerable<Case> CasesOfPrecinct(int id)
        {
            return logic.CasesOfPrecint(id);
        }
    }
}

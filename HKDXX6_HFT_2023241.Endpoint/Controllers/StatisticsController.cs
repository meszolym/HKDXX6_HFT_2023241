using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
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
        public IEnumerable<OfficerCaseStatistic> OfficerCaseStatistics()
        {
            return logic.officerCaseStatistics();
        }

        [HttpGet]
        public IEnumerable<PrecinctCaseStatistic> precinctCaseStatistics()
        {
            return logic.precinctCaseStatistics();
        }

        [HttpPost]
        public void AutoAssignCase([FromBody] Tuple<int,int> IDPair)
        {
            //                   CaseID,       PrecinctID
            logic.AutoAssignCase(IDPair.Item1, IDPair.Item2);
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<Officer, TimeSpan>> CaseAverageOpenTimePerOfficer()
        {
            return logic.OfficerCaseAverageOpenTime();
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<Precinct, TimeSpan>> CaseAverageOpenTimePerPrecinct()
        {
            return logic.PrecinctCaseAverageOpenTime();
        }

        [HttpGet("{id}")]
        public IEnumerable<Case> CasesOfPrecinct(int precinctID)
        {
            return logic.CasesOfPrecint(precinctID);
        }

        [HttpGet]
        public IEnumerable<KeyValuePair<Precinct, IEnumerable<Case>>> CasesOfPrecincts()
        {
            return logic.CasesOfPrecincts();
        }

    }
}

using HKDXX6_HFT_2023241.Models;
using System;
using System.Collections.Generic;

namespace HKDXX6_HFT_2023241.Logic
{
    public interface ICaseLogic
    {
        void AutoAssignCase(int id, int precintID);
        IEnumerable<KeyValuePair<Precinct, IEnumerable<Case>>> CasesOfPrecincts();
        IEnumerable<Case> CasesOfPrecint(int PrecintID);
        void Create(Case item);
        void Delete(int ID);
        IEnumerable<KeyValuePair<Officer, TimeSpan>> OfficerCaseAverageOpenTime();
        IEnumerable<CaseLogic.OfficerCaseStatistic> officerCaseStatistics();
        IEnumerable<KeyValuePair<Precinct, TimeSpan>> PrecinctCaseAverageOpenTime();
        IEnumerable<CaseLogic.PrecinctCaseStatistic> precinctCaseStatistics();
        IEnumerable<Case> Read(int ID);
        IEnumerable<Case> ReadAll();
        void Update(Case item);
    }
}
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Models.NonCrudModels;
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
        IEnumerable<CasesPerOfficerStatistic> casesPerOfficerStatistics();
        IEnumerable<KeyValuePair<Precinct, TimeSpan>> PrecinctCaseAverageOpenTime();
        IEnumerable<CasesPerPrecinctStatistic> casesPerPrecinctStatistics();
        Case Read(int ID);
        IEnumerable<Case> ReadAll();
        void Update(Case item);
    }
}
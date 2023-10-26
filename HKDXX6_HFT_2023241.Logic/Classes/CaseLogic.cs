using Castle.Core.Internal;
using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Logic
{
    public class CaseLogic : ICaseLogic
    {
        IRepository<Case> CaseRepo;
        IRepository<Precinct> PrecinctRepo;

        public CaseLogic(IRepository<Case> caseRepo, IRepository<Precinct> precinctRepo)
        {
            CaseRepo = caseRepo;
            PrecinctRepo = precinctRepo;
        }

        public void Create(Case item)
        {
            CaseRepo.Create(item);
        }

        public void Update(Case item)
        {
            if (CaseRepo.Read(item.ID) == null)
            {
                throw new ArgumentException("Case not found.");
            }
            if (CaseRepo.Read(item.ID).IsClosed && item.IsClosed)
            {
                throw new ArgumentException("Case has to be open to be updated.");
            }
            CaseRepo.Update(item);
        }

        public void Delete(int ID)
        {
            CaseRepo.Delete(ID);
        }

        public Case Read(int ID)
        {
            var c = CaseRepo.Read(ID);
            if (c == null)
            {
                throw new ArgumentException("Case does not exist.");
            }

            return c;
        }

        public IEnumerable<Case> ReadAll()
        {
            return CaseRepo.ReadAll();
        }


        //NonCrud 1
        public IEnumerable<OfficerCaseStatistic> officerCaseStatistics()
        {
            return from x in ReadAll()
                   group x by x.OfficerOnCase into g
                   select new OfficerCaseStatistic
                   {
                       Officer = g.Key,
                       ClosedCases = g.Count(t => t.IsClosed),
                       OpenCases = g.Count(t => !t.IsClosed)
                   };
        }

        public struct OfficerCaseStatistic
        {
            public Officer Officer { get; set; }
            public int ClosedCases { get; set; }
            public int OpenCases { get; set; }
        }

        //NonCrud 2
        public IEnumerable<PrecinctCaseStatistic> precinctCaseStatistics()
        {
            return from x in officerCaseStatistics()
                   group x by x.Officer.Precinct into g
                   select new PrecinctCaseStatistic
                   {
                       Precinct = g.Key,
                       OpenCases = g.Sum(t => t.OpenCases),
                       ClosedCases = g.Sum(t => t.ClosedCases)
                   };
        }

        public struct PrecinctCaseStatistic
        {
            public Precinct Precinct { get; set; }
            public int ClosedCases { get; set; }
            public int OpenCases { get; set; }
        }

        //NonCrud 3
        public void AutoAssignCase(int id, int precintID)
        {
            Case c = Read(id);

            if (c.OfficerOnCase != null || c.IsClosed)
            {
                throw new InvalidOperationException("Cannot auto-assign already assigned/closed case.");
            }
            var Officer = officerCaseStatistics().OrderBy(t => t.OpenCases).First(t => t.Officer.PrecinctID == precintID).Officer;

            c.OfficerOnCaseID = Officer.BadgeNo;

            Update(c);
        }

        //NonCrud 4
        public IEnumerable<KeyValuePair<Officer, TimeSpan>> OfficerCaseAverageOpenTime()
        {
            return from x in ReadAll()
                   group x by x.OfficerOnCase into g
                   select new KeyValuePair<Officer, TimeSpan>
                   (
                       g.Key,
                       TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                   );
        }

        //NonCrud 5
        public IEnumerable<KeyValuePair<Precinct, TimeSpan>> PrecinctCaseAverageOpenTime()
        {
            return from x in ReadAll()
                   group x by x.Precinct into g
                   select new KeyValuePair<Precinct, TimeSpan>
                   (
                       g.Key,
                       TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                   );
        }

        //NonCrud 6
        public IEnumerable<Case> CasesOfPrecint(int PrecintID)
        {
            var p = PrecinctRepo.Read(PrecintID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }
            return ReadAll().Where(t => t.Precinct == p);
        }

        //NonCrud 7
        public IEnumerable<KeyValuePair<Precinct, IEnumerable<Case>>> CasesOfPrecincts()
        {
            return from x in ReadAll()
                   group x by x.Precinct into g
                   select new KeyValuePair<Precinct, IEnumerable<Case>>
                   (
                       g.Key,
                       g
                   );
        }
    }
}

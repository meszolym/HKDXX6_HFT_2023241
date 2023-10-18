using Castle.Core.Internal;
using HKDXX6_HFT_2023241.Logic;
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
        IRepository<Officer> OfficerRepo;
        IRepository<Precinct> PrecinctRepo;

        public CaseLogic(IRepository<Case> caseRepo, IRepository<Officer> officerRepo, IRepository<Precinct> precinctRepo)
        {
            CaseRepo = caseRepo;
            OfficerRepo = officerRepo;
            PrecinctRepo = precinctRepo;
        }

        public void Create(Case item)
        {
            if (item.PrimaryOfficer != null && !item.Officers.Any(t => t.BadgeNo == item.PrimaryOfficerBadgeNo))
            {
                throw new ArgumentException("Primary officer must be in the officers collection.");
            }
            CaseRepo.Create(item);
        }

        public void Update(Case item)
        {
            if (item.PrimaryOfficer != null && !item.Officers.Any(t => t.BadgeNo == item.PrimaryOfficerBadgeNo))
            {
                throw new ArgumentException("Primary officer must be in the officers collection.");
            }
            CaseRepo.Update(item);
        }

        public void Delete(int ID)
        {
            CaseRepo.Delete(ID);
        }

        public IEnumerable<Case> Read(int ID)
        {
            var c = CaseRepo.Read(ID);
            if (c == null)
            {
                throw new ArgumentException("Case does not exist.");
            }

            IEnumerable<Case> result = new Case[] { c };

            return result;
        }

        public IEnumerable<Case> ReadAll()
        {
            return CaseRepo.ReadAll();
        }

        public IEnumerable<OfficerCaseStatistic> officerCaseStatistics()
        {
            return from x in ReadAll()
                   group x by x.PrimaryOfficer into g
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

        public void AutoAssignCase(int id, int precintID, int numberOfOfficers)
        {
            Case c = Read(id).First();

            if (!c.Officers.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot auto-assign already assigned case.");
            }
            var Officers = officerCaseStatistics().OrderBy(t => t.OpenCases).Where(t => t.Officer.PrecinctID == precintID).Take(numberOfOfficers).Select(t => t.Officer);

            c.Officers = Officers as ICollection<Officer>;

            c.PrimaryOfficerBadgeNo = Officers.OrderByDescending(t => t.Rank).First().BadgeNo;

            Update(c);
        }

        public IEnumerable<KeyValuePair<Officer, TimeSpan>> OfficerCaseAverageOpenTime()
        {
            return from x in ReadAll()
                   group x by x.PrimaryOfficer into g
                   select new KeyValuePair<Officer, TimeSpan>
                   (
                       g.Key,
                       TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                   );
        }

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

        public IEnumerable<Case> CasesOfOfficer(int OfficerID)
        {
            return ReadAll().Where(t => t.Officers.Any(x => x.BadgeNo == OfficerID));
        }

        public IEnumerable<Case> CasesOfOfficerAsPrimary(int OfficerID)
        {
            return ReadAll().Where(t => t.PrimaryOfficerBadgeNo == OfficerID);
        }

        public IEnumerable<Case> CasesOfPrecint(int PrecintID)
        {
            var p = PrecinctRepo.Read(PrecintID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }
            return ReadAll().Where(t => t.Precinct == p);
        }

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

        public void AddOfficerToCase(int officerID, int caseID, bool primary = false)
        {
            Case c = Read(caseID).First();
            c.Officers.Add(OfficerRepo.Read(officerID));
            if (primary)
            {
                c.PrimaryOfficerBadgeNo = officerID;
            }
            Update(c);
        }
    }
}

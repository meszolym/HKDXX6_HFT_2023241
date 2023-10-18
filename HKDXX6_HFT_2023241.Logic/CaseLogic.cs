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
    public class CaseLogic
    {
        IRepository<Case> CaseRepo;

        public CaseLogic(IRepository<Case> caseRepo)
        {
            CaseRepo = caseRepo;
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
            if (c == null )
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
            return from x in CaseRepo.ReadAll()
                   group x by x.PrimaryOfficer into g
                   select new OfficerCaseStatistic
                   {
                       Officer = g.Key,
                       ClosedCases = (int)g.Count(t => t.IsClosed),
                       OpenCases = (int)g.Count(t => !t.IsClosed)
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
            return from x in CaseRepo.ReadAll()
                   group x by x.Precinct into g
                   select new PrecinctCaseStatistic
                   {
                       Precinct = g.Key,
                       ClosedCases = (int)g.Count(t => t.IsClosed),
                       OpenCases = (int)g.Count(t => !t.IsClosed)
                   };
        }

        public struct PrecinctCaseStatistic
        {
            public Precinct? Precinct { get; set; }
            public int ClosedCases { get; set;}
            public int OpenCases { get; set; }
        }

        public void AutoAssignCase(int id, int precintID, int numberOfOfficers)
        {
            Case c = CaseRepo.Read(id);

            if (!c.Officers.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot auto-assign already assigned case.");
            }
            var Officers = officerCaseStatistics().OrderBy(t => t.OpenCases).Where(t => t.Officer.PrecinctID == precintID).Take(numberOfOfficers).Select(t => t.Officer);

            c.Officers = Officers as ICollection<Officer>;

            c.PrimaryOfficerBadgeNo = Officers.OrderByDescending(t => t.Rank).First().BadgeNo;

            CaseRepo.Update(c);
        }
    }
}

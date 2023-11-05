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
            if (item.Name.Length < 10)
            {
                throw new ArgumentException("Name of case must be at least 10 characters.");
            }
            if (item.Description.Length < 15)
            {
                throw new ArgumentException("Description of case must be at least 15 characters.");
            }

            CaseRepo.Create(item);
        }

        public void Update(Case item)
        {
            if (Read(item.ID).IsClosed && item.IsClosed)
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
            return CaseRepo.ReadAll().ToList();
        }


        //NonCrud 1
        public IEnumerable<OfficerCaseStatistic> officerCaseStatistics()
        {
            var result = from x in ReadAll()
                   group x by x.OfficerOnCase into g
                   select new OfficerCaseStatistic
                   {
                       Officer = g.Key,
                       ClosedCases = g.Count(t => t.IsClosed),
                       OpenCases = g.Count(t => !t.IsClosed)
                   };

            return result.ToList();
        }

        public class OfficerCaseStatistic
        {
            public Officer Officer { get; set; }
            public int ClosedCases { get; set; }
            public int OpenCases { get; set; }

            public override bool Equals(object obj)
            {
                OfficerCaseStatistic b = obj as OfficerCaseStatistic;
                if (b == null)
                {
                    return false;
                }
                else
                {
                    return this.Officer.Equals(b.Officer)
                        && ClosedCases == b.ClosedCases
                        && OpenCases == b.OpenCases;
                }
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Officer, ClosedCases, OpenCases);
            }
        }

        //NonCrud 2
        public IEnumerable<PrecinctCaseStatistic> precinctCaseStatistics()
        {
            var result = from x in officerCaseStatistics()
                   group x by x.Officer.Precinct into g
                   select new PrecinctCaseStatistic
                   {
                       Precinct = g.Key,
                       OpenCases = g.Sum(t => t.OpenCases),
                       ClosedCases = g.Sum(t => t.ClosedCases)
                   };

            return result.ToList();
        }

        public class PrecinctCaseStatistic
        {
            public Precinct Precinct { get; set; }
            public int ClosedCases { get; set; }
            public int OpenCases { get; set; }

            public override bool Equals(object obj)
            {
                PrecinctCaseStatistic b = obj as PrecinctCaseStatistic;
                if (b == null)
                {
                    return false;
                }
                else
                {
                    return this.Precinct.Equals(b.Precinct)
                        && ClosedCases == b.ClosedCases
                        && OpenCases == b.OpenCases;
                }
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Precinct, ClosedCases, OpenCases);
            }
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
            var result = from x in ReadAll()
                   group x by x.OfficerOnCase into g
                   select new KeyValuePair<Officer, TimeSpan>
                   (
                       g.Key,
                       TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                   );

            return result.ToList();
        }

        //NonCrud 5
        public IEnumerable<KeyValuePair<Precinct, TimeSpan>> PrecinctCaseAverageOpenTime()
        {
            var result = from x in ReadAll()
                   group x by x.Precinct into g
                   select new KeyValuePair<Precinct, TimeSpan>
                   (
                       g.Key,
                       TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                   );
            return result.ToList();
        }

        //NonCrud 6
        public IEnumerable<Case> CasesOfPrecint(int PrecintID)
        {
            var p = PrecinctRepo.Read(PrecintID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }
            return ReadAll().Where(t => t.Precinct == p).ToList();
        }

        //NonCrud 7
        public IEnumerable<KeyValuePair<Precinct, IEnumerable<Case>>> CasesOfPrecincts()
        {
            var result = from x in ReadAll()
                   group x by x.Precinct into g
                   select new KeyValuePair<Precinct, IEnumerable<Case>>
                   (
                       g.Key,
                       g
                   );
            return result.ToList();
        }

    }
}

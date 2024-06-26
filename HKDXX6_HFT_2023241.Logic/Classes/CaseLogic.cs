﻿using Castle.Core.Internal;
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Models.NonCrudModels;
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
        IRepository<Officer> OfficerRepo;

        public CaseLogic(IRepository<Case> caseRepo, IRepository<Precinct> precinctRepo, IRepository<Officer> officerRepo)
        {
            CaseRepo = caseRepo;
            PrecinctRepo = precinctRepo;
            OfficerRepo = officerRepo;
        }

        public void Create(Case item)
        {
            if (item.ID < 0)
            {
                throw new ArgumentException("CaseID has to be positive or zero.");
            }
            if (item.Name.Length < 10 || item.Name.Length > 240)
            {
                throw new ArgumentException("Name of case must be at least 10, at most 240 characters.");
            }
            if (item.Description.Length < 15)
            {
                throw new ArgumentException("Description of case must be at least 15 characters.");
            }
            if (item.OpenedAt > DateTime.Now)
            {
                throw new ArgumentException("Cases in the future cannot be recorded.");
            }
            if (item.ClosedAt < item.OpenedAt)
            {
                throw new ArgumentException("Case cannot be closed before it is opened.");
            }
            if (item.ClosedAt > DateTime.Now)
            {
                throw new ArgumentException("Case closure cannot be in the future.");
            }
            if (item.OfficerOnCaseID != null)
            {
                if (OfficerRepo.Read(item.OfficerOnCaseID.Value) == null)
                {
                    throw new ArgumentException("The officer (to whom the case was to be assigned) does not exist.");
                }
            }
            CaseRepo.Create(item);
        }

        public void Update(Case item)
        {
            if (Read(item.ID).IsClosed && item.IsClosed)
            {
                throw new ArgumentException("Case has to be open to be updated.");
            }
            if (item.ClosedAt != null && item.ClosedAt < item.OpenedAt)
            {
                throw new ArgumentException("Case cannot be closed before it is opened.");
            }
            if (item.OpenedAt > DateTime.Now)
            {
                throw new ArgumentException("Cases in the future cannot be recorded.");
            }
            if (item.Name.Length < 10 || item.Name.Length > 240)
            {
                throw new ArgumentException("Name of case must be at least 10, at most 240 characters.");
            }
            if (item.Description.Length < 15)
            {
                throw new ArgumentException("Description of case must be at least 15 characters.");
            }
            if (item.ClosedAt > DateTime.Now)
            {
                throw new ArgumentException("Case closure cannot be in the future.");
            }
            if (item.OfficerOnCaseID != null)
            {
                if (OfficerRepo.Read(item.OfficerOnCaseID.Value) == null)
                {
                    throw new ArgumentException("The officer (to whom the case was to be assigned) does not exist.");
                }
            }

            CaseRepo.Update(item);
        }

        public void Delete(int ID)
        {
            var c = CaseRepo.Read(ID);
            if (c == null)
            {
                throw new ArgumentException("The case that should be deleted does not exist.");
            }

            CaseRepo.Delete(ID);
        }

        public Case Read(int ID)
        {
            var c = CaseRepo.Read(ID);
            if (c == null)
            {
                throw new ArgumentException("The case that you are looking for does not exist.");
            }

            return c;
        }

        public IEnumerable<Case> ReadAll()
        {
            return CaseRepo.ReadAll().ToList();
        }


        //NonCrud 1
        public IEnumerable<CasesPerOfficerStatistic> casesPerOfficerStatistics()
        {
            var result = from x in ReadAll()
                         where x.OfficerOnCase != null
                         group x by x.OfficerOnCase into g
                         select new CasesPerOfficerStatistic
                         {
                             Officer = g.Key,
                             ClosedCases = g.Count(t => t.IsClosed),
                             OpenCases = g.Count(t => !t.IsClosed)
                         };

            return result.ToList();
        }

        //NonCrud 2
        public IEnumerable<CasesPerPrecinctStatistic> casesPerPrecinctStatistics()
        {
            var result = from x in casesPerOfficerStatistics()
                   group x by x.Officer.Precinct into g
                   select new CasesPerPrecinctStatistic
                   {
                       Precinct = g.Key,
                       OpenCases = g.Sum(t => t.OpenCases),
                       ClosedCases = g.Sum(t => t.ClosedCases)
                   };

            return result.ToList();
        }

        //NonCrud 3
        public void AutoAssignCase(int id, int precintID)
        {
            Case c = Read(id);

            if (c == null)
            {
                throw new ArgumentException("The case that you are looking for does not exist.");
            }
            if (c.OfficerOnCase != null || c.IsClosed)
            {
                throw new InvalidOperationException("Cannot auto-assign already assigned/closed case.");
            }
            var p = PrecinctRepo.Read(precintID);
            if (p == null)
            {
                throw new ArgumentException("Precinct that should be used for auto-assign does not exist.");
            }

            var Officer = PrecinctRepo.ReadAll().First(p => p.ID == precintID).Officers
                .Where(o => o.Rank != Ranks.Captain)
                .OrderBy(o => o.Cases.Count(c => !c.IsClosed))
                .ThenByDescending(o => o.Rank).First();

            c.OfficerOnCaseID = Officer.BadgeNo;

            Update(c);
        }

        //NonCrud 4
        public IEnumerable<OfficerCaseAverageOpenTimeItem> OfficerCaseAverageOpenTime()
        {
            var result = from x in ReadAll()
                        where x.IsClosed == true
                        group x by x.OfficerOnCase into g
                        select new OfficerCaseAverageOpenTimeItem
                        (
                            g.Key,
                            TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                        );

            return result.ToList();
        }

        //NonCrud 5
        public IEnumerable<PrecinctCaseAverageOpenTimeItem> PrecinctCaseAverageOpenTime()
        {
            var result = from x in ReadAll()
                        where x.IsClosed == true
                        group x by x.Precinct into g
                        select new PrecinctCaseAverageOpenTimeItem
                        (
                            g.Key,
                            TimeSpan.FromTicks((long)g.Average(t => t.OpenTimeSpan.Value.Ticks))
                        );
            return result.ToList();
        }

        //NonCrud 6
        public IEnumerable<Case> CasesOfPrecint(int PrecinctID)
        {
            var p = PrecinctRepo.Read(PrecinctID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }
            return ReadAll().Where(t => t.Precinct != null && t.Precinct.ID == PrecinctID).ToList();
        }
    }
}

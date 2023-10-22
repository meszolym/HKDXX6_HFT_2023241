using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Logic
{
    public class OfficerLogic : IOfficerLogic
    {
        IRepository<Officer> OfficerRepo;

        public OfficerLogic(IRepository<Officer> officerRepo, IRepository<Precinct> precinctRepo)
        {
            OfficerRepo = officerRepo;
        }

        public void Create(Officer item)
        {
            if (item.Cases.Count != 0 || item.CasesAsPrimary.Count != 0 || item.OfficersUnderCommand.Count != 0)
            {
                throw new ArgumentException("Cases, CasesAsPrimary and OfficersUnderCommand cannot be filled when creating officer.");
            }
            if (item.Rank == Ranks.Captain && item.Precinct.Officers.Any(t => t.Rank == Ranks.Captain))
            {
                throw new ArgumentException("Can't have two captains at one precinct.");
            }
            OfficerRepo.Create(item);
        }

        public void Update(Officer item)
        {
            var o = OfficerRepo.Read(item.BadgeNo);
            if (o == null)
            {
                throw new ArgumentException("Officer does not exist.");
            }

            if (item.Cases.Count != o.Cases.Count || item.CasesAsPrimary.Count != o.CasesAsPrimary.Count || item.OfficersUnderCommand.Count != o.OfficersUnderCommand.Count)
            {
                throw new ArgumentException("Cases, CasesAsPrimary and OfficersUnderCommand cannot be updated from this side of the relationship.");
            }

            if (item.DirectCO.Precinct != item.Precinct)
            {
                throw new ArgumentException("Commanding officer has to be in the same precinct as officer.");
            }

            if (item.Rank == Ranks.Captain && item.Precinct.Officers.Any(t => t.Rank == Ranks.Captain && t.BadgeNo != item.BadgeNo))
            {
                throw new ArgumentException("Can't have two captains at one precinct.");
            }

            if (item.PrecinctID != o.PrecinctID)
            {
                RedirectOfficersUnderCommand(o);
            }

            OfficerRepo.Update(item);
        }

        private void RedirectOfficersUnderCommand(Officer o)
        {
            var p = o.Precinct;
            Officer c;

            if (p.Officers.Any(t => t.Rank == Ranks.Captain))
            {
                c = p.Officers.Single(t => t.Rank == Ranks.Captain);
            }
            else
            {
                c = p.Officers
                    .OrderByDescending(t => t.Rank)
                    .ThenBy(t => t.HireDate)
                    .ThenBy(t => t.CasesAsPrimary.Count())
                    .ThenBy(t => t.Cases.Count())
                    .ThenBy(t => t.FirstName + " " + t.LastName)
                    .First();
            }

            foreach (var ooc in o.OfficersUnderCommand)
            {
                ooc.DirectCO_BadgeNo = c.BadgeNo;
                Update(ooc);
            }
        }

        public void Delete(int ID)
        {
            OfficerRepo.Delete(ID);
        }

        public IEnumerable<Officer> Read(int ID)
        {
            var o = OfficerRepo.Read(ID);
            if (o == null)
            {
                throw new ArgumentException("Officer does not exist.");
            }

            IEnumerable<Officer> result = new Officer[] { o };

            return result;
        }

        public IEnumerable<Officer> ReadAll()
        {
            return OfficerRepo.ReadAll();
        }
    }
}

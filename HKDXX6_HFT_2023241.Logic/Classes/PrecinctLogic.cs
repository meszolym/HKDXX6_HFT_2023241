using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Logic
{
    public class PrecinctLogic : IPrecinctLogic
    {
        IRepository<Precinct> PrecinctRepo;

        public PrecinctLogic(IRepository<Precinct> precinctRepo)
        {
            PrecinctRepo = precinctRepo;
        }

        public void Create(Precinct item)
        {
            if (item.Officers.Count(t => t.Rank == Ranks.Captain) > 1)
            {
                throw new ArgumentException("Can't have two captains at one precinct.");
            }
            PrecinctRepo.Create(item);
        }

        public void Update(Precinct item)
        {
            if (item.Officers.Count(t => t.Rank == Ranks.Captain) > 1)
            {
                throw new ArgumentException("Can't have two captains at one precinct.");
            }
            PrecinctRepo.Update(item);
        }

        public void Delete(int ID)
        {
            PrecinctRepo.Delete(ID);
        }

        public IEnumerable<Precinct> Read(int ID)
        {
            var p = PrecinctRepo.Read(ID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }

            IEnumerable<Precinct> result = new Precinct[] { p };

            return result;
        }

        public IEnumerable<Precinct> ReadAll()
        {
            return PrecinctRepo.ReadAll();
        }

        public IEnumerable<Officer> GetCaptain(int precintID)
        {
            var p = Read(precintID).First();

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
                    .ThenBy(t => t.FirstName+ " "+t.LastName)
                    .First();
            }

            IEnumerable<Officer> result = new Officer[] { c };

            return result;
        }
    }
}

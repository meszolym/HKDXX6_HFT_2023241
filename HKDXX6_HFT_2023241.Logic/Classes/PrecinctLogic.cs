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
            if (item.Officers.Count != 0)
            {
                throw new ArgumentException("Officers must be empty when creating.");
            }
            PrecinctRepo.Create(item);
        }

        public void Update(Precinct item)
        {
            var p = PrecinctRepo.Read(item.ID);

            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }

            if (item.Officers.Count != p.Officers.Count)
            {
                throw new ArgumentException("Officers cannot be changed from this side of the relationship.");
            }
            PrecinctRepo.Update(item);
        }

        public void Delete(int ID)
        {
            PrecinctRepo.Delete(ID);
        }

        public Precinct Read(int ID)
        {
            var p = PrecinctRepo.Read(ID);
            if (p == null)
            {
                throw new ArgumentException("Precinct does not exist.");
            }

            return p;
        }

        public IEnumerable<Precinct> ReadAll()
        {
            return PrecinctRepo.ReadAll();
        }

        public Officer GetCaptain(int precintID)
        {
            var p = Read(precintID);

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
                    .ThenBy(t => t.Cases.Count())
                    .ThenBy(t => t.FirstName+ " "+t.LastName)
                    .First();
            }

            return c;
        }
    }
}

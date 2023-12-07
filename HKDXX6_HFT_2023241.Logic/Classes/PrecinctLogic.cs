using HKDXX6_HFT_2023241.Models.DBModels;
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
            if (item.ID < 1 || item.ID > 139)
            {
                throw new ArgumentException("PrecinctID must be between 1 and 139 inclusively.");
            }
            if (item.Address == null || item.Address.Length < 10 || item.Address.Length > 100)
            {
                throw new ArgumentException("Length of the precint's address must be between 10 and 100 characters.");
            }
            PrecinctRepo.Create(item);
        }

        public void Update(Precinct item)
        {
            var p = Read(item.ID);

            if (item.Address == null || item.Address.Length < 10 || item.Address.Length > 100)
            {
                throw new ArgumentException("Length of the precint's address must be between 10 and 100 characters.");
            }
            PrecinctRepo.Update(item);
        }

        public void Delete(int ID)
        {
            var p = PrecinctRepo.Read(ID);
            if (p == null)
            {
                throw new ArgumentException("The precinct that should be deleted does not exist.");
            }
            PrecinctRepo.Delete(ID);
        }

        public Precinct Read(int ID)
        {
            var p = PrecinctRepo.Read(ID);
            if (p == null)
            {
                throw new ArgumentException("The precinct you are looking for does not exist.");
            }

            return p;
        }

        public IEnumerable<Precinct> ReadAll()
        {
            return PrecinctRepo.ReadAll().ToList();
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

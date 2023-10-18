using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Logic
{
    public class PrecinctLogic
    {
        IRepository<Precinct> PrecinctRepo;

        public PrecinctLogic(IRepository<Precinct> precinctRepo)
        {
            PrecinctRepo = precinctRepo;
        }

        public void Create(Precinct item)
        {
            PrecinctRepo.Create(item);
        }

        public void Update(Precinct item)
        {
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
    }
}

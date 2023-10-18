using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Logic
{
    public class OfficerLogic
    {
        IRepository<Officer> OfficerRepo;

        public OfficerLogic(IRepository<Officer> caseRepo)
        {
            OfficerRepo = caseRepo;
        }

        public void Create(Officer item)
        {
            if (item.Rank == Ranks.Captain && item.Precinct.Officers.Any(t => t.Rank == Ranks.Captain))
            {
                throw new ArgumentException("Can't have a second captain to precinct.");
            }
            OfficerRepo.Create(item);
        }

        public void Update(Officer item)
        {
            if (item.Rank == Ranks.Captain && item.Precinct.Officers.Any(t => t.Rank == Ranks.Captain && t.BadgeNo != item.BadgeNo))
            {
                throw new ArgumentException("Can't have a second captain to precinct.");
            }

            OfficerRepo.Update(item);
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

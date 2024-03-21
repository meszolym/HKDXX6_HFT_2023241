using HKDXX6_HFT_2023241.Models.DBModels;
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

        public OfficerLogic(IRepository<Officer> officerRepo)
        {
            OfficerRepo = officerRepo;
        }

        public void Create(Officer item)
        {
            if (item.BadgeNo < 0)
            {
                throw new ArgumentException("BadgeNo has to be positive or zero.");
            }
            if (item.FirstName.Length < 2 || item.LastName.Length <2)
            {
                throw new ArgumentException("First and last name of the officer must be at least two characters long");
            }
            if (item.PrecinctID < 1 ||  item.PrecinctID > 139)
            {
                throw new ArgumentException("Officer's PrecinctID must be between 1 and 139 inclusively.");
            }
            if (item.HireDate > DateTime.Now)
            {
                throw new ArgumentException("Officer's hire date cannot be in the future.");
            }
            if (item.Rank == Ranks.Captain
                && OfficerRepo.ReadAll()
                    .Any(t => t.PrecinctID == item.PrecinctID
                    && t.Rank == Ranks.Captain))
            {
                throw new ArgumentException("Cannot have two captains at one precinct.");
            }
            if (item.DirectCO_BadgeNo != null)
            {
                Officer co;
                try
                {
                     co = Read(item.DirectCO_BadgeNo.Value);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message == "The officer you are looking for does not exist.")
                    {
                        throw new ArgumentException("The specified direct CO does not exist.");
                    }
                    else
                    {
                        throw;
                    }
                }
                
                if (co.PrecinctID != item.PrecinctID)
                {
                    throw new ArgumentException("Commanding officer has to be in the same precinct as the officer.");
                }
            }
            OfficerRepo.Create(item);
        }

        public void Update(Officer item)
        {
            var o = Read(item.BadgeNo);


            if (item.Rank == Ranks.Captain
                && OfficerRepo.ReadAll()
                    .Any(t => t.PrecinctID == item.PrecinctID
                    && t.Rank == Ranks.Captain
                    && t.BadgeNo != item.BadgeNo))
            {
                throw new ArgumentException("Cannot have two captains at one precinct.");
            }

            if (item.FirstName.Length < 2 || item.LastName.Length < 2)
            {
                throw new ArgumentException("First and last name of the officer must be at least two characters long");
            }
            if (item.PrecinctID < 1 || item.PrecinctID > 139)
            {
                throw new ArgumentException("Officer's PrecinctID must be between 1 and 139 inclusively.");
            }

            if (item.DirectCO_BadgeNo != null)
            {
                Officer co;
                try
                {
                    co = Read(item.DirectCO_BadgeNo.Value);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message == "The officer you are looking for does not exist.")
                    {
                        throw new ArgumentException("The specified direct CO does not exist.");
                    }
                    else
                    {
                        throw;
                    }
                }

                if (co.PrecinctID != item.PrecinctID)
                {
                    throw new ArgumentException("Commanding officer has to be in the same precinct as officer.");
                }
            }

            if (item.HireDate > DateTime.Now)
            {
                throw new ArgumentException("HireDate cannot be in the future.");
            }

            if (item.PrecinctID != o.PrecinctID)
            {
                RedirectOfficersUnderCommand(o);
            }

            OfficerRepo.Update(item);
        }

        private void RedirectOfficersUnderCommand(Officer o)
        {
            if (o.OfficersUnderCommand.Count == 0)
            {
                return;
            }
            var p = o.Precinct;
            Officer c;
            
            
            if (p.Officers.Any(t => t.Rank == Ranks.Captain && t.BadgeNo != o.BadgeNo))
            {
                c = p.Officers.Single(t => t.Rank == Ranks.Captain && t.BadgeNo != o.BadgeNo);
            }
            else
            {
                c = p.Officers
                    .Where(t => t.BadgeNo != o.BadgeNo)
                    .OrderByDescending(t => t.Rank)
                    .ThenBy(t => t.HireDate)
                    .ThenBy(t => t.Cases.Count())
                    .ThenBy(t => t.FirstName + " " + t.LastName)
                    .First();
            }

            foreach (var ooc in o.OfficersUnderCommand)
            {
                if (ooc.BadgeNo != c.BadgeNo)
                {
                    ooc.DirectCO_BadgeNo = c.BadgeNo;
                }
                else
                {
                    ooc.DirectCO_BadgeNo = null;
                }
                Update(ooc);
            }
        }

        public void Delete(int ID)
        {
            var o = OfficerRepo.Read(ID);
            if (o == null)
            {
                throw new ArgumentException("The officer that should be deleted does not exist.");
            }
            if (o.Cases.Count() > 0)
            {
                throw new ArgumentException("Cannot delete officer if there are any cases to which they are attached to.");
            }

            RedirectOfficersUnderCommand(o);
            OfficerRepo.Delete(ID);
        }

        public Officer Read(int ID)
        {
            var o = OfficerRepo.Read(ID);
            if (o == null)
            {
                throw new ArgumentException("The officer you are looking for does not exist.");
            }

            return o;
        }

        public IEnumerable<Officer> ReadAll()
        {
            return OfficerRepo.ReadAll().ToList();
        }
    }
}

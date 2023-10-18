using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Repository
{
    public class OfficerRepository : Repository<Officer>, IRepository<Officer>
    {
        public OfficerRepository(PoliceDbContext ctx) : base(ctx)
        {
        }
            
        public override Officer Read(int id)
        {
            return ctx.Officers.FirstOrDefault(t => t.BadgeNo == id);
        }

        public override void Update(Officer item)
        {
            var old = Read(item.BadgeNo);

            old.Cases = item.Cases;
            old.DirectCO = item.DirectCO;
            old.DirectCO_BadgeNo = item.DirectCO_BadgeNo;
            old.FirstName = item.FirstName;
            old.LastName = item.LastName;
            old.OfficersUnderCommand = item.OfficersUnderCommand;
            old.Precinct = item.Precinct;
            old.PrecinctID = item.PrecinctID;
            old.Rank = item.Rank;

            ctx.SaveChanges();
        }
    }
}

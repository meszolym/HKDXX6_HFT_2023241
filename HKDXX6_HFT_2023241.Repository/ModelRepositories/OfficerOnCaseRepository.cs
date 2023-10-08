using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Repository
{
    public class OfficerOnCaseRepository : Repository<OfficerOnCase>, IRepository<OfficerOnCase>
    {
        public OfficerOnCaseRepository(PoliceDbContext ctx) : base(ctx)
        {
        }

        public override OfficerOnCase Read(uint id)
        {
            return ctx.Officer_x_Case.FirstOrDefault(x => x.ID == id);
        }

        public override void Update(OfficerOnCase item)
        {
            var old = Read(item.ID);

            old.Case = item.Case;
            old.CaseID = item.CaseID;
            old.IsPrimary = item.IsPrimary;
            old.Officer = item.Officer;
            old.OfficerBadgeNo = item.OfficerBadgeNo;

            ctx.SaveChanges();
        }
    }
}

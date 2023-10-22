using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Repository
{
    public class CaseRepository : Repository<Case>, IRepository<Case>
    {
        public CaseRepository(PoliceDbContext ctx) : base(ctx)
        {
        }

        public override Case Read(int id)
        {
            return ctx.Cases.FirstOrDefault(x => x.ID == id);
        }

        public override void Update(Case item)
        {
            var old = Read(item.ID);

            old.ClosedAt = item.ClosedAt;
            old.Description = item.Description;
            old.Name = item.Name;
            old.Officers = item.Officers;
            old.OpenedAt = item.OpenedAt;
            old.PrimaryOfficerBadgeNo = item.PrimaryOfficerBadgeNo;

            ctx.SaveChanges();
        }
    }
}

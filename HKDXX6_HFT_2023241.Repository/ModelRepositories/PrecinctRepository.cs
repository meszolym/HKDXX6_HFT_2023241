using HKDXX6_HFT_2023241.Models;
using HKDXX6_HFT_2023241.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Repository
{
    public class PrecinctRepository : Repository<Precinct>, IRepository<Precinct>
    {
        public PrecinctRepository(PoliceDbContext ctx) : base(ctx)
        {
        }

        public override Precinct Read(int id)
        {
            return ctx.Precincts.FirstOrDefault(x => x.ID == id);
        }

        public override void Update(Precinct item)
        {
            var old = Read(item.ID);

            old.Address = item.Address;
            old.Officers = item.Officers;

            ctx.SaveChanges();
        }
    }
}

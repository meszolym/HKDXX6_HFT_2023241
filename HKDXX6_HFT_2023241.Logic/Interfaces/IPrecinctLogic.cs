using HKDXX6_HFT_2023241.Models;
using System.Collections.Generic;

namespace HKDXX6_HFT_2023241.Logic
{ 
    public interface IPrecinctLogic
    {
        void Create(Precinct item);
        void Delete(int ID);
        Officer GetCaptain(int precintID);
        Precinct Read(int ID);
        IEnumerable<Precinct> ReadAll();
        void Update(Precinct item);
        
    }
}
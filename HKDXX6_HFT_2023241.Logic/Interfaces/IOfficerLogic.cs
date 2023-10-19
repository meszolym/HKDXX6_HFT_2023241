﻿using HKDXX6_HFT_2023241.Models;
using System.Collections.Generic;

namespace HKDXX6_HFT_2023241.Logic
{
    public interface IOfficerLogic
    {
        void Create(Officer item);
        void Delete(int ID);
        IEnumerable<Officer> Read(int ID);
        IEnumerable<Officer> ReadAll();
        void Update(Officer item);
    }
}
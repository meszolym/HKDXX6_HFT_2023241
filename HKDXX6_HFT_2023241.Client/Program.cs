using System;

namespace HKDXX6_HFT_2023241.Client
{
    internal class Program
    {
        static RestService rest;
        static void Main(string[] args)
        {
            rest = new("http://localhost:24150");
        }
    }
}

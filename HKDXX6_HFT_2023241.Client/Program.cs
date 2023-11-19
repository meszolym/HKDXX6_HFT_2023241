using HKDXX6_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using ConsoleTables;
using System.Diagnostics.Contracts;

namespace HKDXX6_HFT_2023241.Client
{
    internal class Program
    {
        static RestService Rest;

        static void Main(string[] args)
        {
            Rest = new("http://localhost:33410/", "Case");
            List("Precinct");
        }

        

        static void List(string TypeName)
        {
            if (TypeName == nameof(Case))
            {
                List<Case> cases = Rest.Get<Case>("Case");
                var table = new ConsoleTable("ID", "Name", "Officer on case");
                foreach (var item in cases)
                {
                    table.AddRow(item.ID, item.Name, item.OfficerOnCase.FirstName + " " + item.OfficerOnCase.LastName);
                }
                table.Write(Format.Minimal);
                return;
            }
            if (TypeName == nameof(Officer))
            {
                List<Officer> officers = Rest.Get<Officer>("Officer");
                var table = new ConsoleTable("BadgeNo", "Precinct", "Rank", "First Name", "Last Name");
                foreach(var item in officers)
                {
                    table.AddRow(item.BadgeNo, item.PrecinctID, item.Rank.ToString(), item.FirstName, item.LastName);
                }
                table.Write(Format.Minimal);
            }
            if(TypeName == nameof(Precinct))
            {
                List<Precinct> precincts = Rest.Get<Precinct>("Precinct");
                var table = new ConsoleTable("PrecinctNo", "Address", "Ranking Officer");
                foreach (var item in precincts)
                {
                    var cap = Rest.GetSingle<Officer>($"Precinct/GetCaptain/{item.ID}");
                    table.AddRow(item.ID, item.Address, $"{cap.FirstName} {cap.LastName} ({cap.Rank})");
                }
                table.Write(Format.Minimal);
            }
        }

        static void GetDetails(string TypeName)
        {
            Console.WriteLine("Getting details of ");
            Console.Write("Please provide the ID to look up or put * to go back to the main menu: ");
            string input = Console.ReadLine();
            if (input == "*") return;

            int inputInt;

            while (!int.TryParse(input, out inputInt))
            {
                Console.WriteLine("Invalid input. Please try again: ");
                input = Console.ReadLine();
                if (input == "*") return;
            }
            var table = new ConsoleTable("Field", "Data");
            if (TypeName == nameof(Case))
            {
                var c = Rest.GetSingle<Case>($"Case/{inputInt}");
                table.AddRow()

            }
            if (TypeName == nameof(Officer))
            {
                
            }
            if (TypeName == nameof(Precinct))
            {
                
            }
        }


    }
}

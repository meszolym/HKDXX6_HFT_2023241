using HKDXX6_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using ConsoleTables;
using System.Diagnostics.Contracts;
using System.Text;

namespace HKDXX6_HFT_2023241.Client
{
    internal class Program
    {
        static RestService Rest;

        static void Main(string[] args)
        {
            Rest = new("http://localhost:33410/", "Case");
            GetDetails("Precinct");
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
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }
            if (TypeName == nameof(Officer))
            {
                List<Officer> officers = Rest.Get<Officer>("Officer");
                var table = new ConsoleTable("BadgeNo.", "Precinct", "Rank", "First Name", "Last Name");
                foreach(var item in officers)
                {
                    table.AddRow(item.BadgeNo, item.PrecinctID, item.Rank.ToString(), item.FirstName, item.LastName);
                }
                table.Write(Format.Minimal);
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }
            if(TypeName == nameof(Precinct))
            {
                List<Precinct> precincts = Rest.Get<Precinct>("Precinct");
                var table = new ConsoleTable("PrecinctNo", "Address");
                foreach (var item in precincts)
                {
                    table.AddRow(item.ID, item.Address);
                }
                table.Write(Format.Minimal);
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("This kind of information is not stored in the system.");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void GetDetails(string TypeName)
        {
            Console.WriteLine($"Getting details of a {TypeName.ToLower()}");
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
                Case c;
                try
                {
                    c = Rest.GetSingle<Case>($"Case/{inputInt}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"[!] Error: {ex.Message}");
                    Console.WriteLine($"Please contact the administrator of the system for further help.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine();
                table.AddRow("ID", c.ID);
                table.AddRow("Name", c.Name);
                table.AddRow("Recorded at", c.OpenedAt);
                if (c.IsClosed)
                {
                    table.AddRow("Closed at", c.ClosedAt);
                    table.AddRow("Open time", c.OpenTimeSpan);
                }
                if (c.OfficerOnCaseID != null)
                {
                    string ending;
                    switch (c.OfficerOnCase.PrecinctID.ToString()[c.OfficerOnCase.PrecinctID.ToString().Length-1])
                    {
                        case '1':
                            ending = "st";
                            break;
                        case '2':
                            ending = "nd";
                            break;
                        case '3':
                            ending = "rd";
                            break;
                        default:
                            ending = "th";
                            break;
                    }


                    table.AddRow("Officer on case badgeNo.", c.OfficerOnCaseID);
                    table.AddRow("Officer on case", $"{c.OfficerOnCase.Rank} {c.OfficerOnCase.FirstName} {c.OfficerOnCase.LastName}, {c.OfficerOnCase.PrecinctID}{ending} Precinct");
                }
                table.Write(Format.Minimal);
                Console.WriteLine($"Description:\r\n{c.Description}");
                Console.WriteLine();
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;

            }
            if (TypeName == nameof(Officer))
            {
                Officer o;
                try
                {
                    o = Rest.GetSingle<Officer>($"Officer/{inputInt}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"[!] Error: {ex.Message}");
                    Console.WriteLine($"Please contact the administrator of the system for further help.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine();
                table.AddRow("BadgeNo.", o.BadgeNo);
                table.AddRow("First name", o.FirstName);
                table.AddRow("Last name", o.LastName);
                table.AddRow("Rank", o.Rank);
                table.AddRow("Precinct", o.PrecinctID);
                if (o.DirectCO_BadgeNo != null)
                {
                    table.AddRow("Commanding officer badgeNo", o.DirectCO_BadgeNo);
                    table.AddRow("Commanding officer", $"{o.DirectCO.Rank} {o.DirectCO.FirstName} {o.DirectCO.LastName}");
                }
                table.AddRow("Count of all cases", o.Cases.Count);
                table.AddRow("Count of officers under command", o.OfficersUnderCommand.Count);

                table.Write(Format.Minimal);
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;

            }
            if (TypeName == nameof(Precinct))
            {
                Precinct p;
                try
                {
                    p = Rest.GetSingle<Precinct>($"Precinct/{inputInt}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine($"[!] Error: {ex.Message}");
                    Console.WriteLine($"Please contact the administrator of the system for further help.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine();

                table.AddRow("PrecinctNo", p.ID);
                table.AddRow("Address", p.Address);
                if (p.Officers.Count > 0)
                {
                    try
                    {
                        var ro = Rest.GetSingle<Officer>($"Precinct/GetCaptain/{inputInt}");
                        table.AddRow("Ranking officer", $"{ro.Rank} {ro.FirstName} {ro.LastName}");
                    }
                    catch
                    {
                        table.AddRow("Ranking officer", $"[!] Could not find information");
                    }
                }
                table.AddRow("Count of officers", p.Officers.Count);

                table.Write(Format.Minimal);
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;

            }

            Console.WriteLine("This kind of information is not stored in the system.");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void Update(string TypeName)
        {

        }

        static void Create(string TypeName)
        {

        }

        static void Delete(string TypeName)
        {

        }

        static void OfficerCaseStatistics()
        {

        }

        static void PrecinctCaseStatistics()
        {

        }

        static void AutoAssignCase()
        {

        }

        static void CaseAverageOpenTimePerOfficer()
        {

        }

        static void CaseAverageOpenTimePerPrecinct()
        {

        }

        static void CasesOfPrecinct()
        {

        }

        static void CasesOfPrecincts()
        {

        }

    }
}

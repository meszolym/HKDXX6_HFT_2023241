using HKDXX6_HFT_2023241.Models;
using System;
using System.Collections.Generic;
using ConsoleTables;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HKDXX6_HFT_2023241.Client
{
    internal enum Status
    {
        OK,
        ERRORED
    }

    internal class Program
    {
        static RestService Rest;

        static void Main(string[] args)
        {
            Rest = new("http://localhost:33410/", "Case");
            Create("Case");
        }

        static void WriteErrorMsg(string msg)
        {
            Console.WriteLine();
            Console.WriteLine($"[!] Error: {msg}");
            Console.WriteLine($"Please contact the administrator of the system for further help.");
            Console.WriteLine("Press any key to go back.");
        }

        static void List(string TypeName)
        {
            Console.WriteLine($"Listing all {TypeName.ToLower()}s");
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

        static Status GetDetails(string TypeName, int? id = null)
        {
            string input = string.Empty;
            int inputInt;
            if (id == null)
            {
                Console.WriteLine($"Getting details of a(n) {TypeName.ToLower()}");
                Console.Write("Please provide the ID to look up or put an asterisk (*) to go back to the main menu: ");
                input = Console.ReadLine();

                if (input == "*")
                {
                    return Status.OK;
                }
            }
            else
            {
                inputInt = id.Value;
            }

            while (!int.TryParse(input, out inputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine();
                if (input == "*")
                {
                    
                    return Status.OK;
                }
            }
            var table = new ConsoleTable("Field", "Data");
            if (TypeName == nameof(Case))
            {
                Case c;
                try
                {
                    c = Rest.GetSingle<Case>($"{TypeName}/{inputInt}");
                }
                catch (Exception ex)
                {

                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return Status.ERRORED;
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
                if (id == null)
                {
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                }
                
                return Status.OK;

            }
            if (TypeName == nameof(Officer))
            {
                Officer o;
                try
                {
                    o = Rest.GetSingle<Officer>($"{TypeName}/{inputInt}");
                }
                catch (Exception ex)
                {

                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return Status.ERRORED;
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
                if (id == null)
                {
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                }
                
                return Status.OK;

            }
            if (TypeName == nameof(Precinct))
            {
                Precinct p;
                try
                {
                    p = Rest.GetSingle<Precinct>($"{TypeName}/{inputInt}");
                }
                catch (Exception ex)
                {
                    
                    
                    Console.ReadKey();
                    return Status.ERRORED;
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
                if (id == null)
                {
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                }
                
                return Status.OK;

            }

            Console.WriteLine("This kind of information is not stored in the system.");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
            return Status.ERRORED;
        }

        static void Update(string TypeName)
        {
            Console.WriteLine($"Updating a(n) {TypeName.ToLower()}");
            Console.Write("Please provide the ID to update or put an asterisk (*) to go back to the main menu: ");
            string input = Console.ReadLine();
            if (input == "*") return;

            int inputInt;

            while (!int.TryParse(input, out inputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine();
                if (input == "*") return;
            }

            Status result = GetDetails(TypeName, inputInt);
            if (result == Status.ERRORED) return;
            Console.WriteLine();
            Console.Write("Are you sure you want to update this item? (y/n): ");
            string inputYN = Console.ReadLine();
            while (inputYN.ToUpper() != "Y" || inputYN.ToUpper() != "N")
            {
                Console.Write("Invalid input. Please try again: ");
                inputYN = Console.ReadLine();
            }

            if (inputYN.ToUpper() != "Y")
            {
                Console.WriteLine("No changes were made.");
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            Console.WriteLine("Enter new data below. If you would like to skip updating something, please put an asterisk (*) in the field.");

            if (TypeName == nameof(Case))
            {
                Case updated = Rest.GetSingle<Case>($"{TypeName}/{inputInt}");

                Console.Write("Name: ");
                string nameInput = Console.ReadLine();
                if (nameInput != "*") updated.Name = nameInput;

                Console.Write("Description: ");
                string descInput = Console.ReadLine();
                if (descInput != "*") updated.Description = descInput;

                Console.Write("Recorded at: ");
                string openedDtInputString = Console.ReadLine();
                if (openedDtInputString != "*")
                {
                    DateTime openedDt;

                    while (!DateTime.TryParse(openedDtInputString, out openedDt))
                    {
                        Console.Write("Invalid input for recorded at, please try again: ");
                        openedDtInputString = Console.ReadLine();
                    }

                    updated.OpenedAt = openedDt;
                }

                Console.Write("Closed at: ");
                string closedDtInputString = Console.ReadLine();
                if (closedDtInputString != "*")
                {
                    DateTime closedDt;

                    while (!DateTime.TryParse(closedDtInputString, out closedDt))
                    {
                        Console.Write("Invalid input for closed at, please try again: ");
                        closedDtInputString = Console.ReadLine();
                    }

                    updated.ClosedAt = closedDt;
                }    

                Console.Write("Officer on case badgeNo.: ");
                string officerIDInputString = Console.ReadLine();
                if (officerIDInputString != "*")
                {
                    int officerID;

                    while (!int.TryParse(officerIDInputString, out officerID))
                    {
                        Console.Write("Invalid input for officer badgeNo., please try again: ");
                        officerIDInputString = Console.ReadLine();
                    }

                    updated.OfficerOnCaseID = officerID;
                }

                try
                {
                    Rest.Put(updated, TypeName);
                    Console.WriteLine($"Case[{inputInt}] successfully updated.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch(Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }

            }
            if (TypeName == nameof(Officer))
            {
                Officer updated = Rest.GetSingle<Officer>($"{TypeName}/{inputInt}");

                Console.Write("First name: ");
                string fnameInput = Console.ReadLine();
                if (fnameInput != "*") updated.FirstName = fnameInput;

                Console.Write("Last name: ");
                string lnameInput = Console.ReadLine();
                if (lnameInput != "*") updated.LastName = lnameInput;

                Console.Write("Rank: ");
                string rankInputString = Console.ReadLine();
                if (rankInputString != "*")
                {
                    object rank;

                    while (!Enum.TryParse(typeof(Ranks), rankInputString, out rank))
                    {
                        Console.Write("Invalid input for rank, please try again: ");
                        rankInputString = Console.ReadLine();
                    }

                    updated.Rank = (Ranks)rank;
                }

                if (updated.Rank != Ranks.Captain)
                {
                    Console.Write("Direct CO badgeNo.: ");
                    string directCoIdInputString = Console.ReadLine();
                    if (directCoIdInputString != "*")
                    {
                        int directCoId;
                        while (!int.TryParse(directCoIdInputString, out directCoId))
                        {
                            Console.Write("Invalid input for direct CO badgeNo., please try again: ");
                            directCoIdInputString = Console.ReadLine();
                        }
                        updated.DirectCO_BadgeNo = directCoId;
                    }
                }
                else
                {
                    updated.DirectCO_BadgeNo = null;
                }
                

                Console.Write("Precinct ID: ");
                string precinctIdInputString = Console.ReadLine();
                if (precinctIdInputString != "*")
                {
                    int precinctId;

                    while (!int.TryParse(precinctIdInputString, out precinctId))
                    {
                        Console.Write("Invalid input for precinct ID, please try again: ");
                        precinctIdInputString = Console.ReadLine();
                    }
                    updated.PrecinctID = precinctId;
                }


                Console.Write("Hired at: ");
                string hireDateInputString = Console.ReadLine();
                if (hireDateInputString != "*")
                {
                    DateTime hireDate;

                    while (!DateTime.TryParse(hireDateInputString, out hireDate))
                    {
                        Console.Write("Invalid input for hire date, please try again: ");
                        hireDateInputString = Console.ReadLine();
                    }
                    updated.HireDate = hireDate;
                }

                try
                {
                    Rest.Put(updated, TypeName);
                    Console.WriteLine($"Officer[{inputInt}] successfully updated.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch (Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }

            }
            if (TypeName == nameof(Precinct))
            {
                Precinct updated = Rest.GetSingle<Precinct>($"{TypeName}/{inputInt}");

                Console.Write("Address: ");
                string addressInput = Console.ReadLine();
                if (addressInput != "*") updated.Address = addressInput;

                try
                {
                    Rest.Put(updated, TypeName);
                    Console.WriteLine($"Precinct[{inputInt}] successfully updated.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch(Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }
            }
        }

        static void Create(string TypeName)
        {
            Console.WriteLine($"Creating a(n) {TypeName.ToLower()}");

            Console.WriteLine("Enter new data below.");
            if (TypeName == nameof(Case))
            {

                Console.Write("Case name: ");
                string caseNameInput = Console.ReadLine();

                Console.Write("Case description: ");
                string caseDescInput = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("The following fields are not mandatory, you may leave them empty.");
                Console.Write("Case closed at: ");
                string closedAtInputString = Console.ReadLine();
                DateTime? closedAt = null;
                if (closedAtInputString != string.Empty)
                {
                    DateTime parsed;
                    while (!DateTime.TryParse(closedAtInputString, out parsed))
                    {
                        Console.Write("Invalid input for closed at, please try again: ");
                        closedAtInputString = Console.ReadLine();
                    }
                    closedAt = parsed;
                }

                Console.Write("Officer on case badgeNo.: ");
                string officerOnCaseIdInputString = Console.ReadLine();
                int? officerID = null;
                if (officerOnCaseIdInputString != string.Empty)
                {
                    int parsed;
                    while (!int.TryParse(officerOnCaseIdInputString, out parsed))
                    {
                        Console.Write("Invalid input for officer on case badgeNo., please try again: ");
                        officerOnCaseIdInputString = Console.ReadLine();
                    }
                    officerID = parsed;
                }

                Case added = new Case()
                {
                    Name = caseNameInput,
                    Description = caseDescInput,
                    OpenedAt = DateTime.Now,
                    ClosedAt = closedAt,
                    OfficerOnCaseID = officerID
                };

                try
                {
                    Rest.Post(added, TypeName);
                    Console.WriteLine("Case successfully added.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch (Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }

            }
            if (TypeName == nameof(Officer))
            {
                Officer added = new Officer();
                Console.Write("First name: ");
                string fnameInput = Console.ReadLine();
                added.FirstName = fnameInput;

                Console.Write("Last name: ");
                string lnameInput = Console.ReadLine();
                added.LastName = lnameInput;

                Console.WriteLine("Rank: ");
                string rankInputString = Console.ReadLine();

                object parsed;

                while (!Enum.TryParse(typeof(Ranks), rankInputString, out parsed))
                {
                    Console.Write("Invalid input for rank, please try again: ");
                    rankInputString = Console.ReadLine();
                }

                added.Rank = (Ranks)parsed;

                if (added.Rank != Ranks.Captain)
                {
                    Console.Write("Direct CO badgeNo.: ");
                    string directCoIdInputString = Console.ReadLine();

                    int directCoId;
                    while (!int.TryParse(directCoIdInputString, out directCoId))
                    {
                        Console.Write("Invalid input for direct CO badgeNo., please try again: ");
                        directCoIdInputString = Console.ReadLine();
                    }
                    added.DirectCO_BadgeNo = directCoId;
                }
                else
                {
                    added.DirectCO_BadgeNo = null;
                }

                Console.Write("Precinct ID: ");
                string precinctIdInputString = Console.ReadLine();
               
                int precinctId;

                while (!int.TryParse(precinctIdInputString, out precinctId))
                {
                    Console.Write("Invalid input for precinct ID, please try again: ");
                    precinctIdInputString = Console.ReadLine();
                }
                added.PrecinctID = precinctId;
                


                Console.Write("Hired at: ");
                string hireDateInputString = Console.ReadLine();

                DateTime hireDate;

                while (!DateTime.TryParse(hireDateInputString, out hireDate))
                {
                    Console.Write("Invalid input for hire date, please try again: ");
                    hireDateInputString = Console.ReadLine();
                }
                added.HireDate = hireDate;

                try
                {
                    Rest.Post(added, TypeName);
                    Console.WriteLine("Officer successfully added.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch (Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }
                
            }
            if (TypeName == nameof(Precinct))
            {
                Console.Write("Precinct ID: ");
                string precinctIdInputString = Console.ReadLine();

                int precinctId;

                while (!int.TryParse(precinctIdInputString, out precinctId))
                {
                    Console.Write("Invalid input for precinct ID, please try again: ");
                    precinctIdInputString = Console.ReadLine();
                }

                Console.Write("Address: ");
                string addressInput = Console.ReadLine();

                Precinct added = new Precinct(precinctId, addressInput);

                try
                {
                    Rest.Post(added, TypeName);
                    Console.WriteLine("Precinct successfully added.");
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                    return;
                }
                catch(Exception ex)
                {
                    WriteErrorMsg(ex.Message);
                    Console.ReadKey();
                    return;
                }

            }
            Console.WriteLine("This kind of information is not stored in the system.");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void Delete(string TypeName)
        {
            Console.WriteLine($"Deleting a(n) {TypeName.ToLower()}");
            Console.Write("Please provide the ID to delete or put an asterisk (*) to go back to the main menu: ");
            string input = Console.ReadLine();
            if (input == "*") return;

            int inputInt;

            while (!int.TryParse(input, out inputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine();
                if (input == "*") return;
            }

            Status result = GetDetails(TypeName, inputInt);
            if (result == Status.ERRORED) return;


            Console.WriteLine();
            Console.Write("Are you sure you want to delete this item? (y/n): ");
            string inputYN = Console.ReadLine();
            while (inputYN.ToUpper() != "Y" || inputYN.ToUpper() != "N")
            {
                Console.Write("Invalid input. Please try again: ");
                inputYN = Console.ReadLine();
            }

            if (inputYN.ToUpper() != "Y")
            {
                Console.WriteLine("No changes were made.");
                Console.WriteLine("Press any key to go back.");
                Console.ReadKey();
                return;
            }

            Rest.Delete(inputInt, TypeName);
            Console.WriteLine($"{TypeName}[{inputInt}] deleted sucsessfully.");
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void OfficerCaseStatistics()
        {
            //TODO
        }

        static void PrecinctCaseStatistics()
        {
            //TODO
        }

        static void AutoAssignCase()
        {
            //TODO
        }

        static void CaseAverageOpenTimePerOfficer()
        {
            //TODO
        }

        static void CaseAverageOpenTimePerPrecinct()
        {
            //TODO
        }

        static void CasesOfPrecinct()
        {
            //TODO
        }

        static void CasesOfPrecincts()
        {
            //TODO
        }

    }
}

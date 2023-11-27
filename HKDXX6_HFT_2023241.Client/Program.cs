using System;
using System.Collections.Generic;
using ConsoleTables;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using HKDXX6_HFT_2023241.Models.DBModels;
using ConsoleTools;
using HKDXX6_HFT_2023241.Models.NonCrudModels;
using Humanizer;

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

            var precinctSubMenu = new ConsoleMenu(args, 1)
                .Add("List", () => List(nameof(Precinct)))
                .Add("Get details", () => GetDetails(nameof(Precinct)))
                .Add("Create", () => Create(nameof(Precinct)))
                .Add("Update", () => Update(nameof(Precinct)))
                .Add("Delete", () => Delete(nameof(Precinct)))
                .Add("Back to main menu", ConsoleMenu.Close);

            var officerSubMenu = new ConsoleMenu(args, 1)
                .Add("List", () => List(nameof(Officer)))
                .Add("Get details", () => GetDetails(nameof(Officer)))
                .Add("Create", () => Create(nameof(Officer)))
                .Add("Update", () => Update(nameof(Officer)))
                .Add("Delete", () => Delete(nameof(Officer)))
                .Add("Back to main menu", ConsoleMenu.Close);

            var caseSubMenu = new ConsoleMenu(args, 1)
                .Add("List", () => List(nameof(Case)))
                .Add("Get details", () => GetDetails(nameof(Case)))
                .Add("Create", () => Create(nameof(Case)))
                .Add("Assign case automatically", () => AutoAssignCase())
                .Add("Update", () => Update(nameof(Case)))
                .Add("Delete", () => Delete(nameof(Case)))
                .Add("Number of open and closed cases per officers", () => CasesPerOfficerStatistics())
                .Add("Number of open and closed cases per precincts", () => CasesPerPrecinctStatistics())
                .Add("Average case open time per officers", () => CaseAverageOpenTimePerOfficer())
                .Add("Average case open time per precincts", () => CaseAverageOpenTimePerPrecinct())
                .Add("All cases of a precinct", () => CasesOfPrecinct())
                .Add("Back to main menu", ConsoleMenu.Close);

            var mainMenu = new ConsoleMenu(args, 0)
                .Add("Precincts", () => precinctSubMenu.Show())
                .Add("Officers", () => officerSubMenu.Show())
                .Add("Cases", () => caseSubMenu.Show())
                .Add("Quit app", ConsoleMenu.Close);

            mainMenu.Show();
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
            Console.Clear();
            Console.WriteLine($"Listing all {TypeName.ToLower()}s");
            if (TypeName == nameof(Case))
            {
                List<Case> cases = Rest.Get<Case>("Case");
                var table = new ConsoleTable("ID", "Name", "Officer on case");
                foreach (var item in cases)
                {
                    table.AddRow(item.ID, item.Name, $"{item.OfficerOnCase.Rank} {item.OfficerOnCase.FirstName} {item.OfficerOnCase.LastName} ({item.OfficerOnCaseID})");
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
                Console.Clear();
                Console.WriteLine($"Getting details of a(n) {TypeName.ToLower()}");
                Console.Write("Please provide the ID to look up or put an asterisk (*) to go back to the menu: ");
                input = Console.ReadLine();

                if (input == "*")
                {
                    return Status.OK;
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
            }
            else
            {
                inputInt = id.Value;
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
                    table.AddRow("Open time", c.OpenTimeSpan.Value.Humanize(2));
                }
                if (c.OfficerOnCaseID != null)
                {                    
                    table.AddRow("Officer on case", $"{c.OfficerOnCase.Rank} {c.OfficerOnCase.FirstName} {c.OfficerOnCase.LastName} ({c.OfficerOnCaseID})");
                    table.AddRow("Precinct", c.OfficerOnCase.PrecinctID);
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
                    table.AddRow("Commanding officer", $"{o.DirectCO.Rank} {o.DirectCO.FirstName} {o.DirectCO.LastName} ({o.DirectCO_BadgeNo})");
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
                    WriteErrorMsg(ex.Message);
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
                        table.AddRow("Ranking officer", $"{ro.Rank} {ro.FirstName} {ro.LastName} ({ro.BadgeNo})");
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
            Console.Clear();
            Console.WriteLine($"Updating a(n) {TypeName.ToLower()}");
            Console.Write("Please provide the ID to update or put an asterisk (*) to go back to the menu: ");
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
            Console.Write("Are you sure you want to update this item? (y/n): ");
            string inputYN = Console.ReadLine();
            while (inputYN.ToUpper() != "Y" && inputYN.ToUpper() != "N")
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
                //No need for try-catch as getdetails was called: getting the old item is handled there aswell.
                Case updated = Rest.GetSingle<Case>($"{TypeName}/{inputInt}");

                Console.Write("Name: ");
                string nameInput = Console.ReadLine();
                while ((nameInput.Length < 10 || nameInput.Length > 240) && nameInput != "*")
                {
                    Console.Write("Invalid input for name, please try again: ");
                    nameInput = Console.ReadLine();
                }
                if (nameInput != "*") updated.Name = nameInput;

                Console.Write("Description: ");
                string descInput = Console.ReadLine();
                while (descInput.Length<15 && descInput != "*")
                {
                    Console.Write("Invalid input for description, please try again: ");
                    descInput = Console.ReadLine();
                }
                if (descInput != "*") updated.Description = descInput;

                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Recorded at: ");
                string openedDtInputString = Console.ReadLine();
                DateTime openedDt;
                while (!DateTime.TryParse(openedDtInputString, out openedDt)
                    && openedDtInputString != "*"
                    && openedDtInputString.ToLower() != "now"
                    && openedDtInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for recorded at, please try again: ");
                    openedDtInputString = Console.ReadLine();
                }
                if (openedDtInputString != "*")
                {
                    if (openedDtInputString.ToLower() == "now")
                    {
                        updated.OpenedAt = DateTime.Now;
                    }
                    else if(openedDtInputString.ToLower() == "today")
                    {
                        updated.OpenedAt = DateTime.Today;
                    }
                    else
                    {
                        updated.OpenedAt = openedDt;
                    }
                }

                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Closed at (leave empty to leave the case open): ");
                string closedDtInputString = Console.ReadLine();
                DateTime closedDt;

                while (!DateTime.TryParse(closedDtInputString, out closedDt)
                    && closedDtInputString != string.Empty
                    && closedDtInputString != "*"
                    && closedDtInputString.ToLower() != "now"
                    && closedDtInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for closed at, please try again: ");
                    closedDtInputString = Console.ReadLine();
                }
                if (closedDtInputString != "*")
                {
                    if (closedDtInputString == string.Empty)
                    {
                        updated.ClosedAt = null;
                    }
                    else if (openedDtInputString.ToLower() == "now")
                    {
                        updated.ClosedAt = DateTime.Now;
                    }
                    else if (openedDtInputString.ToLower() == "today")
                    {
                        updated.ClosedAt = DateTime.Today;
                    }
                    else
                    {
                        updated.ClosedAt = closedDt;
                    }
                }

                Console.Write("Officer on case badgeNo. (leave empty for unassigned): ");
                string officerIDInputString = Console.ReadLine();
                int officerID;
                while (!int.TryParse(officerIDInputString, out officerID)
                    && officerIDInputString != "*" && officerIDInputString != string.Empty)
                {
                    Console.Write("Invalid input for officer badgeNo., please try again: ");
                    officerIDInputString = Console.ReadLine();
                }
                if (officerIDInputString != "*")
                {
                    if (officerIDInputString == string.Empty)
                    {
                        updated.OfficerOnCaseID = null;
                    }
                    else
                    {
                        updated.OfficerOnCaseID = officerID;
                    }
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
                //No need for try-catch as getdetails was called: getting the old item is handled there aswell.
                Officer updated = Rest.GetSingle<Officer>($"{TypeName}/{inputInt}");

                Console.Write("First name: ");
                string fnameInput = Console.ReadLine();
                while ((fnameInput.Length < 2 || fnameInput.Length > 100) && fnameInput != "*")
                {
                    Console.Write("Invalid input for first name, please try again: ");
                    fnameInput = Console.ReadLine();
                }
                if (fnameInput != "*") updated.FirstName = fnameInput;

                Console.Write("Last name: ");
                string lnameInput = Console.ReadLine();
                while ((lnameInput.Length < 2 || lnameInput.Length > 100) && lnameInput != "*")
                {
                    Console.Write("Invalid input for last name, please try again: ");
                    lnameInput = Console.ReadLine();
                }
                if (lnameInput != "*") updated.LastName = lnameInput;

                Console.Write("Rank: ");
                string rankInputString = Console.ReadLine();
                object rank;
                while (!Enum.TryParse(typeof(Ranks), rankInputString, out rank) && rankInputString != "*")
                {
                    Console.Write("Invalid input for rank, please try again: ");
                    rankInputString = Console.ReadLine();
                }
                if (rankInputString != "*") updated.Rank = (Ranks)rank;

                if (updated.Rank != Ranks.Captain)
                {
                    Console.Write("Direct CO badgeNo.: ");
                    string directCoIdInputString = Console.ReadLine();

                    int directCoId;

                    while (!int.TryParse(directCoIdInputString, out directCoId) && directCoIdInputString != "*")
                    {
                        Console.Write("Invalid input for direct CO badgeNo., please try again: ");
                        directCoIdInputString = Console.ReadLine();
                    }
                    if (directCoIdInputString != "*") updated.DirectCO_BadgeNo = directCoId;
                }
                else
                {
                    updated.DirectCO_BadgeNo = null;
                }
                

                Console.Write("Precinct ID: ");
                string precinctIdInputString = Console.ReadLine();
                int precinctId;
                while (!int.TryParse(precinctIdInputString, out precinctId) && precinctIdInputString != "*")
                {

                    Console.Write("Invalid input for precinct ID, please try again: ");
                    precinctIdInputString = Console.ReadLine();                    
                }
                if (precinctIdInputString != "*") updated.PrecinctID = precinctId;

                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Hired at: ");
                string hireDateInputString = Console.ReadLine();
                DateTime hireDate;

                while (!DateTime.TryParse(hireDateInputString, out hireDate)
                    && hireDateInputString != "*"
                    && hireDateInputString.ToLower() != "now"
                    && hireDateInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for hire date, please try again: ");
                    hireDateInputString = Console.ReadLine();
                }
                if (hireDateInputString != "*")
                {
                    if (hireDateInputString.ToLower() == "now")
                    {
                        updated.HireDate = DateTime.Now;
                    }
                    if (hireDateInputString.ToLower() == "today")
                    {
                        updated.HireDate = DateTime.Today;
                    }
                    else
                    {
                        updated.HireDate = hireDate;
                    }
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
                //No need for try-catch as getdetails was called: getting the old item is handled there aswell.
                Precinct updated = Rest.GetSingle<Precinct>($"{TypeName}/{inputInt}");

                Console.Write("Address: ");
                string addressInput = Console.ReadLine();
                while ((addressInput.Length < 10 || addressInput.Length > 100) && addressInput != "*")
                {
                    Console.Write("Invalid input for address, please try again: ");
                    addressInput = Console.ReadLine();
                }
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
            Console.Clear();
            Console.WriteLine($"Creating a(n) {TypeName.ToLower()}");

            Console.WriteLine("Enter new data below.");
            if (TypeName == nameof(Case))
            {

                Case added = new Case();

                Console.Write("Case name: ");
                string nameInput = Console.ReadLine();
                while (nameInput.Length < 10 || nameInput.Length > 240)
                {
                    Console.Write("Invalid input for name, please try again: ");
                    nameInput = Console.ReadLine();
                }
                added.Name = nameInput;

                Console.Write("Case description: ");
                string descInput = Console.ReadLine();
                while (descInput.Length < 15)
                {
                    Console.Write("Invalid input for description, please try again: ");
                    descInput = Console.ReadLine();
                }
                added.Description = descInput;

                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Case opened at: ");
                string openedAtInputString = Console.ReadLine();
                DateTime parsedOpenDt;
                while (!DateTime.TryParse(openedAtInputString, out parsedOpenDt)
                    && openedAtInputString.ToLower() != "now"
                    && openedAtInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for opened at, please try again: ");
                    openedAtInputString = Console.ReadLine();
                }
                if (openedAtInputString != string.Empty)
                {
                    if (openedAtInputString.ToLower() == "now")
                    {
                        added.ClosedAt = DateTime.Now;
                    }
                    else if (openedAtInputString.ToLower() == "today")
                    {
                        added.ClosedAt = DateTime.Today;
                    }
                    else
                    {
                        added.ClosedAt = parsedOpenDt;
                    }
                }
                else
                {
                    added.ClosedAt = null;
                }

                Console.WriteLine();
                Console.WriteLine("The following fields are not mandatory, you may leave them empty.");
                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Case closed at: ");
                string closedAtInputString = Console.ReadLine();
                DateTime parsedClosedDt;
                while (!DateTime.TryParse(closedAtInputString, out parsedClosedDt) 
                    && closedAtInputString != string.Empty
                    && closedAtInputString.ToLower() != "now"
                    && closedAtInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for closed at, please try again: ");
                    closedAtInputString = Console.ReadLine();
                }
                if (closedAtInputString != string.Empty)
                {
                    if (closedAtInputString.ToLower() == "now")
                    {
                        added.ClosedAt = DateTime.Now;
                    }
                    else if (closedAtInputString.ToLower() == "today")
                    {
                        added.ClosedAt = DateTime.Today;
                    }
                    else
                    {
                        added.ClosedAt = parsedClosedDt;
                    }
                }
                else
                {
                    added.ClosedAt = null;
                }


                Console.Write("Officer on case badgeNo.: ");
                string officerOnCaseIdInputString = Console.ReadLine();
                int parsedOfficerID;
                while (!int.TryParse(officerOnCaseIdInputString, out parsedOfficerID) && officerOnCaseIdInputString != string.Empty)
                {
                    Console.Write("Invalid input for officer on case badgeNo., please try again: ");
                    officerOnCaseIdInputString = Console.ReadLine();
                }
                added.OfficerOnCaseID = (officerOnCaseIdInputString == string.Empty ? null : parsedOfficerID);

                try
                {
                    Rest.Post(added, TypeName);
                    Case addedCase = Rest.Get<Case>("Case").Find(t => t.Equals(added));
                    Console.WriteLine($"Case successfully added. Case ID: {addedCase.ID}");
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
                while (fnameInput.Length < 2 || fnameInput.Length >100)
                {
                    Console.Write("Invalid input for first name, please try again: ");
                    fnameInput = Console.ReadLine();
                }
                added.FirstName = fnameInput;

                Console.Write("Last name: ");
                string lnameInput = Console.ReadLine();
                while (lnameInput.Length < 2 || lnameInput.Length >100)
                {
                    Console.Write("Invalid input for last name, please try again: ");
                    lnameInput = Console.ReadLine();
                }
                added.LastName = lnameInput;

                Console.WriteLine("Rank: ");
                string rankInputString = Console.ReadLine();

                object parsedRank;

                while (!Enum.TryParse(typeof(Ranks), rankInputString, out parsedRank))
                {
                    Console.Write("Invalid input for rank, please try again: ");
                    rankInputString = Console.ReadLine();
                }

                added.Rank = (Ranks)parsedRank;

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


                Console.WriteLine("You may write today to indicate today at 00:00, or write now, to put in the current time.");
                Console.Write("Hired at: ");
                string hireDateInputString = Console.ReadLine();

                DateTime hireDate;

                while (!DateTime.TryParse(hireDateInputString, out hireDate)
                    && hireDateInputString.ToLower() != "now"
                    && hireDateInputString.ToLower() != "today")
                {
                    Console.Write("Invalid input for hire date, please try again: ");
                    hireDateInputString = Console.ReadLine();
                }
                if (hireDateInputString.ToLower() == "now")
                {
                    added.HireDate = DateTime.Now;
                }
                else if (hireDateInputString.ToLower() == "today")
                {
                    added.HireDate = DateTime.Today;
                }
                else
                {
                    added.HireDate = hireDate;
                }
                

                try
                {
                    Rest.Post(added, TypeName);
                    Officer addedOfficer = Rest.Get<Officer>("Officer").Find(t => t.Equals(added));
                    Console.WriteLine($"Officer successfully added. BadgeNo.: {addedOfficer.BadgeNo}");
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
                Precinct added = new Precinct();
                Console.Write("Precinct ID: ");
                string precinctIdInputString = Console.ReadLine();

                int precinctId;

                while (!int.TryParse(precinctIdInputString, out precinctId) && (precinctId < 1 || precinctId > 139))
                {
                    Console.Write("Invalid input for precinct ID, please try again: ");
                    precinctIdInputString = Console.ReadLine();
                }
                added.ID = precinctId;


                Console.Write("Address: ");
                string addressInput = Console.ReadLine();
                while (addressInput.Length < 10 || addressInput.Length > 100)
                {
                    Console.Write("Invalid input for address, please try again: ");
                    addressInput = Console.ReadLine();
                }
                added.Address = addressInput;

                try
                {
                    Rest.Post(added, TypeName);
                    Console.WriteLine($"Precinct successfully added. ID: {precinctId}");
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
            Console.Clear();
            Console.WriteLine($"Deleting a(n) {TypeName.ToLower()}");
            Console.Write("Please provide the ID to delete or put an asterisk (*) to go back to the menu: ");
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
            while (inputYN.ToUpper() != "Y" && inputYN.ToUpper() != "N")
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

            try
            {
                Rest.Delete(inputInt, TypeName);
                Console.WriteLine($"{TypeName}[{inputInt}] deleted sucsessfully.");
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

        static void CasesPerOfficerStatistics()
        {
            Console.Clear();
            Console.WriteLine("Listing number of open and closed cases per officer");
            var table = new ConsoleTable("Officer", "Precinct", "No. of open cases", "No. of closed cases");
            List<CasesPerOfficerStatistic> stat = Rest.Get<CasesPerOfficerStatistic>("Statistics/casesPerOfficerStatistics");
            foreach (var statItem in stat)
            {
                table.AddRow($"{statItem.Officer.Rank} {statItem.Officer.FirstName} {statItem.Officer.LastName} ({statItem.Officer.BadgeNo})",
                    statItem.Officer.PrecinctID,
                    statItem.OpenCases,
                    statItem.ClosedCases);
            }
            table.Write(Format.Minimal);
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();

        }

        static void CasesPerPrecinctStatistics()
        {
            Console.Clear();
            Console.WriteLine("Listing number of open and closed cases per precinct");
            var table = new ConsoleTable("Precinct", "No. of open cases", "No. of closed cases");
            List<CasesPerPrecinctStatistic> stat = Rest.Get<CasesPerPrecinctStatistic>("Statistics/casesPerPrecinctStatistics");
            foreach (var statItem in stat)
            {
                table.AddRow(statItem.Precinct.ID, statItem.OpenCases, statItem.ClosedCases);
            }
            table.Write(Format.Minimal);
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void AutoAssignCase()
        {
            Console.Clear();
            Console.WriteLine("AutoAssigning case");
            Console.Write("Please provide the Case ID to autoassign or put an asterisk (*) to go back to the menu: ");
            string caseInput = Console.ReadLine();
            if (caseInput == "*") return;

            int caseInputInt;

            while (!int.TryParse(caseInput, out caseInputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                caseInput = Console.ReadLine();
                if (caseInput == "*") return;
            }

            Console.Write("Please provide the Precinct ID to autoassign the case to or put an asterisk (*) to go back to the menu: ");
            string precinctInput = Console.ReadLine();
            if (precinctInput == "*") return;

            int precinctInputInt;

            while (!int.TryParse(precinctInput, out precinctInputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                precinctInput = Console.ReadLine();
                if (precinctInput == "*") return;
            }

            var assignData = new AutoAssignData(caseInputInt, precinctInputInt);
            try
            {
                Rest.Post(assignData, "Case/AutoAssign");
                Case c = Rest.GetSingle<Case>($"Case/{caseInputInt}");
                Console.Write("Case sucsessfully assigned to ");
                Console.WriteLine($"{c.OfficerOnCase.Rank} {c.OfficerOnCase.FirstName} {c.OfficerOnCase.LastName}.");
                Console.WriteLine("Press any key to go back");
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

        static void CaseAverageOpenTimePerOfficer()
        {
            Console.Clear();
            Console.WriteLine("Listing average case open time per officer");
            var table = new ConsoleTable("Officer", "Precinct", "Average case open time");
            List<OfficerCaseAverageOpenTimeItem> stat = Rest.Get<OfficerCaseAverageOpenTimeItem>("Statistics/CaseAverageOpenTimePerOfficer");
            foreach (var statItem in stat)
            {
                table.AddRow($"{statItem.officer.Rank} {statItem.officer.FirstName} {statItem.officer.LastName} ({statItem.officer.BadgeNo})",
                    statItem.officer.PrecinctID, statItem.openTimeSpan.Humanize(2));
            }
            table.Write(Format.Minimal);
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();

        }

        static void CaseAverageOpenTimePerPrecinct()
        {
            Console.Clear();
            Console.WriteLine("Listing average case open time per precinct");
            var table = new ConsoleTable("Precinct", "Average case open time");
            List<PrecinctCaseAverageOpenTimeItem> stat = Rest.Get<PrecinctCaseAverageOpenTimeItem>("Statistics/CaseAverageOpenTimePerPrecinct");
            foreach (var statItem in stat)
            {
                table.AddRow(statItem.precinct.ID,
                    statItem.openTimeSpan.Humanize(2));
            }
            table.Write(Format.Minimal);
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
        }

        static void CasesOfPrecinct()
        {
            Console.Clear();
            Console.WriteLine("Listing all cases of precinct");
            Console.Write("Please provide the ID to look up or put an asterisk (*) to go back to the menu: ");
            string input = Console.ReadLine();

            if (input == "*") return;

            int inputInt;

            while (!int.TryParse(input, out inputInt))
            {
                Console.Write("Invalid input. Please try again: ");
                input = Console.ReadLine();
                if (input == "*") return;
            }

            List<Case> cases = Rest.Get<Case>($"Statistics/CasesOfPrecinct/{inputInt}");

            var table = new ConsoleTable("ID", "Name", "Officer on case");
            foreach (var item in cases)
            {
                table.AddRow(item.ID, item.Name, $"{item.OfficerOnCase.Rank} {item.OfficerOnCase.FirstName} {item.OfficerOnCase.LastName} ({item.OfficerOnCaseID})");
            }
            table.Write(Format.Minimal);
            Console.WriteLine("Press any key to go back.");
            Console.ReadKey();
            return;
        }
    }
}

using System;
using Malshinon.Classes;
using Malshinon.DAL;
using Malshinon.factory;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Malshinon system!");

            while (true)
            {

                ShowMenu();
                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "1":
                        AddNewPerson();
                        break;
                    case "2":
                        InsertNewReport();
                        break;
                    case "3":
                        ImportCsv();
                        break;
                    case "4":
                        ShowAllLogs();
                        break;
                    case "5":
                        ShowAllPeople();
                        break;
                    case "6":
                        ShowAllReports();
                        break;
                    case "7":
                        ShowAllPeopleInformation();
                        break;
                    case "0":
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\t1 - Add new person");
            Console.WriteLine("\t2 - Insert new report");
            Console.WriteLine("\t3 - Import CSV");
            Console.WriteLine("\t4 - Show all logs");
            Console.WriteLine("\t5 - Show all people");
            Console.WriteLine("\t6 - Show all reports");
            Console.WriteLine("\t7 - Show all people information");
            Console.WriteLine("\t0 - Exit");
            Console.Write("\tChoose an option: ");
        }

        static void AddNewPerson()
        {
            Console.WriteLine("Enter a full name for the new person: ");
            string fullName = Console.ReadLine().Trim();
            Console.WriteLine("Enter a secret code for the new person (must be unique): ");
            string secretCode = Console.ReadLine().Trim().ToLower();
            PeopleFactory.AddPeople(secretCode, fullName);
        }

        static void InsertNewReport()
        {
            Console.WriteLine("Please enter the secret code of the reporter: ");
            string reporterSecretCode = Console.ReadLine().Trim().ToLower();
            int? reporterId = GetOrCreatePersonId(reporterSecretCode);
            if (reporterId is null) return;

            Console.WriteLine("Please enter the secret code of the target: ");
            string targetSecretCode = Console.ReadLine().Trim().ToLower();
            int? targetId = GetOrCreatePersonId(targetSecretCode);
            if (targetId is null) return;

            Console.WriteLine("Enter the content of the report:");
            string text = Console.ReadLine();

            Console.WriteLine("Do you want to enter the time of the report? (Y)");
            string choice = Console.ReadLine().Trim().ToLower();

            if (choice == "y")
            {
                sendingWithAddTime(reporterId.Value, targetId.Value, text);
            }
            else
            {
                ReportFacrory.AddReoport(reporterId.Value, targetId.Value, text);
                Console.WriteLine("Report without time was successfully added.");
            }
        }

        static void ImportCsv()
        {
            CSV.ImportCsv();
        }

        static void ShowAllLogs()
        {
            Console.WriteLine(Logger.Read());
        }

        static void ShowAllPeople()
        {
            PeopleDAL.ShowAllPeoples();
        }

        static void ShowAllReports()
        {
            ReportDAL.ShowAllReports();
        }

        static void ShowAllPeopleInformation()
        {
            PeopleDAL.ShowAllPeoplesInformation();
        }
        static void sendingWithAddTime(int reporterId, int targetId, string text)
        {
            DateTime reportTime;
            bool isValidTime = false;

            do
            {
                Console.WriteLine("Enter the time in format 'YEAR/MONTH/DAY/HOUR/MINUTE/SECOND':");
                string timeInput = Console.ReadLine().Trim();
                string[] parts = timeInput.Split('/');

                if (parts.Length == 6 &&
                    int.TryParse(parts[0], out int year) &&
                    int.TryParse(parts[1], out int month) &&
                    int.TryParse(parts[2], out int day) &&
                    int.TryParse(parts[3], out int hour) &&
                    int.TryParse(parts[4], out int minute) &&
                    int.TryParse(parts[5], out int second))
                {
                    try
                    {
                        reportTime = new DateTime(year, month, day, hour, minute, second);
                        isValidTime = true;

                        ReportFacrory.AddReoport(reporterId, targetId, text, reportTime);
                        Console.WriteLine("Report with time was successfully added.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error creating DateTime: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid time format. Please try again.");
                }

            } while (!isValidTime);
        }

        static int? GetOrCreatePersonId(string secretCode)
        {
            int? personId = PeopleDAL.GetIdBySecretCode(secretCode);
            if (personId is null)
            {
                Console.WriteLine("Secret code not found.");
                Console.WriteLine("Do you want to create a new person with this secret code? (Y)");
                string createChoice = Console.ReadLine().Trim().ToLower();

                if (createChoice == "y")
                {
                    PeopleFactory.AddPeople(secretCode);
                    personId = PeopleDAL.GetIdBySecretCode(secretCode);
                }
            }
            return personId;
        }
    }
}

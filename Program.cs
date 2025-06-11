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
            Console.WriteLine("Please enter the secret code of the reporter: ");
            string reporterSecretCode = Console.ReadLine().Trim().ToLower();
            int? reporterId = PeopleDAL.GetIdBySecretCode(reporterSecretCode);
            if (reporterId is null)
            {
                Console.WriteLine("Invalid reporter secret code.");
                Console.WriteLine("Do you want to create a new person with this secret code? (Y)");
                string ifCreateNewPeople = Console.ReadLine().Trim().ToLower();
                if (ifCreateNewPeople == "y")
                {
                    PeopleFactory.AddPeople(reporterSecretCode);
                    reporterId = PeopleDAL.GetIdBySecretCode(reporterSecretCode);
                }
                else
                {
                    return;
                }
            }

            Console.WriteLine("Please enter the secret code of the target: ");
            string targetSecretCode = Console.ReadLine().Trim().ToLower();
            int? targetId = PeopleDAL.GetIdBySecretCode(targetSecretCode);
            if (targetId is null)
            {
                Console.WriteLine("Invalid target secret code.");
                Console.WriteLine("Do you want to create a new person with this secret code? (Y)");
                string ifCreateNewPeople = Console.ReadLine().Trim().ToLower();
                if (ifCreateNewPeople == "y")
                {
                    PeopleFactory.AddPeople(targetSecretCode);
                    targetId = PeopleDAL.GetIdBySecretCode(targetSecretCode); // ✅ תיקון כאן
                }
                else
                {
                    return;
                }
            }

            Console.WriteLine("Enter the content of the report:");
            string text = Console.ReadLine();

            Console.WriteLine("Do you want to enter the time of the report? (Y/N)");
            string userChoice = Console.ReadLine().Trim().ToLower();

            if (userChoice == "y")
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

                            ReportFacrory.AddReoport(reporterId.Value, targetId.Value, text, reportTime);
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
            else
            {
                ReportFacrory.AddReoport(reporterId.Value, targetId.Value, text);
            }
        }
    }
}

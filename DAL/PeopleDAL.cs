using System;
using System.Collections.Generic;
using System.Linq;
using Malshinon.Classes;

namespace Malshinon.DAL
{
    static class PeopleDAL
    {
        public static List<Dictionary<string, object>> GetAllPeoples()
        {
            string SQLQuery = $"SELECT * FROM peoples";
            var result = DBConnection.Execute(SQLQuery);
            DBConnection.PrintResult(result);
            return result;
        }

        public static object InsertNewPeople(People newPeople)
        {
            string SQLQuery = ($"INSERT INTO peoples (FullName, SecretCode, IsAgent, IsDangerous)" +
                $"VALUES('{newPeople.GetFullName()}', '{newPeople.GetSecretCode()}', '{newPeople.IsPeopleAgent()}', '{newPeople.IsPeopleDangerous()}');");
            var result = DBConnection.Execute(SQLQuery);
            DBConnection.PrintResult(result);
            return result;
        }

        private static bool IsUp20Targets(int targetID)
        {
            List<Dictionary<string, object>> allReports = ReportDAL.GetAllReports();
            int numberOfReportes = 0;

            foreach (Dictionary<string, object> report in allReports)
            {
                if (report.ContainsKey("targetId") && Convert.ToInt32(report["targetId"]) == targetID)
                {
                    numberOfReportes++;
                }
            }
            return numberOfReportes > 20 ;
        }

        private static bool Is3MessagesIn15Minutes()
        {
            List<Dictionary<string, object>> allReports = ReportDAL.GetAllReports();

            List<DateTime> times = allReports
                .Select(r => Convert.ToDateTime(r["submittedAt"]))
                .OrderBy(t => t)
                .ToList();

            for (int i = 0; i <= times.Count - 3; i++)
            {
                DateTime first = times[i];
                DateTime third = times[i + 2];

                TimeSpan span = third - first;

                if (span.TotalMinutes <= 15)
                {
                    return true;
                }
            }

            return false;
        }

    }
}

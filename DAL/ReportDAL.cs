using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Classes;

namespace Malshinon.DAL
{
    static class ReportDAL
    {
        static public List<Dictionary<string, object>> GetAllReports()
        {
            string SQLQuery = $"SELECT * FROM reports";
            var result = DBConnection.Execute(SQLQuery);
            DBConnection.PrintResult(result);
            return result;
        }
        public static object InsertReport(Report newReport)
        {
            bool hasTime = newReport.GetReportTime() != null;
            int reporterId = newReport.GetReporterId();
            int targetId = newReport.GetTergetId();

            List<Dictionary<string, object>> allPeoples = PeopleDAL.GetAllPeoples();

            bool reporterExists = allPeoples.Any(p => Convert.ToInt32(p["id"]) == reporterId);
            if (!reporterExists)
            {
                Console.WriteLine($"Reporter ID {reporterId} does not exist.");
                return null;
            }
            bool targetExists = allPeoples.Any(p => Convert.ToInt32(p["id"]) == targetId);
            if (!targetExists)
            {
                Console.WriteLine($"Target ID {targetId} does not exist.");
                return null;
            }

            string columns = "reporterId, targetId, reportText";
            string values = $"'{reporterId}', '{targetId}', '{newReport.GetText()}'";

            if (hasTime)
            {
                columns += ", submittedAt";
                values += $", '{newReport.GetReportTime():yyyy-MM-dd HH:mm:ss}'";
            }

            string SQLQuery = $"INSERT INTO reports ({columns}) VALUES ({values})";
            var result = DBConnection.Execute(SQLQuery);

            if (PeopleDAL.IsDangerous(targetId))
            {
                Console.WriteLine($"People with id {targetId} is dangerous!");
                PeopleDAL.MakePeopleDangerous(targetId);
            }

            if (PeopleDAL.IsAgent(reporterId))
            {
                Console.WriteLine($"People with id {reporterId} is agent!");
                PeopleDAL.MakePeopleAgent(reporterId);
            }
            Console.WriteLine($"Report {newReport.GetReporterId()} -> {newReport.GetTergetId()} added succusfully.");
            return result;
        }

    }
}

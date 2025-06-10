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

            string columns = "reporterId, targetId, reportText";
            string values = $"'{newReport.GetReporterId()}', '{newReport.GetTergetId()}', '{newReport.GetText()}'";

            if (hasTime)
            {
                columns += ", submittedAt";
                values += $", '{newReport.GetReportTime():yyyy-MM-dd HH:mm:ss}'";
            }
            string SQLQuery = $"INSERT INTO reports ({columns}) VALUES ({values})";
            var result = DBConnection.Execute(SQLQuery);

            if (PeopleDAL.IsDangerous(newReport.GetTergetId()))
            {
                Console.WriteLine($"People with id {newReport.GetTergetId()} is dangerous!");
                PeopleDAL.MakePeopleDangerous(newReport.GetTergetId());
            }
            if (PeopleDAL.IsAgent(newReport.GetReporterId()))
            {
                Console.WriteLine($"People with id {newReport.GetReporterId()} is agent!");
                PeopleDAL.MakePeopleAgent(newReport.GetReporterId());
            }
            return result;
        }
    }
}

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
        static public object InsertReport(Report newReport)
        {
            string SQLQuery = $"INSERT INTO reports (reporterId, targetId, reportText, submittedAt)" +
                $"VALUES('{newReport.GetReporterId()}', '{newReport.GetTergetId()}', '{newReport.GetText()}', '{newReport.GetReportTime()}')";
            var result = DBConnection.Execute(SQLQuery);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Malshinon.Classes;
using MySqlX.XDevAPI.Common;

namespace Malshinon.DAL
{
    static class PeopleDAL
    {
        public static void ShowAllPeoples()
        {
            string SQLQuery = $"SELECT * FROM peoples";
            var result = DBConnection.Execute(SQLQuery);
            DBConnection.PrintResult(result);
        }

        public static List<Dictionary<string, object>> GetAllPeoples()
        {
            string SQLQuery = $"SELECT * FROM peoples";
            var result = DBConnection.Execute(SQLQuery);
            return result;
        }

        public static object InsertNewPeople(People newPeople)
        {
            List<Dictionary<string, object>> allPeople = GetAllPeoples();
            foreach (Dictionary<string, object> people in allPeople)
            {
                if (people["secretCode"]?.ToString() == newPeople.GetSecretCode())
                {
                    Console.WriteLine("The secret code allreaey in used, please enter new secret code.");
                    return null;
                }
            }
            string SQLQuery = ($"INSERT INTO peoples (FullName, SecretCode, IsAgent, IsDangerous)" +
                $"VALUES('{newPeople.GetFullName()}', '{newPeople.GetSecretCode()}', '{newPeople.IsPeopleAgent()}', '{newPeople.IsPeopleDangerous()}');");
            var result = DBConnection.Execute(SQLQuery);
            Console.WriteLine($"People '{newPeople.GetFullName()}' added succesfully.");
            return result;
        }

        private static bool Is20Targets(int targetID)
        {
            string SQLQuery = $"SELECT COUNT(*) FROM reports WHERE targetId = {targetID}";

            var results = DBConnection.Execute(SQLQuery);

            if (results.Count > 0 && results[0].TryGetValue("COUNT(*)", out object countObj))
            {
                if (countObj != null && int.TryParse(countObj.ToString(), out int count))
                {
                    return count >= 20;
                }
            }
            return false;
        }
        private static bool Is3MessagesIn15Minutes(int targetID)
        {
            string SQLQuery = $@"
                SELECT submittedAt
                FROM reports
                WHERE targetId = {targetID}
                ORDER BY submittedAt ASC";

            var results = DBConnection.Execute(SQLQuery);

            List<DateTime> timestamps = results
                .Select(row => Convert.ToDateTime(row["submittedAt"]))
                .OrderBy(ts => ts)
                .ToList();

            for (int i = 0; i < timestamps.Count - 2; i++)
            {
                DateTime first = timestamps[i];
                DateTime third = timestamps[i + 2];

                if ((third - first).TotalMinutes <= 15)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsDangerous(int targetID) => Is20Targets(targetID) && Is3MessagesIn15Minutes(targetID);
        public static void MakePeopleDangerous(int newDangerousId)
        {
            string SQLQuery = $@"UPDATE peoples
                                SET isDangerous = true
                                WHERE id = {newDangerousId}";
            var result = DBConnection.Execute(SQLQuery);
        }
        //-----------------
        private static bool Is10Reports(int reporterID)
        {
            string SQLQuery = $@"SELECT COUNT(*) AS count FROM reports WHERE reporterId = {reporterID}";

            var results = DBConnection.Execute(SQLQuery);

            if (results.Count > 0 && results[0].TryGetValue("count", out object countObj))
            {
                if (countObj != null && int.TryParse(countObj.ToString(), out int count))
                {
                    return count >= 10;
                }
            }
            return false;
        }
        private static bool IsAvgOfReport100(int reporterID)
        {
            string SQLQuery = $@"
                SELECT AVG(CHAR_LENGTH(reportText)) AS avgLength
                FROM reports
                WHERE reporterId = {reporterID}";

            var results = DBConnection.Execute(SQLQuery);

            if (results.Count > 0 && results[0].TryGetValue("avgLength", out object avgObj))
            {
                if (avgObj != null && double.TryParse(avgObj.ToString(), out double avg))
                {
                    return avg >= 100;
                }
            }
            return false;
        }
        public static bool IsAgent(int reporterID) => Is10Reports(reporterID) && IsAvgOfReport100(reporterID);
        public static void MakePeopleAgent(int newAgentId)
        {
            string SQLQuery = $@"UPDATE peoples
                                SET isAgent = true
                                WHERE id = {newAgentId}";
            var result = DBConnection.Execute(SQLQuery);
        }
    }
}

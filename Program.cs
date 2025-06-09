using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Classes;
using Malshinon.DAL;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<People> peopleList = new List<People>
            //    {
            //        new People("Haim Assraf", "The Shadow"),
            //        new People("Dana Cohen", "The Hacker"),
            //        new People("Lior Levi", "The Whisperer"),
            //        new People("Yossi Mizrahi", "The Falcon"),
            //        new People("Maya Azulay", "The Watcher"),
            //        new People("Ron Ben-David", "The Snake"),
            //        new People("Tamar Golan", "The Coder"),
            //        new People("Eliav Regev", "The Eye"),
            //        new People("Noa Siman-Tov", "The Fox"),
            //        new People("Gadi Peleg", "The Ghost")
            //    };

            //foreach (People p in peopleList)
            //{
            //    PeopleDAL.InsertNewPeople(p);
            //}

            List<Report> reportList = new List<Report>
                {
                    new Report(1, 2, "Suspicious behavior"),
                    new Report(1, 3, "Keeps looking over shoulder"),
                    new Report(4, 2, "Accessed restricted area"),
                    new Report(5, 2, "Asked strange questions"),
                    new Report(6, 7, "Too quiet, never speaks"),
                    new Report(1, 2, "Again acting weird"),
                    new Report(3, 4, "Seems nervous"),
                    new Report(1, 2, "Repeated access attempts"),
                    new Report(8, 9, "Wears sunglasses indoors"),
                    new Report(10, 2, "Too many logins")
                };

            foreach (Report r in reportList)
            {
                ReportDAL.InsertReport(r);
            }

        }
    }
}

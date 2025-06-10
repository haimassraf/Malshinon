using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Classes;

namespace Malshinon.factory
{
    static class PeopleFactory
    {
        public static People AddPeople(string fullName, string secretCode)
        {
            People newPeople = new People(fullName, secretCode);
            return newPeople;
        }
    }
}

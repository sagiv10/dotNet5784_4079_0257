using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class Bl : IBl
    {
        public IEngineer Engineer =>  new EngineerImplementation();
        public ITask Task =>  new TaskImplementation();

        private static DateTime s_Clock = DateTime.Now.Date;
        public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

        public void AddDays(int days)
        {
            throw new NotImplementedException();
        }

        public void AddWeeks(int days)
        {
            throw new NotImplementedException();
        }

        public void AddYear(int days)
        {
            throw new NotImplementedException();
        }

        public void ResetClock()
        {
            throw new NotImplementedException();
        }
    }
}

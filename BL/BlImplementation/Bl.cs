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
        public IEngineer Engineer => new EngineerImplementation(this);
        public ITask Task => new TaskImplementation(this);//we dont use clock there

        public IConfig Config => new ConfigImplementation(this);

        #region
        private static DateTime s_Clock = DateTime.Now.Date;
        public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

        public void AddDay()
        {
            s_Clock = Clock.AddDays(1);
        }

        public void AddWeek()
        {
            s_Clock = Clock.AddDays(7);
        }
        public void AddMonth()
        {
            s_Clock = Clock.AddMonths(1);
        }

        public void AddYear()
        {
            s_Clock = Clock.AddYears(1);
        }

        public void ResetClock()
        {
            s_Clock = DateTime.Now;
        }
        #endregion
    }
}

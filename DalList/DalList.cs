using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalList : IDal
    {
        //SINGLETON CLASS:-----------------------------------
        private static readonly Lazy<IDal> instance =
        new Lazy<IDal>(() => new DalList(), LazyThreadSafetyMode.PublicationOnly);
        //the lazy installation is waiting for the first time we will actually use the instance. 
        //how? the normal singleton class directly create variable when we start the program. but here
        //the property not creating the variable himself.it creats a delegate that
        //will create the variable only in the first time we will actually use the varibale, using
        //the last line [with the (.Value), if you see what .Value do you will see that Lazy class check if the
        //instance exist, if not-so create. and that the first create]
        private DalList() { }
        public static IDal Instance => instance.Value;
        //---------------------------------------------------
        public ITask Task => new TaskImplementation();

        public IEngineer Engineer => new EngineerImplementation();

        public IDependency Dependency => new DependencyImplementation();

        public Iproject Project => new ProjectImplementation();
    }
}

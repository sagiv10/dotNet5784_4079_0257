using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    /// <summary>
    /// interface for the methods that not related to task or engineer directly
    /// </summary>
    public interface IConfig
    {

        /// <summary>
        /// tihs method help us to add dependencies of new task that just been created 
        /// </summary>
        /// <param name="dependentTask"></param>
        /// <param name="dependsOnTask"></param>
        /// <exception cref="BO.BLWrongStageException"></exception>
        /// <exception cref="BO.BLCannotAddCircularDependencyException"></exception>
        /// <exception cref="BO.BLNotFoundException"></exception>
        /// <exception cref="BO.BLAlreadyExistException"></exception>
        public void AddDependency(int dependentTask, int DependsOnTask);

        /// <summary>
        /// tihs method help us to delete dependencies 
        /// </summary>
        /// <param name="dependentTask"></param>
        /// <param name="dependsOnTask"></param>
        /// <exception cref="BO.BLWrongStageException"></exception>
        /// <exception cref="BO.BLNotFoundException"></exception>
        public void DeleteDependency(int dependentTask, int DependsOnTask);


        /// <summary>
        /// this method help us to change the status of the project from Planning to Scheduling. (2 to 3)
        /// </summary>
        /// <param name="StartingDateOfProject"></param>
        /// <exception cref="BLWrongStageException"></exception>
        public void StartSchedule(DateTime StartingDateOfProject);

        /// <summary>
        /// helping method that gets the current progect status
        /// </summary>
        /// <returns>the current status of the project</returns>
        public int getProjectStatus();

        /// <summary>
        /// helping method that get the current progect starting time
        /// </summary>
        public DateTime? getStartingDate();


        /// <summary>
        /// helping method that delete the list of dependenciecies to the marked value. if an error accures then stop the deleting
        /// </summary>
        /// <param name="dependendsOns"> list of depends on ids</param>
        /// <param name="dependentId"> the dependent task</param>
        /// <returns> untill where the method deleted </returns>
        public int DeleteDependencies(List<int> dependendsOns, int dependentId, int endIndex);

        /// <summary>
        /// helping method that adds the list of dependenciecies to the marked value. if an error accures then stop the adding
        /// </summary>
        /// <param name="dependendsOns"> list of depends on ids</param>
        /// <param name="dependentId"> the dependent task</param>
        /// <returns> untill where the method added </returns>
        public int AddDependencies(List<int> dependendsOns, int dependentId, int endIndex);

        /// <summary>
        /// transform string into number, if the string is wrong then it throw exception with yhe wrong filled field
        /// </summary>
        /// <returns> the right int</returns>
        /// <exception cref="BO.BLWrongIdException"></exception>
        public int ParseToInt(string integer, string field);

        /// <summary>
        /// transform string into double number, if the string is wrong then it throw exception with yhe wrong filled field
        /// </summary>
        /// <returns> the right double</returns>
        /// <exception cref="BO.BLWrongIdException"></exception>
        public double ParseToDouble(string number, string field);
        /// <summary>
        /// saves the time of the probjet from the main window to xml 
        /// </summary>
        /// <param name="ProjectCurrentDate"></param>
        public void SaveProjectCurrentDateIntoXml(DateTime ProjectCurrentDate);
        public DateTime? getProjectCurrentDateIntoXml();

        /// <summary>
        ///init the database
        /// </summary>
        public void InitializeData();

        /// <summary>
        /// resets the database
        /// </summary>
        public void ResetData();
    }
}

namespace DalApi;
using DO;
public interface IEngineer : ICrud<Engineer> {
    /// <summary>
    /// return all the deleted tasks
    /// </summary>
    /// <returns> array of the deleted tasks </returns>
    public List<Engineer> getDeleted();
    public void GetEngineerToActive(int id);

}

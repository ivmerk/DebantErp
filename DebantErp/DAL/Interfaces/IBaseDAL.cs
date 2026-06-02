namespace DebantErp.DAL;
public interface IBaseDAL<T>
    where T : class
{
    Task<List<T>> Get();
    Task<T> Get(int id);
    Task<int> Create(T model);
    Task<bool> IsExist(int id);
    Task<int> Update(T model);
    Task<int> Delete(int id);
}

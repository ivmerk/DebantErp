using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IOrderDAL : IBaseDAL<OrderModel> { 
    public Task<bool> IsNumberExist(string number);}
}

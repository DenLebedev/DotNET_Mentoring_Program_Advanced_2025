using CartingService.Entities;

namespace CartingService.DAL.Interfaces
{
    public interface ICartDAO
    {
        Cart Get(int id);
        void Save(Cart cart);
        void Update(Cart cart);
        void Delete(int id);
    }
}

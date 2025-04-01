using CartingService.Entities;

namespace CartingService.BLL.Interfaces
{
    public interface ICartBL
    {
        IEnumerable<Item> GetItems(int id);
        void AddItem(Item item);
        void RemoveItem(Item item);
    }
}

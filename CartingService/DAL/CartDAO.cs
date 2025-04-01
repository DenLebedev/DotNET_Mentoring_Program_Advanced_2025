using CartingService.DAL.Interfaces;
using CartingService.Entities;
using LiteDB;

namespace CartingService.DAL
{
    public class CartDAO : ICartDAO
    {
        private LiteDatabase database;
        private ILiteCollection<Cart> cartSet;

        public CartDAO(LiteDatabase context)
        {
            database = context;
            cartSet = database.GetCollection<Cart>("carts");
        }

        public Cart Get(int id)
        {
            Cart cart = new Cart { Items = new List<Item>() };

            var result = cartSet.Query()
                .Where(x => x.Id == id);

            if (result == null)
            {
                // TODO: Add an Exception to get an empty value from the database
                return cart;
            }
            else
            {
                // TODO: Implement the logic of getting the Item collection for Cart by Cart Id.
                // It is possible only after the mapping is implemented in the Cart class

            }

            return cart;
        }

        public void Save(Cart cart)
        {

        }

        public void Update(Cart cart)
        {

        }

        public void Delete(int id)
        {

        }
    }
}

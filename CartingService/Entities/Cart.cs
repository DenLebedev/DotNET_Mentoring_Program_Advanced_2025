using LiteDB;

namespace CartingService.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public required List<Item> Items { get; set; }

        public Cart()
        {
        }

        // Remove the duplicate constructor
        [BsonCtor]
        public Cart(int id, List<Item> items)
        {
            Id = id;
            Items = items;
        }
    }
}

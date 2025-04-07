using LiteDB;

namespace CartingService.Entities
{
    public class Cart
    {
        public string Key { get; set; }
        public required List<Item> Items { get; set; }

        public Cart()
        {
        }

        // Remove the duplicate constructor
        [BsonCtor]
        public Cart(string key, List<Item> items)
        {
            Key = key;
            Items = items;
        }
    }
}

using LiteDB;

namespace CartingService.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageURL { get; set; }
        public required string ImageAltText { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Item()
        {

        }

        [BsonCtor]
        public Item(int id, string name, string imageURL, string imageAltText, decimal price, int quantity)
        {
            Id = id;
            Name = name;
            ImageURL = imageURL;
            ImageAltText = imageAltText;
            Price = price;
            Quantity = quantity;
        }
    }
}

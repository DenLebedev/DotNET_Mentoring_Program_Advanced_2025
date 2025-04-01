namespace CartingService.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public required List<Item> Items { get; set; }

        public Cart()
        {
        }

        // TODO: implement mapping a reference for working with LiteDB
    }
}

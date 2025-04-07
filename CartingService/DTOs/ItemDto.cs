namespace CartingService.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string ImageAltText { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

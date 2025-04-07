namespace CartingService.DTOs
{
    public class CartDto
    {
        public string Key { get; set; }
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}

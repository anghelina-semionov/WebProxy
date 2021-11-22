namespace Common.Models
{
    public class MenuItem : MongoDocument
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}

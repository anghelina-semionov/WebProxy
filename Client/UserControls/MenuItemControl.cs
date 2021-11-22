using System;

namespace Client.UserControls
{
    public class MenuItemControl
    {
        public Guid Id { get; set; }
        public DateTime LastChangedAt { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }
}

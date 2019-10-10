using System;

namespace gamewebapi
{
    public class NewItem
    {
        
        public int Price { get; set; }
        public int Level {get; set;}
        public DateTime CreationTime { get; set; }
        public ItemType ItemType { get; set; }
    }
}
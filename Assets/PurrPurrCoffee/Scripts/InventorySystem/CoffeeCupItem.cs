using UnityEngine;

namespace PurrPurrCoffee.InventorySystem
{
    using Abstractions;
    
    public class CoffeeCupItem : InventoryItem
    {
        public CoffeeBase Base { get; set; }
        public CoffeeSyrup Syrup { get; set; }
        public bool IsCoveredWithLid { get; set; }
        public bool IsContainsStraw { get; set; }
        public float Fullness { get; set; }
        public GameObject GameObject { get; set; } = null;
    }
    public class CoffeeCupLidItem : InventoryItem { }
    public enum CoffeeBase
    {
        Water,
        Milk
    }
    public enum CoffeeSyrup
    {
        None,
        Caramel,
        Strawberry
    }
}

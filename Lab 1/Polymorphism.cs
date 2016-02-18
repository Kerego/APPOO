using System;
using System.Collections.Generic;

namespace OOP
{
    internal class Animal
    {
        public string Name { get; set; }
        public virtual string Description { get; }
    }
    
    internal class Dog : Animal
    {
        public override string Description => Name + " the Dog";
    }
    
    internal class Cat : Animal
    {
        public override string Description => Name + " the Cat";
    }
    
    class Program
    {
        public static void Main(string[] args)
        {
			var animals = new List<Animal>();
            animals.Add(new Dog() { Name = "Jack" });
            animals.Add(new Cat() { Name = "Barsique"});
            foreach (var item in animals)
            {
                Console.WriteLine(item.Description);
            }
            
            Console.Read();
        }
    }
}
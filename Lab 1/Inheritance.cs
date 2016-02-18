using System;
using System.Collections.Generic;

namespace OOP
{
    internal class Entity
    {
        public Guid Id { get; }
        
        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
    
    internal class NamedEntity : Entity
    {
        public string Name { get; set; }
    }
    
    class Program
    {
        public static void Main(string[] args)
        {
			NamedEntity namedEntity = new NamedEntity();
			Console.WriteLine(namedEntity.Id);
            Console.Read();
        }
    }
}
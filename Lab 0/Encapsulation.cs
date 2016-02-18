using System;
using System.Collections.Generic;

namespace OOP
{
    internal class Encap
    {
        public Guid Id { get; } // getter only property
        public string Name { get; set; } // getter and setter auto-property
        
        private int _data; // accessible only inside this class
        protected int data; // accessible inside this class and derived classes
        public int Data; // accessible from other classes
    }
    internal class DEncap : Encap
    {
        public DEncap()
        {
            Id = 3; //compile, constructor time set
            Name = "test"; // compile
            _data = 5; //erorr
            data = 5; //compile, protected is accessible in child classes
            Data = 5; //compile
        }
    }
    
    class Program
    {
        public static void Main(string[] args)
        {
			Encap encap = new Encap();
            encap.Id = 3; //error getter only
            encap.Name = "test"; // compile
            encap._data = 5; //erorr
            encap.data = 5; //error
            encap.Data = 5; //compile
            
            Console.Read();
        }
    }
}
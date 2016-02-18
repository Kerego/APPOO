/**
 * Animal
 */
abstract class Animal {
    public Name : string;
    constructor(name : string) {
        this.Name = name;
    }
    public abstract Description();
}

class Dog extends Animal{
    public Description() : string
    {
        return this.Name + " the Dog";
    }   
    public Name : string;
}

class Cat extends Animal{
    public Description() : string
    {
        return this.Name + " the Cat";
    }
}

var t:Animal[] = [new Cat("Barsique"), new Dog("Jack")];
t.forEach(element => {
    console.log(element.Description());
});
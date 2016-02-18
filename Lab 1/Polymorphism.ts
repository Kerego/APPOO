/**
 * Animal
 */
abstract class Animal {
    public Name : string;
    constructor(name : string) {
        this.Name = name;
    }
    public abstract Description() : string;
}

class Dog extends Animal{
    public Description() : string
    {
        return this.Name + " the Dog";
    }
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
#Lab 0 Bejenari Marian
###Languages 
1. C#
2. Typescript

##Task 0

###Inheritance

Both languages support inheritance concepts, derived classes inheriting all the properties of the base class. Same time neither of the languages doesn't support multiple inheritance, instead multiple interface implementations pattern is used. In C# a class can be declared as sealed, which will restrict inheritance from that specific class.

####Code examples

#####C# 

```
internal class Entity
{
    public Guid Id { get; }
}

internal class NamedEntity : Entity
{
    public string Name { get; set; }
}
```

#####Typescript

```
class Entity {
    public Data : number;
    constructor(data : number) {
        this.Data = data;
    }
}

class NamedEntity extends Entity {
    public Name : string;
    constructor(name : string, data : number)  {
        super(data);
        this.Name = name;
    }
}

```


###Polymorphism

In C# every type is polymorphic because all types inherit from Object.


In both languages polymorphism is aquired using either an interface or a base class from which other classes inherit. 

No cast is required, conversion from child classes to base is done implicitly in both languages. 

C# being a strictly typed language requires virtual keyword for base class methods/properties and override for derived ones, while typescript does not.

####Code examples

#####C# 

```
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
```
#####Typescript
```
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
```

####Output in both cases
```
Jack the Dog
Barsique the Cat
```

###Encapsulation

C# supports 5 types of access modifiers

1. private
2. protected
3. protected internal
4. internal
5. public

Typescript supports only 3

1. private
2. protected
3. public


Since being a language compiled to JavaScript, private access modifier of Typescript is working only in design time, compiled JavaScript code will have private fields public.
A common practice in C# is encapsulating fields in properties instead of using getter and setter methods.

####Code Examples

#####C# 

```
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
```
Besides simple fields, C# has a feature called auto properties. Which provides basic functionality of encapsulating fields and accessing it via getter and setter.


```
public Int Data { get; set; }
//usage
instance.Data = 5;
Int data = instance.Data;
```
Is the same as
```
private int _data;
public Int GetData() => _data;
public void SetData(int value) {
    _data = value;
}
//usage
instance.SetData(5);
int data = instance.GetData();
```

#####Typescript

```
class Encap {
    public Name : string;
    private _data : number;
    public setData(data : number)
    {
        this._data = data;
    }
    public getData() :number
    {
        return this._data;
    }
}

var encap = new Encap();
encap.Name = "test";
encap._data = 3; // error
encap.setData(3); // works
```


##Task 1

Feature       |          C#       | Typescript
------------ | -------------  | -----------
Encapsulation | <p style="color : green"> 5 access modifiers, nice auto property feature</p> | <p style="color : red"> 3 access modifiers, doesn't work when compiled to JavaScript</p>
Overriding | <p style="color : red"> Only **virtual** methods and properties</p> | <p style="color : green"> Any Field</p>
Base class access | <p style="color : green"> Any field, using **base** keyword</p> | <p style="color : red"> Methods only, using **super** keyword</p>
Polymorphism | <p style="color : green"> Implicit conversion from derived to base class</p> | <p style="color : green"> Implicit conversion from derived to base class</p>
Generics | <p style="color : green"> Full support</p> | <p style="color : green"> Full support</p>

#Conlusion

Both languages support most of oop concepts, and can be considered object oriented languages.
class Entity {
    public Data : number;
    constructor(data : number) {
        this.Data = data;
    }
}

class  NamedEntity extends Entity{
    public Name : string;
    constructor(name : string, data : number)  {
        super(data);
        this.Name = name;
    }
}


var entity = new NamedEntity("World", 42);
alert(entity.Name + ' : ' + entity.Data);
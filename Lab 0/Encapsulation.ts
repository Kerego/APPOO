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
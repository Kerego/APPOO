var Encap = (function () {
    function Encap() {
    }
    Encap.prototype.setData = function (data) {
        this._data = data;
    };
    Encap.prototype.getData = function () {
        return this._data;
    };
    return Encap;
})();
var encap = new Encap();
encap.Name = "test";
encap._data = 3; // error
encap.setData(3); // works

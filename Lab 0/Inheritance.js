var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Entity = (function () {
    function Entity(data) {
        this.Data = data;
    }
    return Entity;
})();
var NamedEntity = (function (_super) {
    __extends(NamedEntity, _super);
    function NamedEntity(name, data) {
        _super.call(this, data);
        this.Name = name;
    }
    return NamedEntity;
})(Entity);
var entity = new NamedEntity("World", 42);
alert(entity.Name + ' : ' + entity.Data);

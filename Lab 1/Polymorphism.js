var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/**
 * Animal
 */
var Animal = (function () {
    function Animal(name) {
        this.Name = name;
    }
    return Animal;
})();
var Dog = (function (_super) {
    __extends(Dog, _super);
    function Dog() {
        _super.apply(this, arguments);
    }
    Dog.prototype.Description = function () {
        return this.Name + " the Dog";
    };
    return Dog;
})(Animal);
var Cat = (function (_super) {
    __extends(Cat, _super);
    function Cat() {
        _super.apply(this, arguments);
    }
    Cat.prototype.Description = function () {
        return this.Name + " the Cat";
    };
    return Cat;
})(Animal);
var t = [new Cat("Barsique"), new Dog("Jack")];
t.forEach(function (element) {
    console.log(element.Description());
});

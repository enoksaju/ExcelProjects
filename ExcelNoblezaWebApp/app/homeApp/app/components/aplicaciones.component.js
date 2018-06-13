"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var flex_layout_1 = require("@angular/flex-layout");
var http_1 = require("@angular/http");
var router_1 = require("@angular/router");
var user_service_1 = require("../../../common/Services/user.service");
var AplicacionesComponent = /** @class */ (function () {
    function AplicacionesComponent(media, http, router, userService) {
        this.media = media;
        this.http = http;
        this.router = router;
        this.userService = userService;
        this.Apps = new Array();
    }
    AplicacionesComponent.prototype.ngOnInit = function () {
        var _this = this;
        // Retorna al inicio si no se ha iniciado sesion
        this.userService.isLoged().then(function (y) {
            if (!y)
                _this.router.navigate(["/main"]);
        }).catch(function () { return _this.router.navigate(["/main"]); });
        this.userService.GetApps()
            .then(function (t) {
            t.forEach(function (v) { return _this.Apps.push(v); });
        })
            .catch();
    };
    AplicacionesComponent.prototype.toLink = function (e) {
        console.log(e);
    };
    AplicacionesComponent = __decorate([
        core_1.Component({
            selector: 'home-aplicaciones',
            templateUrl: './aplicaciones.component.html',
            styleUrls: ['./aplicaciones.component.css']
        }),
        __metadata("design:paramtypes", [flex_layout_1.ObservableMedia,
            http_1.Http,
            router_1.Router,
            user_service_1.UserService])
    ], AplicacionesComponent);
    return AplicacionesComponent;
}());
exports.AplicacionesComponent = AplicacionesComponent;
//# sourceMappingURL=aplicaciones.component.js.map
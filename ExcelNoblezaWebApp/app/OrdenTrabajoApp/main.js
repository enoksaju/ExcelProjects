"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var core_1 = require("@angular/core");
var app_module_1 = require("./app/app.module");
require("../common/prototypes/date.extensions");
require("../common/prototypes/string.extensions");
core_1.enableProdMode();
platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(app_module_1.AppModule).catch(function (err) { return console.log(err); });
//# sourceMappingURL=main.js.map
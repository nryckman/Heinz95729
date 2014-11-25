/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine) {
        "use strict";

        // GET /#/cart
        routes.get(/^\/#\/cart\/?/i, function (context) {  // /books
            viewEngine.setView({
                template: 't-cart',
                message: 'hello word!'
            });
        });    
    }
});

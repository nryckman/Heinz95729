/*global define, JSON*/
define('controllers/checkoutController', {
    init: function ($, routes, viewEngine) {
        "use strict";

        // GET /#/checkout
        routes.get(/^\/#\/checkout\/?/i, function (context) {
            viewEngine.setView({
                template: 't-checkout',
                data: { searchterm: 'blah' }
            });
        });
    }
});

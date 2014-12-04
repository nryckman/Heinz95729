/*global define, JSON*/
define('controllers/checkoutController', {
    init: function ($, routes, viewEngine, CartItems, CartItem) {
        "use strict";

        // checkout view model
        var CheckoutViewModel = function (template, data) {
            var self = {};
        
            self.template = template;
            self.data = data;
            self.after = function () {
            };

            return self;
        };

        // GET /#/checkout
        routes.get(/^\/#\/checkout\/?/i, function (context) {
            $.ajax({
                url: '/api/cart/list',
                method: 'GET'
            }).done(function (data) {
                var results = new CartItems(JSON.parse(data));

                if (results.cartItems().length > 0) {
                    var checkoutViewModel = CheckoutViewModel('t-checkout', results);
                    viewEngine.setView(checkoutViewModel);
                } else {
                    viewEngine.setView({
                        template: 't-checkout',
                        data: { searchterm: 'blah' }
                    });
                }
            });
        });
    }
});

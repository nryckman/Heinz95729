/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, CartItems, CartItem) {
        "use strict";

        var onAddProduct = function (template, data) {
            var self = {};
        
            self.template = template;
            self.data = data;
            self.calculate = function () {
                var unit_costs = [];
                $("input#unit_cost").each(function () {
                    unit_costs.push($(this).val());
                });

                var quantities = [];
                $("select#quantity").each(function () {
                    quantities.push($(this).val());
                });

                var count = unit_costs.length;
                var total = 0;
                for (var i = 0; i < count; i++) {
                    var subtotal = unit_costs[i] * quantities[i];
                    total = total + subtotal;
                    $("span#subtotal").eq(i).text('$' + subtotal.toFixed(2));
                }

                $("span#total").first().text('$' + total.toFixed(2));
            };

            self.after = function () {
                self.calculate();
            };

            return self;
        };

        // POST /#/cart/remove
        routes.del(/^\/#\/cart\/remove\/?/i, function (context) {  // /books
            $.ajax({
                url: '/api/cart/remove',
                method: 'POST'
            }).done(function (data) {
                alert('deleted');
                viewEngine.headerVw.subtractFromCart();
            });
        });

        // POST /#/cart/add
        routes.post(/^\/#\/cart\/add\/?/i, function (context) {  // /books
            var cart_id = $.cookie('cart_id');
            if (cart_id) {
                alert('cart_id: ' + cart_id);
            } else {
                alert('cart_id is undefined');
                $.cookie('cart_id', '555', { expires: 365, path: '/' });
            }

            $.ajax({
                url: '/api/cart/add/' + this.params['uid'],
                method: 'POST'
            }).done(function (data) {
                viewEngine.headerVw.addToCart();
                location.href = '/#/cart';
            });
        });

        // GET /#/cart
        routes.get(/^\/#\/cart\/?/i, function (context) {  // /books
            $.ajax({
                url: '/api/cart/list',
                method: 'GET'
            }).done(function (data) {
                var results = new CartItems(JSON.parse(data));

                if (results.cartItems().length > 0) {
                    var viewModel = onAddProduct('t-cart', results);
                    viewEngine.setView(viewModel);
                } else {
                    viewEngine.setView({
                        template: 't-cart',
                        data: { searchterm: 'blah' }
                    });
                }
            });            
        });



    }
});

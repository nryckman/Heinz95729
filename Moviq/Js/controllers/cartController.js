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

        function refreshCart() {
            routes.refresh();
        };

        // POST /#/cart/remove
        routes.del(/^\/#\/cart\/remove\/?/i, function (context) {  // /books
            // get cart and product id
            var cart_id = $.cookie('cart_id');
            var product_id = this.params['product_uid'];

            // remove cart_id/product_id
            if (cart_id !== undefined) {
                $.ajax({
                    url: '/api/cart/remove/' + cart_id + '/' + product_id,
                    method: 'POST'
                }).done(function (data) {
                    viewEngine.headerVw.subtractFromCart();
                    refreshCart();
                });
            }
        });

        // POST /#/cart/add
        routes.post(/^\/#\/cart\/add\/?/i, function (context) {  // /books
            // get cart and product id
            var cart_id = $.cookie('cart_id');
            var product_id = this.params['product_uid'];

            // get new cart id
            $.ajax({
                url: '/api/cart/new/',
                method: 'GET'
            }).done(function (data) {
                // save new cart id
                if (cart_id === undefined) {
                    var cart_id_obj = JSON.parse(data);
                    $.cookie('cart_id', cart_id_obj.cart_id, { expires: 365, path: '/' });
                    cart_id = $.cookie('cart_id');
                }

                // add cart item
                $.ajax({
                    url: '/api/cart/add/' + cart_id + '/' + product_id,
                    method: 'POST'
                }).done(function (data) {
                    viewEngine.headerVw.addToCart();
                    location.href = '/#/cart';
                });
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

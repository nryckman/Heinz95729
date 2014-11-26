/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, Products, Product) {
        "use strict";

        var onAddProduct = function (template, data) {
            var self = {};
        
            self.template = template;
            self.data = data;
            self.test = function () {
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
                var results = new Products(JSON.parse(data));

                if (results.products().length > 0) {
                    viewEngine.setView(onAddProduct('t-cart', results));
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

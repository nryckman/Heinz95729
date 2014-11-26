/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, Products, Product) {
        "use strict";

        var onAddProduct;


        // POST /#/addcart
        routes.post(/^\/#\/addcart\/?/i, function (context) {  // /books
            $.ajax({
                url: '/api/cart/',
                method: 'POST'
            }).done(function (data) {
                alert('posted');
                viewEngine.headerVw.addToCart();
            });
        });

        // GET /#/cart
        routes.get(/^\/#\/cart\/?/i, function (context) {  // /books
            $.ajax({
                url: '/api/cart/',
                method: 'GET'
            }).done(function (data) {
                var results = new Products(JSON.parse(data));

                if (results.products().length > 0) {
                    viewEngine.setView({
                        template: 't-cart',
                        data: results
                    });
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

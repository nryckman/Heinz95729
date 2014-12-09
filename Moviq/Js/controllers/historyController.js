/*global define, JSON*/
define('controllers/historyController', {
    init: function ($, routes, viewEngine, CartItems, CartItem) {
        "use strict";

        // checkout view model
        var HistoryViewModel = function (template, data) {
            var self = {};

            self.template = template;
            self.data = data;
            self.after = function () {
                var unit_costs = [];
                $("input#unit_cost").each(function () {
                    unit_costs.push($(this).val());
                });

                var quantities = [];
                $("input#quantity").each(function () {
                    quantities.push($(this).val());
                });

                var count = unit_costs.length;
                var total = 0;
                for (var i = 0; i < count; i++) {
                    var subtotal = unit_costs[i] * quantities[i];
                    total = total + subtotal;
                    $("span#subtotal").eq(i).text('$' + subtotal.toFixed(2));
                }
            };

            return self;
        };

        // GET /#/history
        routes.get(/^\/#\/history\/?/i, function (context) {  // /books
            $.ajax({
                url: '/api/history/list',
                method: 'GET'
            }).done(function (data) {
                var results = new CartItems(JSON.parse(data));

                if (results.cartItems().length > 0) {
                    var historyViewModel = HistoryViewModel('t-history', results);
                    viewEngine.setView(historyViewModel);
                } else {
                    viewEngine.setView({
                        template: 't-history',
                        data: { searchterm: 'blah' }
                    });
                }
            });
        });
    }
});

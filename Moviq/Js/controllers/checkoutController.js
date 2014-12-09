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
                // populate year field with reasonable values
                var select = $(".card-expiry-year");
                var year = new Date().getFullYear();
                for (var i = 0; i < 12; i++) {
                    select.append($("<option value='" + (i + year) + "' " + (i === 0 ? "selected" : "") + ">" + (i + year) + "</option>"))
                }

                // set stripe public key
                Stripe.setPublishableKey('pk_test_6pRNASCoBOKtIshFeQd4XMUh');
            };

            self.stripeResponseHandler = function (status, response) {
                var $form = $('#payment-form');

                if (response.error) {
                    // Show the errors on the form
                    $form.find('.payment-errors').text(response.error.message);
                   // $form.find(':input:disabled').prop('disabled', false);
                    $form.find(':input:disabled').prop('disabled', false);
                   // $form.find('button').prop('disabled', false);
                } else {
                    // response contains id and card, which contains additional card details
                    var card_number = $('.card-number').val();
                    var cvc = $('.card-cvc').val();
                    var exp_month = $('.card-expiry-month').val();
                    var exp_year =  $('.card-expiry-year').val();
                    var cart_id = $.cookie('cart_id');

                    if (cart_id !== undefined) {
                        // process token with our middleware
                        $.ajax({
                            url: '/api/checkout/pay/' + cart_id + '/' + card_number + '/' + cvc + '/' + exp_month + '/' + exp_year,
                            method: 'POST'
                        }).done(function (data) {
                            $.removeCookie('cart_id');
                            viewEngine.headerVw.updateCartCount();
                            location.href = '/#/history?checked_out=true';
                        });
                    }


                    // Insert the token into the form so it gets submitted to the server
                    //$form.append($('<input type="hidden" name="stripeToken" />').val(token));
                    // and submit
                    //$form.get(0).submit();
                }
                

                //alert('here: ' + status);
            };

            self.pay = function () {
                var $form = $("#payment-form");
                $form.find('.payment-errors').empty();

                // Disable the submit form to prevent repeated clicks
                $form.find(':input:not(:disabled)').prop('disabled', true)


                Stripe.card.createToken($form, self.stripeResponseHandler);
                
            };

            return self;
        };

        // GET /#/checkout
        routes.get(/^\/#\/checkout\/?/i, function (context) {
            var cart_id = $.cookie('cart_id');

            if (cart_id !== undefined)
            {
                $.ajax({
                    url: '/api/cart/list/' + cart_id,
                    method: 'GET'
                }).done(function (data) {
                    var results = new CartItems(JSON.parse(data));

                    if (results.cartItems().length > 0) {
                        var viewModel = CheckoutViewModel('t-checkout', results);
                        viewEngine.setView(viewModel);
                    } else {
                        viewEngine.setView({
                            template: 't-empty',
                            data: { searchterm: 'blah' }
                        });
                    }
                });

            } else {
                viewEngine.setView({
                    template: 't-empty',
                    data: { searchterm: 'blah' }
                });
            }
        });
    }
});

/*jslint plusplus: true*/
/*global define*/
define('models/cartItems', {
    init: function (ko, CartItem) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cartItems module');
        }

        if (!CartItem) {
            throw new Error('Argument Exception: cartItem is required to init the cartItems module');
        }

        var CartItems = function (cartItems) {
            var $this = this;
            $this.cartItems = ko.observableArray();

            $this.addCartItem = function (cartItem) {
                if (!cartItem) {
                    throw new Error('Argument Exception: the argument, cartItem, must be defined to add a cartItem');
                }

                if (!(cartItem instanceof CartItem)) {
                    cartItem = new CartItem(cartItem);
                }

                $this.cartItems.push(cartItem);
            };

            $this.addCartItems = function (cartItems) {
                if (!cartItems) {
                    throw new Error('Argument Exception: the argument, cartItems, must be defined to add cartItems');
                }

                var i = 0;

                for (i; i < cartItems.length; i++) {
                    $this.addCartItem(cartItems[i]);
                }
            };

            if (cartItems) {
                $this.addCartItems(cartItems);
            }
        };

        return CartItems;
    }
});

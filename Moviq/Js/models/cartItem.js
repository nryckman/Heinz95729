/*jslint nomen: true*/
/*global define*/
define('models/cartItem', {
    init: function (ko) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cartItem module');
        }

        var CartItem = function (cartItem) {
            var $this = this;

            $this.setCartItemData = function (cartItem, cartItemData) {
                if (!cartItem) {
                    throw new Error('cannot extend the properties of undefined');
                }

                cartItemData = cartItemData || {};

                var type = cartItemData._type || 'cartItem';

                cartItem.guid = ko.observable(cartItemData.guid);
                cartItem.userGuid = ko.observable(cartItemData.userGuid);
                cartItem.productUid = ko.observable(cartItemData.productUid);
                cartItem.title = ko.observable(cartItemData.title || undefined);
                cartItem.price = ko.observable(cartItemData.price || undefined);
                cartItem.thumbnailLink = ko.observable(cartItemData.thumbnailLink || '/images/cartItems/default.png');
                cartItem.lastModified = ko.observable(cartItemData.lastModified);
                cartItem.purchaseDate = ko.observable(cartItemData.purchaseDate);
                cartItem.quantity = ko.observable(cartItemData.quantity);
            };

            if (cartItem) {
                $this.setCartItemData($this, cartItem);
            }
        };

        return CartItem;
    }
});
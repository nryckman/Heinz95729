/*global define*/
define('views/viewEngine', { init: function ($, ko) {
    "use strict";
    
    var mainVw, headerVw, setView;
    
    mainVw = {
        viewModel: ko.observable()
    };
    
    headerVw = function () {
        var self = {};
        
        self.home = { text: 'Moviq', path: '/' };
        self.links = [];
        self.cartCount = ko.observable();
        self.showCartCount = ko.computed(function () {
            return self.cartCount() > 0;
        }, self);
        
        self.addToCart = function () {
            self.cartCount((self.cartCount() || 0) + 1);
        };
        
        self.subtractFromCart = function () {
            var count = (self.cartCount() || 1) - 1;
            
            if (count > 0) {
                self.cartCount(count);
            } else {
                self.cartCount('');
            }
        };
        
        self.updateCartCount = function () {
            var cart_id = $.cookie('cart_id');
            $.ajax({
                url: '/api/cart/'+ cart_id + '/count',
                method: 'GET'
            }).done(function (data) {
                var cart_count_obj = JSON.parse(data);
                var cart_count = cart_count_obj.cart_count;
                self.cartCount(cart_count);
            });
        };

        self.links.push({ text: 'BOOKS', href: 'books' });
        self.links.push({ text: 'MUSIC', href: 'music' });
        self.links.push({ text: 'MOVIES', href: 'movies' });
        
        self.updateCartCount();

        return self;
    };
    
    setView = function (viewModel) {
        if (!viewModel) {
            throw new Error('viewModel is undefined. The mainVw cannot be updated.');
        }
        
        if (window.scroll) {
            window.scroll(0, 0);
        }
        
        $('.main').removeClass('in').addClass('out');
        setTimeout(function () {
            mainVw.viewModel(viewModel);
            $('.main').removeClass('out').addClass('in');
            
            if (typeof viewModel.after === 'function') {
                viewModel.after();
            }
        }, 500);
    };
    
    return {
        // mainVw is a singleton - there will only ever be one
        // it is the master view model that is used for refreshing page content
        mainVw: mainVw,
        
        // headerVw is a 
        headerVw: headerVw(),
        
        // setView provides a safter approach to udpating the viewModel 
        // property of the mainVw.
        // @param viewModel (object): The active viewModel
        setView: setView
    };

}});
/**
*   Composition Root / Startup / Bootstrapper
*/
/*jslint plusplus: true*/
/*globals require, define*/
require(['routeEngine', 'views/viewEngine', 'config', 'utils',
         'controllers/homeController', 'controllers/booksController',
         'controllers/authController', 'controllers/cartController',
         'controllers/historyController',
         'controllers/profileController',
         'models/product', 'models/products',
         'models/cartItem', 'models/cartItems',
         'models/book', 'models/books',
         'views/headerVw',
         'ko', 'lib/ko.binders', 'sammy', 'jquery', 'lib/jquery.cookie'],
        function (routeEngineCtor, viewEngineCtor, configCtor, utilsCtor,
                   homeControllerCtor, booksControllerCtor,
                   authControllerCtor, cartControllerCtor,
                   historyControllerCtor,
                   profileControllerCtor,
                   ProductCtor, ProductsCtor, CartItemCtor, CartItemsCtor, BookCtor, BooksCtor,
                   headerVwCtor,
                   ko, koBinders, sammy, $) {
        "use strict";

        var config,
            utils,
            routeEngine,
            viewEngine,
            Product,
            Products,
            CartItem,
            CartItems,
            Book,
            Books,
            homeController,
            booksController,
            authController,
            cartController,
            historyController,
            profileController;
            
        // initialize ko binding extensions
        koBinders.init($, ko);
        
        //region CORE         =================================================================
        (function () {
            config = configCtor.init();
            utils = utilsCtor.init();
            viewEngine = viewEngineCtor.init($, ko);
            routeEngine = routeEngineCtor.init($, sammy, config, utils, viewEngine);

            define('routes', function () { return routeEngine; });
            define('views', function () { return viewEngine; });
        }());
        //endregion CORE

        //region MODELS       =================================================================
        (function () {
            Product = ProductCtor.init(ko);
            Products = ProductsCtor.init(ko, Product);
            CartItem = CartItemCtor.init(ko);
            CartItems = CartItemsCtor.init(ko, CartItem);
            Book = BookCtor.init(ko, Product);
            Books = BooksCtor.init(ko, Book);
        }());
        //endregion MODELS
        
        //region CONTROLLERS  =================================================================
        (function () {
            booksController = booksControllerCtor.init($, routeEngine, viewEngine, Books, Book);
            homeController = homeControllerCtor.init(routeEngine, viewEngine, Products, Product);
            authController = authControllerCtor.init($, routeEngine, viewEngine);
            cartController = cartControllerCtor.init($, routeEngine, viewEngine, CartItems, CartItem);
            historyController = historyControllerCtor.init($, routeEngine, viewEngine);
            profileController = profileControllerCtor.init($, routeEngine, viewEngine);
        }());
        //endregion CONTROLLERS
            
        //region CONTROLLERS  =================================================================
        (function () {
            headerVwCtor.init($, routeEngine);
        }());
        //endregion CONTROLLERS
            
        ko.applyBindings(viewEngine.mainVw, $('.main')[0]);
        ko.applyBindings(viewEngine.headerVw, $('header')[0]);
        routeEngine.listen();
    
    });
/*global define, JSON*/
define('controllers/historyController', {
    init: function ($, routes, viewEngine) {
        "use strict";

        // GET /#/history
        routes.get(/^\/#\/history\/?/i, function (context) {  // /books
            viewEngine.setView({
                template: 't-history',
                data: { searchterm: 'blah' }
            });
        });
    }
});

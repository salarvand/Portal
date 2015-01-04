﻿angular
    .module("LayoutEditor")
    .directive("orcLayoutGrid", function ($compile, elementConfigurator, baseUrl) {
        return {
            restrict: "E",
            scope: { element: "=" },
            controller: function ($scope, $element) {
                elementConfigurator.addElementFunctions($scope, $element);
                elementConfigurator.addContainerFunctions($scope, $element);
            },
            templateUrl: baseUrl.get() + "/Templates/orc-layout-grid.html",
            replace: true
        };
    });
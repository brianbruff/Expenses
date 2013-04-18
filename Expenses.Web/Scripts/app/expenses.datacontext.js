window.expensesApp = window.expensesApp || {};

window.expensesApp.datacontext = (function () {

    var datacontext = {
        getExpenseReports: getExpenseReports,
        getExpenseReport: getExpenseReport,
        getExpenseImage: getExpenseImage,
        createExpense: createExpense,
        createExpenseReport: createExpenseReport,
        saveNewExpense: saveNewExpense,
        saveChangedExpenseImage: saveChangedExpenseImage,
        saveNewExpenseReport: saveNewExpenseReport,
        saveChangedExpense: saveChangedExpense,
        saveChangedExpenseReport: saveChangedExpenseReport,
        deleteExpense: deleteExpense,
        deleteExpenseReport: deleteExpenseReport,
        getCurrencies: getCurrencies,
        getExpenseTypes: getExpenseTypes
    };

    return datacontext;
    

    function getCurrencies(currenciesObservable, errorObservable) {
        return ajaxRequest("get", currencyUrl())
            .done(getSucceeded)
            .fail(getFailed);
            

        function getSucceeded(data) {
            var mappedCurrencies = $.map(data, function (list) { return new createCurrency(list); });
            currenciesObservable(mappedCurrencies);
        }

        function getFailed() {
            errorObservable("Error retrieving .");
        }
    }
    
    function getExpenseTypes(expenseTypesObservable, errorObservable) {
        return ajaxRequest("get", expenseTypesUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedExpenseTypes = $.map(data, function (list) { return new createExpenseType(list); });
            expenseTypesObservable(mappedExpenseTypes);
        }

        function getFailed() {
            errorObservable("Error retrieving .");
        }
    }

    function getExpenseReports(expenseReportsObservable, errorObservable) {
        return ajaxRequest("get", expenseReportUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedExpenseReports = $.map(data, function (list) { return new createExpenseReport(list); });
            expenseReportsObservable(mappedExpenseReports);
        }

        function getFailed() {
            errorObservable("Error retrieving expense reports.");
        }
    }
    function getExpenseReport(expenseReportId, expenseReportObservable, errorObservable) {
        return ajaxRequest("get", expenseReportUrl() + "/" + expenseReportId)
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            expenseReportObservable(createExpenseReport(data));
        }

        function getFailed() {
            errorObservable("Error retrieving expense reports.");
        }
    }
    function getExpenseImage(expenseObj, expenseObservable, errorObservable) {
        expenseObservable(expenseObj);
        return ajaxRequest("get", expenseImageUrl() + "/" + expenseObj.expenseId)
            .done(getSucceeded)
            .fail(getFailed);


        function getSucceeded(data) {
            expenseObservable().image(data.image);
            expenseObservable().imageType(data.imageType);
        }

        function getFailed() {
            errorObservable("Error retrieving expense reports.");
        }
    }
    function createExpense(data) {
        return new datacontext.expense(data); // expense is injected by expenses.model.js
    }
    function createExpenseReport(data) {
        return new datacontext.expenseReport(data); // ExpenseReport is injected by expenses.model.js
    }
    function createCurrency(data) {
        return new datacontext.currency(data); // currency is injected by expenses.model.js
    }
    
    function createExpenseType(data) {
        return new datacontext.expenseType(data); // currency is injected by expenses.model.js
    }
    

    function saveNewExpense(expense) {
        clearErrorMessage(expense);
        return ajaxRequest("post", expenseUrl(), expense)
            .done(function (result) {
                expense.expenseId = result.expenseId;
            })
            .fail(function () {
                expense.errorMessage("Error adding a new expense item.");
            });
    }
    
    function saveChangedExpenseImage(expense) {
        clearErrorMessage(expense);
        return ajaxRequest("put", expenseImageUrl(expense.expenseId), expense)
            .done(function (result) {
                expense.expenseId = result.expenseId;
            })
            .fail(function (e) {
                if (e.status == 200)
                    return; // todo not sure why jQuery is giving an error
                var msg = {};
                if (e.responseText && e.responseText.length > 0) {
                    var message = JSON.parse(e.responseText);
                    msg = message.message || {};
                }
                expense.errorMessage("Error adding a new expense item:" + msg);
            });
    }
    
    function saveNewExpenseReport(expenseReport) {
        clearErrorMessage(expenseReport);
        return ajaxRequest("post", expenseReportUrl(), expenseReport)
            .done(function (result) {
                expenseReport.expenseReportId = result.expenseReportId;
                expenseReport.userId = result.userId;
            })
            .fail(function () {
                expenseReport.errorMessage("Error adding a new todo list.");
            });
    }
    function deleteExpense(expense) {
        return ajaxRequest("delete", expenseUrl(expense.expenseId))
            .fail(function (e) {
                expense.errorMessage("Error removing expense item.");
            });
    }
    function deleteExpenseReport(expenseReport) {
        return ajaxRequest("delete", expenseReportUrl(expenseReport.expenseReportId))
            .fail(function () {
                expenseReport.errorMessage("Error removing todo list.");
            });
    }
    function saveChangedExpense(expense) {
        clearErrorMessage(expense);
        return ajaxRequest("put", expenseUrl(expense.expenseId), expense, "text")
            .fail(function () {
                expense.errorMessage("Error updating expense item.");
            });
    }
    function saveChangedExpenseReport(expenseReport) {
        clearErrorMessage(expenseReport);
        return ajaxRequest("put", expenseReportUrl(expenseReport.expenseReportId), expenseReport, "text")
            .fail(function () {
                expenseReport.errorMessage("Error updating the expense report.");
            });
    }

    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            };
        }
        return $.ajax(url, options);
    }
    // routes
    function expenseReportUrl(id) { return "/api/ExpenseReports/" + (id || ""); }
    function expenseUrl(id) { return "/api/Expenses/" + (id || ""); }
    function expenseImageUrl(id) { return "/api/ExpenseImage/" + (id || ""); }
    function currencyUrl(id) { return "/api/Currencies/" + (id || ""); }
    function expenseTypesUrl(id) { return "/api/ExpenseTypes/" + (id || ""); }

})();
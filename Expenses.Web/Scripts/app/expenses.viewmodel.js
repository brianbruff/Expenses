window.expensesApp.ExpenseReportViewModel = (function (ko, datacontext) {
    var expenseReports = ko.observableArray(),
        currencies = ko.observableArray(),       
        expenseTypes = ko.observableArray(),
        error = ko.observable(),
        selectedReport = ko.observable(),
        getReport = function (expenseReport) {
            datacontext.getExpenseReport(expenseReport, selectedReport, error);
        },
        addExpenseReport = function () {
            var expenseReport = datacontext.createExpenseReport();
            expenseReport.isEditing(true);
            datacontext.saveNewExpenseReport(expenseReport)
                .then(addSucceeded)
                .fail(addFailed);

            function addSucceeded() {
                showExpenseReport(expenseReport);
            }
            function addFailed() {
                error("Save of new ExpenseReport failed");
            }
        },
        showExpenseReport = function (expenseReport) {
            expenseReports.unshift(expenseReport); // Insert new ExpenseReport at the front
        },
        deleteExpenseReport = function (expenseReport) {
            expenseReports.remove(expenseReport);
            datacontext.deleteExpenseReport(expenseReport)
                .fail(deleteFailed);

            function deleteFailed() {
                showExpenseReport(expenseReport); // re-show the restored list
            }
        };

    datacontext.getExpenseReports(expenseReports, error); // load ExpenseReports
    datacontext.getCurrencies(currencies, error); // load currencies
    datacontext.getExpenseTypes(expenseTypes, error); // todo: race to display an error

    return {
        selectedReport: selectedReport,
        getReport: getReport,
        expenseReports: expenseReports,
        currencies: currencies,
        expenseTypes: expenseTypes,
        error: error,
        addExpenseReport: addExpenseReport,
        deleteExpenseReport: deleteExpenseReport,
        //submitReport: submitReport
    };

})(ko, expensesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.expensesApp.ExpenseReportViewModel);

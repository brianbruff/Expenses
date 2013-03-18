﻿window.expensesApp.ExpenseReportViewModel = (function (ko, datacontext) {
    /// <field name="expenseReports" value="[new datacontext.expenseReport()]"></field>
    var expenseReports = ko.observableArray(),
        error = ko.observable(),
        selectedReport = ko.observable(),
        getReport = function(expenseReportId) {
            datacontext.getExpenseReport(expenseReportId, selectedReport, error);
            
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

    return {
        selectedReport: selectedReport,
        getReport: getReport,
        expenseReports: expenseReports,
        error: error,
        addExpenseReport: addExpenseReport,
        deleteExpenseReport: deleteExpenseReport
    };

})(ko, expensesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.expensesApp.ExpenseReportViewModel);
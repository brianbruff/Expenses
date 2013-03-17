window.expensesApp.ExpenseReportViewModel = (function (ko, datacontext) {
    /// <field name="ExpenseReports" value="[new datacontext.ExpenseReport()]"></field>
    var ExpenseReports = ko.observableArray(),
        error = ko.observable(),
        addExpenseReport = function () {
            var ExpenseReport = datacontext.createExpenseReport();
            ExpenseReport.isEditingListTitle(true);
            datacontext.saveNewExpenseReport(ExpenseReport)
                .then(addSucceeded)
                .fail(addFailed);

            function addSucceeded() {
                showExpenseReport(ExpenseReport);
            }
            function addFailed() {
                error("Save of new ExpenseReport failed");
            }
        },
        showExpenseReport = function (ExpenseReport) {
            ExpenseReports.unshift(ExpenseReport); // Insert new ExpenseReport at the front
        },
        deleteExpenseReport = function (ExpenseReport) {
            ExpenseReports.remove(ExpenseReport);
            datacontext.deleteExpenseReport(ExpenseReport)
                .fail(deleteFailed);

            function deleteFailed() {
                showExpenseReport(ExpenseReport); // re-show the restored list
            }
        };

    datacontext.getExpenseReports(ExpenseReports, error); // load ExpenseReports

    return {
        ExpenseReports: ExpenseReports,
        error: error,
        addExpenseReport: addExpenseReport,
        deleteExpenseReport: deleteExpenseReport
    };

})(ko, expensesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.expensesApp.ExpenseReportViewModel);

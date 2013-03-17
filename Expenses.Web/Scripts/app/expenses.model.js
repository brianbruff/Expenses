(function (ko, datacontext) {
    datacontext.expense = expense;
    datacontext.expenseReport = expenseReport;

    function expense(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseId = data.expenseId;
        self.title = ko.observable(data.title);
        self.isDone = ko.observable(data.isDone);
        self.expenseReportId = data.expenseReportId;

        // Non-persisted properties
        self.errorMessage = ko.observable();

        saveChanges = function () {
            return datacontext.saveChangedExpense(self);
        };

        // Auto-save when these properties change
        self.isDone.subscribe(saveChanges);
        self.title.subscribe(saveChanges);

        self.toJson = function () { return ko.toJSON(self) };
    };

    function expenseReport(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseReportId = data.expenseReportId;
        self.userId = data.userId || "to be replaced";
        self.title = ko.observable(data.title || "My todos");
        self.todos = ko.observableArray(importExpenses(data.todos));

        // Non-persisted properties
        self.isEditingListTitle = ko.observable(false);
        self.newTodoTitle = ko.observable();
        self.errorMessage = ko.observable();

        self.deleteTodo = function () {
            var expense = this;
            return datacontext.deleteExpense(expense)
                 .done(function () { self.todos.remove(expense); });
        };

        // Auto-save when these properties change
        self.title.subscribe(function () {
            return datacontext.saveChangedExpenseReport(self);
        });

        self.toJson = function () { return ko.toJSON(self) };
    };
    // convert raw expense data objects into array of Expenses
    function importExpenses(expenses) {
        /// <returns value="[new expense()]"></returns>
        return $.map(expenses || [],
                function (expenseData) {
                    return datacontext.createExpense(expenseData);
                });
    }
    expenseReport.prototype.addExpense = function () {
        var self = this;
        if (self.newTodoTitle()) { // need a title to save
            var expense = datacontext.createExpense(
                {
                    title: self.newTodoTitle(),
                    expenseReportId: self.expenseReportId
                });
            self.todos.push(expense);
            datacontext.saveNewExpense(expense);
            self.newTodoTitle("");
        }
    };
})(ko, expensesApp.datacontext);

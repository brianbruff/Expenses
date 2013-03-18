(function (ko, datacontext) {
    datacontext.expense = expense;
    datacontext.expenseReport = expenseReport;

    function expense(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseId = data.expenseId;
        self.expenseReportId = data.expenseReportId;
        
        self.description = ko.observable(data.description);
        

        // Non-persisted properties
        self.errorMessage = ko.observable();

        saveChanges = function () {
            return datacontext.saveChangedExpense(self);
        };

        // Auto-save when these properties change
        ///self.isDone.subscribe(saveChanges);
        self.description.subscribe(saveChanges);

        self.toJson = function () { return ko.toJSON(self) };
    };

    function expenseReport(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseReportId = data.expenseReportId;
        self.name = ko.observable(data.name || "<New...>");
        self.date = ko.observable(data.date);
        self.expenses = ko.observableArray(importExpenses(data.expenses));

        // Non-persisted properties
        self.isEditing = ko.observable(false);
        self.newTodoTitle = ko.observable();
        self.errorMessage = ko.observable();
        // Non-persisted properties
        self.isSubmitted = ko.computed(function () {
            return self.date !== null;
        });

        self.deleteTodo = function () {
            var expense = this;
            return datacontext.deleteExpense(expense)
                 .done(function () { self.todos.remove(expense); });
        };

        // Auto-save when these properties change
        self.name.subscribe(function () {
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
            self.expenses.push(expense);
            datacontext.saveNewExpense(expense);
            self.newTodoTitle("");
        }
    };
})(ko, expensesApp.datacontext);

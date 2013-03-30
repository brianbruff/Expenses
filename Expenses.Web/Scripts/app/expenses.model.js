(function (ko, datacontext) {
    // Inject the models
    datacontext.expense = expense;
    datacontext.expenseReport = expenseReport;
    datacontext.currency = currency;
    datacontext.expenseType = expenseType;

    function expense(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseId = data.expenseId;
        self.expenseReportId = data.expenseReportId;
        self.date = ko.observable(new Date(data.date));
        self.description = ko.observable(data.description);
        self.currencyId = ko.observable(data.currencyId);
        self.typeId = ko.observable(data.typeId);
        self.image = ko.observable('data:image/jpg;base64,'+data.image);
        self.amount = ko.observable(data.amount);
        self.exchangeRate = ko.observable(data.exchangeRate);
        
        self.baseAmount = ko.computed(function () {
            return self.amount() * self.exchangeRate();
        });
        

        // Non-persisted properties
        self.errorMessage = ko.observable();
        self.displayDate = ko.computed(function () {
            return moment(data.date).format("DD/MM/YYYY");
        });

        self.saveChanges = function () {
            return datacontext.saveChangedExpense(self);
        };

        // Auto-save when these properties change
        ///self.isDone.subscribe(saveChanges);
        self.description.subscribe(self.saveChanges);

        self.toJson = function () { return ko.toJSON(self) };
    };

    function expenseReport(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseReportId = data.expenseReportId;
        self.name = ko.observable(data.name || "Unsubmitted");
        self.date = ko.observable(data.date);
        self.expenses = ko.observableArray(importExpenses(data.expenses));
        self.selectedExpense = ko.observable();
        self.reportVisible = ko.observable(true);
        
        self.getExpense = function (expense) {
            self.reportVisible(false);
            datacontext.getExpense(expense.expenseId, self.selectedExpense, self.errorMessage);
        };
        
        self.done = function () {
            self.reportVisible(true);
            self.selectedExpense(null);
        };

        // Non-persisted properties
        self.isEditing = ko.observable(false);
        self.newTodoTitle = ko.observable();
        self.errorMessage = ko.observable();
        // Non-persisted properties
        self.isSubmitted = ko.computed(function () {
            return self.date !== null;
        });

        self.baseTotal = ko.computed(function () {
            var total = 0;
            $.each(self.expenses(), function (e) {
                 total += this.baseAmount();
            });
            return total;
        });

        self.deleteExpense = function () {
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
        //var self = this;
        //if (self.newTodoTitle()) { // need a title to save
        //    var expense = datacontext.createExpense(
        //        {
        //            description: self.description(),
        //            expenseId: self.expenseId
        //        });
        //    self.expenses.push(expense);
        //    datacontext.saveNewExpense(expense);
        //    self.newTodoTitle("");
        //}
    };
    

    function currency(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.currencyId = data.id;
        self.code = data.code;
    };
    
    function expenseType(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseTypeId = data.id;
        self.code = data.name;
    };


})(ko, expensesApp.datacontext);

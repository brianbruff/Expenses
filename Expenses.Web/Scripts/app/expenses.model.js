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
        self.image = ko.observable(data.image);
        self.imageFile = ko.observable();
        self.imageObjectURL = ko.observable();
        self.imageBinary = ko.observable();
        self.amount = ko.observable(data.amount);
        self.exchangeRate = ko.observable(data.exchangeRate);
        
        self.baseAmount = ko.computed(function () {
            return self.amount() * self.exchangeRate();
        });
        

        // Non-persisted properties
        self.errorMessage = ko.observable();
        self.displayDate = ko.computed(function () {
            return moment(self.date()).format("DD/MM/YYYY");
        });
        self.imageSrc = ko.computed(function () {
            return 'data:image/jpg;base64,' + self.image();
        });

        self.saveChanges = function () {
            return datacontext.saveChangedExpense(self);
        };

        // Auto-save when these properties change
        self.description.subscribe(self.saveChanges);
        self.date.subscribe(self.saveChanges);
        self.currencyId.subscribe(self.saveChanges);
        self.typeId.subscribe(self.saveChanges);
        self.amount.subscribe(self.saveChanges);
        self.imageFile.subscribe(function () {
            // select the form and post it
            //var form = $("form[id='imageForm']");
            //form.submit();
        });

        self.toJson = function () {
            var json = ko.toJSON(self, function(key,value) {
                if (key !== 'image')
                    return value;
                return null;
            });
            // we don't send the image here, it's done on it's own
            
            return json;
        };
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
        
        self.getExpenseImage = function (expense) {
            self.reportVisible(false);
            datacontext.getExpenseImage(expense, self.selectedExpense, self.errorMessage);
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

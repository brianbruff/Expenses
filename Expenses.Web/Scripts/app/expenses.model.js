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
        self.imageType = ko.observable(data.imageType);
        self.imageFile = ko.observable();
        self.imagePath = ko.observable();
        self.imageObjectURL = ko.observable();
        self.amount = ko.observable(data.amount);
        self.exchangeRate = ko.observable(data.exchangeRate);
        
        self.baseAmount = ko.computed(function () {
            return self.amount() * self.exchangeRate();
        });
        
        self.imageSrc = ko.computed(function () {
            return self.imageType() + "," + self.image();
        });

        // Non-persisted properties
        self.errorMessage = ko.observable();
        self.displayDate = ko.computed(function () {
            if (!self.date())
                self.date(new Date());
            return moment(self.date()).format("DD/MM/YYYY");
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
        self.imageObjectURL.subscribe(function () {
            datacontext.saveChangedExpenseImage(new expenseImageDto(self.expenseId, self.image(), self.imageType(), self.errorMessage));
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
    
    function expenseImageDto(expenseId, expenseImage, expenseImageType, errorMessage) {
        var self = this;
        
        self.expenseId = expenseId;
        self.image = expenseImage;
        // self.imageParts = expenseImage.split(";base64,");
        self.image = expenseImage;
        self.imageType = expenseImageType;
        self.errorMessage = errorMessage;
        
        self.toJson = function () {
            var json = ko.toJSON(self);
            return json;
        };
    }

    function expenseReport(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.expenseReportId = data.expenseReportId;
        self.date = ko.observable(data.date);
        self.name = ko.observable(self.date() || "Unsubmitted");
        self.expenses = ko.observableArray(importExpenses(data.expenses));
        self.selectedExpense = ko.observable();
        self.reportVisible = ko.observable(true);
        
        self.getExpenseImage = function (expense) {
            self.reportVisible(false);
            datacontext.getExpenseImage(expense, self.selectedExpense, self.errorMessage);
        };

        self.isSubmitted = ko.computed(function () {
            return self.date() != null;
        });
        
        self.date.subscribe(function () {
            self.name(moment(self.date()).format("DD/MM/YYYY"));
        });
        
        self.name.subscribe(function () {
            var match = ko.utils.arrayFirst(window.expensesApp.ExpenseReportViewModel.expenseReports(), function (item) {
                return self.expenseReportId === item.expenseReportId;
            });

            if (match && match.name() !== self.name())
                match.name(self.name());
            
        });
         
        //self.name = ko.computed(function () {
        //    if (self.date())
        //        return moment(self.date()).format("DD/MM/YYYY");
                
        //    return "Unsubmitted";
        //});
        
        self.done = function () {
            self.reportVisible(true);
            self.selectedExpense(null);
        };
        
        self.submit = function () {
            return datacontext.submitReport(self).done(function () {

            });
        };

        // Non-persisted properties
        self.isEditing = ko.observable(false);
        self.newTodoTitle = ko.observable();
        self.errorMessage = ko.observable();
        // Non-persisted properties
        self.isSubmitted = ko.computed(function () {
            return self.date() !== null;
        });

        self.baseTotal = ko.computed(function () {
            var total = 0;
            $.each(self.expenses(), function (e) {
                 total += this.baseAmount();
            });
            return total.toFixed(2);
        });

        self.deleteExpense = function () {
            var expense = this;
            return datacontext.deleteExpense(expense)
                 .done(function () { self.expenses.remove(expense); });
        };

        // Auto-save when these properties change
        //self.name.subscribe(function () {
        //    return datacontext.saveChangedExpenseReport(self);
        //});

        self.toJson = function () { return ko.toJSON(self) };
    };
    // convert raw expense data objects into array of Expenses
    function importExpenses(expenses) {
        return $.map(expenses || [],
                function (expenseData) {
                    return datacontext.createExpense(expenseData);
                });
    };
    
    
    expenseReport.prototype.newExpense = function () {
        var self = this;
            var expense = datacontext.createExpense(
                {
                    description: "_newExpense",
                    date: new Date(),
                    currencyId: expensesApp.ExpenseReportViewModel.currencies()[0].currencyId,
                    typeId: expensesApp.ExpenseReportViewModel.expenseTypes()[0].expenseTypeId,
                    expenseReportId: self.expenseReportId,
                    exchangeRate: 1,
                    amount: 0,
                    
                });
            self.expenses.push(expense);
            datacontext.saveNewExpense(expense);
    };
    
    //expenseReport.prototype.submit = function () {
       
    //};
    

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

// Hooks up a form to jQuery Validation
ko.bindingHandlers.validate = {
    init: function (elem, valueAccessor) {
        $(elem).validate();
    }
};

ko.bindingHandlers.img = {
    update: function (element, valueAccessor) {
        //grab the value of the parameters, making sure to unwrap anything that could be observable
        var value = ko.utils.unwrapObservable(valueAccessor()),
            src = ko.utils.unwrapObservable(value.src),
            fallback = ko.utils.unwrapObservable(value.fallback),
            $element = $(element);

        //now set the src attribute to either the bound or the fallback value
        if (src !== "data:image/jpg;base64,null") {
            $element.attr("src", src);
        } else {
            $element.attr("src", fallback);
        }
    },
    init: function (element, valueAccessor) {
        var $element = $(element);

        //hook up error handling that will unwrap and set the fallback value
        $element.error(function () {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                busy = ko.utils.unwrapObservable(value.busy);

            $element.attr("src", busy);
        });
    },
};


// Controls whether or not the text in a textbox is selected based on a model property
ko.bindingHandlers.selected = {
    init: function (elem, valueAccessor) {
        $(elem).blur(function () {
            var boundProperty = valueAccessor();
            if (ko.isWriteableObservable(boundProperty)) {
                boundProperty(false);
            }
        });
    },
    update: function (elem, valueAccessor) {
        var shouldBeSelected = ko.utils.unwrapObservable(valueAccessor());
        if (shouldBeSelected) {
            $(elem).select();
        }
    }
};

// Makes a textbox lose focus if you press "enter"
ko.bindingHandlers.blurOnEnter = {
    init: function (elem, valueAccessor) {
        $(elem).keypress(function (evt) {
            if (evt.keyCode === 13 /* enter */) {
                evt.preventDefault();
                $(elem).triggerHandler("change");
                $(elem).blur();
            }
        });
    }
};

// Simulates HTML5-style placeholders on older browsers
ko.bindingHandlers.placeholder = {
    init: function (elem, valueAccessor) {
        var placeholderText = ko.utils.unwrapObservable(valueAccessor()),
            input = $(elem);

        input.attr('placeholder', placeholderText);

        // For older browsers, manually implement placeholder behaviors
        if (!Modernizr.input.placeholder) {
            input.focus(function () {
                if (input.val() === placeholderText) {
                    input.val('');
                    input.removeClass('placeholder');
                }
            }).blur(function () {
                setTimeout(function () {
                    if (input.val() === '' || input.val() === placeholderText) {
                        input.addClass('placeholder');
                        input.val(placeholderText);
                    }
                }, 0);
            }).blur();

            input.parents('form').submit(function () {
                if (input.val() === placeholderText) {
                    input.val('');
                }
            });
        }
    }
};


/** Binding to make content appear with 'fade' effect */
//ko.bindingHandlers['fadeIn'] = {
//    'update': function (element, valueAccessor) {
//        var options = valueAccessor();
//        if (options() === true)
//            $(element).slideDown();
//    }
//};
///** Binding to make content disappear with 'fade' effect */
//ko.bindingHandlers['fadeOut'] = {
//    'update': function (element, valueAccessor) {
//        var options = valueAccessor();
//        if (options() === true)
//            $(element).slideUp();
//    }
//};

var previousElement = null;
ko.bindingHandlers.fadeSwitcher = {
    init: function (element, valueAccessor) {
        //var value = valueAccessor();
        //$(element).toggle(ko.utils.unwrapObservable(value));
    },
    update: function (element, valueAccessor) {

        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            if (previousElement == null) { // initial fade
                $(element).fadeIn();
            } else {
                $(previousElement).fadeOut('slow', function () {
                    $(element).fadeIn();
                });
            }
            previousElement = element;
        } else {
            $(element).fadeOut('slow');
        }
            
    }
};


ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datepicker("getDate");

        if (value - current !== 0) {
            $(element).datepicker("setDate", value);
        }
    }
};
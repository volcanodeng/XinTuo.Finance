
// extend the 'telNum' rule
$.extend($.fn.validatebox.defaults.rules, {
    telNum: {
        validator: function (value, param) {
            return (/0\d{2,3}-\d{5,9}|0\d{2,3}-\d{5,9}/g).test(value);
        },
        message: '电话号码不匹配。'
    }
});

// extend the 'mobile' rule
$.extend($.fn.validatebox.defaults.rules, {
    mobile: {
        validator: function (value, param) {
            return (/^1[3|4|5|7|8]\d{9}$/g).test(value);
        },
        message: '手机号码无效。'
    }
});


// extend the 'equals' rule
$.extend($.fn.validatebox.defaults.rules, {
    equals: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '输入内容不一致。'
    }
});

$.validator.addMethod('CheckDisplayName', function (value, element) {
    var result = {};
    $("body").mask("正在校验花名……");
    $.ajax({
        type: "post",
        url: ajaxurl4DisplayCheck,
        async: false,
        cache: false,
        data: { workId: value },
        success: function (data) {
            result = data;
        }
    });
    $("body").unmask();
    return result.Status != undefined && result.Status == "True";
}, '花名或姓名(工号)不正确，请修正！');

$.validator.addMethod('CheckIsFormal', function (value, element) {
    var result = {};
    $("body").mask("正在校验是否为正式员工,请稍后……");
    $.ajax({
        type: "post",
        url: ajaxurl4CheckIsFormal,
        async: false,
        cache: false,
        data: { workId: value },
        success: function (data) {
            result = data;
        }
    });
    $("body").unmask();
    return result.Status != undefined && result.Status == "True";
}, '花名或姓名(工号)不正确，请修正！');
$.validator.addMethod('CheckLetters', function (value, element) {
    return /^[A-Za-z]+$/.test(value);
}, '必须为英文字母！');
$.validator.addMethod('CheckCellPhoneNumber', function (value, element) {
    return /^((\(\d{2,3}\))|(\d{3}\-))?1[3|5|8]\d{9}$/.test(value);
}, '手机号码不正确！');
$.validator.addMethod('CheckIdCardNumber', function (value, element) {
    if (value == null || value == "")
        return true;
    return /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/.test(value) || /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/.test(value);
}, '身份证格式不正确！');
$.validator.imgType = ["gif", "jpeg", "jpg", "bmp", "png"];
$.validator.addMethod('checkImageExt', function (value, element) {
    if (value == null || value == "")
        return true;
    if (!RegExp("\.(" + $.validator.imgType.join("|") + ")$", "i").test(value.toLowerCase())) {
         return false;
    }
    return true;
}, "图片类型必须是" + $.validator.imgType.join("，") + "中的一种");
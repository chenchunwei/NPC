var utility = {};
utility.formSerialize = function (form) {
    var o = {};
    $.each(form.serializeArray(), function (index) {
        if (o[this['name']]) {
            o[this['name']] = o[this['name']] + "," + this['value'];
        } else {
            o[this['name']] = this['value'];
        }
    });
    return o;
};
// 弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}
function showSlide(message, dueTime) {
    $.messager.show({
        title: '提示信息',
        msg: message,
        timeout: dueTime,
        showType: 'slide'
    });
}
function formatNewtonJsonDate(date) {
    return date.replace('T', ' ');
}
function evalJson(data) {
    return eval('(' + data + ')');
}
var controller = {};
controller.del = function (url, datagridSelector, idFeild) {
    if (datagridSelector == undefined)
        datagridSelector = '#datagrid';
    if (idFeild == undefined)
        idFeild = "id";
    var ids = [];
    var rows = $(datagridSelector).datagrid("getChecked");
    if (rows.length == 0)
        return;
    for (var i = 0; i < rows.length; i++) {
        ids.push(rows[i][idFeild]);
    }
    $.messager.confirm("删除提醒信息", "确认删除选中的" + ids.length + "条记录吗？",
        function (result) {
            if (result == true) {
                $("body").mask("正在进行删除操作，请稍等……");
                $.post(url, { 'ids': ids.join(',') }, function (data) {
                    data = evalJson(data);
                    $("body").unmask();
                    $(datagridSelector).datagrid("reload");
                    showSlide(data.Message, 2000);
                });
            }
        }
    );
};
$.validator.setDefaults({
    errorPlacement: function (error, element) {
        error.appendTo($(element).parent());
    }
});
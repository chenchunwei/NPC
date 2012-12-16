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
controller.del = function (url, ids, windowArg) {
    if (ids == null || ids.length == 0) {
        msgShow("没有选中任务记录", "请选择要删除的记录行", "warning");
        return;
    }
    var message = "确定要删除该条记录？";
    if (ids.length > 1) {
        message = "确认删除被选中的" + ids.length + "条记录吗？";
    }

    $.messager.confirm("删除提醒信息", message,
        function (result) {
            if (result == true) {
                $("body").mask("正在进行删除操作，请稍等……");
                $.post(url, { 'ids': ids.join(',') }, function (data) {
                    data = evalJson(data);
                    $("body").unmask();
                    if (windowArg == null)
                        window.location.reload();
                    else {
                        windowArg.location.reload();
                    }
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
//0全选 1取消  2反选
controller.batchOfChk = function (chkSelector, status) {
    $(chkSelector).each(function (i, o) {
        switch (status) {
            case 1:
                $(o).attr("checked", "checked");
                break;
            case 0:
                $(o).removeAttr("checked");
                break;
            case 2:
                if (o.checked) {
                    $(o).removeAttr("checked");
                } else {
                    $(o).attr("checked", "checked");
                }
                break;
        }
    });
};
//**********************图片上传预览插件************************
//作者：IDDQD(2009-07-01)
//Email：iddqd5376@163.com
//http://blog.sina.com.cn/iddqd
//版本：1.0

//说明：图片上传预览插件
//上传的时候可以生成固定宽高范围内的等比例缩放图

//参数设置：
//width                     存放图片固定大小容器的宽
//height                    存放图片固定大小容器的高
//imgDiv                    页面DIV的JQuery的id
//imgType                   数组后缀名
//**********************图片上传预览插件*************************
(function ($) {
    jQuery.fn.extend({
        uploadPreview: function (opts) {
            opts = jQuery.extend({
                width: 0,
                height: 0,
                imgDiv: "#imgDiv",
                imgType: ["gif", "jpeg", "jpg", "bmp", "png"],
                callback: function () { return false; }
            }, opts || {});
            var _self = this;
            var _this = $(this);
            var imgDiv = $(opts.imgDiv);
            imgDiv.width(opts.width);
            imgDiv.height(opts.height);

            autoScaling = function () {
                if ($.browser.version == "7.0" || $.browser.version == "8.0") imgDiv.get(0).filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = "image";
                var img_width = imgDiv.width();
                var img_height = imgDiv.height();
                if (img_width > 0 && img_height > 0) {
                    var rate = (opts.width / img_width < opts.height / img_height) ? opts.width / img_width : opts.height / img_height;
                    if (rate <= 1) {
                        if ($.browser.version == "7.0" || $.browser.version == "8.0") imgDiv.get(0).filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = "scale";
                        imgDiv.width(img_width * rate);
                        imgDiv.height(img_height * rate);
                    } else {
                        imgDiv.width(img_width);
                        imgDiv.height(img_height);
                    }
                    var left = (opts.width - imgDiv.width()) * 0.5;
                    var top = (opts.height - imgDiv.height()) * 0.5;
                    imgDiv.css({ "margin-left": left, "margin-top": top });
                    imgDiv.show();
                }
            }
            _this.change(function () {
                if (this.value) {
                    if (!RegExp("\.(" + opts.imgType.join("|") + ")$", "i").test(this.value.toLowerCase())) {
                        alert("图片类型必须是" + opts.imgType.join("，") + "中的一种");
                        this.value = "";
                        return false;
                    }
                    imgDiv.hide();
                    if ($.browser.msie) {
                        if ($.browser.version == "6.0") {
                            var img = $("<img />");
                            imgDiv.replaceWith(img);
                            imgDiv = img;

                            var image = new Image();
                            image.src = 'file:///' + this.value;

                            imgDiv.attr('src', image.src);
                            autoScaling();
                        }
                        else {
                            imgDiv.css({ filter: "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=image)" });
                            imgDiv.get(0).filters.item("DXImageTransform.Microsoft.AlphaImageLoader").sizingMethod = "image";
                            try {
                                imgDiv.get(0).filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = this.value;
                            } catch (e) {
                                alert("无效的图片文件！");
                                return;
                            }
                            setTimeout("autoScaling()", 100);

                        }
                    }
                    else {
                        var img = $("<img />");
                        imgDiv.replaceWith(img);
                        imgDiv = img;
                        imgDiv.attr('src', this.files.item(0).getAsDataURL());
                        imgDiv.css({ "vertical-align": "middle" });
                        setTimeout("autoScaling()", 100);
                    }
                }
            });
        }
    });
})(jQuery);
//页面部分:
//<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
//<html xmlns="http://www.w3.org/1999/xhtml">
//<head runat="server">
//    <title></title>
//    <meta content="" name="Keywords" />
//    <meta content="" name="Description" />
//    <meta content="text/html;charset=utf-8" http-equiv="Content-Type" />
//    <script type="text/javascript" src="js/jquery.pack.js"></script>
//    <script type="text/javascript" src="js/uploadPreview/jquery.uploadPreview.js"></script>
//    <script type="text/javascript">
//        $(document).ready(function() {
//            //建议在#imgDiv的父元素上加个overflow:hidden;的css样式
//            $("input").uploadPreview({ width: 200, height: 200, imgDiv: "#imgDiv", imgType: ["bmp", "gif", "png", "jpg"] });
//        });
//    </script>
//</head>
//<body>
//    <form id="form1" runat="server">
//    <input type="file" />
//    <br />
//    <div style="width: 200px; height: 200px; overflow: hidden; border: 1px solid red;">
//        <div id="imgDiv">
//        </div>
//    </div>
//    </form>
//</body>
//</html>

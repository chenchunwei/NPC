/* JavaScript Document 
2008年11月21日，修改 1+4=模式5
Example
----------------------
var pg = new showPages('pg');
pg.pageCount = 12; //定义总页数(必要)
pg.argName = 'p';  //定义参数名(可选,缺省为page)
pg.printHtml();    //显示


Supported in Internet Explorer, Mozilla Firefox
*/

function showPages(name) { //初始化属性
    this.name = name;      //对象名称
    this.page = 1;         //当前页数
    this.pageCount = 1;    //总页数
    this.argName = 'page'; //参数名
    this.showTimes = 1;    //打印次数
    this.count = 0;
    this.newPage = 1;   //判断查询后设置从第１页开始，2009/2/1
}

showPages.prototype.getPage = function () { //丛url获得当前页数,如果变量重复只获取最后一个
    var args = location.search;
    var reg = new RegExp('[\?&]?' + this.argName + '=([^&]*)[&$]?', 'gi');
    var chk = args.match(reg);
    this.page = RegExp.$1;
}
showPages.prototype.checkPages = function () { //进行当前页数和总页数的验证
    if (isNaN(parseInt(this.page))) this.page = 1;
    if (isNaN(parseInt(this.pageCount))) this.pageCount = 1;
    if (this.page < 1) this.page = 1;
    if (this.newPage == 1) this.page = 1;   //判断查询后设置从第１页开始，2009/2/1
    if (this.pageCount < 1) this.pageCount = 1;

    if (parseInt(this.page) > parseInt(this.pageCount))  //整数比较必须用 parseInt 函数转换为整数进行比较 2008/11/30 此Bug修正
    {
        this.page = this.pageCount;
    }

    this.page = parseInt(this.page);
    this.pageCount = parseInt(this.pageCount);
}
showPages.prototype.createHtml = function (mode) { //生成html代码
    var strHtml = '', prevPage = this.page - 1, nextPage = this.page + 1;
    if (mode == '' || typeof (mode) == 'undefined') mode = 0;
    switch (mode) {
        case 1:
            strHtml += '<span class="font">';
            if (prevPage < 1) {
                strHtml += '首页&nbsp;&nbsp;';
                strHtml += '|&nbsp;&nbsp;上一页&nbsp;&nbsp;';
            } else {
                strHtml += '<a href="javascript:' + this.name + '.toPage(1);">首页</a>&nbsp;&nbsp;';
                strHtml += '|&nbsp;&nbsp;<a href="javascript:' + this.name + '.toPage(' + prevPage + ');">上一页</a>&nbsp;&nbsp;';
            }
            if (nextPage > this.pageCount) {
                strHtml += '|&nbsp;&nbsp;下一页&nbsp;&nbsp;';
                strHtml += '|&nbsp;&nbsp;末页';
            } else {
                strHtml += '|&nbsp;&nbsp;<a href="javascript:' + this.name + '.toPage(' + nextPage + ');">下一页</a>&nbsp;&nbsp;';
                strHtml += '|&nbsp;&nbsp;<a href="javascript:' + this.name + '.toPage(' + this.pageCount + ');">末页</a>';
            }
            strHtml += '</span>';
            if (this.pageCount > 0) {
                var chkSelect;
                strHtml += '<select name="toPage" onchange="' + this.name + '.toPage(this);">';
                for (var i = 1; i <= this.pageCount; i++) {
                    if (this.page == i) chkSelect = ' selected="selected"';
                    else chkSelect = '';
                    strHtml += '<option value="' + i + '"' + chkSelect + '>' + i + ' / ' + this.pageCount + '</option>';
                }
            }
            strHtml += '</select>';
            break;
        default:
            strHtml = '未定义该模式' + mode;
            break;
    }
    return strHtml;
}
showPages.prototype.createUrl = function (page) { //生成页面跳转url
    if (isNaN(parseInt(page))) page = 1;
    if (page < 1) page = 1;
    if (page > this.pageCount) page = this.pageCount;
    var url = location.protocol + '//' + location.host + location.pathname;
    var args = location.search;
    var reg = new RegExp('([\?&]?)' + this.argName + '=[^&]*[&$]?', 'gi');
    args = args.replace(reg, '$1');
    if (args == '' || args == null) {
        args += '?' + this.argName + '=' + page;
    } else if (args.substr(args.length - 1, 1) == '?' || args.substr(args.length - 1, 1) == '&') {
        args += this.argName + '=' + page;
    } else {
        args += '&' + this.argName + '=' + page;
    }
    return url + args;
}
showPages.prototype.toPage = function (page) { //页面跳转
    var turnTo = 1;
    if (typeof (page) == 'object') {
        turnTo = page.options[page.selectedIndex].value;
    } else {
        turnTo = page;
    }
    self.location.href = this.createUrl(turnTo);
}
showPages.prototype.printHtml = function (mode) { //显示html代码
    this.getPage();
    this.checkPages();
    this.showTimes += 1;
    document.write('<div id="pages_' + this.name + '_' + this.showTimes + '"></div>');
    document.getElementById('pages_' + this.name + '_' + this.showTimes).innerHTML = this.createHtml(mode);

}
showPages.prototype.formatInputPage = function (e) { //限定输入页数格式
    var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false;
    if (!ie) var key = e.which;
    else var key = event.keyCode;
    if (key == 8 || key == 46 || (key >= 48 && key <= 57)) return true;
    return false;
}


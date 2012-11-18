/**
* 扩展树表格级联勾选方法：
* @param {Object} container
* @param {Object} options
* @return {TypeName}
*/
$.extend($.fn.treegrid.methods, {
    /**
    * 级联选择
    * @param {Object} target
    * @param {Object} param
    *      param包括两个参数:
    *          id:勾选的节点ID
    *          deepCascade:是否深度级联
    * @return {TypeName}
    */
    cascadeCheck: function (target, param) {
        var opts = $.data(target[0], "treegrid").options;
        if (opts.singleSelect)
            return;
        var idField = opts.idField; //这里的idField其实就是API里方法的id参数
        var status = false; //用来标记当前节点的状态，true:勾选，false:未勾选
        var selectNodes = $(target).treegrid('getSelections'); //获取当前选中项
        for (var i = 0; i < selectNodes.length; i++) {
            if (selectNodes[i][idField] == param.id)
                status = true;
        }
        //级联选择父节点
        selectParent(target[0], param.id, idField, status);
        selectChildren(target[0], param.id, idField, param.deepCascade, status);
        /**
        * 级联选择父节点
        * @param {Object} target
        * @param {Object} id 节点ID
        * @param {Object} status 节点状态，true:勾选，false:未勾选
        * @return {TypeName}
        */
        function selectParent(targetArg, id, idFieldArg, statusArg) {
            var parent = $(targetArg).treegrid('getParent', id);
            if (parent) {
                var parentId = parent[idFieldArg];
                if (statusArg)
                    $(targetArg).treegrid('select', parentId);
                else
                    $(targetArg).treegrid('unselect', parentId);
                selectParent(targetArg, parentId, idFieldArg, statusArg);
            }
        }

        /**
        * 级联选择子节点
        * @param {Object} target
        * @param {Object} id 节点ID
        * @param {Object} deepCascade 是否深度级联
        * @param {Object} status 节点状态，true:勾选，false:未勾选
        * @return {TypeName}
        */
        function selectChildren(targetArg, id, idFieldArg, deepCascade, statusArg) {
            //深度级联时先展开节点
            if (!statusArg && deepCascade)
                $(targetArg).treegrid('expand', id);
            //根据ID获取下层孩子节点
            var children = $(targetArg).treegrid('getChildren', id);
            for (var i = 0; i < children.length; i++) {
                var childId = children[i][idFieldArg];
                if (statusArg)
                    $(targetArg).treegrid('select', childId);
                else
                    $(targetArg).treegrid('unselect', childId);
                selectChildren(statusArg, childId, idFieldArg, deepCascade, statusArg); //递归选择子节点
            }
        }
    }
});

/**
* 扩展树表格级联选择（点击checkbox才生效）：
*      自定义两个属性：
*      cascadeCheck ：普通级联（不包括未加载的子节点）
*      deepCascadeCheck ：深度级联（包括未加载的子节点）
*/
$.extend($.fn.treegrid.defaults, {
    onLoadSuccess: function (data) {
        var target = $(this);
        var opts = $.data(this, "treegrid").options;
        var panel = $(this).datagrid("getPanel");
        var gridBody = panel.find("div.datagrid-body");
        var idField = opts.idField; //这里的idField其实就是API里方法的id参数
        var chkInterval = setInterval(handleChlidrenChk, 200);
        function handleChlidrenChk() {
            clearInterval(chkInterval);
            if (opts.singleSelect) return; //单选不管
            if (opts.cascadeCheck || opts.deepCascadeCheck) {
                var trChk = getTrChk(data[idField]);
                if (trChk.attr("checked") == "checked") {
                    for (var i = 0; i < data.children.length; i++) {
                        target.treegrid('select', data.children[i][idField]);
                    }
                }
            }
        }
        function getTrChk(id) {
            return gridBody.find("tr[node-id=" + id + "]").find("div.datagrid-cell-check input[type=checkbox]");
        }
        gridBody.find("div.datagrid-cell-check input[type=checkbox],tr.datagrid-row").unbind(".treegrid").click(handerWrapper);
        function handerWrapper(e) {
            if (opts.singleSelect) return; //单选不管
            var tagname = $(this)[0].tagName;
            if ($(e.target).hasClass("tree-hit"))
                return;
            var isChk = tagname == "input" || tagname == "INPUT";
            if (opts.cascadeCheck || opts.deepCascadeCheck) {
                var id = $(this).parent().parent().parent().attr("node-id");
                if (id == undefined) {
                    id = $(this).attr("node-id");
                }
                var status = false;
                if (isChk) {
                    if ($(this).attr("checked")) status = true;
                } else {
                    if (getTrChk(id).attr("checked") != "checked") {
                        status = true;
                    }
                }
                hander(id, status, isChk);
            }
            if (isChk)
                e.stopPropagation(); //停止事件传播
        }
        function hander(id, status, isChk) {
            //级联选择父节点
            selectParent(target, id, idField, status, isChk);
            selectChildren(target, id, idField, opts.deepCascadeCheck, status);
            /**
            * 级联选择父节点
            * @param {Object} target
            * @param {Object} id 节点ID
            * @param {Object} status 节点状态，true:勾选，false:未勾选
            * @return {TypeName}
            */
            function selectParent(targetArg, idArg, idFieldArg, statusArg, isChkArg) {
                var parent = targetArg.treegrid('getParent', idArg);
                if (parent) {
                    var parentId = parent[idFieldArg];
                    if (statusArg) {
                        var children = targetArg.treegrid('getChildren', parentId);
                        //var children = targetArg.treegrid('getChildren', parentId);
                        for (var i = 0; i < children.length; i++) {
                            var trChk = gridBody.find("tr[node-id=" + children[i][idFieldArg] + "]").find("div.datagrid-cell-check input[type=checkbox]");
                            if (id == children[i][idFieldArg] && trChk.attr("checked") == undefined && !isChkArg) {
                                continue;
                            }
                            if (!trChk.attr("checked")) {
                                return;
                            }
                        }
                        targetArg.treegrid('select', parentId);
                    }
                    else
                        targetArg.treegrid('unselect', parentId);
                    selectParent(targetArg, parentId, idFieldArg, statusArg, isChkArg);
                }
            }
            /**
            * 级联选择子节点
            * @param {Object} target
            * @param {Object} id 节点ID
            * @param {Object} deepCascade 是否深度级联
            * @param {Object} status 节点状态，true:勾选，false:未勾选
            * @return {TypeName}
            */
            function selectChildren(targetArg, idArg, idFieldArg, deepCascade, statusArg, isChkArg) {
                //深度级联时先展开节点
                if (statusArg && deepCascade)
                    targetArg.treegrid('expand', idArg);
                //根据ID获取下层孩子节点
                var children = targetArg.treegrid('getChildren', idArg);
                for (var i = 0; i < children.length; i++) {
                    var childId = children[i][idFieldArg];
                    if (statusArg)
                        targetArg.treegrid('select', childId);
                    else
                        targetArg.treegrid('unselect', childId);
                    selectChildren(targetArg, childId, idFieldArg, deepCascade, statusArg); //递归选择子节点
                }
            }
        }
    }
});

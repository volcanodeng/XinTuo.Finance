
var valObj = {
    Category: undefined,        //目录id
    EditIndex: undefined,       //grid的编辑行索引
    CurTableId: undefined,      //当前Tab的Grid ID   
    data:[]                     //列表绑定的数据（数据总和）
}


function SearchSubject(cid)
{
    valObj.Category = cid;
    valObj.CurTableId = "#t" + valObj.Category;
    if ($(valObj.CurTableId).datagrid("getRows") == 0) {
        $.post("/api/s/GetSubjectsByCategory",
            {
                SubjectCategory: valObj.Category
            },
            function (data) {
                $(valObj.CurTableId).datagrid("loadData", data);
                $.merge(valObj.data, data);
            }
            );
    }
}

function onTabSelected(title,index)
{
    var tab = $('#categoryTab').tabs('getSelected').panel('options');
    if (tab != null) {
        SearchSubject(tab.id.replace(/c/gi,""));
    }
}

function append(cid)
{
    $("#subjectDetail").dialog("open");
}

function edit(cid) {
    $("#subjectDetail").dialog("open");
}

function onGridSelect(index,row)
{
    var pSubject = $.grep(valObj.data, function (n, i) {return n.subjectCode == row.parentSubjectCode });

    $("#sCode").textbox("setValue", row.subjectCode); $("#sCode").textbox("disable");
    $("#sName").textbox("setValue", row.subjectName);
    if (row.parentSubjectCode || row.parentSubjectCode == undefined)
        $("#pSCode").textbox("setValue", "无上级科目");
    else
        $("#pSCode").textbox("setValue", row.parentSubjectCode + " " + (pSubject.length > 0 ? pSubject[0].subjectName : ""));
    $("#pSCode").textbox("disable");

    $("#sCategory").combobox("setValue", row.subjectCategory); $("#sCategory").combobox("disable");
    $("input:radio[name='dir'][value='" + row.balanceDirection + "']").attr('checked', true); $("input:radio[name='dir']").attr("disabled", true);
    
}

function saveSubject()
{
    $.post("/api/s/SaveSubject",
        { 
            SubjectCode: $("#sCode").textbox("getValue"),
            SubjectName: $("#sName").textbox("getValue"),
            ParentSubjectCode: $("#pSCodeHidden").val(),
            BalanceDirection: $("#subjectDetail :radio:checked").val()
        },
        function (data) { });
}

/*
function endEditing() {
    if (valObj.EditIndex == undefined) { return true }
    if ($(valObj.CurTableId).datagrid('validateRow', valObj.EditIndex)) {
        var ed = $(valObj.CurTableId).datagrid('getEditor', { index: valObj.EditIndex, field: 'subjectCategory' });
        var categoryName = $(ed.target).combobox('getText');
        $(valObj.CurTableId).datagrid('getRows')[valObj.EditIndex]['category'].categoryFullName = categoryName;
        $(valObj.CurTableId).datagrid('endEdit', valObj.EditIndex);
        valObj.EditIndex = undefined;
        return true;
    } else {
        return false;
    }
}
function onClickCell(index, field) {
    if (valObj.EditIndex != index) {
        if (endEditing()) {
            $(valObj.CurTableId).datagrid('selectRow', index).datagrid('beginEdit', index);
            var ed = $(valObj.CurTableId).datagrid('getEditor', { index: index, field: field });
            if (ed) {
                ($(ed.target).data('textbox') ? $(ed.target).textbox('textbox') : $(ed.target)).focus();
            }
            valObj.EditIndex = index;
        } else {
            setTimeout(function () {
                $(valObj.CurTableId).datagrid('selectRow', valObj.EditIndex);
            }, 0);
        }
    }
}

*/

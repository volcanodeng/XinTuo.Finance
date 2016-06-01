
var valObj = {
    Category: undefined,        //目录id
    EditIndex: undefined,       //grid的编辑行索引
    CurTableId: undefined      //当前Tab的Grid ID   
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


function endEditing() {
    if (valObj.EditIndex == undefined) { return true }
    if ($(valObj.CurTableId).datagrid('validateRow', valObj.EditIndex)) {
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
            $(valObj.CurTableId).datagrid('selectRow', index)
                    .datagrid('beginEdit', index);
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
function onEndEdit(index, row) {
    var ed = $(this).datagrid('getEditor', {
        index: index,
        field: 'subjectCode'
    });
    row.category.categoryFullName = $(ed.target).combobox('getText');
}


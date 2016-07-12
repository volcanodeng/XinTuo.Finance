
$(function () {
    initData();
    loadVoucher();
    
});

function initData()
{
    $("#cw").combobox("setValue", "记");

    onDateChange($("#dd").datebox("getValue"));
}


function loadVoucher()
{
    $.get('/api/v/GetComVouchers', function (data) {
        if (data && data.length > 0) {
            $("#voucherTab").datagrid("loadData", data[data.length - 1].voucherDetails);
            $("#voucherTab").datagrid("enableCellEditing");
        }
    });
}


function debiteFormatter(value, row, index) {
    if (value.toString().indexOf(".") == -1)
        value = value * 100;
    else
        value = value.replace(/./gi, "");

    return '<div class="col_debite"><div class="cell_val debit_val">' + value + '</div></div>';
}
function creditFormatter(value, row, index) {
    if (value.toString().indexOf(".") == -1)
        value = value * 100;
    else
        value = value.replace(/./gi, "");

    return '<div class="col_credit"><div class="cell_val credit_val">' + value + '</div></div>';
}
function scFormatter(value, row, index)
{
    return value + " " + row.subjectName;
}
function absClickButton()
{
    $("#abstractsWin").dialog("open");
}
function absClose()
{
    $("#abstractsWin").dialog("close");
}
function onDateChange(newValue, oldValue)
{
    var dArr = newValue.split('-');
    if (dArr.length == 3) {
        var m = new Number(dArr[1]);
        $("#vch_year").text(dArr[0]);
        $("#vch_period").text(m);
    }
}
function datagridEndEdit()
{
    var dg = $("#voucherTab");
    var editInd = dg.datagrid('options').editIndex;
    if (editInd == undefined) return true;

    if (dg.datagrid('validateRow', editInd)) {
        if (typeof (fieldSettingFun) == "function") fieldSettingFun(editInd);

        dg.datagrid('endEdit', editInd);
        dg.datagrid('options').editIndex = undefined;

        return true;
    }
    else {
        return false;
    }
}

function saveVoucher()
{
    if (datagridEndEdit()) {
        var voucherDetails = [];
        var changeRows = $("#voucherTab").datagrid("getChanges");
        if (changeRows.length == 0)
        {
            $.messager.show({ title: '保存', msg: '没有修改内容' });
            return;
        }

        $.post("/api/v/SaveVoucher",
            {
                VId: 1,
                CertWord: $("#cw").combobox("getValue"),
                CertWordSn: $("#ss").numberspinner('getValue'),
                AttachedInvoices: $("#attCw").numberbox('getValue'),
                VoucherTime: $("#dd").datebox("getValue"),
                VoucherDetails: changeRows
                    //[
                    //{
                    //    VId: 1,
                    //    Abstracts: '测试凭证',
                    //    SubjectCode: 1002,
                    //    Debit: 30,
                    //    Credit: 0,
                    //    Quantity: 0
                    //},
                    //{
                    //    VId: 1,
                    //    Abstracts: '测试1',
                    //    SubjectCode: 1404,
                    //    Debit: 0,
                    //    Credit: 30,
                    //    Quantity: 0
                    //}
                    //]
            },
            function (data) {
                if (data >= 1) {
                    $.messager.show({ title: '保存', msg: '保存操作成功完成' });
                }
                else {
                    $.messager.alert('保存', '保存失败，请联系管理员处理', 'warning');
                }
            });
    }
}
function addVoucher()
{
    if(datagridEndEdit())
    {
        var dg = $("#voucherTab");
        dg.datagrid('appendRow', { abstracts: '', subjectCode: '', subjectName: '', debit: 0, credit: 0 });
        
    }
}

function fieldSettingFun(editIndex)
{
    var ed = $('#voucherTab').datagrid('getEditor', { index: editIndex, field: 'subjectCode' });
    if (!ed) return;

    var subjectName = $(ed.target).combotree('getText');
    var sNames = subjectName.split(" ");
    if (sNames.length > 1) $('#voucherTab').datagrid('getRows')[editIndex]['subjectName'] = sNames[1];
}
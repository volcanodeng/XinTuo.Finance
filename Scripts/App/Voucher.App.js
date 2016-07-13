
//全局定义
var globVar = {
    voucher: [],             //凭证数据集合
    voucherIndex: -1         //当前凭证实例在集合中的索引
};

//======初始化=======
$(function () {
    loadVoucher();
});

function loadVoucher()
{
    $.get('/api/v/GetComVouchers', function (data) {
        globVar.voucher = data;
        if (data && data.length > 0) {
            globVar.voucherIndex = data.length - 1;
            binding(data[globVar.voucherIndex]);

            $("#voucherTab").datagrid("loadData", data[globVar.voucherIndex].voucherDetails);
            $("#voucherTab").datagrid("enableCellEditing");
        }
    });
}

function binding(voucher)
{
    $("#cw").combobox("setValue", voucher.certWord);
    $("#ss").numberspinner("setValue", voucher.certWordSn);
    $("#dd").datebox("setValue", voucher.voucherTime.substr(0, voucher.voucherTime.indexOf("T")));
    onDateChange($("#dd").datebox("getValue"));

    $("#attCw").numberbox("setValue", voucher.attachedInvoices);
}

//=========初始化=======

//=========事件响应=====
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
function prevClick()
{
    if (globVar.voucherIndex == 0) { $.messager.alert("前一项", "已到达第一项", "info"); return; }

    if (globVar.voucherIndex > 0) globVar.voucherIndex--;
    binding(globVar.voucher[globVar.voucherIndex]);
}
function nextClick() {

    if (globVar.voucherIndex == globVar.voucher.length - 1) { $.messager.alert("下一项", "已到达最后一项", "info");  }

    if (globVar.voucherIndex < globVar.voucher.length-1) globVar.voucherIndex++;
    binding(globVar.voucher[globVar.voucherIndex]);
}
//=========事件响应=====
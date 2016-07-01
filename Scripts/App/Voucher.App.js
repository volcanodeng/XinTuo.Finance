﻿
$(function () {

    loadVoucher();
    
});


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
function absClickButton()
{
    $("#abstractsWin").dialog("open");
}
function absClose()
{
    $("#abstractsWin").dialog("close");
}


function saveVoucher()
{
    $.post("/api/v/SaveVoucher",
        {
            VId:1,
            CertWord: $("#cw").combobox("getValue"),
            CertWordSn: $("#ss").numberspinner('getValue'),
            AttachedInvoices: $("#attCw").numberbox('getValue'),
            VoucherTime:$("#dd").datebox("getValue"),
            VoucherDetails: [
                {
                    VId: 1,
                    Abstracts: '测试凭证',
                    SubjectCode: 1002,
                    Debit: 30,
                    Credit:0,
                    Quantity:0
                },
                {
                    VId: 1,
                    Abstracts: '测试1',
                    SubjectCode: 1404,
                    Debit: 0,
                    Credit: 30,
                    Quantity: 0
                }
            ]
        },
        function (data) {
            if(data>=1)
            {
                $.messager.show({title:'保存',msg:'保存操作成功完成'});
            }
            else
            {
                $.messager.alert('保存','保存失败，请联系管理员处理','warning');
            }
        });
}


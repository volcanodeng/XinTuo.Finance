function onProvinceSelected(newValue, oldValue) {
    $("#county").combobox("clear");
    $("#city").combobox({
        url: '/api/c/GetRegion/' + newValue, valueField: 'cityId', textField: 'cityName', onChange: onCitySelected
    });
}

function onCitySelected(newValue, oldValue) {
    $("#county").combobox({
        url: '/api/c/GetRegion/' + newValue, valueField: 'regionId', textField: 'countyName'
    });
}


function onCompanySave()
{
    $.post("/api/c/Save",
        {
            CompanyId:$("#CompanyId").val(),
            ComFullName: $("#ComFullName").textbox("getText"),
            ComShortName: $("#ComShortName").textbox("getText"),
            RegionId: $("#county").combobox("getValue"),
            ComAddress: $("#ComAddress").textbox("getText"),
            ComTel: $("#ComTel").textbox("getText"),
            ContactsName: $("#ContactsName").textbox("getText"),
            ContactsMobile: $("#ContactsMobile").textbox("getText"),
            ContactsEmail: $("#ContactsEmail").textbox("getText")
        },
        function (data) {
            $.messager.show(
                {
                    title: '保存',
                    msg: '公司信息已成功保存.'
                }
                );
        }
    );
}
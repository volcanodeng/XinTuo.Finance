
function SearchSubject(cid)
{
    if ($("#t" + cid).datagrid("getRows") == 0) {
        $.post("/api/s/GetSubjectsByCategory",
            {
                SubjectCategory: cid
            },
            function (data) {
                $("#t" + cid).datagrid("loadData", data);
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

function SearchSubject(cid)
{
    $.post("/api/s/GetSubjectsByCategory",
        {
            cid: cid
        },
        function (data) {
            $("")
        }
        );
}
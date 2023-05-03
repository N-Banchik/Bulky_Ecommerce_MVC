$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) { loadDataTable("inprocess"); }
    else if (url.includes("pending")) { loadDataTable("pending"); }
    else if (url.includes("completed")) { loadDataTable("completed"); }
    else if (url.includes("approved")) { loadDataTable("approved"); }
    else { loadDataTable("all"); }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: `/admin/Order/getall?status=${status}`},
        "columns": [
            { data: 'id' },
            { data: 'name' },
            { data: 'phoneNumber' },
            { data: 'user.email' },
            { data: 'orderStatus' },
            { data: 'orderTotal' },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
    <a href="/admin/order/details?id=${data}" class="btn shadow btn-primary mx-2"><i class="bi bi-pencil-fill"></i>Details</a>
    </div>`;
                }
            }]
    });
}

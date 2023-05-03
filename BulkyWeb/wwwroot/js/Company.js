$(document).ready(function
    () { loadDataTable(); });

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: 'name' },
            { data: 'phoneNumber' },
            { data: 'streetAddress' },
            { data: 'city' },
            { data: 'country' },
            { data: 'zipcode' },
            
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
    <a href="/admin/company/upsertCompany?id=${data}" class="btn shadow btn-primary mx-2"><i class="bi bi-pencil-fill"></i>Edit</a>
    <a href="/admin/company/deleteCompany?id=${data}" class="btn shadow btn-danger mx-2"><i class="bi bi-trash3-fill"></i>Delete</a></div>`;
                }
            }]
    });
}

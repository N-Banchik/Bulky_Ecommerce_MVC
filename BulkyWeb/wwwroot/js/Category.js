$(document).ready(function
    () { loadDataTable(); });

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/category/getall' },
        "order": [[1,'asc']],
        "columns": [
            { data: 'name' },
            { data: 'displayOrder' },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
    <a href="/admin/category/createCategory?id=${data}" class="btn shadow btn-primary mx-2"><i class="bi bi-pencil-fill"></i>Edit</a>
    <a href="/admin/category/deleteCategory?id=${data}" class="btn shadow btn-danger mx-2"><i class="bi bi-trash3-fill"></i>Delete</a></div>`;
                }
            }]
    });
}

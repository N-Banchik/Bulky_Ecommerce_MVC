$(document).ready(function
    () { loadDataTable(); });

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'imageUrl' },
            { data: 'title' },
            { data: 'description' },
            { data: 'isbn' },
            { data: 'author' },
            { data: 'listPrice' },
            { data: 'price' },
            { data: 'category.name' },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
    <a href="/admin/product/upsertProduct?id=${data}" class="btn shadow btn-primary mx-2"><i class="bi bi-pencil-fill"></i>Edit</a>
    <a href="/admin/product/deleteProduct?id=${data}" class="btn shadow btn-danger mx-2"><i class="bi bi-trash3-fill"></i>Delete</a></div>`;
                }}]
    });
}

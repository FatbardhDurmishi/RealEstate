var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#properties-list").DataTable({
        pageLength: 15,
        responsive: true,
        autoWidth: false,
        order: [[0, "desc"]],
        //"processing": true,
        //"serverSide": true,
        //"filter": true,
        //"orderMulti": false,
        ajax: {
            url: '/Company/Properties/GetPropertiesJson',
            dataSrc: ""
        },
        columns: [
            { data: "id", title: "Id" },
            { data: "name", title: "Name" },
            { data: "user.name", title: "Agent", },
            { data: "user.email", title: "Agent Email" },
            { data: "propertyTypeNavigation.name", title: "Property Type" },
            { data: "transactionTypeNavigation.name", title: "Transaction Type" },
            {
                data: "status", title: "Status"
                //data: "status", title: "Email Confirmed", sClass: "text-center", render: function (a, b, data, d) {
                //    if (data.emailConfirmed == "Free") {
                //        return '<i class="fa fa-check text-success"></i>';
                //    }
                //    else {
                //        return '<i class="fa fa-check text-danger"></i>';
                //    }
                //}
            },
            {
                data: "price", title: "Price"
            },
            {
                data: "id", title: "Actions",
                render: function (data) {
                    return `
                          <div class="w-75 btn-group align-items-center" role="group">
                            <a href="/Company/Properties/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                            <a onClick=Delete('/Company/Properties/Delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i></a>
                            <a href="/Company/Properties/Details?propertyId=${data}" class="btn btn-secondary mx-2"><i class="bi bi-eye"></i></a>
                        </div>
                            `
                }
            },

        ]
    });

}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
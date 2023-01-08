
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#transactions-list").DataTable({
        pageLength: 15,
        responsive: true,
        autoWidth: false,
        order: [[0, "desc"]],
        //"processing": true,
        //"serverSide": true,
        //"filter": true,
        //"orderMulti": false,
        ajax: {
            url: '/Company/Transaction/GetTransactions',
            dataSrc: ""
        },
        columns: [
            { data: "id", title: "Id", "width": "5%" },
            { data: "owner.name", title: "Seller" },
            { data: "buyer.name", title: "Buyer", },
            { data: "rentPrice", title: "Rent Price" },
            { data: "property.name", title: "Property Name" },
            { data: "transactionTypeNavigation.name", title: "Transaction Type", "width": "15%" },
            { data: "totalPrice", title: "Total Price" },
            { data: "status", title: "Status" },
            {
                data: { id: "id", showButtons: "showButtons", title: "Actions" },

                render: function (data) {
                    if (data.showButtons == true) {
                        return `
                          <div class="w-75 btn-group align-items-center" role="group">
                            <a href="/Company/Transaction/ApproveRequest?id=${data.id}" class="btn btn-primary mx-2"><i class="fa-solid fa-check"></i></a>
                            <a href="/Company/Transaction/RejectRequest?id=${data.id}" class="btn btn-danger mx-2"><i class="fa-solid fa-xmark"></i></a>
                            <a href="/Company/Transaction/Details?id=${data.id}" class="btn btn-secondary mx-2"><i class="fa-solid fa-circle-info"></i></i></a>
                            <a onClick=Delete('/Company/Transaction/Delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i></a>
                        </div>
                            `
                    } else {
                        return ` 
                        <div class="w-50 btn-group align-items-center" role="group">
                            <a href="/Company/Transaction/Details?id=${data.id}" class="btn btn-secondary mx-2"><i class="fa-solid fa-circle-info"></i></a>
                        </div>`;
                    }
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
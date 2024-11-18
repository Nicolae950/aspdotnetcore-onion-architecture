
var accId = window.location.href.split("/")[5];

$(document).ready(function () {
    $('#transactionTable').DataTable({
        processing: true,
        serverSide: true,
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            },
            topStart: null
        },
        searching: false,
        ajax: {
            url: "/Transaction/PaginatedIndex/" + accId,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { "data": "id", name: "id" },
            { "data": "operationType", name: "operation" },
            { "data": "amount", name: "amount" },
            { "data": "destinationFirstName", name: "firstname" },
            { "data": "destinationLastName", name: "lastname" },
            {
                data: "id",
                orderable: false,
                searchable: false,
                render: function (data) {
                    return "<a href='/Transaction/Details/" + accId + "/" + data + "' class='btn btn-outline-secondary'>Details</a>"
                }
            }
        ],
        columnDefs: [
            {
                targets: "operation-type",
                render: function (data) {
                    if (data == 0) {
                        return "Deposit";
                    }
                    if (data == 1) {
                        return "Withdraw";
                    }
                    if (data == 2) {
                        return "Transfer";
                    }
                }
            },
            {
                targets: "amount-currency",
                render: function (data) {
                    return data + " $";
                }
            }
        ]
    });
});
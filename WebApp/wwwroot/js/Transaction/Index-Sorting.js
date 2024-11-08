
var originUrl = document.getElementById("origin-url").href;

function setSortingClass() {
    if (location.href.includes("OrderBy=operation")) {
        document.getElementById("operation-sort").classList.add("sorting_asc");
    }
    if (location.href.includes("OrderByDescending=operation")) {
        document.getElementById("operation-sort").classList.add("sorting_desc");
    }
    if (location.href.includes("OrderBy=amount")) {
        document.getElementById("amount-sort").classList.add("sorting_asc");
    }
    if (location.href.includes("OrderByDescending=amount")) {
        document.getElementById("amount-sort").classList.add("sorting_desc");
    }
}

document.getElementById("operation-sort").addEventListener('click', function (event) {
    event.preventDefault();
    if (window.location.href == originUrl + "&OrderBy=operation") {
        window.location.href = originUrl + "&OrderByDescending=operation";
    } else {
        window.location.href = originUrl + "&OrderBy=operation";
    }
});

document.getElementById("amount-sort").addEventListener('click', function (event) {
    event.preventDefault();
    if (window.location.href == originUrl + "&OrderBy=amount") {
        window.location.href = originUrl + "&OrderByDescending=amount";
    } else {
        window.location.href = originUrl + "&OrderBy=amount";
    }
});

$(function () {
    $('#transactionTable').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});
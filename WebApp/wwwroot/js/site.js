var fromDatePicker = document.getElementById("from-report-date");
var toDatePicker = document.getElementById("to-report-date");
var reportActionLink = document.getElementById("report-link");
var reportButton = document.getElementById("report-button");

var hrefOriginalString = reportActionLink.getAttribute("href");

reportActionLink.onload = function () {
    reportActionLink.setAttribute("href", hrefOriginalString + "?From=" + fromDatePicker.value + "&To=" + toDatePicker.value);
}

fromDatePicker.onchange = function () {
    reportActionLink.setAttribute("href", hrefOriginalString + "?From=" + fromDatePicker.value + "&To=" + toDatePicker.value);
}

toDatePicker.onchange = function () {
    reportActionLink.setAttribute("href", hrefOriginalString + "?From=" + fromDatePicker.value + "&To=" + toDatePicker.value);
}

var rejectButton = document.getElementById("update-reject-button");
var acceptButton = document.getElementById("update-accept-button");
var stateOfTransaction = document.getElementById("state-of-transaction");

rejectButton.click = function () {
    stateOfTransaction.value = "Rejected";
}

acceptButton.click = function () {
    stateOfTransaction.value = "Done";
}

$('.toastsDefaultSuccess').onload(function () {
    $(document).Toasts('create', {
        class: 'bg-success',
        title: 'Toast Title',
        subtitle: 'Subtitle',
        body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
    })
});

$('.toastsDefaultWarning').onload(function () {
    $(document).Toasts('create', {
        class: 'bg-warning',
        title: 'Toast Title',
        subtitle: 'Subtitle',
        body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
    })
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
var rejectButton = document.getElementById("update-reject-button");
var acceptButton = document.getElementById("update-accept-button");
var stateOfTransaction = document.getElementById("state-of-transaction");

//rejectButton.click = function () {
//    stateOfTransaction.value = "Rejected";
//}

//acceptButton.click = function () {
//    stateOfTransaction.value = "Done";
//}

document.getElementById("update-reject-button").addEventListener('click', function () {
    stateOfTransaction.value = "Rejected";
});

document.getElementById("update-accept-button").addEventListener('click', function () {
    stateOfTransaction.value = "Done";
});

//$('.toastsDefaultSuccess').onload(function () {
//    $(document).Toasts('create', {
//        class: 'bg-success',
//        title: 'Toast Title',
//        subtitle: 'Subtitle',
//        body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//    })
//});

//$('.toastsDefaultWarning').onload(function () {
//    $(document).Toasts('create', {
//        class: 'bg-warning',
//        title: 'Toast Title',
//        subtitle: 'Subtitle',
//        body: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
//    })
//});

function showToast() {
    var x = document.getElementById("snackbar");
    x.hidden = false;
    x.className = "show";

    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 3000);
}

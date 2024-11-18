var rejectButton = document.getElementById("update-reject-button");
var acceptButton = document.getElementById("update-accept-button");
var stateOfTransaction = document.getElementById("state-of-transaction");

document.getElementById("update-reject-button").addEventListener('click', function () {
    stateOfTransaction.value = "Rejected";
});

document.getElementById("update-accept-button").addEventListener('click', function () {
    stateOfTransaction.value = "Done";
});

function showToast() {
    var x = document.getElementById("snackbar");
    x.hidden = false;
    x.className = "show";

    setTimeout(function () {
        x.className = x.className.replace("show", "");
    }, 3000);
}

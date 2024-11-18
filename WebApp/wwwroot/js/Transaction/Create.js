var select = document.getElementById("operation-select");
var destinationInput = document.getElementById("destination-id-input");

document.getElementById("operation-select").addEventListener('click', function () {
    var selectedValue = select.value;
    if (selectedValue == "Transfer") {
        destinationInput.placeholder = "00000000-0000-0000-0000-000000000000";
        destinationInput.disabled = false;
    } else {
        destinationInput.placeholder = "For Transfer";
        destinationInput.disabled = true;
    }
})
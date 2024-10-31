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
 
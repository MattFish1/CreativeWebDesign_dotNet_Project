function getTime() {
    var day = new Date();
    return day.getTime();
}

var siteStartTime = getTime();
var siteStopTime;

$(function () {

    $("#proceed").on("click", function () {
        var timeOnPage = ((getTime() - siteStartTime) / 1000) / 60;
        $("#timeOnPage").attr("value", timeOnPage);
        $("#demographicsForm").submit();

    });
});

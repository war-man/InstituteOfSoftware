
function dateConvert (datetime) {

    var date = new Date(datetime);

    var year = date.getFullYear();

    var mouth = date.getMonth() + 1;

    var day = date.getDate();

    var obj = {
        year = year,
        month = mouth,
        day = day
    };

    return obj;

}
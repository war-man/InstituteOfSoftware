//转换时间的方法
function TimeChange(newtime) {
    if (newtime == null)
        return "";
    var date = new Date(parseInt(newtime.slice(6)));
    var year = date.getFullYear();
    var month = date.getMonth();
    if ((month + 1) < 10) {
        month = "0" + Number(month + 1);
    } else {
        month = Number(month + 1);
    }
    var day = date.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var result = year + '-' + month + '-' + day;
    return result;
}
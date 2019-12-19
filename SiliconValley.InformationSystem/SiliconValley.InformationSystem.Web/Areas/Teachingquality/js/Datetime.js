
////系统时间
var curDate = new Date();
var curYearMonthrDay = curDate.getFullYear() + "-" + MoDay(parseInt(curDate.getMonth()) + 1) + "-" + MoDay(parseInt(curDate.getDate()));



//月份小于10的前面加个0
function MoDay(a) {
    if (a < 10) {
        return "0" + a;
    }
    return a;
}
//时间转换方法
function TimeChange(newtime) {
    if (newtime == null)
        return "";
    var date = new Date(parseInt(newtime.slice(6)));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    if (month < 10) {
        month = "0" + parseInt(month);
    }
    var day = date.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var result = year + '-' + month + '-' + day;
    return result;
}
//转换成：2016 - 07 - 11 15: 00: 28

function getFDate(date) {
    var d = eval('new ' + date.substr(1, date.length - 2));

    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    var ar_time = [d.getHours(), d.getMinutes(), d.getSeconds()];

    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    for (var i = 0; i < ar_time.length; i++) ar_time[i] = dFormat(ar_time[i]);

    return ar_date.join('-') + ' ' + ar_time.join(':');
}
function dFormat(i) {
    return i < 10 ? "0" + i.toString() : i;
}

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
    var month = date.getMonth();
    if (month < 10) {
        month = "0" + (parseInt(month) + 1);
    }
    var day = date.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var result = year + '-' + month + '-' + day;
    return result;
}
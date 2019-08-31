
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
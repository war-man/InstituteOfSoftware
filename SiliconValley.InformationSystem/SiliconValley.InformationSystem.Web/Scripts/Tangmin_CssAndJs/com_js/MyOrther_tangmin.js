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
//判断星期几
function judgeDate(date) {
    // 标准时间 Wed Jul 31 2019 00:00:00 GMT+0800 (中国标准时间)
    var _date = new Date(date);
    // getDay() 返回表示星期的某一天
    var num = _date.getDay(_date);
    var week;
    switch (num) {
        case 0:
            week = "周日";
            break;
        case 1:
            week = "周一";
            break;
        case 2:
            week = "周二"
            break;
        case 3:
            week = "周三"
            break;
        case 4:
            week = "周四"
            break;
        case 5:
            week = "周五"
            break;
        case 6:
            week = "周六"
            break;
        default:
            break;
    };
    return week;
}
//关闭弹出层
function MycloseEject(layer,index) {
    parent.layer.close(index); //再执行关闭   
}
//转换时间的方法包含时分秒
function formatTen(num) {
    return num > 9 ? (num + "") : ("0" + num);
}
function formatDate(dates) {
    var date = new Date(parseInt(dates.slice(6)));
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();
    return year + "-" + formatTen(month) + "-" + formatTen(day) + " " + formatTen(hour) + ":" + formatTen(minute) + ":" + formatTen(second);
}

loadxml();
var $xmlDoc;
//获取全部的xml节点
function loadxml() {
    $.ajax({
        url: '../../../xmlfile/RoomdeWithPage.xml',
        type: 'GET',
        async: false,//因为这个请求有点慢，所以就同步，后面有数据要操作这个返回的值对象
        dataType: 'xml',
        success: function (xmlDoc, textStatus) {
            $xmlDoc=xmlDoc;
        }
    });
}

//性别数据对象
function $getsex(sex) {
    var sextype = new Object();
    $($xmlDoc).find(sex).each(function (i) {
        sextype.val = $(this).attr("val");
        sextype.icon = $(this).children("icon").text();
        sextype.Type = $(this).children("Type").text();
    });
    return sextype;
}

//获取字体样式
function $getfonticon() {
    var fontstylearr = new Array();
    $($xmlDoc).find("iconfontstyle").each(function (i) {
        var obj = new Object();
        obj.val = $(this).attr("val");
        obj.value = $(this).children("value").text();
        obj.id = $(this).children("id").text();
        fontstylearr.push(obj);
    });
    return fontstylearr;
}

//房间类型
function $roomstaytype(type) {
    var roomstaytype = new Object();
    $($xmlDoc).find(type).each(function (i) {
        roomstaytype.id = $(this).attr("id");
        roomstaytype.RoomStayTypeName = $(this).children("RoomStayTypeName").text();
    });
    return roomstaytype;
}

loadxml();
var $xmlDoc;
//获取全部的xml节点
function loadxml() {
    $.ajax({
        url: '../../../xmlfile/entxml.xml',
        type: 'GET',
        async: false,//因为这个请求有点慢，所以就同步，后面有数据要操作这个返回的值对象
        dataType: 'xml',
        success: function (xmlDoc, textStatus) {
            $xmlDoc = xmlDoc;
        }
    });
}



//获取公司规模
function $getEntScales() {
    var EntScalearr = new Array();
    $($xmlDoc).find("EntScale").each(function (i) {
        var obj = new Object();
        obj.val = $(this).attr("val");
        EntScalearr.push(obj);
    });
    return EntScalearr;
}

//房间类型
function $getEntNatures() {
    var EntNaturearr = new Array();
    $($xmlDoc).find("EntNature").each(function (i) {
        var obj = new Object();
        obj.val = $(this).attr("val");
        EntNaturearr.push(obj);
    });
    return EntNaturearr;
}
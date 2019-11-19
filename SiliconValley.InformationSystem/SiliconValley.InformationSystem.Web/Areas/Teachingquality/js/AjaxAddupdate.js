//月份小于10的前面加个0
function MoDay(a) {
    if (a < 10) {
        return "0" + a;
    }
    return a;
}


//关闭弹出层
function closeopen() {
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    parent.layer.close(index); //再执行关
}
//弹出层
function Popup(url, mytitle, width, hegin) {
    layui.use('layer', function () {
        var layer = layui.layer;
        layer.open({
            type: 2,
            content: url
            , area: [width + 'px', hegin + 'px'],
            title: mytitle
            , shade: 0.3
            , anim: 1
         
        });
    });
}
    var i;
    //正在执行中
    function showLoad() {

        return layer.msg('拼命执行中...', { icon: 16, shade: [0.5, '#f5f5f5'], scrollbar: false, offset: 'auto', time: 100000 });

    }
    //关闭执行中的窗体
    function closeLoad(index) {
        layer.close(index);

    }
    //修改数据
    function ajaxpostUpdata(url, datae) {
        $.ajax({
            url: url,
            type: 'POST',
            async: true,
            cache: false,
            timeout: 2000,
            data: datae,
            beforeSend: function () {//执行中
                i = showLoad();
            },
            success: function (data) {//执行成功
                closeLoad(i);
                if (data.Success==true) {
                    layer.open({
                        type: 1
                        , title: false //不显示标题栏
                        , closeBtn: false
                        , area: '300px;'
                        , shade: 0.8
                        , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                        , btn: ['确认']
                        , btnAlign: 'c'
                        , moveType: 1 //拖拽模式，0或者1
                        , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'

                        , success: function (layero) {

                            var btn = layero.find('.layui-layer-btn');

                            btn.find('.layui-layer-btn0').click(function () {
                                var index = parent.layer.getFrameIndex(window.name);
                                parent.layer.close(index);
                            });
                        }

                    });
                } else {

                    layer.msg(data.Msg, {
                        time: 5000, //20s后自动关闭

                        btn: ['确定']
                    });
                }
            }
            , error: function (xmlhttprequest, textstatus, message) {
                closeLoad(i);
                layer.msg("您的网络出现问题，请稍后再试！！！", {
                    time: 8000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        });
    }

    //错误消息提示
    function Errye(err) {
        layer.msg(err, {
            time: 8000, //20s后自动关闭

            btn: ['确定']
        });
}
//年月日
    function getNowFormatDate(date) {

    var seperator1 = "-";
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = year + seperator1 + month + seperator1 + strDate;
    return currentdate;
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
//添加数据
  function ajaxpost(url, datae,name) {

  
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        cache: false,
        timeout: 2000,
        data: datae,
       
        beforeSend: function () {//执行中
            i = showLoad();
        },
        success: function (data) {//执行成功
            closeLoad(i);
            if (data.Success == true) {
                layer.open({
                    type: 1
                    , title: false //不显示标题栏
                    , closeBtn: false
                    , area: '300px;'
                    , shade: 0.8
                    , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                    , btn: [name, '确认']
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'

                    , success: function (layero) {

                        var btn = layero.find('.layui-layer-btn');
                        btn.find('.layui-layer-btn0').click(function () {
                            document.getElementById("form").reset();
                            reset();
                        });
                        btn.find('.layui-layer-btn1').click(function () {
                            var index = parent.layer.getFrameIndex(window.name);
                            parent.layer.close(index);
                        });
                    }

                });
            } else {

                layer.msg(data.Msg, {
                    time: 5000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        }
        , error: function (xmlhttprequest, textstatus, message) {
            closeLoad(i);
            layer.msg("您的网络出现问题，请稍后再试！！！", {
                time: 8000, //20s后自动关闭

                btn: ['确定']
            });
        }
    });
}

//学生会成员添加
function ajaxUniom(url, datae) {
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        cache: false,
        timeout: 2000,
        data: datae,

        beforeSend: function () {//执行中
            i = showLoad();
        },
        success: function (data) {//执行成功
            closeLoad(i);
            if (data.Success == true) {
                layer.open({
                    type: 1
                    , title: false //不显示标题栏
                    , closeBtn: false
                    , area: '300px;'
                    , shade: 0.8
                    , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                    , btn: ['确认']
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'
                    , success: function (layero) {

                        var btn = layero.find('.layui-layer-btn');
                      
                        btn.find('.layui-layer-btn0').click(function () {
                            var index = parent.layer.getFrameIndex(window.name);
                            parent.layer.close(index);
                        });
                    }

                });
            } else {

                layer.msg(data.Msg, {
                    time: 5000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        }
        , error: function (xmlhttprequest, textstatus, message) {
            closeLoad(i);
            layer.msg("您的网络出现问题，请稍后再试！！！", {
                time: 8000, //20s后自动关闭

                btn: ['确定']
            });
        }
    });
}
//拆班合班 parent.GetPartsItmes(x, c);
function ajaxUniomss(url, datae) {
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        cache: false,
        timeout: 2000,
        data: datae,

        beforeSend: function () {//执行中
            i = showLoad();
        },
        success: function (data) {//执行成功
            closeLoad(i);
            if (data.Success == true) {
                layer.open({
                    type: 1
                    , title: false //不显示标题栏
                    , closeBtn: false
                    , area: '300px;'
                    , shade: 0.8
                    , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                    , btn: ['确认']
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'
                    , success: function (layero) {

                        var btn = layero.find('.layui-layer-btn');

                        btn.find('.layui-layer-btn0').click(function () {
                            var index = parent.layer.getFrameIndex(window.name);
                            parent.GetPartsItmesMy();
                            parent.layer.close(index);
                        });
                    }

                });
            } else {

                layer.msg(data.Msg, {
                    time: 5000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        }
        , error: function (xmlhttprequest, textstatus, message) {
            closeLoad(i);
            layer.msg("您的网络出现问题，请稍后再试！！！", {
                time: 8000, //20s后自动关闭

                btn: ['确定']
            });
        }
    });
}

//学员费用数据加载
function Price(dbase,callback) {
    $.post("/Finance/Pricedetails/Singlecostitems", dbase, function (data) {
        callback(data);
    });
}
//本科费用
function Otherexpenses(id, callback) {
    $.post("/Finance/Pricedetails/Otherexpenses" , id, function (data) {
        callback(data);
    });
}
//学员学费录入
function ajaxprice(url, datae,callback) {
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        cache: false,
        timeout: 2000,
        data: datae,

        beforeSend: function () {//执行中
            i = showLoad();
        },
        success: function (data) {//执行成功
            closeLoad(i);
            if (data.Success == true) {
                layer.open({
                    type: 1
                    , title: false //不显示标题栏
                    , closeBtn: false
                    , area: '300px;'
                    , shade: 0.8
                    , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                    , btn: ['确认']
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'
                    , success: function (layero) {

                        var btn = layero.find('.layui-layer-btn');

                        btn.find('.layui-layer-btn0').click(function () {
                            callback();
                        });
                    }

                });
            } else {

                layer.msg(data.Msg, {
                    time: 5000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        }
        , error: function (xmlhttprequest, textstatus, message) {
            closeLoad(i);
            layer.msg("您的网络出现问题，请稍后再试！！！", {
                time: 8000, //20s后自动关闭

                btn: ['确定']
            });
        }
    });
}

//班委业务操作
function ClasscommitteePost(stuid, MenName, Entity, callback) {

    $.post("/Teachingquality/ClassSchedule/AddMembers?Stuid=" + stuid + "&MenName=" + MenName + "&Entity=" + Entity, function (db) {
        layer.msg(db.Msg);

        callback();


    });

}

//班主任职业培训课件录入
function ajaxPorfessionala(url, datae) {
    $.ajax({
        url: url,
        type: 'POST',
        async: true,
        cache: false,
        timeout: 2000,
        data: datae,

        beforeSend: function () {//执行中
            i = showLoad();
        },
        success: function (data) {//执行成功
            closeLoad(i);
            if (data.Success == true) {
                layer.open({
                    type: 1
                    , title: false //不显示标题栏
                    , closeBtn: false
                    , area: '300px;'
                    , shade: 0.8
                    , id: 'LAY_layuipro' //设定一个id，防止重复弹出
                    , btn: ['确认']
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'
                    , success: function (layero) {

                        var btn = layero.find('.layui-layer-btn');

                        btn.find('.layui-layer-btn0').click(function () {
                            var index = parent.layer.getFrameIndex(window.name);
                            parent.GetDateProfessionala(data.page, data.limit);
                            parent.layer.close(index);
                            
                        });
                    }

                });
            } else {

                layer.msg(data.Msg, {
                    time: 5000, //20s后自动关闭

                    btn: ['确定']
                });
            }
        }
        , error: function (xmlhttprequest, textstatus, message) {
            closeLoad(i);
            layer.msg("您的网络出现问题，请稍后再试！！！", {
                time: 8000, //20s后自动关闭

                btn: ['确定']
            });
        }
    });
}
//单击行勾选checkbox事件
    
$(document).on("click", ".layui-table-body table.layui-table tbody tr", function () {
    var index = $(this).attr('data-index');
    var tableBox = $(this).parents('.layui-table-box');
    //存在固定列
    if (tableBox.find(".layui-table-fixed.layui-table-fixed-l").length > 0) {
        tableDiv = tableBox.find(".layui-table-fixed.layui-table-fixed-l");
    } else {
        tableDiv = tableBox.find(".layui-table-body.layui-table-main");
    }
    var checkCell = tableDiv.find("tr[data-index=" + index + "]").find("td div.laytable-cell-checkbox div.layui-form-checkbox I");
    if (checkCell.length > 0) {
        checkCell.click();
    }
});

$(document).on("click", "td div.laytable-cell-checkbox div.layui-form-checkbox", function (e) {
    e.stopPropagation();
});
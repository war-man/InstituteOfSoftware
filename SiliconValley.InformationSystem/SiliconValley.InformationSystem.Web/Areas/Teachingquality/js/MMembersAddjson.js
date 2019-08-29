var i;
//正在执行中
function showLoad() {

    return layer.msg('拼命执行中...', { icon: 16, shade: [0.5, '#f5f5f5'], scrollbar: false, offset: 'auto', time: 100000 });

}
//关闭执行中的窗体
function closeLoad(index) {
    layer.close(index);

}

//错误消息提示
function Errye(err) {
    layer.msg(err, {
        time: 8000, //20s后自动关闭

        btn: ['确定']
    });
}
//添加数据
function ajaxpost(url, datae) {

    console.log("aaa11");
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
                  
                    , btnAlign: 'c'
                    , moveType: 1 //拖拽模式，0或者1
                    , content: '<div style="padding: 50px; line-height: 22px; background-color: #393D49; color: #fff; font-weight: 300;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.Msg + '！！！</div>'

                    , success: function (layero) {

                      
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

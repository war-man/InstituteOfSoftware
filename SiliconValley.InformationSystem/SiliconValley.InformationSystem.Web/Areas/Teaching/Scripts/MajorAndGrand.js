
//&&&&&&&&&&&&&&&&&&&&&& 专业和阶段的业务解读

///添加
function AddGrandOnMajor(teacherid, majorId, grandId) {

    $.post("/Teaching/Teacher/AddGrandOnMajor", { teacherid: teacherid, majorid: majorId, grandid: grandId }, function (resultObj) {
       
        layer.msg(resultObj.Msg);

    });
}

//获取专业的阶段

function GetHaveGrand(teacherid, majorid, callback) {

    $.post("/Teaching/Teacher/GetHaveGrandData", { teacherid: teacherid, majorid: majorid,}, function (resultObj) {

        callback(resultObj);

    });

}


layui.use(['element','layer'], function () { 

    var element = layui.element;
    var layer = layui.layer;

    $("#add1").click(function () {
       var select = $(this).attr("select");

        if (select == "true") {
            $(this).attr("select", "false");

            $("#showGrand")[0].innerHTML = "";

        }

        if (select == "false") {
            $(this).attr("select", "true");

        var currentBtn = this;

        //showGrand

        //发送请求获取阶段

        var teacherid = $("#teacherid").val();

        //拿取专业
        var majorid = $(this).attr("majorid");


        if (teacherid != undefined && teacherid != null && majorid != undefined && teacherid != null) {

            //发送请求
            $.post("/Teaching/Teacher/GetNoGrandOnMajor", { teacherId: teacherid, majorId: majorid }, function (resultObj) {

                console.log(resultObj);

                if (resultObj.ErrorCode != 200) {

                    layer.msg(resultObj.msg);

                } else {


                    for (var i = 0; i < resultObj.Data.length; i++) {

                        var div = $("<div style='width:80px; height:80px;float:left;margin-left:30px; cursor:pointer; border-radius:50%;background-color:#64c291;text-align:center;line-height:80px'></div>");

                        div.addClass = "grandDiv";

                        div.attr("grandid", resultObj.Data[i].Id);

                        div.text(resultObj.Data[i].GrandName);

                        //给div注册点击事件和鼠标移入、移除 事件

                        div.on({
                            mouseover: function () {

                                console.log(this);

                                //background-color:#40a14b
                                $(this).addClass("backcolor");


                            },
                            mouseout: function () {
                                $(this).removeClass("backcolor");
                            },
                            dblclick: function () {

                                var click = $(this);

                                layer.confirm('确定?', { icon: 3, title: '提示' }, function (index) {

                                    var clickgrandid = click.attr("grandid");

                                    var reslt = AddGrandOnMajor(teacherid, majorid, clickgrandid);


                                    click.remove();


                                    layer.close(index);



                                    //重新渲染所拥有的阶段
                                    GetHaveGrand(1, 2, function (data) {

                                        if (data.ErrorCode == 200) {

                                            for (var i = 0; i < data.Data.length; i++) {


                                                var btn = $('<button class="layui-btn grand" grandid="" type="button" style="margin-top:50px;"> </button>');

                                                btn.attr("grandid", data.Data[i].Id);
                                                btn.text(data.Data[i].GrandName);

                                                console.log(btn);
                                                //$("#sethaveGrand .grand").remove();
                                                $("#sethaveGrand").append(btn);
                                            }
                                        }
                                       
                                    });

                                });


                            }
                        });

                        console.log($(currentBtn).parent());

                        $("#showGrand").append(div);

                    }
                }



            });

        }
       

        }
    });



 

});
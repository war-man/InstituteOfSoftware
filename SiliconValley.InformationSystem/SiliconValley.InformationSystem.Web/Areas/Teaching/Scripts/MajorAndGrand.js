

function GetGrandByTeacheridAndMajorid(teacherid, majorid,successcallback,errorcallback) {


    Ajax("/Teaching/Teacher/GetHaveGrandData", { teacherid: teacherid, majorid: majorid }, "post", function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);
        });


}


function GetnNoGrandByTeacheridAndMajorid(teacherid, majorid, successcallback, errorcallback) {


    Ajax("/Teaching/Teacher/GetNoGrandOnMajor", { teacherId: teacherid, majorId: majorid }, "post", function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);
    });



}

layui.use(['layer', 'element'], function () {

    var layer = layui.layer;
    var element = layui.element;


    //监听面板展开事件

    element.on('collapse(filter)', function (data) {

                //console.log(data.show); //得到当前面板的展开状态，true或者false
                //console.log(data.title); //得到当前点击面板的标题区域DOM对象
                //console.log(data.content); //得到当前点击面板的内容区域DOM对象


        //如果被展开
        if (data.show) {

            /*  获取这个教员的这个专业的阶段  */

            //获取teacherid
            var teacherid = $("#teacherid").val();

            //获取专业id
            var majorid = data.title[0].attributes["majorid"].nodeValue;

            GetGrandByTeacheridAndMajorid(teacherid, majorid, function (result) {

                $($(data.content).children()[1]).html("");

                if (result.ErrorCode == 200) {

                    //获取数据
                  
                    //渲染
                    for (var i = 0; i < result.Data.length; i++) {

                        var btn = $('<button class="layui-btn layui-btn-primary"></button>');

                        btn.text(result.Data[i].GrandName);

                        $($(data.content).children()[1]).append(btn);


                    }


                } else {
                    layer.msg("抱歉！ 系统繁忙......");

                }

              




            }, function (error) {

                layer.msg("抱歉！ 网络出现了一点小问题......");

                });

            
        }
    });

    //添加阶段的事件

    $(".addgrandbtn").click(function () {

        //获取专业ID
        var majorid = $(this).attr("majorid");

        var teacherid = $("#teacherid").val();

        GetnNoGrandByTeacheridAndMajorid(teacherid, majorid, function (result) {

            if (result.ErrorCode == 200) {

                //成功获取数据


                


            } else {

                layer.msg("系统正在喝茶！请稍后再试");

            }


        }, function (error) {

            layer.msg("抱歉! 网络出现了点小问题......");


            });

    });

});
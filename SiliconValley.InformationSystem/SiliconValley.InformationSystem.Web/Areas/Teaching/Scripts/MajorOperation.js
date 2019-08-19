
//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>所有函数操作>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>//

//*********添加专业函数***********//
//参数:ManorName(专业名称) successcallback(成功回调函数) errorcallback(失败回调函数)
function AddMajor(majorName, successcallback, errorcallback) {

    Ajax("/Teaching/Major/AddMajor", { majorName: majorName }, 'post', function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);

        });

}


//参数:ManorName(专业名称) successcallback(成功回调函数) errorcallback(失败回调函数)
//*********获取专业名称相似函数***********//
function ContainsMajorName(majorName, successcallback, errorcallback) {

    Ajax("/Teaching/Major/ContainsMajorName", { majorName: majorName }, 'post', function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);

    });
}

//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<所有函数操作<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//


layui.use(['layer'], function () {

    var layer = layui.layer;


    //************************************》》》》》》》》》》》》》》》》》》》》》》》按钮的隐藏和显示

    $(document).off("mouseover", ".major").on('mouseover', '.major', function () {
        $($(this).children()[0]).addClass("checkCurroseBtn");
        $($(this).children()[1]).addClass("checkTeacherBtn");
    });

    $(document).off("mouseout", ".major").on('mouseout', '.major', function () {
        $($(this).children()[0]).removeClass("checkCurroseBtn");
        $($(this).children()[1]).removeClass("checkTeacherBtn");
    });

  



     //************************************《《《《《《《《《《《《《《《《《《《《《《《《《《《《《按钮的隐藏和显示


    // 添加专业的事件

    $("#addMajorBtn").click(function () {

        layer.msg("正在准备中");

        //获取模板内容

        var addMajorTemp = $("#addMajorTemp").html();

        layer.open({
            type: 1,
            title:"新增专业",
            area:["450px","180px"],
            btn: ['确定', '关闭'],
            content: addMajorTemp,
            yes: function (index) {

                layer.msg("请稍等......");

                //获取专业名称
                var majorName = $("#addMajorNameInput").val().trim();

                if (majorName == undefined || majorName == "") {

                    layer.msg("请输入专业名称");

                    return;
                }

                ContainsMajorName(majorName, function (data) {


                    console.log(data);

                    if (data.Data.length == 0) {


                        layer.confirm("您需要添加的专业为: (" + majorName + " )", function (index) {

                            //发送请求
                            AddMajor(majorName, function (data) {

                                if (data.ErrorCode == 200) {

                                    layer.msg("添加成功");

                                    //创建元素



                                    var MajorTemp = _.template($("#MajorTemp").html());

                                    var html2 = MajorTemp({ majorName: data.Data.SpecialtyName, majorid: data.Data.Id });

                                    $("#majorlist").append($(html2));

                                    var majorcount = $("#majorcount").text();

                                    $("#majorcount").text(parseInt(majorcount) + 1);

                                }
                                else {

                                    layer.msg("系统忙");
                                }

                                layer.close(index);

                            }, function (error) {

                                layer.msg("请检查您的网络......");

                            });


                        });
                       
                        return;
                    }

                    var ContainsName="";

                    for (var i = 0; i < data.Data.length; i++) {


                        if (data.Data[i].SpecialtyName.toLowerCase() == majorName.toLowerCase()) {

                            layer.msg("已存在名为：" + majorName +"的专业 拒绝添加！");

                            return;
                        }

                        if (data.Data[i].SpecialtyName.toLowerCase().search(majorName.toLowerCase()) != -1) {

                            ContainsName += data.Data[i].SpecialtyName + ',';

                        }

                    }

                    layer.confirm("具有相似名称的专业(" + ContainsName + ") 确定添加吗？", function (index) {

                        //发送请求
                        layer.msg("准备发送请求");


                    });


                }, function (error) {

                    layer.msg("网络出现了点小问题......");


                    });


                layer.close(index);
            }

        });


    });

    //查看专业下课程的按钮事件

    $(document).off("click", ".checkCurroseBtn1").on('click', '.checkCurroseBtn1', function () {
        layer.msg("敬请期待！");
    });

    //查看的按钮事件
    $(document).off("click", ".checkTeacher1").on('click', '.checkTeacher1', function () {

        //获取点击专业信息
        var major = $(this).parent().attr("majorid");

        var majorName = $(this).parent().text().trim();
        //加密
        var passwd = escape(major);

        window.location.href = "/Teaching/Teacher/Teachersinfo?majorname=" + majorName + "&major=" + passwd;
    });


});
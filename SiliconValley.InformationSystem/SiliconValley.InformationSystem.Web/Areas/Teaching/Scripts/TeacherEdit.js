layui.use(['form', 'layer'], function () {

    var form = layui.form;
    var layer = layui.layer;

    form.on('submit(addteacher)', function (data) {


        console.log(data.field) //当前容器的全部表单字段，名值对形式：{name: value}
        //阻止表单跳转。如果需要表单跳转，去掉这段即可。


        Ajax("/Teaching/Teacher/AddTeacher", data.field, "POST", function (successResult) {

            layer.msg(successResult.Msg, function () {

                //当你在iframe页面关闭自身时
                var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                parent.layer.close(index); //再执行关闭

                table.reload('Teacherlist', {

                });

            });

        }, function (errorResult) {

            layer.msg("网络连接错误!");

            });


        return false;
    });

    $(document).keydown(function (event) {

        console.log(event.keyCode);

        if (event.keyCode == 13) {

            $("#submit").click();

        }

    });

    $("#close").click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭

    });

    
});
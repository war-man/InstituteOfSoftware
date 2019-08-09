layui.use(['form', 'layer'], function () {

    var form = layui.form;
    var layer = layui.layer;

    form.on('submit(addteacher)', function (data) {


        console.log(data.field) //当前容器的全部表单字段，名值对形式：{name: value}
        //阻止表单跳转。如果需要表单跳转，去掉这段即可。

        $.post("/Teaching/Teacher/AddTeacher", data.field, function (result) {

            console.log(result)

            if (result.ErrorCode ==200) {

                layer.msg('修改成功');
            }
            else {
                //当你在iframe页面关闭自身时
                var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                parent.layer.close(index); //再执行关闭

            }

        });

        return false;
    });
    
});
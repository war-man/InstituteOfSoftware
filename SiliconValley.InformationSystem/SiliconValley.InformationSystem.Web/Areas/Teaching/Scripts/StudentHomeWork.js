/////////////////////////学员作业业务////////////////////////

layui.use(['form', 'layer', 'laydate'], function () {

    var form = layui.form;


    var laydate = layui.laydate;

    form.render('checkbox'); //刷新select选择框渲染

    //执行一个laydate实例
    laydate.render({
        elem: '#date' //指定元素
    });



    form.on('submit(tijiao)', function (data) {
      
        console.log(data.field) //当前容器的全部表单字段，名值对形式：{name: value}

        //发送请求

        Ajax('/Teaching/Class/RecordStuHomeWorkSubmission', data.field, 'post', function (data) {

            if (data.ErrorCode == 200) {


                layer.msg('记录成功', {
                    icon: 1,
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    parent.layer.close(index); //再执行关闭   
                });


            }

            else if (data.ErrorCode == 501)
            {

                layer.msg('抱歉 ！数据重复', {
                    icon: 2,
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    //do something
                });
            }
            else {

                layer.msg('服务器错误', {
                    icon: 2,
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    //do something
                });
            }


        }, function (error) {

            layer.msg("请检查您的网络");

            });



        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $("#close").click(function () {


        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭   

    });


});







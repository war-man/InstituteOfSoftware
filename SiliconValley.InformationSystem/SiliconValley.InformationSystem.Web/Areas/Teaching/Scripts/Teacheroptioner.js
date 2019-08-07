
layui.use(['table', 'layer'], function () {
    var table = layui.table;
    var layer = layui.layer;

    table.render({
        elem: '#teacherlist'
        , toolbar: '#topBar'
        , url: '/Teaching/Teacher/TeacherData/'
        , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
        , cols: [[
            { field: 'TeacherID', width: 80, title: 'ID', sort: true }
            , { field: 'TeacherName', width: 80, title: '姓名' }
            , { field: 'WorkExperience', width: 80, title: '工作经验', sort: true }

        ]]
        , page: true
    });


    //头工具栏事件
    table.on('toolbar(teacherlist_filter)', function (obj) {

        var checkStatus = table.checkStatus(obj.config.id);
        console.log(checkStatus);
        switch (obj.event) {
            case 'AddTeacher':
                layer.open({

                    type: 2,
                    area: ["800px", "650px"],
                    content: "/Teaching/Teacher/Operating"

                });
                break;
        };
    });


    //监听行工具事件
    table.on('tool(teacherlist_filter)', function (obj) {
        var data = obj.data;
        //console.log(obj)
        if (obj.event === 'del') {
            layer.confirm('真的删除行么', function (index) {
                obj.del();
                layer.close(index);
            });
        } else if (obj.event === 'edit') {
            console.log(obj.data);
            var id=obj.data.TeacherID

            layer.open({

                type: 2,
                area: ["800px", "650px"],
                content: "/Teaching/Teacher/Operating/"

            });     
        }
    });

});

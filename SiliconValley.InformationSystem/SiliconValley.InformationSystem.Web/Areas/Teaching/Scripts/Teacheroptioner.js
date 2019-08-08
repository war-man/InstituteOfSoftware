
layui.use(['table', 'layer','form'], function () {
    var table = layui.table;
    var layer = layui.layer;
    var form = layui.form;

    table.render({
        elem: '#teacherlist'
        , id:'Teacherlist'
        , toolbar: '#topBar'
        , url: '/Teaching/Teacher/TeacherData/'
        , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
        , cols: [[
            { field: 'TeacherID', width: 120, title: 'ID', sort: true }
            , { field: 'TeacherName', width: 120, title: '姓名' }
            ,{ field: 'Major', width: 120, title: '专业' }
            , { field: 'WorkExperience', width: 120, title: '工作经验', sort: true }
            , { field: 'ProjectExperience', width: 120, title: '项目经验', sort: true }
            , { field: 'TeachingExperience', width: 120, title: '教学经验', sort: true }
            , { field: 'AttendClassStyle', width: 120, title: '上课风格', sort: true }
            , { fixed: 'right', title: '操作', toolbar: '#editBar', width: 150 }
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
        var id = obj.data.TeacherID
        //console.log(obj)
        if (obj.event === 'detail') {
            layer.open({

                type: 2,
                area: ["1000px", "800px"],
                content: "/Teaching/Teacher/TeacherDetailView/" + id,
                end: function () {

                    //刷新表格
                    layer.msg('修改成功');

                    table.reload('Teacherlist', {

                    });

                }

            });   
        } else if (obj.event === 'edit') {
           
           

            layer.open({

                type: 2,
                area: ["1000px", "800px"],
                content: "/Teaching/Teacher/Operating/" + id,
                end: function () {

                    //刷新表格
                    layer.msg('修改成功');

                    table.reload('Teacherlist', {
                     
                    });

                }

            });     
        }
    });


    //修改教员监听表单提交



});

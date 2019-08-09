


layui.use(['table', 'layer', 'form','element'], function () {
    var table = layui.table;
    var layer = layui.layer;
    var form = layui.form;
    var element = layui.element;

    table.render({
        elem: '#teacherlist'
        , id:'Teacherlist'
        , toolbar: '#topBar'
        , url: '/Teaching/Teacher/TeacherData/'
        , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
        , cols: [[
            { type: 'checkbox' ,fixed:'left'}
            ,{ field: 'TeacherID', width: 120, title: 'ID', sort: true }
            , { field: 'TeacherName', width: 120, title: '姓名' }
            , { field: 'Number', width: 120, title: '员工编号' }
            , { field: 'Sex', width: 120, title: '性别' }
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

        switch (obj.event) {

            case 'EditMajorAdnGrand':
         

                layer.open({
                    title:"专业-阶段信息",
                    type: 2,
                    area: ["1000px", "850px"],
                    content: "/Teaching/Teacher/EditMajorAndGrandView/" + checkStatus.data[0].TeacherID

                });
                
                break;

            case 'Add':


                layer.open({
                    type: 2
                    , area: ["600px", "300px"],
                    shade: [0],
                    title: "选择员工",
                    btn: ['确定', '关闭'],
                    content: '/Teaching/Teacher/Teacherlist'
                    //, end: function () { table.reload('idTest', {}); }
                    , yes: function (index) {
                        //当点击‘确定’按钮的时候，获取弹出层返回的值
                        var res = window["layui-layer-iframe" + index].callbackdata();
                        console.log(res);

                        //打印返回的值，看是否有我们想返回的值。
                        if (res) {

                            // 获取员工ID
                            var empid = res.data[0].EmployeeId;
                            
                            layer.close(index);

                            //

                            layer.open({

                                type=2,
                                area: ["900px", "800px"],
                                shade: [0],
                                title: "添加教员",
                                content: '/Teaching/Teacher/AddTeacher/' + empid

                            });
                           


                        } else {
                            layer.msg('请选择员工', { icon: 0 });
                        }

                        layer.close(index);
                    }

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
                area: ["1300px", "1000px"],
                content: "/Teaching/Teacher/TeacherDetailView/" + id,
                end: function () {


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



});

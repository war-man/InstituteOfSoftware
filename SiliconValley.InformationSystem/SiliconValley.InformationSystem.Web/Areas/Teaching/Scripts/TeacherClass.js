///教员班级

function GetStudentByClass(classnumber,successcabllack, errorcallback) {

    Ajax("/Teaching/Class/GetStudentByClass", { classnumber: classnumber }, "post", function (data) {
        successcabllack(data);
    }, function (error) {

        errorcallback(error);

        });

}


layui.use(["table", "layer"], function () {

    var table = layui.table;
    var layer = layui.layer;

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //加载第一个班级的班级学员
   var clickclassnumber = $($("#classlist .classbtn")[0]).attr("classnumber");
  
    //发送请求
    GetStudentByClass(clickclassnumber, function (data) {

            /*BirthDate: "/Date(985881600000)/"
            Education: "大专"
            Familyaddress: "中国"
            Guardian: "陈神仙,父亲              "
            Hobby: null
            InsitDate: "/Date(1566230400000)/"
            IsDelete: null
            Name: "陈海石"
            Nation: "汉"
            Password: "000000"
            Picture: null
            Reack: null
            Sex: true
            State: null
            StudentNumber: "19082001033000001"
            StudentPutOnRecord_Id: 1020
            Telephone: "15673151748"
            Traine: null
            WeChat: null
            identitydocument: "431124200103303850"
            qq: null*/

      

        if (data.length != 0) {
            //渲染数据

            for (var i = 0; i < data.length; i++) {
                var studenthtml = _.template($("#studenthtml").html());

                var studentdata = studenthtml({ studentnumber: data[i].StudentNumber, studentname: data[i].Name });

                $("#studenttable").append($(studentdata));
            }

           

        }



    }, function (error) {

        layer.msg("数据加载异常");

        });//
//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //班级按钮的点击事件
    $(".classbtn").click(function () {


    

        $(this).parent().siblings().each(function () {

            $($(this).children()[0]).removeClass("layui-btn-warm");

        });
   

        $(this).addClass("layui-btn-warm");


        $("#studenttable").html("");


        var classnumber = $(this).attr("classnumber");

        //加载班级学生

        GetStudentByClass(classnumber, function (data) {

      
            if (data.length > 0) {
             
             

                for (var i = 0; i < data.length; i++) {

                    //渲染数据
                    var studenthtml = _.template($("#studenthtml").html());

                    var studentdata = studenthtml({ studentnumber: data[i].StudentNumber, studentname: data[i].Name, posi: data[i].PositionName});

                    $("#studenttable").append($(studentdata));

                }

            }

        }, function (error) {

            layer.msg("数据加载异常");

        });//

    });




    $(document).off("mouseover", ".studentbtn button").on('mouseover', '.studentbtn button', function () {
  
        $(this).addClass("layui-btn layui-btn-warm");
    });


    $(document).off("mouseout", ".studentbtn button").on('mouseout', '.studentbtn button', function () {

        $(this).removeClass("layui-btn layui-btn-warm");
    });


//学生的按钮点击事件

    $(document).off("click", ".studentbtn button").on('click', '.studentbtn button', function () {

        layer.msg("准备中。。。。。。");

        var studentnumber = $(this).attr("studentnumber");

        //获取弹窗模板html

        var layerthtml = _.template($("#layerthtml").html());

        var layerdata = layerthtml({ studentnumber: studentnumber});

        layer.open({
            type: 1,
            area:["400px", "300px"],
            title: "操作选择框",
            content: layerdata

        });
    });

    //查看学生详细信息
    
    $(document).off("click", "#detailinfo").on('click', '#detailinfo', function () {

        var studentnumber = $(this).attr("studentnumber");

        layer.open({
            type: 2,
            area:["500px","800px;"],
            title: "学员基本信息",
            content: '/Teaching/Class/StudentDetailInfo?studentnumber='+studentnumber

        });

    });

})
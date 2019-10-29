///教员班级

function GetStudentByClass(classnumber,successcabllack, errorcallback) {

    Ajax("/Teaching/Class/GetStudentByClass", { classnumber: classnumber }, "post", function (data) {
        successcabllack(data);
    }, function (error) {

        errorcallback(error);

        });

}

//获取班级的信息

function GetClassInfo(classnumber,successcallback, errorcallback) {

    Ajax("/Teaching/Class/GetClassInfo", { classnumber: classnumber }, 'post', function (data) {
        successcallback(data);

    }, function (error) {

        errorcallback(error);
        });

}


///渲染班级信息
function loadclassinfohtml(data) {

    //渲染数据
   
    var classinfohtml = _.template($("#classinfohtml").html());

    var classdata = classinfohtml({ classnumber: data.ClassNumber, studentcount: data.ClassSize, headermaster: data.Headmaster, qqgourp: data.qqGroup, grandName: data.GradeName });

    $("#classinfodiv").append($(classdata));


}

function loadclassCadres(classnumber1) {

    //获取班级班干部
    Ajax("/Teaching/Class/GetClassCadres", { classnumber: classnumber1 }, 'post', function (data) {
        $("#classCadres").html("");
       

        if (data.ErrorCode == 200) {



            for (var item in data.Data) {


                var ClassCadreshtml = _.template($("#ClassCadreshtml").html());

                var name;

                if (data.Data[item] == null || data.Data[item] == undefined) {

                    name = "暂无";

                } else {
                    name = data.Data[item].Name;
                }

                var ClassCadresdata = ClassCadreshtml({ CadresName: item, studentname: name });

                $("#classCadres").append($(ClassCadresdata));


            }

        }


    }, function (error) {
        layer.msg("班干部信息加载异常");

    });

}

$("#classlist").children("div:last-child").remove();


$(document).bind('click', function (e) {
    var e = e || window.event; //浏览器兼容性 
    var elem = e.target || e.srcElement;
    while (elem) { //循环判断至跟节点，防止点击的是div子元素 
        if (elem.id && elem.id == 'div') {
            return;
        }
        elem = elem.parentNode;
    }
    $('#div').css('display', 'none'); //点击的不是div或其子元素 
});

layui.use(["table", "layer","element"], function () {

    var table = layui.table;
    var layer = layui.layer;
    var element = layui.element;

    element.init();

//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //加载第一个班级的班级学员
    var clickclassnumber = $($("#classlist .classbtn")[0]).attr("classnumber");

    var classnumber1 = $($("#classlist .classbtn")[0]).attr("classnumber");

    //发送请求获取班级信息

    GetClassInfo(classnumber1, function (data) {

        if (data.ErrorCode == 200) {

            loadclassinfohtml(data.Data)

        }

    }, function (error) {

        layer.msg("班级信息数据加载异常...");

        });


    loadclassCadres(classnumber1);




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

                var studentdata = studenthtml({ studentnumber: data[i].StudentNumber, studentname: data[i].Name, posi: data[i].PositionName });

                $("#studentlist").append($(studentdata));
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


        $("#studentlist").html("");


        var classnumber = $(this).attr("classnumber");

        //加载班级学生

        GetStudentByClass(classnumber, function (data) {

            console.log(data);

      
            if (data.length > 0) {
             
             

                for (var i = 0; i < data.length; i++) {

                    //渲染数据
                  

                    var studenthtml = _.template($("#studenthtml").html());

                    var studentdata = studenthtml({ studentnumber: data[i].StudentNumber, studentname: data[i].Name, posi: data[i].PositionName});

                    $("#studentlist").append($(studentdata));

                }

            }

        }, function (error) {

            layer.msg("数据加载异常");

            });//


        //获取班级信息

        $("#classinfodiv").html("");

        GetClassInfo(classnumber, function (data) {

            if (data.ErrorCode == 200) {

                loadclassinfohtml(data.Data)

            }

        }, function (error) {

            layer.msg("班级信息数据加载异常...");

            });

        loadclassCadres(classnumber);


    });




    $(document).off("mouseover", ".studentbtn button").on('mouseover', '.studentbtn button', function () {
  
        $(this).addClass("layui-btn-warm");
    });


    $(document).off("mouseout", ".studentbtn button").on('mouseout', '.studentbtn button', function () {

        $(this).removeClass("layui-btn-warm");
    });


//学生的按钮点击事件

    $(document).off("click", ".studentbtn button").on('click', '.studentbtn button', function () {

    

        $("#div").show(); 

        var studentnumber = $(this).attr("studentnumber");


        $("#div div").attr("studentnumber", studentnumber);


        ////获取弹窗模板html

        //var layerthtml = _.template($("#layerthtml").html());

        //var layerdata = layerthtml({ studentnumber: studentnumber});

        //layer.open({
        //    type: 1,
        //    area:["400px", "500px"],
        //    title: "操作选择框",
        //    content: layerdata

        //});
    });

    //查看学生详细信息
    
    $(document).off("click", ".detailinfo").on('click', '.detailinfo', function () {

        var studentnumber = $(this).attr("studentnumber");

        layer.open({
            type: 2,
            area:["500px","800px;"],
            title: "学员基本信息",
            content: '/Teaching/Class/StudentDetailInfo?studentnumber='+studentnumber

        });

    });


    //添加学生谈话记录
    $(document).off("click", "#AddConversationRecordBtn").on('click', '#AddConversationRecordBtn', function () {

        var studentnumber = $(this).attr("studentnumber");

        layer.open({
            type: 2,
            area: ["800px", "550px;"],
            title: "谈话记录",
            content: '/Teaching/RecordOfConversation/Operations'

        });

    });

   

    //查看学生访谈记录
    $(document).off("click", "#GetConversationRecordBtn").on('click', '#GetConversationRecordBtn', function () {

        var studentnumber = $(this).attr("studentnumber");


        window.location.href = '/Teaching/RecordOfConversation/ConversationIndex?studentnumber=' + studentnumber;

        

    });

    //
    //查看学生作业提交情况
    $(document).off("click", "#GetHomeWorkSubmission").on('click', '#GetHomeWorkSubmission', function () {

        var studentnumber = $(this).attr("studentnumber");


        layer.open({
            type: 2,
            area: ["1300px", "1000px;"],
            title: "作业提交情况  （提示：按月份统计）",
            content: '/Teaching/Class/StuHomeWorkSubmission?studentnumber=' + studentnumber

        });



    });
    //RecordtHomeWorkSubmission
      //记录学员未完成情况
    $(document).off("click", "#RecordtHomeWorkSubmission").on('click', '#RecordtHomeWorkSubmission', function () {

        var studentnumber = $(this).attr("studentnumber");


        layer.open({
            type: 2,
            area: ["500px", "500px;"],
            title: "作业提交情况",
            content: '/Teaching/Class/RecordStuHomeWorkSubmission?studentnumber=' + studentnumber

        });



    });


    //function dragPanelMove(downDiv, moveDiv) {
      
    //    $(downDiv).mousedown(function (e) {
    //        var isMove = true;
    //        var div_x = e.pageX - $(moveDiv).offset().left;
    //        var div_y = e.pageY - $(moveDiv).offset().top;
    //        $(document).mousemove(function (e) {
    //            if (isMove) {
    //                var obj = $(moveDiv);
    //                obj.css({ "left": e.pageX - div_x, "top": e.pageY - div_y });
    //            }
    //        }).mouseup(
    //            function () {
    //                isMove = false;
    //            });
    //    });

    //}


    //dragPanelMove("#div", "div");

     



















})
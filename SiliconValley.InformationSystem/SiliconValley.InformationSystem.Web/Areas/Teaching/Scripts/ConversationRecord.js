
///////////////////////////谈话记录业务////////////////////////////////////


function GetConversationRecord(begindate, enddate, studentname, successcallback, errorcallback) {

    Ajax('/Teaching/RecordOfConversation/GetConversationRecord', { begindate: begindate, enddate: enddate, studentname: studentname }, "post", function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);

        });

}

function GetConversationRecords(successcallback, errorcallback) {

    Ajax('/Teaching/RecordOfConversation/GetConversationrecords', {}, "post", function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);

    });

}



layui.use(['layer', 'date'], function () {

    var layer = layui.layer;


    Ajax('/Teaching/RecordOfConversation/GetConversationrecords', {}, "post", function (data) {

        successcallback(data);

    }, function (error) {

        errorcallback(error);

    });

});
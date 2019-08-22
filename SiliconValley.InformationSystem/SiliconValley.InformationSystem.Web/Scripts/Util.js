
function htmlmuban (mubanelem,) {


    var studenthtml = _.template($("#studenthtml").html());

    var studentdata = studenthtml({ studentnumber: data[0].StudentNumber, studentname: data[0].Name });

    $("#studenttable").append($(studentdata));
}
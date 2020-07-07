

function Ajax(url, dataJson, methd, successcallback, errorcallback, asycn = true) {


    var loadindex = layer.load(2); //又换了种风格，并且设定最长等待10秒 

    $.ajax({
        url: url,
        type: methd,
        data: dataJson ,
        timeout:100000,
        dataType: 'json',
        async: asycn,
        success: function (successResult) {

          

            layer.close(loadindex);   

            successcallback(successResult);
        },
        error: function (errorResult) {
           
            layer.close(loadindex);   

            errorcallback(errorResult);
            
        }
    });
}



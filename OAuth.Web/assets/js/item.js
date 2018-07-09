$(function () {
    jQuery.js_page_action = function (option) {
        $.post("/Subject/GetItemPage", { itemMode: option.itemMode }, function (data) {
            var st = "/subject/" + data.uri + "/" + option.id;
            location.href = st;
        });
    }
    //jQuery.send_email = function (option) {
    //    $.post("/Subject/EmailNotice", { id: option.id }, function (data) {
    //        if (data.code==200) {
    //            alert(data.message);
    //        }
    //    });
    //}
});
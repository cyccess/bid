

var login = login || {};


$(function () {
    landCenter(".login");

    login.init();
});


//登陆提交
login.submit = function () {

    var that = this, $signIn = $("#signIn");

    $("#login-form").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //去掉验证成功后的提示信息
            that.$Tips.removeClass("Validform_right").html("");
            $signIn.attr("disabled", true).html("正在登录...");
        },
        success: function (data, textStatus) {
            if (data.code === 200) {
                location.href = data.url;
            }
            else {
                $signIn.attr("disabled", false).html("登录");
                that.$Tips.addClass("Validform_wrong").html(data.message);
            }
        },
        error: function (data, status, e) {
            $signIn.attr("disabled", false).html("登录");
        },
        type: "post",
        dataType: "json",
        timeout: 60000
    });
}

login.init = function () {
    var that = this;

    this.$Tips = $(".tips");

    this.checkForm = $("#login-form").Validform({
        tiptype: function (msg, o, cssctl) {
            var objtip = that.$Tips;
            cssctl(objtip, o.type);
            objtip.html(msg);
        },
        callback: function () {
            that.submit();
            return false;
        }
    });

    var checkRule = [
        { ele: "#inputName", datatype: "*", nullmsg: "请输入用户名" },
        { ele: "#inputPassword", datatype: "*", nullmsg: "请输入密码" }
    ];

    this.checkForm.addRule(checkRule);

}

//div居中
function landCenter(a) {
    var land = $(a);
    land.css({
        "left": ($(window).width() - land.outerWidth()) / 2,
        "top": ($(window).height() - land.outerHeight()) / 2
    });
}

function bgimg(){
    var bg = $("bgimg");
    var bgi = bg.find("img");

    landCenter("#bgimg img");
}

$(window).resize(function () {
    bgimg();
    landCenter(".login");
});

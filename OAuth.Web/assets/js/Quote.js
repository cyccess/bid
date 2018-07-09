//=====================
// 物料竞价
//=====================
+(function ($, window) {
    var quote = function (element, options) {

        quote.fn._init(element, options);
        quote.fn.stopEventBubble();
    };

    quote.fn = quote.prototype = {

        _init: function (elememt, options) {

            this.options = options;

            this.$btnOpen = $(elememt);
            this.actionType = this.$btnOpen.data("flag");

            this.data = null;

            this.$sidebar = this.$sidebar || $(".sidebar.right").sidebar({ side: "width", speed: 550 });
            this.$sidebarLoading = this.$sidebar.find(".sidebar-loading");
            this.$sidebarContent = this.$sidebar.find(".sidebar-content");
            this.isShowSidebar = false;

            this.$actionLabels = null;

            this.$popbox = this.$popbox || null;
            this.isShowPopbox = false;
            this.$validForm = null;

            this.$sidebarLoading.show();
            this.$sidebarContent.html("");

            !this.$popbox || this.$popbox.close();

            this.show();
            this._event();
        },
        show: function () {
            var that = this;
            this.$sidebar.trigger("sidebar:open");
            this.isShowSidebar = true;
            this.$sidebar.close = function () {
                that.$sidebarContent.html("");
                that.$sidebar.trigger("sidebar:close");
                that.isShowSidebar = false;
            }

            if (that.actionType === "update") {
                that.getQuote(function (data) {
                    that.$sidebarContent.html($("#item_quote_tmpl").tmpl(data));
                    that._validForm(".aui");
                });
            }
            else if(that.actionType === "new"){
                that.$sidebarContent.html($("#item_quote_tmpl").tmpl({ Id: 0, Price: "", Memo: "" }));
                that._validForm(".aui");
            }
            else if (that.actionType === "detail") {
                that.getQuoteDetail(function (data) {
                    that.$sidebarContent.html($("#item_quote_detail_tmpl").tmpl(data));
                });
            }

            that.$sidebarLoading.hide();
            //that.customScrollbar(".content");
        },
        //绑定模块事件
        _event: function () {
            var that = this;

            this.$sidebar.on("click", ".js-close", function (e) {
                that.$sidebar.close();
                if (that.$popbox) {
                    that.$popbox.close();
                }
                return false;
            });

            $(window).on("keydown", function (event) {
                var code = event.keyCode;
                if (code === 27) {
                    that._close(event);
                }
            });

            $(document).bind('click', function (e) {
                that._close();
            });

            $("#cancel").on("click", function (e) {
                that.$sidebar.close();
            });

        },
        //关闭面板
        _close: function (e) {
            e = e || window.event; //浏览器兼容性
            var target = e.target || e.srcElement;

            var compareElement = function (elem, element) {
                return !(elem === element || $.contains(element, elem));
            };

            if (this.isShowSidebar && this.$sidebar) {
                if (compareElement(target, this.$sidebar[0])) {
                    this.$sidebar.close();
                }
            }

            this.$popbox && this.$popbox.close();
        },
        //表单验证
        _validForm: function (element, rule) {

            this.$validForm = $(element).Validform({
                tiptype: function (msg, o, cssctl) { }
            });

            if (rule) {
                this.$validForm.addRule(rule);
            }

            return this;
        },
        //提交型材报价
        js_post_quote: function () {
            var that = this,
            formConfig = {
                ajaxpost: {
                    url: "/Supplier/MaterialQuote",
                    data: that.$validForm.forms.serialize() + "&ItemMaterialId=" + that.options.id,
                    success: function (data) {
                        if (data.code === 200) {
                            if (that.actionType === "new") {
                                that.$btnOpen.removeClass("btn-success").addClass("btn-default").data("flag", "update").text("修改竞价");
                            }
                            that.noty(data.message, "information");
                            that.$sidebar.close();
                        }
                    },
                    error: function () {
                        that.noty("操作失败，请重新尝试！", "error");
                    }
                }
            }

            that.$validForm.config(formConfig);
            //ajax 提交表单
            that.$validForm.ajaxPost(false);
            return false;
        },
        //获取型材报价
        getQuote: function (fn) {
            var that = this;
            $.post("/Supplier/GetItemQuote", { materialId: that.options.id }, function (data) {
                typeof (fn === "function") && fn(data);
            });
        },
        getQuoteDetail: function (fn) {
            var that = this;
            $.post("/Supplier/GetItemSureQuote", { materialId: that.options.id }, function (data) {
                typeof (fn === "function") && fn(data);
            });
        },
        //滚动条
        customScrollbar: function (element) {
            $(element).mCustomScrollbar({
                axis: "y",
                theme: "dark",
                scrollInertia: 500,
                scrollbarPosition: "outside"
            });
        },
        stopEventBubble: function (event) {
            var e = event || window.event;
            if (e && e.stopPropagation) {
                e.stopPropagation();
            }
            else {
                e.cancelBubble = true;
            }
        },
        //通知
        noty: function noty(message, type) {
            $.noty.closeAll();
            $.noty({
                layout: "topCenter",
                text: message,
                type: type,
                closeButton: false
            });
        }
    };

    window.locator = window.locator || {};
    window.locator.openQuote = quote;
    window.uv = quote.fn;

})(jQuery, this);


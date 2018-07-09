//=====================
// 供应商类别
//=====================
+(function ($, window) {
    var mode = function (element, options) {

        mode.fn._init(options);
        mode.fn.stopEventBubble();
    };

    mode.fn = mode.prototype = {

        _init: function (options) {

            this.options = options;

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

            $.post("/subject/getmodejson", that.options, function (data) {
                that.data = data;
                that.$sidebarLoading.hide();
                that.$sidebarContent.html($("#mode_tmpl").tmpl(data));

                that.customScrollbar(".content");
                var list = $("#"+that.options.modeId).val().split(",");
                $.each(list, function (i, p) {
                    $(".entry-part-content li[data-id='" + p + "']").addClass("active");
                });
            });
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

        js_set_part: function (element) {
            var that = this;
            var $element = $(element);
            var partLabel = $element.parent();

            if (!partLabel.hasClass("active")) {
                partLabel.addClass("active");
            }
            else {
                partLabel.removeClass("active");
            }
        },

        js_get_active: function (element) {
            var that = this;
            var idArr = [];
            var nameArr = [];

            $(".entry-part-content li[class='active'] a").each(function (i, p) {
                idArr.push($(p).data("role-id"));
                nameArr.push($(p).text());
            });

            $("#" + that.options.modeId).val(idArr.join());
            $("#" + that.options.modeName).val(nameArr.join()).blur();

            that.$sidebar.close();
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
        }
    };

    window.locator = window.locator || {};
    window.locator.openMode = mode;
    window.uv = mode.fn;

})(jQuery, this);

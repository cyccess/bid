//=====================
// 模块详细
//=====================
~(function ($, window) {
    var module = function (options) {

        module.fn.stopEventBubble(event);

        return module.fn._init(options);
    };

    module.fn = module.prototype = {
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

            this._load(options);
            this._event();
        },
        //加载模块数据
        _load: function (config) {
            var that = this;

            this.$sidebar.trigger("sidebar:open");
            this.isShowSidebar = true;
            this.$sidebar.close = function () {
                that.$sidebarContent.html("");
                that.$sidebar.trigger("sidebar:close");
                that.isShowSidebar = false;
            }

            $.post("/Module/GetModule", config, function (data) {
                that.data = data;

                that.$sidebarLoading.hide();
                that.$sidebarContent.html($("#module_detail").tmpl(data));

                that.$actionLabels = that.$sidebar.find(".module-action-labels");
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
        //动作添加
        js_assign_action: function (element) {
            var that = this;
            this.$popbox = $.popbox({
                target: $(element),
                content: $("#add_module_action").html(),
                init: function () {
                    that._validForm(".add-action-form");
                },
                sure: function () {
                    return false;
                }
            });

            that.stopEventBubble();
        },
        //编辑模块
        js_module_edit: function (obj) {
            var that = this;
            var $this = $(obj);

            //初始化模块编辑数据
            that.$sidebar.find(".entry-well-content").html($("#module-edit").tmpl(that.data.module));

            that.$sidebar.find("input").iCheck({
                checkboxClass: 'icheckbox_minimal-red',
                radioClass: 'iradio_minimal-red',
                increaseArea: '10%'
            });

            that._validForm(".module-add-form");

            //取消编辑
            that.$sidebar.find("#cancel").on("click", function () {
                that.$sidebarContent.html($("#module_detail").tmpl(that.data));
                return false;
            });

            //编辑保存模块
            that.$sidebar.find("#sure").on("click", function () {
                var formConfig = {
                    ajaxpost: {
                        url: "/Module/Edit",
                        data: that.$validForm.forms.serialize() + "&Id=" + that.data.module.Id,
                        success: function (data) {
                            if (data.code === 200) {
                                var moduleName = $('[name="ModuleName"]').val();
                                $("#module_main_" + that.data.module.ModuleNo).find(".entry-module-title").html(moduleName);
                                that._load(that.options);
                                that.noty(data.message, "information");
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
            });
        },
        //删除模块
        js_module_remove: function (element) {
            var that = this,
                $this = $(element);

            this.$popbox = $.popbox({
                target: $this,
                content: $("#module_remove").html(),
                init: function () { },
                sure: function () {
                    $.ajax({
                        type: "post",
                        url: "/Module/Delete",
                        data: { moduleId: that.data.module.Id },
                        success: function (data) {
                            if (data.code === 200) {
                                that._close();
                                $("#module_main_" + that.data.module.ModuleNo).remove();
                                that.noty(data.message, "information");
                            }
                        },
                        error: function () {
                            that.noty("操作失败，请重新尝试！", "error");
                        }
                    });
                    return false;
                }
            });

            that.stopEventBubble();
        },
        //添加动作
        js_add_action: function () {
            var that = this;

            var formConfig = {
                ajaxpost: {
                    data: that.$validForm.forms.serialize() + "&moduleId=" + that.data.module.Id,
                    url: "/Permission/Add",
                    success: function (data, obj) {
                        if (data.code === 200) {
                            $("#module-action-labels").tmpl(data.action).appendTo(that.$actionLabels);
                            $.popbox.fn.close();
                            that.noty(data.message, "information");
                        }
                    },
                    error: function (data, obj) {
                        that.noty("操作失败，请重新尝试！", "error");
                    }
                }
            };

            that.$validForm.config(formConfig);

            that.$validForm.ajaxPost(false);

            that.stopEventBubble();
        },
        //移除动作
        js_remove_action: function (element) {
            var that = this;
            var $element = $(element);
            var id = $element.data("action-id");
            $.ajax({
                type: "post",
                url: "/Permission/Delete",
                data: { id: id },
                success: function (data) {
                    if (data.code === 200) {
                        $("#action_item_" + id).remove();
                        that.noty(data.message, "information");
                    }
                },
                error: function () {
                    that.noty("操作失败，请重新尝试！", "error");
                }
            });

            this.stopEventBubble();
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

    module.setting = {
        pid: 0,
        mid: 0
    };

    window.locator = window.locator || {};
    window.locator.module = module;

}(jQuery, this));


//=====================
// 添加模块
//=====================
+(function ($, window) {
    var module = {
        init: function () {
            this._event();
        },
        _event: function () {

            var that = this;


            $('#btnAdd').click(function (e) {
                var appid = $(this).data("appid");

                $.get("/Module/Add", { appid: appid }, function (content) {
                    that._addModule(appid, content);
                });
            });
        },
        _addModule: function (appid, content) {
            $.dialog({
                content: content,
                init: function () {
                    var that = this;

                    $(".add-part-form").find("input").iCheck({
                        checkboxClass: 'icheckbox_minimal-red',
                        radioClass: 'iradio_minimal-red',
                        increaseArea: '10%'
                    });

                    var _from = $(".add-part-form").Validform({
                        tiptype: function (msg, o, cssctl) {
                            var $tip = o.obj.siblings(".Validform_checktip");
                            cssctl($tip, o.type);
                            $tip.text(msg);
                        },
                        ajaxPost: true
                    });

                    _from.config({
                        ajaxpost: {
                            success: function (data, obj) {
                                $.noty.closeAll();
                                noty({
                                    layout: "topCenter",
                                    text: data.message,
                                    type: "information",
                                    onShown: function () {
                                        location.href = location.href;
                                    }
                                });
                                that.modal.close();
                            },
                            error: function (data, obj) {
                                noty({
                                    layout: "topCenter",
                                    text: "操作失败，请重新尝试！",
                                    type: "error"
                                });
                            }
                        }
                    });
                },
                sure: function () { return false; }
            });
        }
    };

    window.Module = module;

})(jQuery, this);


$(function () {

    window.Module.init();

});

+(function ($, window) {
    var User = function (element, options) {

        User.fn._init(options);
        User.fn.stopEventBubble();
    };

    User.fn = User.prototype = {

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

            this.projects = this.projects || null;

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

            $.post("/User/GetUser", that.options, function (data) {
                that.data = data;
                that.$sidebarLoading.hide();
                that.$sidebarContent.html($("#user_detail").tmpl(data));
                that.customScrollbar(".content");
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
            var currEle = e.target || e.srcElement;

            var compareElement = function (ele, element) {
                return !(ele === element || $.contains(element, ele));
            };

            if (this.isShowSidebar && this.$sidebar) {
                if (compareElement(currEle, this.$sidebar[0])) {
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
        js_user_add: function (element) {
            var that = this;
            var $element = $(element);

            $.get("/User/Add", function (content) {
                $.dialog({
                    content: content,
                    init: function () {
                        that._validForm("#add_user_form");
                    },
                    sure: function () {

                        var _dialog = this;

                        var formConfig = {
                            ajaxpost: {
                                url: "/User/Add",
                                data: that.$validForm.forms.serialize(),
                                success: function (data) {
                                    if (data.code === 200) {
                                        _dialog.modal.close();
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
                    }
                });
            });

            that.stopEventBubble();
        },
        //编辑用户
        js_user_edit: function (element) {
            var that = this;
            var $this = $(element);

            //初始化模块编辑数据
            that.$sidebar.find(".entry-well-content").html($("#user_edit").tmpl(that.data));

            that.$sidebar.find("input").iCheck({
                checkboxClass: 'icheckbox_minimal-red',
                radioClass: 'iradio_minimal-red',
                increaseArea: '10%'
            });

            that._validForm(".user-add-form");

            //取消编辑
            that.$sidebar.find("#cancel").on("click", function () {
                that.$sidebarContent.html($("#user_detail").tmpl(that.data));
                that.customScrollbar(".content");
                return false;
            });

            //编辑保存模块
            that.$sidebar.find("#sure").on("click", function () {
                var formConfig = {
                    ajaxpost: {
                        url: "/User/Edit",
                        data: that.$validForm.forms.serialize() + "&Id=" + that.data.Id,
                        success: function (data) {
                            if (data.code === 200) {
                                var username = $('[name="UserName"]').val();
                                $("#user_item_" + that.data.Id).find("a.entry-item-title").html(username);
                                that.show(that.options);
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
        //重置密码
        js_reset_pass: function (element) {
            var that = this;
            this.$popbox = $.popbox({
                target: $(element),
                content: $("#user_reset_pass").html(),
                init: function () {
                    that._validForm(".reset-pass-form");
                },
                sure: function () {
                    var formConfig = {
                        ajaxpost: {
                            url: "/User/ResetPassword",
                            data: that.$validForm.forms.serialize() + "&uid=" + that.data.Id,
                            success: function (data) {
                                if (data.code === 200) {
                                    that.noty(data.message, "information");
                                    that.$popbox.close();;
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
                }
            });
            this.stopEventBubble();
        },
        //删除模块
        js_user_remove: function (element) {
            var that = this,
                $this = $(element);

            this.$popbox = $.popbox({
                target: $this,
                content: $("#user_remove").html(),
                init: function () { },
                sure: function () {
                    $.ajax({
                        type: "post",
                        url: "/User/Delete",
                        data: { uid: that.data.Id },
                        success: function (data) {
                            if (data.code === 200) {
                                that._close();
                                $("#user_item_" + that.data.Id).hide(500);
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
        //项目对应的角色
        js_dis_part: function () {
            var that = this;

            if (!this.projects) {
                $.post("/Project/GetProjectsAndParts", function (data) {
                    that.projects = data;
                    renderRole(data, that.data.UserRoles);
                });
            }
            else {
                renderRole(this.projects, this.data.UserRoles);
            }

            function renderRole(projects, userRoles) {
                that.$sidebar.find(".entry-well-content").html($("#part_list").tmpl(
                   { projects: projects, op: userRoles }, {
                       check: function (rid) {
                           for (var i in this.data.op) {
                               if (this.data.op[i].RoleId === rid) return "active";
                           }
                           return "";
                       }
                   }));
            }

            this.customScrollbar(".content");
            this.stopEventBubble();
        },
        //设置用户角色
        js_set_part: function (element) {
            var that = this;
            var $element = $(element);
            var roleId = $element.data("role-id");
            var partLabel = $element.parent();

            if (!partLabel.hasClass("active")) {
                partLabel.addClass("active");
            }
            else {
                partLabel.removeClass("active");
            }

            $.ajax({
                type: "post",
                url: "/Role/SetPart",
                data: { userId: that.data.Id, partId: roleId },
                success: function (data) {
                    if (data.code === 200) {
                        that.noty(data.message, "information");
                    }
                },
                error: function () {
                    that.noty("操作失败，请重新尝试！", "error");
                }
            });
        },
        js_open_project: function (element) {
            var that = this;
            var $element = $(element);

            this.$popbox = $.popbox({
                target: $element,
                bottom: 100,
                left: 70,
                content: $("#popbox_project").html(),
                init: function () {
                    var _popbox = this;
                    $.post("/Project/GetProjects", function (data) {
                        var tmpl = $("#set_project_list").tmpl(data, {
                            check: function (pid) {
                                for (var i in that.data.UserProjects) {
                                    if (that.data.UserProjects[i].Project.Id === pid) return true;
                                }
                                return false;
                            }
                        });
                        _popbox.$element.find(".popbox-menu").html(tmpl);
                    });
                }
            });
            this.stopEventBubble();
        },
        //设置用户项目
        js_set_project: function (element) {

            var that = this;
            var $element = $(element);
            var pid = $element.data("project-id");

            $.ajax({
                type: "post",
                url: "/User/SetUserProject",
                data: { uid: that.data.Id, pid: pid },
                success: function (data) {
                    if (data.code === 200) {
                        var $icon = $element.find(".icon");
                        if ($icon.hasClass("icon-check")) {
                            $icon.removeClass("icon-check");
                            $("#user_project_item_" + pid).remove();
                        }
                        else {
                            $icon.addClass("icon-check");
                            $("#add_project_item").tmpl({ Id: pid, Name: $element.find("span").text() }).insertBefore(".set-project");
                        }
                        that.noty(data.message, "information");
                    }
                },
                error: function () {
                    that.noty("操作失败，请重新尝试！", "error");
                }
            });
        },
        js_remove_project: function (element) {
            var that = this;
            var $element = $(element);
            var pid = $element.data("project-id");
            $.ajax({
                type: "post",
                url: "/User/SetUserProject",
                data: { uid: that.data.Id, pid: pid },
                success: function (data) {
                    if (data.code === 200) {
                        $("#user_project_item_" + pid).remove();
                        that.noty(data.message, "information");
                    }
                },
                error: function () {
                    that.noty("操作失败，请重新尝试！", "error");
                }
            });

        },
        //设置角色
        customScrollbar: function (element) {
            $(element).mCustomScrollbar({
                axis: "y",
                theme: "dark",
                scrollInertia: 500,
                scrollbarPosition: "outside"
            });
        },
        //滚动条
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
    window.locator.openUser = User;
    window.uv = User.fn;

})(jQuery, this);

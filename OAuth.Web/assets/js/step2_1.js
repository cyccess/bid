+(function ($, window) {
    var step = function (element, options) {

        step.fn._init(options);
        step.fn.stopEventBubble();
    };

    step.fn = step.prototype = {

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

            if (that.options.option == 1) {
                $.post("/subject/GetMaterial", that.options, function (data) {
                    that.data = data;
                    that.$sidebarLoading.hide();
                    that.$sidebarContent.html($("#step2_1_edit").tmpl(data));
                    that.customScrollbar(".content");
                    if (that.options.status === 0) {
                        $("#sure").attr("disabled", "disabled")
                        return false;
                    }


                    $("#form1").Validform({
                        label: '.control-label',
                        tiptype: 3,
                        beforeSubmit: function () {
                            $("#form1").ajaxSubmit({
                                beforeSubmit: function (formData, jqForm, options) {

                                },
                                success: function (data, textStatus) {
                                    //that.noty(data.message, 'information');
                                    that._close();
                                    $.noty({
                                        text: data.message,
                                        layout: 'topCenter',
                                        type: 'information',
                                        timeout: 500,
                                        onClosed: function () {
                                            window.location.reload();
                                        }
                                    });
                                },
                                error: function (data, status, e) { },
                                url: "/subject/editmaterial/?timestamp=" + new Date().getTime(),
                                type: "post",
                                dataType: "json",
                                timeout: 600000
                            });
                            return false;
                        }
                    });
                });
            }
            else
            {
                that.$sidebarLoading.hide();
                that.$sidebarContent.html($("#file_upload").html());
                that.customScrollbar(".content");

                $('#uploadify').uploadify({
                    uploader: '/subject/upload',           // 服务器端处理地址
                    swf: '/assets/misc/uploadify.swf',    // 上传使用的 Flash
                    width:80,                          // 按钮的宽度
                    height: 30,                         // 按钮的高度
                    buttonText: "浏览...",                 // 按钮上的文字
                    buttonCursor: 'pointer',                // 按钮的鼠标图标
                    fileObjName: 'Filedata',            // 上传参数名称
                    formData: { 'id': that.options.id },
                    // 两个配套使用
                    fileTypeExts: "*.xls;*.xlsx",             // 扩展名
                    fileTypeDesc: "请选择excel文件",     // 文件说明
                    auto: false,                // 选择之后，自动开始上传
                    multi: false,               // 是否支持同时上传多个文件
                    queueSizeLimit: 1,          // 允许多文件上传的时候，同时上传文件的个数
                    onUploadSuccess: function (file, data, respone) {
                        var msg = $.parseJSON(data);
                        //that._close();
                        $.noty({
                            text: msg.message,
                            layout: 'topCenter',
                            type: 'information',
                            timeout: 500,
                            onClosed: function () {
                                window.location.reload();
                            }
                        });
                        //that.noty(msg.message, 'information');
                    }
                });
                
                $("#btn_upload").click(function () {
                    $('#uploadify').uploadify('upload');
                });
            }
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
            var e = e || window.event; //浏览器兼容性
            var elem = e.target || e.srcElement;

            var compareElement = function (elem, element) {
                return !(elem === element || $.contains(element, elem));
            };

            if (this.isShowSidebar && this.$sidebar) {
                if (compareElement(elem, this.$sidebar[0])) {
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
            var formConfig = {
                ajaxpost: {
                    url: "/subject/EditMaterial",
                    data: that.$validForm.forms.serialize(),
                    success: function (data) {
                        if (data.code === 200) {
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
            that.$validForm.ajaxPost(true);
            return false;
            //编辑保存模块
            //that.$sidebar.find("#sure").on("click", function () {
                //var formConfig = {
                //    ajaxpost: {
                //        url: "/subject/EditMaterial",
                //        data: that.$validForm.forms.serialize() + "&Id=" + that.data.Id,
                //        success: function (data) {
                //            if (data.code === 200) {
                //                that.noty(data.message, "information");
                //            }
                //        },
                //        error: function () {
                //            that.noty("操作失败，请重新尝试！", "error");
                //        }
                //    }
                //}

                //that.$validForm.config(formConfig);
                ////ajax 提交表单
                //that.$validForm.ajaxPost(false);
                //return false;
            //});
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
        js_materiel_remove: function (element) {
            var that = this,
            $this = $(element);
            var idArr = [];
            $(".table-bordered input[type=checkbox]:checked").each(function (i, p) {
                idArr.push($(p).val());
            });
            if (idArr.length == 0) {
                that.noty("未选中任何删除行！", "information");
                return false;
            }
            

            this.$popbox = $.popbox({
                target: $this,
                content: $("#materiel_remove").html(),
                init: function () { },
                sure: function () {
                    $.ajax({
                        type: "post",
                        url: "/subject/RemoveMaterial",
                        data: { id: idArr.join()},
                        success: function (data) {
                            if (data.code === 200) {
                                that._close();
                                $.noty({
                                    text: data.message,
                                    layout: 'topCenter',
                                    type: 'information',
                                    timeout: 500,
                                    onClosed: function () {
                                        window.location.reload();
                                    }
                                });
                                //that.noty(data.message, "information");
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

            //$.ajax({
            //    type: "post",
            //    url: "/Role/SetPart",
            //    data: { userId: that.data.Id, partId: roleId },
            //    success: function (data) {
            //        if (data.code === 200) {
            //            that.noty(data.message, "information");
            //        }
            //    },
            //    error: function () {
            //        that.noty("操作失败，请重新尝试！", "error");
            //    }
            //});
        },
        //获取激活标记
        js_get_active: function (element) {
            var that = this;
            var st_id = "";
            var st_name = "";
            $(".entry-part-content li[class='active'] a").each(function (i, p) {
                st_id += $(p).attr("data-role-id") + ",";
                st_name += $(p).text() + ",";
            });
            $("#ModeID").val(st_id.substring(0, st_id.length - 1));
            $("#ModeName").val(st_name.substring(0, st_name.length - 1));
            that.$sidebar.close();
            if (that.$popbox) {
                that.$popbox.close();
            }
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
    window.locator.openEdit = step;
    window.uv = step.fn;

})(jQuery, this);

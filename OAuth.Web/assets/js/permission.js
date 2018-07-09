~(function ($, window) {
    var permission = {
        init: function () {

            //初始化侧边栏
            $(".sidebar.right").sidebar({ side: "width", speed: 500 });
            this._addEvent();
            this.partId = 0
        },
        _addEvent: function () {
            var that = this;

            //添加角色
            $(".permission-add").on("click", function (e) {
                that._losesFocus(this);
                var $that = $(this),
                    projectId = $that.data("project-id");

                $that.hide();
                $that.after($("#add_part_item").html());

                var valid_form = $(".add-part").Validform({
                    tiptype: function (msg, o, cssctl) { }
                });

                valid_form.addRule([
                    { ele: '[name="Name"]', datatype: "*" },
                    { ele: '[name="Description"]', datatype: "*" }
                ]);


                //确定
                $that.next().on("click", function (event) {
                    var target = event.target,
                        $then = $(this);
                    switch (target.id) {
                        case "sure":
                            valid_form.config({
                                ajaxpost: {
                                    data: valid_form.forms.serialize() + "&projectId=" + projectId,
                                    success: function (data, obj) {
                                        $that.next().remove();
                                        $that.show();
                                        $("#new_part_item").tmpl(data.model).insertBefore($that.parent());
                                        noty(data.message, "information");
                                    },
                                    error: function (data, obj) {
                                        noty("操作失败，请重新尝试！", "error");
                                    }
                                }
                            });
                            valid_form.ajaxPost(false);
                            break;
                        case "cancel":
                            $then.remove();
                            $that.show();
                            break;
                    }

                    return false;
                });

                e.stopPropagation();
            });

            //设置权限
            $(".entries-panel").on("click", ".js-set-permission", function (e) {
                that._losesFocus(this);
                $(".sidebar.right").trigger("sidebar:open");
                $(".sidebar-loading").show();

                var then = $(this),
                    roleId = then.data("roleid"),
                    projectId = then.data("projectid");

                that.getPermission(projectId, roleId);

                return false;
            });

            $(".sidebar").on("click", "a.name,.js-close", function () {
                that._sidebarClose();
            });

            $(window).on("keydown", function (event) {
                var code = event.keyCode;
                if (code === 27) {
                    that._sidebarClose()
                }
            });

            $(document).on("click", function (e) {
                that._sidebarClose(e.target)
                that._losesFocus(e.target);
            });

            //全选
            $('.sidebar').on("ifClicked", ".permission-title input", function () {
                var flag = !this.checked,
                    permissionArr = [],
                    roleId = $(this).data("roleid"),
                    permission_container = $(".permission-container");

                permission_container.find(":checkbox").iCheck(flag ? "check" : "uncheck");

                permission_container.find(":checkbox:checked").each(function () {
                    permissionArr.push($(this).val());
                });

                that.requestSetRoleRight(roleId, permissionArr, flag);
            });

            //勾选菜单
            $('.sidebar').on("ifClicked", ".permission-container input", function (e) {
                e.preventDefault();
                var $then = $(this),
                    flag = !this.checked,
                    $permissionGroup = $then.closest(".role-menu-parent").next(".role-menu-group").find(":checkbox"),
                    $parentCheckbox = $then.closest("ul").prev(".role-menu-parent").find(":checkbox:first"),
                    permissionArr = [],
                    roleId = $then.data("roleid");

                permissionArr.push($then.val());

                if ($parentCheckbox.length > 0 && !$parentCheckbox.prop("checked")) {
                    $parentCheckbox.iCheck(flag ? "check" : "uncheck");
                    permissionArr.push($parentCheckbox.val());
                }

                if ($permissionGroup.length > 0) {
                    $permissionGroup.iCheck(flag ? "check" : "uncheck");
                    $permissionGroup.each(function () {
                        permissionArr.push($(this).val());
                    });
                }

                $.ajax({
                    type: "post",
                    traditional: true,
                    url: "/Role/SetSingleRoleRight",
                    data: { roleId: roleId, permissionArr: permissionArr, isChecked: flag },
                    success: function (data) {
                        that.setAllCheckboxStatus();
                    },
                    error: function () {
                        noty("操作失败，请重新尝试！", "error");
                    }
                });
            });
        },
        /*全选request*/
        requestSetRoleRight: function (roleId, permissionArr, flag) {
            $.ajax({
                type: "post",
                traditional: true,
                url: "/Role/SetRoleRight",
                data: { roleId: roleId, permissionArr: permissionArr, isChecked: flag },
                success: function (data) {
                    noty(data.message, "information");
                },
                error: function () {
                    noty("操作失败，请重新尝试！", "error");
                }
            });
        },
        getPermission: function (pid, rid) {
            var that = this;
            $.ajax({
                type: "post",
                traditional: true,
                url: "/Role/GetRoleRight",
                data: { projectId: pid, roleId: rid },
                success: function (content) {
                    $(".sidebar-panel-wrapper").html(content);
                    $(".sidebar-loading").hide();
                    that._binding_ui_style();
                    that._deletePermission(rid);
                    that.setAllCheckboxStatus();
                },
                error: function () {
                    noty("操作失败，请重新尝试！", "error");
                }
            });
        },
        _binding_ui_style: function () {
            $("input").iCheck({
                checkboxClass: "icheckbox_minimal-red",
                increaseArea: '10%'
            });

            $(".content").mCustomScrollbar({
                axis: "y",
                theme: "dark",
                scrollInertia: 500,
                scrollbarPosition: "outside"
            });
        },
        setAllCheckboxStatus: function () {
            var checkboxAll = $("#chkAll"),
                permission_container = $(".permission-container"),
                uncheck = permission_container.find(":checkbox"),
                check = permission_container.find(":checkbox:checked");

            checkboxAll.iCheck("uncheck");
            if (check.length !== 0 && uncheck.length === check.length) {
                checkboxAll.iCheck("check");
            }
        },
        _deletePermission: function (roleId) {

            var that = this;
            $("a.delete").popbox({
                layout: "right",
                right: 5,
                content: $("#part_item_delete").html(),
                init: function () {
                    //
                },
                sure: function () {
                    $.ajax({
                        type: "post",
                        url: "/Role/Delete",
                        data: { roleId: roleId },
                        success: function (data) {
                            if (data.code === 200) {
                                that._losesFocus(this);
                                $('[part-id="' + roleId + '"]').remove();
                                noty(data.message, "information");
                            }
                        },
                        error: function () {
                            noty("操作失败，请重新尝试！", "error");
                        }
                    });
                    return false;
                }
            });
        },
        _sidebarClose: function (target) {
            var $sidebar = $(".sidebar.right");
            if ($sidebar.length > 0) {
                if (!(target === $sidebar[0] || $.contains($sidebar[0], target))) {
                    $sidebar.find(".sidebar-panel-wrapper").html("");
                    $sidebar.trigger("sidebar:close");
                }
            }
            return false;
        },
        _losesFocus: function (target) {
            var $partNew = $(".entry-new .m_10"),
                $popbox = $(".popbox");

            if (existsObj(target, $partNew)) {
                $partNew.prev().show();
                $partNew.remove();
            }

            if (existsObj(target, $popbox)) {
                $popbox.remove();
            }

            this._sidebarClose(target);

            function existsObj(targetObj, $obj) {
                if ($obj.length > 0) {
                    return !(targetObj === $obj[0] || $.contains($obj[0], targetObj));
                }
                return false;
            }
        }
    };

    window.permission = $.permission = permission;

}(this.jQuery, this));

/*
 * 角色头部菜单
 * 包括：新建角色、复制角色、重命名，以及删除角色
 */
+(function ($, window) {

    var menuSwitch = function (element) {

        element = element || this;

        return menuSwitch.fn._init(element);
    }

    menuSwitch.fn = menuSwitch.prototype = {

        constructor: menuSwitch,

        _init: function (element) {
            var that = this;

            this.$element = $(element);

            this.partId = that.$element.data("partid");
            this.projectId = that.$element.data("projectid");
            this.partName = that.$element.prev().find("h4").text();
            this.$validForm = null;
            this.$popbox_step = null;

            $.permission._losesFocus();

            $.popbox({
                content: $("#part_header_menu").html(),
                target: that.$element,
                init: function () { }
            });

            this.js_step(0);
        },
        js_step: function (step_id) {
            var that = this;

            this.stepId = step_id;
            this.$popbox_step = $(".popbox-step");
            this.$popbox_step.html($("#switchWhen_" + step_id).html());

            this._init_step();
        },
        _init_step: function () {
            var that = this,
                rule = null;
                
            rule = [{ ele: '[name="Name"]', datatype: "*" }];
            if (this.stepId === 1) {
                rule.push({ ele: '[name="Description"]', datatype: "*" });
                that._validForm("#part-copy-form", rule);
            }
            if (this.stepId === 2) {
                that._validForm("#part-rename-form", rule);
                var $name = that.$popbox_step.find(rule[0].ele);
                $name.val(this.partName);
            }
        },
        js_create_new: function () {
            $(".permission-add").click();
            this._stopEventBubble(event);
        },
        js_copy: function () {
            var that = this;
            that.$validForm.config({
                ajaxpost: {
                    data: that.$validForm.forms.serialize() + "&ProjectId=" + that.projectId + "&Id=" + that.partId,
                    url: "/Role/Copy",
                    success: function (data, obj) {
                        if (data.code === 200) {
                            $("#new_part_item").tmpl(data.model).insertBefore($(".entry-new"));
                            $.popbox.fn.close();
                            noty(data.message, "information");
                        }
                    },
                    error: function (data, obj) {
                        noty("操作失败，请重新尝试！", "error");
                    }
                }
            });

            that.$validForm.ajaxPost(false);
            that._stopEventBubble(event);
        },
        /*
         * 重命名
         */
        js_rename: function () {
            var that = this;

            that.$validForm.config({
                ajaxpost: {
                    url: "/Role/Rename",
                    data: that.$validForm.forms.serialize() + "&ProjectId=" + that.projectId + "&Id=" + that.partId,
                    success: function (data) {
                        if (data.code === 200) {

                            var name = that.$popbox_step.find('[name="Name"]').val();
                            $('[part-id="' + that.partId + '"]').find(".entry-header-title h4").text(name);

                            $.popbox.fn.close();

                            noty(data.message, "information");
                        }
                    },
                    error: function () {
                        noty("操作失败，请重新尝试！", "error");
                    }
                }
            });

            that.$validForm.ajaxPost(false);
            that._stopEventBubble(event);

        },
        js_delete: function () {
            var that = this;
            $.ajax({
                type: "post",
                url: "/Role/Delete",
                data: { roleId: that.partId },
                success: function (data) {
                    if (data.code === 200) {
                        $.popbox.fn.close();
                        $('[part-id="' + that.partId + '"]').remove();
                        noty(data.message, "information");
                    }
                },
                error: function () {
                    noty("操作失败，请重新尝试！", "error");
                }
            });
        },
        /*
         * 表单验证
         */
        _validForm: function (eleForm, rule) {
            var that = this,
                $eleForm = $(eleForm);

            that.$validForm = $eleForm.Validform({
                tiptype: function (msg, o, cssctl) { }
            });

            if (rule) {
                that.$validForm.addRule(rule);
            }

            return that;
        },
        _stopEventBubble: function (event) {
            var e = event || window.event;
            if (e && e.stopPropagation) {
                e.stopPropagation();
            }
            else {
                e.cancelBubble = true;
            }

            if (e && e.preventDefault) {
                e.preventDefault()
            }
            else {
                e.returnValue = false;
            }
        }
    };

    menuSwitch.fn._init.prototype = menuSwitch.fn;

    /*! 使用jQ方式调用窗口 */
    $.fn.menuSwitch = function () {
        this.bind('click', function () { menuSwitch.apply(this, null); return false; });
        return this;
    };

    window.menuSwitch = $.menuSwitch = menuSwitch;

}(jQuery, this));


function noty(message, type) {
    $.noty.closeAll();
    $.noty({
        layout: "topCenter",
        text: message,
        type: type,
        closeButton: false
    });
}
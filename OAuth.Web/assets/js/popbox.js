
~(function ($, window) {

    var popbox = function (config) {

        config = config || {};

        config.target = config.target || $(this),
            setting = popbox.setting;

        // 合并默认配置
        for (var i in setting) {
            if (config[i] === undefined) config[i] = setting[i];
        }

        // 自定义按钮队列
        config.button = config.button || [];

        config.sure &&
        config.button.push({
            id: config.sureBtn,
            callback: config.sure
        });

        config.cancel &&
        config.button.push({
            id: config.cancelBtn,
            callback: config.cancel
        });

        return popbox.fn._init(config);
    }

    popbox.fn = popbox.prototype = {

        constructor: popbox,

        _init: function (config) {
            this.$element = this.$element || null;
            this.config = config;

            this._box();
            this.button.apply(this, config.button);
            this._addEvent();

            if (config.init) {
                config.init.call(this, window);
            }

            return this;
        },
        _box: function () {

            !this.$element || this.$element.close();

            this.$element = $(this.config.content),
                _css = null,
                offset = this.config.target.offset(),
                _position = { x: offset.left, y: offset.top + this.config.target.outerHeight() };

            this.$element.appendTo("body");

            var popboxWidth = this.$element.width();

            if (_position.x + popboxWidth > $(document).width()) {
                this.config.layout = "right";
                this.config.right = 20;
            }

            _css = { "left": _position.x, "top": _position.y };

            if (this.config.layout === "right") {
                _css.left = "auto";
                _css.right = this.config.right;
            }
            else {
                if (this.config.left !== 'auto') {
                    _css.left = _css.left + this.config.left;
                }
            }

            if (this.config.bottom !== "auto") {
                delete _css.top;
                _css.bottom = this.config.bottom;
            }else if (this.config.top !== 'auto') {
                _css.top = _css.top + this.config.top;
            }

            this.$element.css(_css);
            this.$element.show(20);

            this.$element.close = function () {
                this.remove();
            }
            return this;
        },
        button: function () {
            var that = this, buttons = [],
                listeners = that._listeners = that._listeners || {},
                ags = [].slice.call(arguments),
                item, value, id;

            for (var i in ags) {
                item = ags[i];
                id = item.id;

                if (!listeners[id])
                    listeners[id] = {};

                if (item.callback)
                    listeners[id].callback = item.callback;
            }
            return that;
        },
        close: function () {
            this.$element.close();
        },
        _addEvent: function () {
            var that = this;

            this.$element.on("click", function (event) {
                var target = event.target, btnID;

                if (target.disabled) return false; // IE BUG

                if (target === $(this).find('.cancel')[0]) {
                    that.close();
                    return false;
                }
                else {
                    if (!$(target).hasClass("btn")) {
                        return false;
                    }

                    that._click(target.id);
                }
                return false;
            });

            $(window).on("keydown", function (event) {
                if (event.keyCode === 27) {
                    that.close();
                }
            });
        },
        _click: function (id) {
            var that = this,
                fn = that._listeners[id] && that._listeners[id].callback;
            return typeof fn !== "function" || fn.call(that, window) !== false ? that.$element.close() : that;
        }
    };

    popbox.fn._init.prototype = popbox.fn;

    /*! 使用jQ方式调用窗口 */
    $.fn.popbox = function () {
        var config = arguments;
        this.bind('click', function () { popbox.apply(this, config); return false; });
        return this;
    };

    window.popbox = $.popbox = popbox;

    /*! popbox 的全局默认配置 */
    popbox.setting = {
        layout: "left",
        right: "auto",
        left: "auto",
        top: "auto",
        bottom: "auto",
        content: null,
        target: null,           //当前触发的按钮 例如：$('.btn')
        sure: null,             //确定按钮回调函数
        sureBtn: "sure",        //确定按钮ID
        cancelBtn: "cancel",    //取消按钮ID
        cancel: null,           //取消按钮回调函数
        init: null,             //对话框初始化后执行的函数
        close: null             //对话框关闭前执行的函数
    };

}(jQuery, this));

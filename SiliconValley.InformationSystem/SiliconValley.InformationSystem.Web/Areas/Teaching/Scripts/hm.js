(function () {
    var h = {}
        , mt = {}
        , c = {
            id: "382f81c966395258f239157654081890",
            dm: ["17sucai.com"],
            js: "tongji.baidu.com/hm-web/js/",
            etrk: [],
            cetrk: [],
            icon: '',
            ctrk: false,
            align: -1,
            nv: -1,
            vdur: 1800000,
            age: 31536000000,
            rec: 0,
            rp: [],
            trust: 0,
            vcard: 0,
            qiao: 0,
            lxb: 0,
            kbtrk: 0,
            pt: 0,
            spa: 0,
            aet: '',
            hca: '0C3AD89B95CEF364',
            conv: 0,
            med: 0,
            cvcc: {
                q: /tencent:\/\/|qq\.(com|htm)|kefu|openkf|swt|zoos|53kf|doyoo|looyu|leyu|zixun|chat|talk|openQQ|open_ask|online/i
            },
            cvcf: ['user_submit'],
            apps: ''
        };
    var r = void 0
        , s = !0
        , t = null
        , v = !1;
    mt.cookie = {};
    mt.cookie.set = function (b, a, e) {
        var d;
        e.J && (d = new Date,
            d.setTime(d.getTime() + e.J));
        document.cookie = b + "=" + a + (e.domain ? "; domain=" + e.domain : "") + (e.path ? "; path=" + e.path : "") + (d ? "; expires=" + d.toGMTString() : "") + (e.$b ? "; secure" : "")
    }
        ;
    mt.cookie.get = function (b) {
        return (b = RegExp("(^| )" + b + "=([^;]*)(;|$)").exec(document.cookie)) ? b[2] : t
    }
        ;
    mt.cookie.xb = function (b, a) {
        try {
            var e = "Hm_ck_" + +new Date;
            mt.cookie.set(e, "is-cookie-enabled", {
                domain: b,
                path: a,
                J: r
            });
            var d = "is-cookie-enabled" === mt.cookie.get(e) ? "1" : "0";
            mt.cookie.set(e, "", {
                domain: b,
                path: a,
                J: -1
            });
            return d
        } catch (g) {
            return "0"
        }
    }
        ;
    mt.lang = {};
    mt.lang.e = function (b, a) {
        return "[object " + a + "]" === {}.toString.call(b)
    }
        ;
    mt.lang.Da = function (b) {
        return mt.lang.e(b, "Number") && isFinite(b)
    }
        ;
    mt.lang.ia = function () {
        return mt.lang.e(c.aet, "String")
    }
        ;
    mt.lang.g = function (b) {
        return b.replace ? b.replace(/'/g, "'0").replace(/\*/g, "'1").replace(/!/g, "'2") : b
    }
        ;
    mt.lang.trim = function (b) {
        return b.replace(/^\s+|\s+$/g, "")
    }
        ;
    mt.lang.H = function (b, a) {
        var e = v;
        if (b == t || !mt.lang.e(b, "Array") || a === r)
            return e;
        if (Array.prototype.indexOf)
            e = -1 !== b.indexOf(a);
        else
            for (var d = 0; d < b.length; d++)
                if (b[d] === a) {
                    e = s;
                    break
                }
        return e
    }
        ;
    (function () {
        var b = mt.lang;
        mt.f = {};
        mt.f.xa = function (a) {
            return document.getElementById(a)
        }
            ;
        mt.f.eb = function (a) {
            if (!a)
                return t;
            try {
                a = String(a);
                if (0 === a.indexOf("!HMCC!"))
                    return document.querySelector(a.substring(6, a.length));
                for (var b = a.split(">"), d = document.body, g = b.length - 1; 0 <= g; g--)
                    if (-1 < b[g].indexOf("#")) {
                        var f = b[g].split("#")[1];
                        (d = document.getElementById(f)) || (d = document.getElementById(decodeURIComponent(f)));
                        b = b.splice(g + 1, b.length - (g + 1));
                        break
                    }
                for (a = 0; d && a < b.length;) {
                    var n = String(b[a]).toLowerCase();
                    if (!("html" === n || "body" === n)) {
                        var g = 0
                            , l = b[a].match(/\[(\d+)\]/i)
                            , f = [];
                        if (l)
                            g = l[1] - 1,
                                n = n.split("[")[0];
                        else if (1 !== d.childNodes.length) {
                            for (var p = 0, w = 0, m = d.childNodes.length; w < m; w++) {
                                var k = d.childNodes[w];
                                1 === k.nodeType && k.nodeName.toLowerCase() === n && p++;
                                if (1 < p)
                                    return t
                            }
                            if (1 !== p)
                                return t
                        }
                        for (p = 0; p < d.childNodes.length; p++)
                            1 === d.childNodes[p].nodeType && d.childNodes[p].nodeName.toLowerCase() === n && f.push(d.childNodes[p]);
                        if (!f[g])
                            return t;
                        d = f[g]
                    }
                    a++
                }
                return d
            } catch (q) {
                return t
            }
        }
            ;
        mt.f.ga = function (a, b) {
            var d = []
                , g = [];
            if (!a)
                return g;
            for (; a.parentNode != t;) {
                for (var f = 0, n = 0, l = a.parentNode.childNodes.length, p = 0; p < l; p++) {
                    var w = a.parentNode.childNodes[p];
                    if (w.nodeName === a.nodeName && (f++ ,
                        w === a && (n = f),
                        0 < n && 1 < f))
                        break
                }
                if ((l = "" !== a.id) && b) {
                    d.unshift("#" + encodeURIComponent(a.id));
                    break
                } else
                    l && (l = "#" + encodeURIComponent(a.id),
                        l = 0 < d.length ? l + ">" + d.join(">") : l,
                        g.push(l)),
                        d.unshift(encodeURIComponent(String(a.nodeName).toLowerCase()) + (1 < f ? "[" + n + "]" : ""));
                a = a.parentNode
            }
            g.push(d.join(">"));
            return g
        }
            ;
        mt.f.jb = function (a) {
            return (a = mt.f.ga(a, s)) && a.length ? String(a[0]) : ""
        }
            ;
        mt.f.ib = function (a) {
            return mt.f.ga(a, v)
        }
            ;
        mt.f.Ub = function (a, b) {
            for (b = b.toUpperCase(); (a = a.parentNode) && 1 == a.nodeType;)
                if (a.tagName == b)
                    return a;
            return t
        }
            ;
        mt.f.cb = function (a) {
            return 9 === a.nodeType ? a : a.ownerDocument || a.document
        }
            ;
        mt.f.Vb = function (a) {
            var b = {
                top: 0,
                left: 0
            };
            if (!a)
                return b;
            var d = mt.f.cb(a).documentElement;
            "undefined" !== typeof a.getBoundingClientRect && (b = a.getBoundingClientRect());
            return {
                top: b.top + (window.pageYOffset || d.scrollTop) - (d.clientTop || 0),
                left: b.left + (window.pageXOffset || d.scrollLeft) - (d.clientLeft || 0)
            }
        }
            ;
        mt.f.getAttribute = function (a, b) {
            var d = a.getAttribute && a.getAttribute(b) || t;
            if (!d && a.attributes && a.attributes.length)
                for (var g = a.attributes, f = g.length, n = 0; n < f; n++)
                    g[n].nodeName === b && (d = g[n].nodeValue);
            return d
        }
            ;
        mt.f.R = function (a) {
            var b = "document";
            a.tagName !== r && (b = a.tagName);
            return b.toLowerCase()
        }
            ;
        mt.f.mb = function (a) {
            var e = "";
            a.textContent ? e = b.trim(a.textContent) : a.innerText && (e = b.trim(a.innerText));
            e && (e = e.replace(/\s+/g, " ").substring(0, 255));
            return e
        }
            ;
        mt.f.ya = function (a, e) {
            var d = mt.f.R(a);
            "input" === d && e && ("button" === a.type || "submit" === a.type) ? d = b.trim(a.value) || "" : "input" === d && !e && "password" !== a.type ? d = b.trim(a.value) || "" : "img" === d ? (d = mt.f.getAttribute,
                d = d(a, "alt") || d(a, "title") || d(a, "src")) : d = "body" === d || "html" === d ? ["(hm-default-content-for-", d, ")"].join("") : mt.f.mb(a);
            return String(d).substring(0, 255)
        }
            ;
        (function () {
            (mt.f.Ab = function () {
                function a() {
                    if (!a.U) {
                        a.U = s;
                        for (var d = 0, b = g.length; d < b; d++)
                            g[d]()
                    }
                }
                function b() {
                    try {
                        document.documentElement.doScroll("left")
                    } catch (d) {
                        setTimeout(b, 1);
                        return
                    }
                    a()
                }
                var d = v, g = [], f;
                document.addEventListener ? f = function () {
                    document.removeEventListener("DOMContentLoaded", f, v);
                    a()
                }
                    : document.attachEvent && (f = function () {
                        "complete" === document.readyState && (document.detachEvent("onreadystatechange", f),
                            a())
                    }
                    );
                (function () {
                    if (!d)
                        if (d = s,
                            "complete" === document.readyState)
                            a.U = s;
                        else if (document.addEventListener)
                            document.addEventListener("DOMContentLoaded", f, v),
                                window.addEventListener("load", a, v);
                        else if (document.attachEvent) {
                            document.attachEvent("onreadystatechange", f);
                            window.attachEvent("onload", a);
                            var g = v;
                            try {
                                g = window.frameElement == t
                            } catch (l) { }
                            document.documentElement.doScroll && g && b()
                        }
                }
                )();
                return function (d) {
                    a.U ? d() : g.push(d)
                }
            }()).U = v
        }
        )();
        return mt.f
    }
    )();
    mt.event = {};
    mt.event.d = function (b, a, e) {
        b.attachEvent ? b.attachEvent("on" + a, function (a) {
            e.call(b, a)
        }) : b.addEventListener && b.addEventListener(a, e, v)
    }
        ;
    mt.event.preventDefault = function (b) {
        b.preventDefault ? b.preventDefault() : b.returnValue = v
    }
        ;
    (function () {
        var b = mt.event;
        mt.i = {};
        mt.i.Ca = /msie (\d+\.\d+)/i.test(navigator.userAgent);
        mt.i.vb = /msie (\d+\.\d+)/i.test(navigator.userAgent) ? document.documentMode || +RegExp.$1 : r;
        mt.i.cookieEnabled = navigator.cookieEnabled;
        mt.i.javaEnabled = navigator.javaEnabled();
        mt.i.language = navigator.language || navigator.browserLanguage || navigator.systemLanguage || navigator.userLanguage || "";
        mt.i.Db = (window.screen.width || 0) + "x" + (window.screen.height || 0);
        mt.i.colorDepth = window.screen.colorDepth || 0;
        mt.i.S = function () {
            var a;
            a = a || document;
            return parseInt(window.pageYOffset || a.documentElement.scrollTop || a.body && a.body.scrollTop || 0, 10)
        }
            ;
        mt.i.L = function () {
            var a = document;
            return parseInt(window.innerHeight || a.documentElement.clientHeight || a.body && a.body.clientHeight || 0, 10)
        }
            ;
        mt.i.orientation = 0;
        (function () {
            function a() {
                var a = 0;
                window.orientation !== r && (a = window.orientation);
                screen && (screen.orientation && screen.orientation.angle !== r) && (a = screen.orientation.angle);
                mt.i.orientation = a
            }
            a();
            b.d(window, "orientationchange", a)
        }
        )();
        return mt.i
    }
    )();
    mt.o = {};
    mt.o.parse = function (b) {
        return (new Function("return (" + b + ")"))()
    }
        ;
    mt.o.stringify = function () {
        function b(a) {
            /["\\\x00-\x1f]/.test(a) && (a = a.replace(/["\\\x00-\x1f]/g, function (a) {
                var d = e[a];
                if (d)
                    return d;
                d = a.charCodeAt();
                return "\\u00" + Math.floor(d / 16).toString(16) + (d % 16).toString(16)
            }));
            return '"' + a + '"'
        }
        function a(a) {
            return 10 > a ? "0" + a : a
        }
        var e = {
            "\b": "\\b",
            "\t": "\\t",
            "\n": "\\n",
            "\f": "\\f",
            "\r": "\\r",
            '"': '\\"',
            "\\": "\\\\"
        };
        return function (d) {
            switch (typeof d) {
                case "undefined":
                    return "undefined";
                case "number":
                    return isFinite(d) ? String(d) : "null";
                case "string":
                    return b(d);
                case "boolean":
                    return String(d);
                default:
                    if (d === t)
                        return "null";
                    if (d instanceof Array) {
                        var g = ["["], f = d.length, e, l, p;
                        for (l = 0; l < f; l++)
                            switch (p = d[l],
                            typeof p) {
                                case "undefined":
                                case "function":
                                case "unknown":
                                    break;
                                default:
                                    e && g.push(","),
                                        g.push(mt.o.stringify(p)),
                                        e = 1
                            }
                        g.push("]");
                        return g.join("")
                    }
                    if (d instanceof Date)
                        return '"' + d.getFullYear() + "-" + a(d.getMonth() + 1) + "-" + a(d.getDate()) + "T" + a(d.getHours()) + ":" + a(d.getMinutes()) + ":" + a(d.getSeconds()) + '"';
                    e = ["{"];
                    l = mt.o.stringify;
                    for (f in d)
                        if (Object.prototype.hasOwnProperty.call(d, f))
                            switch (p = d[f],
                            typeof p) {
                                case "undefined":
                                case "unknown":
                                case "function":
                                    break;
                                default:
                                    g && e.push(","),
                                        g = 1,
                                        e.push(l(f) + ":" + l(p))
                            }
                    e.push("}");
                    return e.join("")
            }
        }
    }();
    mt.localStorage = {};
    mt.localStorage.Y = function () {
        if (!mt.localStorage.l)
            try {
                mt.localStorage.l = document.createElement("input"),
                    mt.localStorage.l.type = "hidden",
                    mt.localStorage.l.style.display = "none",
                    mt.localStorage.l.addBehavior("#default#userData"),
                    document.getElementsByTagName("head")[0].appendChild(mt.localStorage.l)
            } catch (b) {
                return v
            }
        return s
    }
        ;
    mt.localStorage.set = function (b, a, e) {
        var d = new Date;
        d.setTime(d.getTime() + e || 31536E6);
        try {
            window.localStorage ? (a = d.getTime() + "|" + a,
                window.localStorage.setItem(b, a)) : mt.localStorage.Y() && (mt.localStorage.l.expires = d.toUTCString(),
                    mt.localStorage.l.load(document.location.hostname),
                    mt.localStorage.l.setAttribute(b, a),
                    mt.localStorage.l.save(document.location.hostname))
        } catch (g) { }
    }
        ;
    mt.localStorage.get = function (b) {
        if (window.localStorage) {
            if (b = window.localStorage.getItem(b)) {
                var a = b.indexOf("|")
                    , e = b.substring(0, a) - 0;
                if (e && e > (new Date).getTime())
                    return b.substring(a + 1)
            }
        } else if (mt.localStorage.Y())
            try {
                return mt.localStorage.l.load(document.location.hostname),
                    mt.localStorage.l.getAttribute(b)
            } catch (d) { }
        return t
    }
        ;
    mt.localStorage.remove = function (b) {
        if (window.localStorage)
            window.localStorage.removeItem(b);
        else if (mt.localStorage.Y())
            try {
                mt.localStorage.l.load(document.location.hostname),
                    mt.localStorage.l.removeAttribute(b),
                    mt.localStorage.l.save(document.location.hostname)
            } catch (a) { }
    }
        ;
    mt.sessionStorage = {};
    mt.sessionStorage.set = function (b, a) {
        try {
            window.sessionStorage && window.sessionStorage.setItem(b, a)
        } catch (e) { }
    }
        ;
    mt.sessionStorage.get = function (b) {
        try {
            return window.sessionStorage ? window.sessionStorage.getItem(b) : t
        } catch (a) {
            return t
        }
    }
        ;
    mt.sessionStorage.remove = function (b) {
        try {
            window.sessionStorage && window.sessionStorage.removeItem(b)
        } catch (a) { }
    }
        ;
    mt.Ka = {};
    mt.Ka.log = function (b, a) {
        var e = new Image
            , d = "mini_tangram_log_" + Math.floor(2147483648 * Math.random()).toString(36);
        window[d] = e;
        e.onload = function () {
            e.onload = t;
            e = window[d] = t;
            a && a(b)
        }
            ;
        e.src = b
    }
        ;
    mt.oa = {};
    mt.oa.nb = function () {
        var b = "";
        if (navigator.plugins && navigator.mimeTypes.length) {
            var a = navigator.plugins["Shockwave Flash"];
            a && a.description && (b = a.description.replace(/^.*\s+(\S+)\s+\S+$/, "$1"))
        } else if (window.ActiveXObject)
            try {
                if (a = new ActiveXObject("ShockwaveFlash.ShockwaveFlash"))
                    (b = a.GetVariable("$version")) && (b = b.replace(/^.*\s+(\d+),(\d+).*$/, "$1.$2"))
            } catch (e) { }
        return b
    }
        ;
    mt.oa.Tb = function (b, a, e, d, g) {
        return '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" id="' + b + '" width="' + e + '" height="' + d + '"><param name="movie" value="' + a + '" /><param name="flashvars" value="' + (g || "") + '" /><param name="allowscriptaccess" value="always" /><embed type="application/x-shockwave-flash" name="' + b + '" width="' + e + '" height="' + d + '" src="' + a + '" flashvars="' + (g || "") + '" allowscriptaccess="always" /></object>'
    }
        ;
    mt.url = {};
    mt.url.m = function (b, a) {
        var e = b.match(RegExp("(^|&|\\?|#)(" + a + ")=([^&#]*)(&|$|#)", ""));
        return e ? e[3] : t
    }
        ;
    mt.url.Wb = function (b) {
        return (b = b.match(/^(https?:)\/\//)) ? b[1] : t
    }
        ;
    mt.url.gb = function (b) {
        return (b = b.match(/^(https?:\/\/)?([^\/\?#]*)/)) ? b[2].replace(/.*@/, "") : t
    }
        ;
    mt.url.K = function (b) {
        return (b = mt.url.gb(b)) ? b.replace(/:\d+$/, "") : b
    }
        ;
    mt.url.ga = function (b) {
        return (b = b.match(/^(https?:\/\/)?[^\/]*(.*)/)) ? b[2].replace(/[\?#].*/, "").replace(/^$/, "/") : t
    }
        ;
    (function () {
        function b() {
            for (var a = v, b = document.getElementsByTagName("script"), d = b.length, d = 100 < d ? 100 : d, g = 0; g < d; g++) {
                var f = b[g].src;
                if (f && 0 === f.indexOf("https://hm.baidu.com/h")) {
                    a = s;
                    break
                }
            }
            return a
        }
        return h.bb = b
    }
    )();
    var A = h.bb;
    h.z = {
        Xb: "http://tongji.baidu.com/hm-web/welcome/ico",
        Ha: "hm.baidu.com/hm.gif",
        Ra: /^(tongji|hmcdn).baidu.com$/,
        Ma: "tongji.baidu.com",
        rb: "hmmd",
        tb: "hmpl",
        Nb: "utm_medium",
        qb: "hmkw",
        Pb: "utm_term",
        ob: "hmci",
        Mb: "utm_content",
        ub: "hmsr",
        Ob: "utm_source",
        pb: "hmcu",
        Lb: "utm_campaign",
        I: 0,
        D: Math.round(+new Date / 1E3),
        protocol: "https:" === document.location.protocol ? "https:" : "http:",
        la: A() || "https:" === document.location.protocol ? "https:" : "http:",
        Yb: 0,
        Rb: 6E5,
        yb: 6E5,
        Zb: 5E3,
        Sb: 5,
        ba: 1024,
        Qb: 1,
        V: 2147483647,
        La: "hca bs kb cc cf ci ck cl cm cp cu cw ds vl ep et fl ja ln lo lt rnd si su v cv lv api sn p ct u tt hh".split(" "),
        M: s,
        ua: ["a", "input", "button"],
        Ta: {
            id: "data-hm-id",
            aa: "data-hm-class",
            ra: "data-hm-xpath",
            content: "data-hm-content",
            pa: "data-hm-tag",
            link: "data-hm-link"
        },
        ta: "data-hm-enabled",
        sa: "data-hm-disabled",
        zb: "https://hmcdn.baidu.com/static/tongji/plugins/",
        Ga: ["UrlChangeTracker"]
    };
    (function () {
        var b = {
            A: {},
            d: function (a, b) {
                this.A[a] = this.A[a] || [];
                this.A[a].push(b)
            },
            F: function (a, b) {
                this.A[a] = this.A[a] || [];
                for (var d = this.A[a].length, g = 0; g < d; g++)
                    this.A[a][g](b)
            }
        };
        return h.G = b
    }
    )();
    (function () {
        function b(b, d) {
            var g = document.createElement("script");
            g.charset = "utf-8";
            a.e(d, "Function") && (g.readyState ? g.onreadystatechange = function () {
                if ("loaded" === g.readyState || "complete" === g.readyState)
                    g.onreadystatechange = t,
                        d()
            }
                : g.onload = function () {
                    d()
                }
            );
            g.src = b;
            var f = document.getElementsByTagName("script")[0];
            f.parentNode.insertBefore(g, f)
        }
        var a = mt.lang;
        return h.load = b
    }
    )();
    (function () {
        var b = mt.lang
            , a = mt.f
            , e = {
                P: function (d, b) {
                    return function (f) {
                        var n = f.target || f.srcElement;
                        if (n) {
                            var l = n.getAttribute(d.X);
                            f = f.clientX + ":" + f.clientY;
                            if (l && l === f)
                                n.removeAttribute(d.X);
                            else if (b && 0 < b.length && (n = a.ib(n)) && n.length)
                                if (l = n.length,
                                    f = n[n.length - 1],
                                    1E4 > l * f.split(">").length)
                                    for (f = 0; f < l; f++)
                                        e.Ja(d, n[f]);
                                else
                                    e.Ja(d, f)
                        }
                    }
                },
                Ja: function (a, g) {
                    for (var f = {}, e = String(g).split(">").length, l = 0; l < e; l++)
                        f[g] = "",
                            g = g.substring(0, g.lastIndexOf(">"));
                    a && (b.e(a, "Object") && a.va) && a.va(f)
                },
                Bb: function (a, b) {
                    return function (e) {
                        (e.target || e.srcElement).setAttribute(a.X, e.clientX + ":" + e.clientY);
                        a && a.r && (b ? a.r(b) : a.r("#" + encodeURIComponent(this.id), e.type))
                    }
                }
            };
        return h.da = e
    }
    )();
    (function () {
        var b = mt.f
            , a = mt.o
            , e = mt.event
            , d = mt.lang
            , g = h.da
            , f = {
                X: "HM_ce",
                Na: function () {
                    if (c.cetrk && c.cetrk.length) {
                        e.d(document, "click", g.P(f, c.cetrk));
                        for (var d = 0, l = c.cetrk.length; d < l; d++) {
                            var p;
                            try {
                                p = a.parse(decodeURIComponent(String(c.cetrk[d])))
                            } catch (w) {
                                p = {}
                            }
                            var m = p.p || "";
                            -1 === m.indexOf(">") && (0 === m.indexOf("#") && (m = m.substring(1)),
                                (m = b.xa(m)) && e.d(m, "click", g.Bb(f, p)))
                        }
                    }
                },
                va: function (b) {
                    if (c.cetrk && c.cetrk.length)
                        for (var d = 0, e = c.cetrk.length; d < e; d++) {
                            var g;
                            try {
                                g = a.parse(decodeURIComponent(String(c.cetrk[d])))
                            } catch (m) {
                                g = {}
                            }
                            b.hasOwnProperty(g.p) && f.r(g)
                        }
                },
                r: function (a) {
                    h.c.b.et = 7;
                    var g = a && a.k || ""
                        , g = d.g(g)
                        , e = [];
                    if (a && a.a && d.e(a.a, "Object"))
                        for (var f in a.a)
                            if (a.a.hasOwnProperty(f)) {
                                e.push(d.g(f));
                                var m = b.eb(a.a[f] || "")
                                    , m = m ? b.ya(m, v) : "";
                                e.push(d.g(m))
                            }
                    a = "";
                    e.length && (a = "*" + e.join("*"));
                    h.c.b.ep = "ce!_iden*" + g + a;
                    h.c.j()
                }
            };
        h.G.d("pv-b", f.Na);
        return f
    }
    )();
    (function () {
        var b = mt.lang
            , a = mt.f
            , e = mt.event
            , d = mt.i
            , g = h.z
            , f = h.G
            , n = +new Date
            , l = []
            , p = {
                P: function () {
                    return function (d) {
                        if (h.c && h.c.M && c.aet && c.aet.length) {
                            var e = d.target || d.srcElement;
                            if (e) {
                                var k = h.c.ua
                                    , q = a.getAttribute(e, g.ta) != t ? s : v;
                                if (a.getAttribute(e, g.sa) == t)
                                    if (q)
                                        p.Z(p.fa(e, d));
                                    else {
                                        var f = a.R(e);
                                        if (b.H(k, "*") || b.H(k, f))
                                            p.Z(p.fa(e, d));
                                        else
                                            for (; e.parentNode != t;) {
                                                var q = e.parentNode
                                                    , f = a.R(q)
                                                    , l = "a" === f && b.H(k, "a") ? s : v
                                                    , f = "button" === f && b.H(k, "button") ? s : v
                                                    , u = a.getAttribute(q, g.ta) != t ? s : v;
                                                if (a.getAttribute(q, g.sa) == t && (l || f || u)) {
                                                    p.Z(p.fa(q, d));
                                                    break
                                                }
                                                e = e.parentNode
                                            }
                                    }
                            }
                        }
                    }
                },
                fa: function (e, f) {
                    var k = {}
                        , q = g.Ta;
                    k.id = a.getAttribute(e, q.id) || a.getAttribute(e, "id") || "";
                    k.aa = a.getAttribute(e, q.aa) || a.getAttribute(e, "class") || "";
                    k.ra = a.getAttribute(e, q.ra) || a.jb(e);
                    k.content = a.getAttribute(e, q.content) || a.ya(e, s);
                    k.pa = a.getAttribute(e, q.pa) || a.R(e);
                    k.link = a.getAttribute(e, q.link) || a.getAttribute(e, "href") || "";
                    k.type = f.type || "click";
                    q = b.Da(e.offsetTop) ? e.offsetTop : 0;
                    "click" === f.type ? q = d.Ca ? f.clientY + Math.max(document.documentElement.scrollTop, document.body.scrollTop) : f.pageY : "touchend" === f.type && (f.ma && b.e(f.ma.changedTouches, "Array") && f.ma.changedTouches.length) && (q = f.ma.changedTouches[0].pageY);
                    k.Kb = q;
                    return k
                },
                Z: function (a) {
                    var d = b.g;
                    a = [+new Date - (h.c.B !== r ? h.c.B : n), d(a.id), d(a.aa), d(a.pa), d(a.ra), d(a.link), d(a.content), a.type, a.Kb].join("*");
                    p.$(a);
                    b.e(this.O(), "Function") && this.O()()
                },
                $: function (a) {
                    a.length > g.ba || (encodeURIComponent(l.join("!") + a).length > g.ba && (p.r(l.join("!")),
                        l = []),
                        l.push(a))
                },
                r: function (a) {
                    h.c.b.et = 5;
                    h.c.b.ep = a;
                    h.c.j()
                },
                O: function () {
                    return function () {
                        l && l.length && (p.r(l.join("!")),
                            l = [])
                    }
                }
            };
        b.ia() && "" !== c.aet && f.d("pv-b", function () {
            e.d(document, "click", p.P());
            "ontouchend" in document && e.d(window, "touchend", p.P());
            e.d(window, "unload", p.O())
        });
        return p
    }
    )();
    (function () {
        var b = mt.event
            , a = mt.i
            , e = h.z
            , d = h.G
            , g = +new Date
            , f = []
            , n = t
            , l = {
                Za: function () {
                    return function () {
                        h.c && (h.c.M && c.aet && c.aet.length) && (window.clearTimeout(n),
                            n = window.setTimeout(function () {
                                l.Pa(a.S() + a.L())
                            }, 150))
                    }
                },
                Pa: function (a) {
                    l.$([+new Date - (h.c.B !== r ? h.c.B : g), a].join("*"))
                },
                $: function (a) {
                    if (encodeURIComponent(f.join("!") + a).length > e.ba || 3 < f.length)
                        l.r(f.join("!")),
                            f = [];
                    f.push(a)
                },
                r: function (d) {
                    h.c.b.et = 6;
                    h.c.b.vh = a.L();
                    h.c.b.ep = d;
                    h.c.j()
                },
                O: function () {
                    return function () {
                        f && f.length && (l.r(f.join("!")),
                            f = [])
                    }
                }
            };
        mt.lang.ia() && "" !== c.aet && d.d("pv-b", function () {
            b.d(window, "scroll", l.Za());
            b.d(window, "unload", l.O())
        });
        return l
    }
    )();
    (function () {
        function b() {
            function a(d) {
                function e() { }
                for (var f = s, m = 0, k = 0, q = t, y = t, z = d.length, u = 0, x = t, m = 0; m < z; m++)
                    if (q = d[m],
                        q.chromeVer === n) {
                        f = s;
                        y = q.tests;
                        u = y.length;
                        for (k = 0; k < u; k++)
                            if (e = b[y[k].name],
                                e() !== y[k].expect) {
                                f = v;
                                break
                            }
                        if (u && f) {
                            x = {
                                name: "360browser",
                                type: q.type,
                                version: q.ver
                            };
                            break
                        }
                    }
                return x
            }
            var d = [];
            d.push({
                ver: "8.3",
                type: "fast",
                tests: [{
                    name: "seamlessiframe",
                    expect: 1
                }],
                chromeVer: "42"
            });
            var b = {
                pointerevents: function () {
                    return "maxTouchPoints" in window.navigator ? 1 : "msMaxTouchPoints" in window.navigator || "mozMaxTouchPoints" in window.navigator || "webkitMaxTouchPoints" in window.navigator ? 9 : 0
                },
                webgl3D: function () {
                    for (var a = "webgl ms-webgl experimental-webgl moz-webgl opera-3d webkit-3d ms-3d 3d".split(" "), d = a.length, b = "", e = document.createElement("canvas"), k = v, f = 0; f < d; f++)
                        try {
                            if (e.getContext(a[f])) {
                                k = s;
                                b = a[f];
                                break
                            }
                        } catch (g) { }
                    return k ? "webgl" === b ? 1 : 9 : 0
                },
                seamlessiframe: function () {
                    return "seamless" in document.createElement("iframe") ? 1 : 0
                },
                speechsynthesis: function () {
                    return "speechSynthesis" in window ? 1 : "webkitSpeechSynthesis" in window || "mozSpeechSynthesis" in window || "oSpeechSynthesis" in window || "msSpeechSynthesis" in window ? 9 : 0
                }
            }
                , f = t
                , n = function () {
                    var a = navigator.userAgent.toLowerCase().match(/chrome\/(\d+)/i);
                    return a ? a[1] : "-1"
                }();
            "-1" !== n && (f = a(d));
            return f
        }
        var a = function () {
            var a = navigator.userAgent.toLowerCase();
            if (0 <= a.indexOf("chrome")) {
                var a = (a = a.match(/chrome\/(\d+)/)) ? a[1] : -1
                    , d = window.document
                    , g = "track" in d.createElement("track")
                    , d = "scoped" in d.createElement("style")
                    , f = "v8Locale" in window
                    , n = document.createElement("canvas")
                    , l = "seamless" in document.createElement("iframe")
                    , p = v;
                try {
                    n.getContext("webgl") && (p = s)
                } catch (w) { }
                if (n = b())
                    return "fast" === n.type ? 7 : 1;
                if (!p && "31" === a)
                    return 1;
                if (!(l = (l || !p) && "38" === a))
                    if (l = g)
                        if (l = !d)
                            if (l = !f) {
                                var m;
                                a: {
                                    l = window.navigator.mimeTypes;
                                    for (m in l)
                                        if ("application/vnd.chromium.remoting-viewer" === l[m].type) {
                                            m = s;
                                            break a
                                        }
                                    m = v
                                }
                                l = !m && 46 > +a
                            }
                if (l)
                    return 7;
                if (g && d && f)
                    return 1
            }
            return -1
        }();
        return h.Ua = a
    }
    )();
    (function () {
        function b() {
            return function () {
                h.c.b.nv = 0;
                h.c.b.st = 4;
                h.c.b.et = 3;
                h.c.b.ep = h.ca.hb() + "," + h.ca.fb();
                h.c.j()
            }
        }
        function a() {
            clearTimeout(x);
            var d;
            z && (d = "visible" == document[z]);
            u && (d = !document[u]);
            l = "undefined" == typeof d ? s : d;
            if ((!n || !p) && l && w)
                y = s,
                    k = +new Date;
            else if (n && p && (!l || !w))
                y = v,
                    q += +new Date - k;
            n = l;
            p = w;
            x = setTimeout(a, 100)
        }
        function e(a) {
            var k = document
                , d = "";
            if (a in k)
                d = a;
            else
                for (var b = ["webkit", "ms", "moz", "o"], u = 0; u < b.length; u++) {
                    var e = b[u] + a.charAt(0).toUpperCase() + a.slice(1);
                    if (e in k) {
                        d = e;
                        break
                    }
                }
            return d
        }
        function d(k) {
            if (!("focus" == k.type || "blur" == k.type) || !(k.target && k.target != window))
                w = "focus" == k.type || "focusin" == k.type ? s : v,
                    a()
        }
        var g = mt.event, f = h.G, n = s, l = s, p = s, w = s, m = +new Date, k = m, q = 0, y = s, z = e("visibilityState"), u = e("hidden"), x;
        a();
        (function () {
            var k = z.replace(/[vV]isibilityState/, "visibilitychange");
            g.d(document, k, a);
            g.d(window, "pageshow", a);
            g.d(window, "pagehide", a);
            "object" == typeof document.onfocusin ? (g.d(document, "focusin", d),
                g.d(document, "focusout", d)) : (g.d(window, "focus", d),
                    g.d(window, "blur", d))
        }
        )();
        h.ca = {
            hb: function () {
                return +new Date - m
            },
            fb: function () {
                return y ? +new Date - k + q : q
            }
        };
        f.d("pv-b", function () {
            g.d(window, "unload", b())
        });
        f.d("duration-send", b());
        f.d("duration-done", function () {
            k = m = +new Date;
            q = 0
        });
        return h.ca
    }
    )();
    (function () {
        var b = mt.lang
            , a = h.z
            , e = h.load
            , d = {
                wb: function (d) {
                    if ((window._dxt === r || b.e(window._dxt, "Array")) && "undefined" !== typeof h.c) {
                        var f = h.c.Q();
                        e([a.protocol, "//datax.baidu.com/x.js?si=", c.id, "&dm=", encodeURIComponent(f)].join(""), d)
                    }
                },
                Jb: function (a) {
                    if (b.e(a, "String") || b.e(a, "Number"))
                        window._dxt = window._dxt || [],
                            window._dxt.push(["_setUserId", a])
                }
            };
        return h.Xa = d
    }
    )();
    (function () {
        function b(a, d, b, e) {
            if (!(a === r || d === r || e === r)) {
                if ("" === a)
                    return [d, b, e].join("*");
                a = String(a).split("!");
                for (var u, x = v, f = 0; f < a.length; f++)
                    if (u = a[f].split("*"),
                        String(d) === u[0]) {
                        u[1] = b;
                        u[2] = e;
                        a[f] = u.join("*");
                        x = s;
                        break
                    }
                x || a.push([d, b, e].join("*"));
                return a.join("!")
            }
        }
        function a(k) {
            for (var b in k)
                if ({}.hasOwnProperty.call(k, b)) {
                    var e = k[b];
                    d.e(e, "Object") || d.e(e, "Array") ? a(e) : k[b] = String(e)
                }
        }
        var e = mt.url
            , d = mt.lang
            , g = mt.o
            , f = mt.i
            , n = h.z
            , l = h.G
            , p = h.Xa
            , w = h.load
            , m = {
                N: [],
                W: 0,
                Ea: v,
                w: {
                    qa: "",
                    page: ""
                },
                init: function () {
                    m.h = 0;
                    l.d("pv-b", function () {
                        m.Ya();
                        m.$a()
                    });
                    l.d("pv-d", function () {
                        m.ab();
                        m.w.page = ""
                    });
                    l.d("stag-b", function () {
                        h.c.b.api = m.h || m.W ? m.h + "_" + m.W : "";
                        h.c.b.ct = [decodeURIComponent(h.c.getData("Hm_ct_" + c.id) || ""), m.w.qa, m.w.page].join("!")
                    });
                    l.d("stag-d", function () {
                        h.c.b.api = 0;
                        m.h = 0;
                        m.W = 0
                    })
                },
                Ya: function () {
                    var a = window._hmt || [];
                    if (!a || d.e(a, "Array"))
                        window._hmt = {
                            id: c.id,
                            cmd: {},
                            push: function () {
                                for (var a = window._hmt, k = 0; k < arguments.length; k++) {
                                    var b = arguments[k];
                                    d.e(b, "Array") && (a.cmd[a.id].push(b),
                                        "_setAccount" === b[0] && (1 < b.length && /^[0-9a-f]{32}$/.test(b[1])) && (b = b[1],
                                            a.id = b,
                                            a.cmd[b] = a.cmd[b] || []))
                                }
                            }
                        },
                            window._hmt.cmd[c.id] = [],
                            window._hmt.push.apply(window._hmt, a)
                },
                $a: function () {
                    var a = window._hmt;
                    if (a && a.cmd && a.cmd[c.id])
                        for (var d = a.cmd[c.id], b = /^_track(Event|MobConv|Order|RTEvent)$/, e = 0, u = d.length; e < u; e++) {
                            var x = d[e];
                            b.test(x[0]) ? m.N.push(x) : m.na(x)
                        }
                    a.cmd[c.id] = {
                        push: m.na
                    }
                },
                ab: function () {
                    if (0 < m.N.length)
                        for (var a = 0, d = m.N.length; a < d; a++)
                            m.na(m.N[a]);
                    m.N = t
                },
                na: function (a) {
                    var b = a[0];
                    if (m.hasOwnProperty(b) && d.e(m[b], "Function"))
                        m[b](a)
                },
                _setAccount: function (a) {
                    1 < a.length && /^[0-9a-f]{32}$/.test(a[1]) && (m.h |= 1)
                },
                _setAutoPageview: function (a) {
                    if (1 < a.length && (a = a[1],
                        v === a || s === a))
                        m.h |= 2,
                            h.c.Aa = a
                },
                _trackPageview: function (a) {
                    if (1 < a.length && a[1].charAt && "/" === a[1].charAt(0)) {
                        m.h |= 4;
                        h.c.b.sn = h.c.za();
                        h.c.b.et = 0;
                        h.c.b.ep = "";
                        h.c.b.vl = f.S() + f.L();
                        h.c.b.kb = 0;
                        h.c.ha ? (h.c.b.nv = 0,
                            h.c.b.st = 4) : h.c.ha = s;
                        var d = h.c.b.u
                            , b = h.c.b.su;
                        h.c.b.u = n.protocol + "//" + document.location.host + a[1];
                        m.Ea || (h.c.b.su = document.location.href);
                        h.c.j();
                        h.c.b.u = d;
                        h.c.b.su = b;
                        h.c.B = +new Date
                    }
                },
                _trackEvent: function (a) {
                    2 < a.length && (m.h |= 8,
                        h.c.b.nv = 0,
                        h.c.b.st = 4,
                        h.c.b.et = 4,
                        h.c.b.ep = d.g(a[1]) + "*" + d.g(a[2]) + (a[3] ? "*" + d.g(a[3]) : "") + (a[4] ? "*" + d.g(a[4]) : ""),
                        h.c.j())
                },
                _setCustomVar: function (a) {
                    if (!(4 > a.length)) {
                        var b = a[1]
                            , e = a[4] || 3;
                        if (0 < b && 6 > b && 0 < e && 4 > e) {
                            m.W++;
                            for (var f = (h.c.b.cv || "*").split("!"), u = f.length; u < b - 1; u++)
                                f.push("*");
                            f[b - 1] = e + "*" + d.g(a[2]) + "*" + d.g(a[3]);
                            h.c.b.cv = f.join("!");
                            a = h.c.b.cv.replace(/[^1](\*[^!]*){2}/g, "*").replace(/((^|!)\*)+$/g, "");
                            "" !== a ? h.c.setData("Hm_cv_" + c.id, encodeURIComponent(a), c.age) : h.c.Cb("Hm_cv_" + c.id)
                        }
                    }
                },
                _setUserTag: function (a) {
                    if (!(3 > a.length)) {
                        var e = d.g(a[1]);
                        a = d.g(a[2]);
                        if (e !== r && a !== r) {
                            var f = decodeURIComponent(h.c.getData("Hm_ct_" + c.id) || "")
                                , f = b(f, e, 1, a);
                            h.c.setData("Hm_ct_" + c.id, encodeURIComponent(f), c.age)
                        }
                    }
                },
                _setVisitTag: function (a) {
                    if (!(3 > a.length)) {
                        var e = d.g(a[1]);
                        a = d.g(a[2]);
                        if (e !== r && a !== r) {
                            var f = m.w.qa
                                , f = b(f, e, 2, a);
                            m.w.qa = f
                        }
                    }
                },
                _setPageTag: function (a) {
                    if (!(3 > a.length)) {
                        var e = d.g(a[1]);
                        a = d.g(a[2]);
                        if (e !== r && a !== r) {
                            var f = m.w.page
                                , f = b(f, e, 3, a);
                            m.w.page = f
                        }
                    }
                },
                _setReferrerOverride: function (a) {
                    1 < a.length && (h.c.b.su = a[1].charAt && "/" === a[1].charAt(0) ? n.protocol + "//" + window.location.host + a[1] : a[1],
                        m.Ea = s)
                },
                _trackOrder: function (b) {
                    b = b[1];
                    d.e(b, "Object") && (a(b),
                        m.h |= 16,
                        h.c.b.nv = 0,
                        h.c.b.st = 4,
                        h.c.b.et = 94,
                        h.c.b.ep = g.stringify(b),
                        h.c.j())
                },
                _trackMobConv: function (a) {
                    if (a = {
                        webim: 1,
                        tel: 2,
                        map: 3,
                        sms: 4,
                        callback: 5,
                        share: 6
                    }[a[1]])
                        m.h |= 32,
                            h.c.b.et = 93,
                            h.c.b.ep = a,
                            h.c.j()
                },
                _trackRTPageview: function (b) {
                    b = b[1];
                    d.e(b, "Object") && (a(b),
                        b = g.stringify(b),
                        512 >= encodeURIComponent(b).length && (m.h |= 64,
                            h.c.b.rt = b))
                },
                _trackRTEvent: function (b) {
                    b = b[1];
                    if (d.e(b, "Object")) {
                        a(b);
                        b = encodeURIComponent(g.stringify(b));
                        var e = function (a) {
                            var b = h.c.b.rt;
                            m.h |= 128;
                            h.c.b.et = 90;
                            h.c.b.rt = a;
                            h.c.j();
                            h.c.b.rt = b
                        }
                            , f = b.length;
                        if (900 >= f)
                            e.call(this, b);
                        else
                            for (var f = Math.ceil(f / 900), l = "block|" + Math.round(Math.random() * n.V).toString(16) + "|" + f + "|", u = [], x = 0; x < f; x++)
                                u.push(x),
                                    u.push(b.substring(900 * x, 900 * x + 900)),
                                    e.call(this, l + u.join("|")),
                                    u = []
                    }
                },
                _setDataxId: function (a) {
                    a = a[1];
                    p.wb();
                    p.Jb(a)
                },
                _setUserId: function (a) {
                    a = d.g(a[1]);
                    if (a !== r) {
                        var b = h.c.b.p
                            , e = h.c.b.ep;
                        h.c.b.et = 8;
                        h.c.b.ep = "";
                        h.c.b.p = "uid_*" + a;
                        h.c.j();
                        h.c.b.p = b;
                        h.c.b.ep = e
                    }
                },
                _setAutoTracking: function (a) {
                    if (1 < a.length && (a = a[1],
                        v === a || s === a))
                        h.c.Ba = a
                },
                _setAutoEventTracking: function (a) {
                    if (1 < a.length && (a = a[1],
                        v === a || s === a))
                        h.c.M = a
                },
                _trackPageDuration: function (a) {
                    1 < a.length ? (a = a[1],
                        2 === String(a).split(",").length && (h.c.b.et = 3,
                            h.c.b.ep = a,
                            h.c.j())) : l.F("duration-send");
                    l.F("duration-done")
                },
                _require: function (a) {
                    1 < a.length && (a = a[1],
                        n.Ra.test(e.K(a)) && w(a))
                },
                _providePlugin: function (a) {
                    if (1 < a.length) {
                        var b = window._hmt
                            , e = a[1];
                        a = a[2];
                        if (d.H(n.Ga, e) && d.e(a, "Function") && (b.plugins = b.plugins || {},
                            b.C = b.C || {},
                            b.plugins[e] = a,
                            b.s = b.s || [],
                            a = b.s.slice(),
                            e && a.length && a[0][1] === e))
                            for (var f = 0, u = a.length; f < u; f++) {
                                var x = a[f][2] || {};
                                if (b.plugins[e] && !b.C[e])
                                    b.C[e] = new b.plugins[e](x),
                                        b.s.shift();
                                else
                                    break
                            }
                    }
                },
                _requirePlugin: function (a) {
                    if (1 < a.length) {
                        var b = window._hmt
                            , e = a[1]
                            , f = a[2] || {};
                        if (d.H(n.Ga, e))
                            if (b.plugins = b.plugins || {},
                                b.C = b.C || {},
                                b.plugins[e] && !b.C[e])
                                b.C[e] = new b.plugins[e](f);
                            else {
                                b.s = b.s || [];
                                for (var f = 0, u = b.s.length; f < u; f++)
                                    if (b.s[f][1] === e)
                                        return;
                                b.s.push(a);
                                m._require([t, n.zb + e + ".js"])
                            }
                    }
                },
                _trackCustomEvent: function (a) {
                    if (1 < a.length) {
                        var b = d.g(a[1])
                            , e = "";
                        a = a[2];
                        if (d.e(a, "Object")) {
                            var e = [], f;
                            for (f in a)
                                a.hasOwnProperty(f) && (e.push(d.g(f)),
                                    d.e(a[f], "Object") ? e.push(d.g(g.stringify(a[f]))) : e.push(d.g(String(a[f]))));
                            e = "*" + e.join("*")
                        }
                        h.c.b.et = 7;
                        h.c.b.ep = "ce!_iden*" + b + e;
                        h.c.j()
                    }
                }
            };
        m.init();
        h.Sa = m;
        return h.Sa
    }
    )();
    (function () {
        function b() {
            "undefined" === typeof window["_bdhm_loaded_" + c.id] && (window["_bdhm_loaded_" + c.id] = s,
                this.b = {},
                this.Ba = this.Aa = s,
                this.M = k.M,
                this.ua = g.ia() && 0 < c.aet.length ? c.aet.split(",") : "",
                this.ha = v,
                this.init())
        }
        var a = mt.url
            , e = mt.Ka
            , d = mt.oa
            , g = mt.lang
            , f = mt.cookie
            , n = mt.i
            , l = mt.localStorage
            , p = mt.sessionStorage
            , w = mt.o
            , m = mt.event
            , k = h.z
            , q = h.Ua
            , y = h.load
            , z = h.G;
        b.prototype = {
            ka: function (a, b) {
                a = "." + a.replace(/:\d+/, "");
                b = "." + b.replace(/:\d+/, "");
                var d = a.indexOf(b);
                return -1 < d && d + b.length === a.length
            },
            Fa: function (a, b) {
                a = a.replace(/^https?:\/\//, "");
                return 0 === a.indexOf(b)
            },
            T: function (b) {
                for (var d = 0; d < c.dm.length; d++)
                    if (-1 < c.dm[d].indexOf("/")) {
                        if (this.Fa(b, c.dm[d]))
                            return s
                    } else {
                        var e = a.K(b);
                        if (e && this.ka(e, c.dm[d]))
                            return s
                    }
                return v
            },
            Q: function () {
                for (var a = document.location.hostname, b = 0, d = c.dm.length; b < d; b++)
                    if (this.ka(a, c.dm[b]))
                        return c.dm[b].replace(/(:\d+)?[/?#].*/, "");
                return a
            },
            ea: function () {
                for (var a = 0, b = c.dm.length; a < b; a++) {
                    var d = c.dm[a];
                    if (-1 < d.indexOf("/") && this.Fa(document.location.href, d))
                        return d.replace(/^[^/]+(\/.*)/, "$1") + "/"
                }
                return "/"
            },
            lb: function () {
                if (!document.referrer)
                    return k.D - k.I > c.vdur ? 1 : 4;
                var b = v;
                this.T(document.referrer) && this.T(document.location.href) ? b = s : (b = a.K(document.referrer),
                    b = this.ka(b || "", document.location.hostname));
                return b ? k.D - k.I > c.vdur ? 1 : 4 : 3
            },
            getData: function (a) {
                try {
                    return f.get(a) || p.get(a) || l.get(a)
                } catch (b) { }
            },
            setData: function (a, b, d) {
                try {
                    f.set(a, b, {
                        domain: this.Q(),
                        path: this.ea(),
                        J: d
                    }),
                        d ? l.set(a, b, d) : p.set(a, b)
                } catch (e) { }
            },
            Cb: function (a) {
                try {
                    f.set(a, "", {
                        domain: this.Q(),
                        path: this.ea(),
                        J: -1
                    }),
                        p.remove(a),
                        l.remove(a)
                } catch (b) { }
            },
            Hb: function () {
                var a, b, d, e, g;
                k.I = this.getData("Hm_lpvt_" + c.id) || 0;
                13 === k.I.length && (k.I = Math.round(k.I / 1E3));
                b = this.lb();
                a = 4 !== b ? 1 : 0;
                if (d = this.getData("Hm_lvt_" + c.id)) {
                    e = d.split(",");
                    for (g = e.length - 1; 0 <= g; g--)
                        13 === e[g].length && (e[g] = "" + Math.round(e[g] / 1E3));
                    for (; 2592E3 < k.D - e[0];)
                        e.shift();
                    g = 4 > e.length ? 2 : 3;
                    for (1 === a && e.push(k.D); 4 < e.length;)
                        e.shift();
                    d = e.join(",");
                    e = e[e.length - 1]
                } else
                    d = k.D,
                        e = "",
                        g = 1;
                this.setData("Hm_lvt_" + c.id, d, c.age);
                this.setData("Hm_lpvt_" + c.id, k.D);
                d = f.xb(this.Q(), this.ea());
                if (0 === c.nv && this.T(document.location.href) && ("" === document.referrer || this.T(document.referrer)))
                    a = 0,
                        b = 4;
                this.b.nv = a;
                this.b.st = b;
                this.b.cc = d;
                this.b.lt = e;
                this.b.lv = g
            },
            Gb: function () {
                for (var a = [], b = this.b.et, d = +new Date, e = 0, f = k.La.length; e < f; e++) {
                    var g = k.La[e]
                        , l = this.b[g];
                    "undefined" !== typeof l && "" !== l && ("tt" !== g || "tt" === g && 0 === b) && (("ct" !== g || "ct" === g && 0 === b) && ("kb" !== g || "kb" === g && 3 === b)) && a.push(g + "=" + encodeURIComponent(l))
                }
                switch (b) {
                    case 0:
                        this.b.rt && a.push("rt=" + encodeURIComponent(this.b.rt));
                        break;
                    case 5:
                        a.push("_lpt=" + this.B);
                        a.push("_ct=" + d);
                        break;
                    case 6:
                        a.push("_lpt=" + this.B);
                        a.push("_ct=" + d);
                        break;
                    case 90:
                        this.b.rt && a.push("rt=" + this.b.rt)
                }
                return a.join("&")
            },
            Ib: function () {
                this.Hb();
                this.b.si = c.id;
                this.b.sn = this.za();
                this.b.su = document.referrer;
                this.b.hh = window.location.hash;
                this.b.ds = n.Db;
                this.b.cl = n.colorDepth + "-bit";
                this.b.ln = String(n.language).toLowerCase();
                this.b.ja = n.javaEnabled ? 1 : 0;
                this.b.ck = n.cookieEnabled ? 1 : 0;
                this.b.bs = q;
                this.b.lo = "number" === typeof _bdhm_top ? 1 : 0;
                this.b.fl = d.nb();
                this.b.v = "1.2.56";
                this.b.cv = decodeURIComponent(this.getData("Hm_cv_" + c.id) || "");
                this.b.tt = document.title || "";
                this.b.vl = n.S() + n.L();
                var b = document.location.href;
                this.b.cm = a.m(b, k.rb) || "";
                this.b.cp = a.m(b, k.tb) || a.m(b, k.Nb) || "";
                this.b.cw = a.m(b, k.qb) || a.m(b, k.Pb) || "";
                this.b.ci = a.m(b, k.ob) || a.m(b, k.Mb) || "";
                this.b.cf = a.m(b, k.ub) || a.m(b, k.Ob) || "";
                this.b.cu = a.m(b, k.pb) || a.m(b, k.Lb) || ""
            },
            init: function () {
                try {
                    this.Ib(),
                        0 === this.b.nv ? this.Fb() : this.wa(),
                        h.c = this,
                        this.Wa(),
                        this.Va(),
                        z.F("pv-b"),
                        this.Eb()
                } catch (a) {
                    var b = [];
                    b.push("si=" + c.id);
                    b.push("n=" + encodeURIComponent(a.name));
                    b.push("m=" + encodeURIComponent(a.message));
                    b.push("r=" + encodeURIComponent(document.referrer));
                    e.log(k.la + "//" + k.Ha + "?" + b.join("&"))
                }
            },
            Eb: function () {
                function a() {
                    z.F("pv-d")
                }
                this.Aa ? (this.ha = s,
                    this.b.et = 0,
                    this.b.ep = "",
                    this.b.vl = n.S() + n.L(),
                    this.j(a)) : a();
                this.B = +new Date
            },
            j: function (a) {
                if (this.Ba) {
                    var b = this;
                    b.b.rnd = Math.round(Math.random() * k.V);
                    z.F("stag-b");
                    var d = k.la + "//" + k.Ha + "?" + b.Gb();
                    z.F("stag-d");
                    b.Qa(d);
                    e.log(d, function (d) {
                        b.Ia(d);
                        g.e(a, "Function") && a.call(b)
                    })
                }
            },
            Wa: function () {
                var b = document.location.hash.substring(1)
                    , d = RegExp(c.id)
                    , e = a.K(document.referrer) === k.Ma ? 1 : 0
                    , f = a.m(b, "jn")
                    , g = /^heatlink$|^select$|^pageclick$/.test(f);
                b && (d.test(b) && e && g) && (this.b.rnd = Math.round(Math.random() * k.V),
                    b = document.createElement("script"),
                    b.setAttribute("type", "text/javascript"),
                    b.setAttribute("charset", "utf-8"),
                    b.setAttribute("src", k.protocol + "//" + c.js + f + ".js?" + this.b.rnd),
                    f = document.getElementsByTagName("script")[0],
                    f.parentNode.insertBefore(b, f))
            },
            Va: function () {
                if (window.postMessage && window.self !== window.parent) {
                    var b = this;
                    m.d(window, "message", function (d) {
                        if (a.K(d.origin) === k.Ma) {
                            var e = d.data || {};
                            d = e.sd || "";
                            var e = e.jn || ""
                                , f = /^customevent$/.test(e);
                            RegExp(c.id).test(d) && f && (b.b.rnd = Math.round(Math.random() * k.V),
                                y(k.protocol + "//" + c.js + e + ".js?" + b.b.rnd))
                        }
                    });
                    window.parent.postMessage({
                        id: c.id,
                        url: document.location.href,
                        status: "__Messenger__hmLoaded"
                    }, "*")
                }
            },
            Qa: function (a) {
                var b;
                try {
                    b = w.parse(p.get("Hm_unsent_" + c.id) || "[]")
                } catch (d) {
                    b = []
                }
                var e = this.b.u ? "" : "&u=" + encodeURIComponent(document.location.href);
                b.push(a.replace(/^https?:\/\//, "") + e);
                p.set("Hm_unsent_" + c.id, w.stringify(b))
            },
            Ia: function (a) {
                var b;
                try {
                    b = w.parse(p.get("Hm_unsent_" + c.id) || "[]")
                } catch (d) {
                    b = []
                }
                if (b.length) {
                    a = a.replace(/^https?:\/\//, "");
                    for (var e = 0; e < b.length; e++)
                        if (a.replace(/&u=[^&]*/, "") === b[e].replace(/&u=[^&]*/, "")) {
                            b.splice(e, 1);
                            break
                        }
                    b.length ? p.set("Hm_unsent_" + c.id, w.stringify(b)) : this.wa()
                }
            },
            wa: function () {
                p.remove("Hm_unsent_" + c.id)
            },
            Fb: function () {
                var a = this, b;
                try {
                    b = w.parse(p.get("Hm_unsent_" + c.id) || "[]")
                } catch (d) {
                    b = []
                }
                if (b.length)
                    for (var f = function (b) {
                        e.log(k.la + "//" + b, function (b) {
                            a.Ia(b)
                        })
                    }, g = 0; g < b.length; g++)
                        f(b[g])
            },
            za: function () {
                return Math.round(+new Date / 1E3) % 65535
            }
        };
        return new b
    }
    )();
    (function () {
        var b = mt.event
            , a = mt.lang
            , e = h.z;
        if (c.kbtrk && "undefined" !== typeof h.c) {
            h.c.b.kb = a.Da(h.c.b.kb) ? h.c.b.kb : 0;
            var d = function () {
                h.c.b.et = 85;
                h.c.b.ep = h.c.b.kb;
                h.c.j()
            };
            b.d(document, "keyup", function () {
                h.c.b.kb++
            });
            b.d(window, "unload", function () {
                d()
            });
            setInterval(d, e.yb)
        }
    }
    )();
    var B = h.z
        , C = h.load;
    c.pt && C([B.protocol, "//ada.baidu.com/phone-tracker/insert_bdtj?sid=", c.pt].join(""));
    (function () {
        var b = mt.i
            , a = mt.lang
            , e = mt.event
            , d = mt.o;
        if ("undefined" !== typeof h.c && (c.med || (!b.Ca || 7 < b.vb) && c.cvcc)) {
            var g, f, n, l, p = function (a) {
                if (a.item) {
                    for (var b = a.length, d = Array(b); b--;)
                        d[b] = a[b];
                    return d
                }
                return [].slice.call(a)
            }, w = function (a, b) {
                for (var d in a)
                    if (a.hasOwnProperty(d) && b.call(a, d, a[d]) === v)
                        return v
            }, m = function (b, e) {
                var f = {};
                f.n = g;
                f.t = "clk";
                f.v = b;
                if (e) {
                    var k = e.getAttribute("href")
                        , l = e.getAttribute("onclick") ? "" + e.getAttribute("onclick") : t
                        , m = e.getAttribute("id") || "";
                    n.test(k) ? (f.sn = "mediate",
                        f.snv = k) : a.e(l, "String") && n.test(l) && (f.sn = "wrap",
                            f.snv = l);
                    f.id = m
                }
                h.c.b.et = 86;
                h.c.b.ep = d.stringify(f);
                h.c.j();
                for (f = +new Date; 400 >= +new Date - f;)
                    ;
            };
            if (c.med)
                f = "/zoosnet",
                    g = "swt",
                    n = /swt|zixun|call|chat|zoos|business|talk|kefu|openkf|online|\/LR\/Chatpre\.aspx/i,
                    l = {
                        click: function () {
                            for (var a = [], b = p(document.getElementsByTagName("a")), b = [].concat.apply(b, p(document.getElementsByTagName("area"))), b = [].concat.apply(b, p(document.getElementsByTagName("img"))), d, e, f = 0, g = b.length; f < g; f++)
                                d = b[f],
                                    e = d.getAttribute("onclick"),
                                    d = d.getAttribute("href"),
                                    (n.test(e) || n.test(d)) && a.push(b[f]);
                            return a
                        }
                    };
            else if (c.cvcc) {
                f = "/other-comm";
                g = "other";
                n = c.cvcc.q || r;
                var k = c.cvcc.id || r;
                l = {
                    click: function () {
                        for (var a = [], b = p(document.getElementsByTagName("a")), b = [].concat.apply(b, p(document.getElementsByTagName("area"))), b = [].concat.apply(b, p(document.getElementsByTagName("img"))), d, e, f, g = 0, l = b.length; g < l; g++)
                            d = b[g],
                                n !== r ? (e = d.getAttribute("onclick"),
                                    f = d.getAttribute("href"),
                                    k ? (d = d.getAttribute("id"),
                                        (n.test(e) || n.test(f) || k.test(d)) && a.push(b[g])) : (n.test(e) || n.test(f)) && a.push(b[g])) : k !== r && (d = d.getAttribute("id"),
                                            k.test(d) && a.push(b[g]));
                        return a
                    }
                }
            }
            if ("undefined" !== typeof l && "undefined" !== typeof n) {
                var q;
                f += /\/$/.test(f) ? "" : "/";
                var y = function (b, d) {
                    if (q === d)
                        return m(f + b, d),
                            v;
                    if (a.e(d, "Array") || a.e(d, "NodeList"))
                        for (var e = 0, g = d.length; e < g; e++)
                            if (q === d[e])
                                return m(f + b + "/" + (e + 1), d[e]),
                                    v
                };
                e.d(document, "mousedown", function (b) {
                    b = b || window.event;
                    q = b.target || b.srcElement;
                    var d = {};
                    for (w(l, function (b, e) {
                        d[b] = a.e(e, "Function") ? e() : document.getElementById(e)
                    }); q && q !== document && w(d, y) !== v;)
                        q = q.parentNode
                })
            }
        }
    }
    )();
    (function () {
        var b = mt.f
            , a = mt.lang
            , e = mt.event
            , d = mt.o;
        if ("undefined" !== typeof h.c && a.e(c.cvcf, "Array") && 0 < c.cvcf.length) {
            var g = {
                Oa: function () {
                    for (var a = c.cvcf.length, d, l = 0; l < a; l++)
                        (d = b.xa(decodeURIComponent(c.cvcf[l]))) && e.d(d, "click", g.da())
                },
                da: function () {
                    return function () {
                        h.c.b.et = 86;
                        var a = {
                            n: "form",
                            t: "clk"
                        };
                        a.id = this.id;
                        h.c.b.ep = d.stringify(a);
                        h.c.j()
                    }
                }
            };
            b.Ab(function () {
                g.Oa()
            })
        }
    }
    )();
    (function () {
        var b = mt.event
            , a = mt.o;
        if (c.med && "undefined" !== typeof h.c) {
            var e = {
                n: "anti",
                sb: 0,
                kb: 0,
                clk: 0
            }
                , d = function () {
                    h.c.b.et = 86;
                    h.c.b.ep = a.stringify(e);
                    h.c.j()
                };
            b.d(document, "click", function () {
                e.clk++
            });
            b.d(document, "keyup", function () {
                e.kb = 1
            });
            b.d(window, "scroll", function () {
                e.sb++
            });
            b.d(window, "load", function () {
                setTimeout(d, 5E3)
            })
        }
    }
    )();
    c.spa !== r && "1" === String(c.spa) && (window._hmt = window._hmt || [],
        window._hmt.push(["_requirePlugin", "UrlChangeTracker"]));
}
)();

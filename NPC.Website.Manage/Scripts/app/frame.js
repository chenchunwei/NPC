function DefineClass() {
    return function () {
        if (this.initialize != undefined) {
            this.initialize.apply(this, arguments);
        }
    };
}
var FrameContainer = DefineClass();
FrameContainer.prototype = {
    initialize: function () {
        this.frames = [];
        this.frameUpdateListeners = [];
        this.frameAddListeners = [];
        this.frameRemoveListeners = [];
    },
    frames: [],
    currentIndex: 0,
    currentOrderSort: 0,
    addFrameWithOptions: function (options) {
        var frame = new FrameInfo();
        frame.id = options.id;
        frame.txt = options.txt;
        frame.index = options.orderSort;
        frame.img = options.img;
        frame.timeDuring = options.timeDuring;
        frame.voice = options.voice;
        frame.index = options.index;
        frame.orderSort = options.orderSort;
        frame.positionStyle = options.positionStyle;
        this.frames.push(frame);
        this.currentIndex++;
        this.currentOrderSort++;
        this.onFrameAdd(frame);
    },
    getFrameByIndex: function (index) {
        var target = null;
        $.each(this.frames, function (i, o) {
            if (o.index == index) {
                target = o;
                return;
            }
        });
        return target;
    },
    getFrameByOrderSort: function (orderSort) {
        var target = null;
        $.each(this.frames, function (i, o) {
            if (o.orderSort == orderSort) {
                target = o;
                return;
            }
        });
        return target;
    },
    addFrame: function (frame, after) {
        var container = this;
        this.currentIndex++;
        this.currentOrderSort++;
        frame.index = this.currentIndex;
        var afterFrame;
        if (after != null) {
            frame.orderSort = parseInt(after) + 1;
            $.each(this.frames, function (i, o) {
                if (o.orderSort == parseInt(after))
                    afterFrame = o;
                if (o.orderSort >= frame.orderSort) {
                    o.orderSort++;
                    container.onFrameUpdate(o);
                }
            });
        } else {
            frame.orderSort = this.currentOrderSort;
        }
        this.frames.push(frame);
        this.onFrameAdd(frame, afterFrame);
    },
    removeFrame: function (index) {
        var container = this;
        var cframe;
        var spliceIndex;
        $.each(this.frames, function (i, frame) {
            if (frame.index == index) {
                spliceIndex = i;
                cframe = frame;
                return;
            }
        });
        this.frames.splice(spliceIndex, 1);
        $.each(this.frames, function (i, o) {
            if (o.orderSort > cframe.orderSort) {
                o.orderSort--;
                container.onFrameUpdate(o);
            }
        });
        this.currentOrderSort--;
        this.onFrameRemove(cframe);
    },
    onFrameUpdate: function (frame) {
        $.each(this.frameUpdateListeners, function (i, o) {
            if (o.updateTrigger != undefined)
                o.updateTrigger(frame);
        });
    },
    onFrameRemove: function (frame) {
        $.each(this.frameRemoveListeners, function (i, o) {
            if (o.removeTrigger != undefined)
                o.removeTrigger(frame);
        });
    },
    onFrameAdd: function (frame, before) {
        $.each(this.frameAddListeners, function (i, o) {
            if (o.addTrigger != undefined)
                o.addTrigger(frame, before);
        });
    },
    serializeFrames: function () {
        return $.toJSON(this.frames);
    },
    frameAddListeners: [],
    frameUpdateListeners: [],
    frameRemoveListeners: []
};

var FrameInfo = DefineClass();
FrameInfo.prototype = {
    img: '',
    imgSize: 0,
    txt: '',
    txtSize: 0,
    type: 0,
    voice: '',
    voiceSize: 0,
    orderSort: 0,
    id: 0,
    index: 0,
    positionStyle: 0,
    timeDuring: 10
};

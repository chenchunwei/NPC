namespace Npc.OpenMas.OmsHelper
{
    /// <summary>
    /// 用于指明 Web 服务支持的身份验证方法。
    /// 支持的值有“passport”和“other”。
    /// 如果不支持“passport”，则使用“other”，这说明用户只要使用普通的用户 ID 和密码即可通过验证。
    /// </summary>
    public enum AuthenticationType
    {
        ///<summary>
        ///</summary>
        Unknow,
        ///<summary>
        ///</summary>
        Passport,
        ///<summary>
        ///</summary>
        Other
    }
    ///<summary>
    ///</summary>
    public enum SeverityType
    {
        ///<summary>
        ///</summary>
        Unknow,

        /// <summary>
        /// 成功
        /// </summary>
        Neutral,

        /// <summary>
        /// 失败
        /// </summary>
        Failure
    }
    ///<summary>
    ///</summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 完全成功。
        /// </summary>
        Ok,

        /// <summary>
        /// 用户 ID 或密码无效或无法识别
        /// </summary>
        InvalidUser,

        /// <summary>
        /// 用户尚未注册此服务。如果服务提供商无法根据用户 ID 作出判断，就会返回“invalidUser”值
        /// </summary>
        UnregisteredUser,

        /// <summary>
        /// 用户尚未订阅 content 元素中列出的服务
        /// </summary>
        UnregisteredService,

        /// <summary>
        /// 用户的预付话费已用完或者注册已到期。此错误代码适用于提供预付费服务的服务提供商
        /// </summary>
        ExpiredUser,

        /// <summary>
        /// 一个或多个收件人无效或无法识别。收件人会以分号分隔的形式在 recipientList 元素中返回
        /// </summary>
        InvalidRecipient,

        /// <summary>
        /// 一个或多个收件人来自发件人运营商不支持的运营商。收件人会以分号分隔的形式在 recipientList 元素中返回
        /// </summary>
        CrossCarrier,

        /// <summary>
        /// 信息主题或正文包含当地政策不允许或服务提供商不支持的字符或文字
        /// </summary>
        InvalidChar,

        /// <summary>
        /// 媒体无效或不受支持。无效媒体的内容 ID 会在 content 元素中返回。（此属性仅适用于 MMS 信息。）
        /// </summary>
        InvalidMedia,

        /// <summary>
        /// 超出了用户每天可发送的信息数限制。信息数限制会在 content 元素中返回
        /// </summary>
        PerDayMsgLimit,

        /// <summary>
        /// 超出了用户每月可发送的信息数限制。信息数限制会在 content 元素中返回
        /// </summary>
        PerMonthMsgLimit,

        /// <summary>
        /// 超出了 SMS 信息的长度限制。单字节信息和双字节信息的最大长度会在 content 元素中返回
        /// </summary>
        LengthLimit,

        /// <summary>
        /// 超出了 MMS 信息的长度限制
        /// </summary>
        SizeLimit,

        /// <summary>
        /// 超出了 MMS 信息可以包含的幻灯片数量限制
        /// </summary>
        SlidesLimit,

        /// <summary>
        /// 服务端网络问题，例如无法连接到 SMSC 或 MMSC
        /// </summary>
        ServiceNetwork,

        /// <summary>
        /// 不支持按计划发送。该信息已被立即发送
        /// </summary>
        NoScheduled,

        /// <summary>
        /// 用户的帐户余额不足。当前余额和发送单条信息的资费会在 content 元素中返回
        /// </summary>
        LowBalance,

        /// <summary>
        /// 服务应在其属性每次更改时返回此信息。客户端可以在收到此信息时调用 GetServiceInfo() 以更新服务属性
        /// </summary>
        ServiceUpdate,

        /// <summary>
        /// 通知客户端该服务已终止
        /// </summary>
        CeasedService,

        /// <summary>
        /// 对所有其他错误都使用此错误代码
        /// </summary>
        Other
    }

    /// <summary>
    /// 客户端要求的服务。serviceInfo 字符串中定义的支持服务之一。有效值为 SMS_SENDER 或 MMS_SENDER
    /// </summary>
    public enum RequiredServiceType
    {
        ///<summary>
        ///</summary>
        Unknow,
        ///<summary>
        ///</summary>
        Sms_Sender,
        ///<summary>
        ///</summary>
        Mms_Sender
    }

    ///<summary>
    ///</summary>
    public enum FitType
    {
        ///<summary>
        ///</summary>
        Unknow,

        /// <summary>
        /// 保持多媒体片断的尺寸不变，从窗口的左上角开始显示。
        /// 如果多媒体片断尺寸比窗口的尺寸小，那么空白的地方将用背景色填充。
        /// 如果多媒体片断尺寸比窗口的尺寸大，那么多媒体片断超出窗口部分被裁去，不被显示
        /// </summary>
        Hidden,

        /// <summary>
        /// 保持多媒体片断宽/高比例不变的情况下，对多媒体片断的尺寸进行缩放。
        /// 从左上角开始显示，缩放到高度和宽度中的一个尺寸等于窗口的相应的尺寸，
        /// 而另外的一个小于窗口的相应的尺寸。空白处用背景色填充
        /// </summary>
        Meet,

        /// <summary>
        /// 缩放多媒体片断使得其大小正好和窗口的大小一致。
        /// 如果多媒体片断的宽/高比例和窗口的宽/高比例不等，那么多媒体片断就会变形，非常难看。
        /// 强烈建议不要采用这种方式
        /// </summary>
        Fill,

        /// <summary>
        /// 对多媒体片断的尺寸不做什么修改，它以正常的尺寸大小显示。
        /// 但是，如果多媒体片断的尺寸超出了窗口的尺寸，那么将会相应出现水平或者垂直滚动条。
        /// 该种发式适合于长时间的多媒体片断的显示。如果多媒体片断的显示时间很短
        /// </summary>
        Scroll,

        /// <summary>
        /// 保持多媒体片断宽/高比例不变的情况下，对多媒体片断的尺寸进行缩放。
        /// 从左上角开始显示，缩放到高度和宽度中的一个尺寸等于窗口的相应的尺寸，而另外的一个大于窗口的相应的尺寸。
        /// 超出的不分被裁去而不显示。 
        /// </summary>
        Slice
    }
}
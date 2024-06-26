﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Jon.Util.Data
{
    public static class AppSettingsMgr
    {

        // Message Queue Notification for File Activity
        /// <summary> Enables publishing messages to the Message Bus for listed flows.</summary>
        public static ConcurrentDictionary<string,int> NotifyMQList { get; set; } = new ConcurrentDictionary<string,int>();
        /// <summary> Settings for the Activity Server Message Bus.</summary>
        public static MQOptions ActivityMQOptions { get; set; } = new MQOptions();
        /// <summary> Enables publishing messages to the Message Bus for listed flows.</summary>
        public static WSServerOptions ActivityServerOptions { get; set; } = new WSServerOptions();
    }
}

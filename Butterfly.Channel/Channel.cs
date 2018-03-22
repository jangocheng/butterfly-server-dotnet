﻿using System;
using System.Collections.Generic;

using Dict = System.Collections.Generic.Dictionary<string, object>;

namespace Butterfly.Channel {
    public class Channel : IDisposable {

        protected readonly IChannelServerConnection connection;
        protected readonly string channelKey;
        protected readonly Dict vars;

        public Channel(IChannelServerConnection connection, string channelKey, Dict vars) {
            this.connection = connection;
            this.channelKey = channelKey;
            this.vars = vars;
        }

        public IChannelServerConnection Connection => this.connection;

        public string ChannelKey => this.channelKey;

        public Dict Vars => this.vars;

        protected readonly List<IDisposable> disposables = new List<IDisposable>();

        public void Attach(IDisposable disposable) {
            this.disposables.Add(disposable);
        }

        public void Queue(string messageType, object value) {
            this.connection.QueueChannelMessage(messageType, this.channelKey, value);
        }

        public void Dispose() {
            foreach (var disposable in this.disposables) {
                disposable.Dispose();
            }
            this.disposables.Clear();
        }

    }
}

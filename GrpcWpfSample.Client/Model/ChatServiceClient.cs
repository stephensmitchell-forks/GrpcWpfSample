﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcWpfSample.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcWpfSample.Client.Model
{
    class ChatServiceClient
    {
        private readonly Chat.ChatClient m_client;

        public ChatServiceClient()
        {
            var channel = new Channel("127.0.0.1:50052", ChannelCredentials.Insecure);
            m_client = new Chat.ChatClient(channel);
        }

        public async Task Write(ChatLog chatLog)
        {
            await m_client.WriteAsync(chatLog);
        }

        public async Task Subscribe(Action<ChatLog> onRead)
        {
            using (var call = m_client.Subscribe(new Empty()))
            {
                await call.ResponseStream
                    .ToAsyncEnumerable()
                    .ForEachAsync((x) => onRead(x));
            }
        }
    }

}

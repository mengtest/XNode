﻿// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNode.Logging;

namespace XNode.Serializer.MsgPack
{
    /// <summary>
    /// 基于MsgPack的序列化器
    /// </summary>
    public class MsgPackSerializer : ISerializer
    {
        private ILogger logger;

        public string Name
        {
            get
            {
                return "MsgPack";
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loggerFactory">日志工厂</param>
        public MsgPackSerializer(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<MsgPackSerializer>();
        }

        public Task<object> DeserializeAsync(Type type, byte[] data)
        {
            if (type == null)
            {
                throw new InvalidOperationException("Deserialize failed, type is null.");
            }

            if (data == null || data.Length == 0)
            {
                logger.LogDebug($"Deserialize data is null.");
                return Task.Factory.StartNew<object>(() =>
                {
                    return null;
                });
            }

            var serializer = MessagePackSerializer.Get(type);

            logger.LogDebug($"Do deserialize.");
            return serializer.UnpackSingleObjectAsync(data, CancellationToken.None);
        }

        public Task<byte[]> SerializeAsync(object obj)
        {
            if (obj == null)
            {
                logger.LogDebug($"Serialize data is null.");
                return Task.Factory.StartNew<byte[]>(() =>
                {
                    return null;
                });
            }

            var serializer = MessagePackSerializer.Get(obj.GetType());

            logger.LogDebug($"Do serialize.");
            return serializer.PackSingleObjectAsync(obj, CancellationToken.None);
        }
    }
}
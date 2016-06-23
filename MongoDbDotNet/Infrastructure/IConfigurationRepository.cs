﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbDotNet.Infrastructure
{
    public interface IConfigurationRepository
    {
        T GetConfigurationValue<T>(string key);
        T GetConfigurationValue<T>(string key, T defaultValue);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public class MessageToConnect
    {
        public long IdUser { get; set; }
        public long IdConnector { get; set; }
        public string Link { get; set; }
    }
}

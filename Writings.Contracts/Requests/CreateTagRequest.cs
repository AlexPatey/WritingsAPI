﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Contracts.Requests
{
    public class CreateTagRequest
    {
        public required string TagName { get; set; }
        public required Guid WritingId { get; set; }
    }
}

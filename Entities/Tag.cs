﻿using System;
using System.Collections.Generic;

namespace FUNewsApp.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public string? Note { get; set; }

    public virtual NewsTag? NewsTag { get; set; }
}

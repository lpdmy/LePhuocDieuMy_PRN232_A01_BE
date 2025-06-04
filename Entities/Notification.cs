using System;
using System.Collections.Generic;

namespace FUNewsApp.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public virtual SystemAccount User { get; set; } = null!;
}

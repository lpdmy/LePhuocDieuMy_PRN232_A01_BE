using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsApp.Models;

public partial class NewsTag
{
    [Key]
    public int NewsArticleId { get; set; }

    public int TagId { get; set; }

    public virtual NewsArticle NewsArticle { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}

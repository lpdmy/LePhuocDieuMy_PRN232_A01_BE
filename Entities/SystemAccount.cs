using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUNewsApp.Models;

public partial class SystemAccount
{
    [Key]
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string AccountEmail { get; set; } = null!;

    public int AccountRole { get; set; }

    public string AccountPassword { get; set; } = null!;

    public virtual ICollection<NewsArticle> NewsArticleCreatedBies { get; set; } = new List<NewsArticle>();

    public virtual ICollection<NewsArticle> NewsArticleUpdatedBies { get; set; } = new List<NewsArticle>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}

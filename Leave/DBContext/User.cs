using System;
using System.Collections.Generic;

namespace Leave.DBContext;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AllLeaf> AllLeaves { get; set; } = new List<AllLeaf>();
}

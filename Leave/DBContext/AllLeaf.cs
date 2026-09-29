using System;
using System.Collections.Generic;

namespace Leave.DBContext;

public partial class AllLeaf
{
    public int LeaveId { get; set; }

    public int UserId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? IsApprove { get; set; }

    public virtual User User { get; set; } = null!;
}

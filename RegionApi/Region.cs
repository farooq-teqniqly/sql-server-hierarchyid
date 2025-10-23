using System;
using System.Collections.Generic;

namespace RegionApi;

public partial class Region
{
    public Guid RegionId { get; set; }

    public string Name { get; set; } = null!;
}

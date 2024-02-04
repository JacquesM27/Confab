﻿namespace Confab.Modules.Attendances.Domain.Policies;

public class SlotPolicyFactory : ISlotPolicyFactory
{
    public ISlotPolicy Get(params string[] tags)
        => tags switch
        {
            not null when tags.Contains("stationary") => new RegularSlotPolicy(),
            not null when tags.Contains("workshops") => new RegularSlotPolicy(),
            _ => new OverbookingSlotPolicy()
        };
}
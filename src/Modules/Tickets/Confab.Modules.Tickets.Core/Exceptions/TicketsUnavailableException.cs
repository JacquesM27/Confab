﻿using Confab.Shared.Abstractions.Exceptions;

namespace Confab.Modules.Tickets.Core.Exceptions;

internal sealed class TicketsUnavailableException(Guid conferenceId) 
    : ConfabException("There are no available tickets for the conference.")
{
    public Guid ConferenceId => conferenceId;
}
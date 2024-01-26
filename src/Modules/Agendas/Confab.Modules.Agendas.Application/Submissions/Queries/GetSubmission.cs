﻿using Confab.Modules.Agendas.Application.Submissions.DTO;
using Confab.Shared.Abstractions.Queries;

namespace Confab.Modules.Agendas.Application.Submissions.Queries;

public record GetSubmission : IQuery<SubmissionDto>
{
    public Guid Id { get; set; }
}
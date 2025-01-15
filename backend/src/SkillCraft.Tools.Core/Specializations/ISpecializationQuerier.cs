﻿using Logitar.Portal.Contracts.Search;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations;

public interface ISpecializationQuerier
{
  Task<SpecializationId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<SpecializationModel> ReadAsync(Specialization specialization, CancellationToken cancellationToken = default);
  Task<SpecializationModel?> ReadAsync(SpecializationId id, CancellationToken cancellationToken = default);
  Task<SpecializationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SpecializationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<SpecializationModel>> SearchAsync(SearchSpecializationsPayload payload, CancellationToken cancellationToken = default);
}

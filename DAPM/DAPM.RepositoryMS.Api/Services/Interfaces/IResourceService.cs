﻿using DAPM.RepositoryMS.Api.Models.PostgreSQL;

namespace DAPM.RepositoryMS.Api.Services.Interfaces
{
    public interface IResourceService
    {
        Task<int> AddResource(Resource resource);

        Task<Resource> RetrieveResource(string resourceName);
    }
}

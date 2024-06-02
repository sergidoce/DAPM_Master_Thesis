﻿using DAPM.ClientApi.Models;
using DAPM.ClientApi.Models.DTOs;
using DAPM.ClientApi.Services;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQLibrary.Models;

namespace DAPM.ClientApi.Controllers
{
    [ApiController]
    [Route("organizations/")]
    public class RepositoryController : ControllerBase
    {

        private readonly ILogger<RepositoryController> _logger;
        private readonly IRepositoryService _repositoryService;
        public RepositoryController(ILogger<RepositoryController> logger, IRepositoryService repositoryService)
        {
            _logger = logger;
            _repositoryService = repositoryService;
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}")]
        public async Task<ActionResult<Guid>> GetRepositoryById(int organizationId, int repositoryId)
        {
            Guid id = _repositoryService.GetRepositoryById(organizationId, repositoryId);
            return Ok(new ApiResponse { RequestName = "GetRepositoryById", TicketId = id});
        }

        [HttpGet("{organizationId}/repositories/{repositoryId}/resources")]
        public async Task<ActionResult<Guid>> GetResourcesOfRepository(int organizationId, int repositoryId)
        {
            Guid id = _repositoryService.GetResourcesOfRepository(organizationId, repositoryId);
            return Ok(new ApiResponse { RequestName = "GetResourcesOfRepository", TicketId = id});
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/resources")]
        public async Task<ActionResult<Guid>> PostResourceToRepository(int organizationId, int repositoryId, [FromForm]ResourceForm resourceForm)
        {
            if (resourceForm.Name == null || resourceForm.ResourceFile == null)
                return BadRequest();

            Guid id = _repositoryService.PostResourceToRepository(organizationId, repositoryId, resourceForm.Name, resourceForm.ResourceFile);
            return Ok(new ApiResponse { RequestName = "PostResourceToRepository", TicketId = id });
        }

        [HttpPost("{organizationId}/repositories/{repositoryId}/pipelines")]
        public async Task<ActionResult<Guid>> PostPipelineToRepository(int organizationId, int repositoryId, [FromBody]PipelineApiDto pipelineApiDto)
        {
            Guid id = _repositoryService.PostPipelineToRepository(organizationId, repositoryId, pipelineApiDto);
            return Ok(new ApiResponse { RequestName = "PostPipelineToRepository", TicketId = id });
        }

    }
}

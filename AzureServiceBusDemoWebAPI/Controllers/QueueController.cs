using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusDemoWebAPI.Services;
using AzureServiceBusModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureServiceBusDemoWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly ILogger<QueueController> _logger;
        private readonly IQueueService queueService;

        public QueueController(ILogger<QueueController> logger, IQueueService queueService)
        {
            _logger = logger;
            this.queueService = queueService;
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> AddPerson(Person person)
        {
            await queueService.SendMessage(person);
            _logger.LogInformation($"Successfully created Person with Name: {person.Name} and Age: {person.Age}");
            return Created($"Request.GetEncodedUrl()/{person.Age}", person);
        }
    }
}

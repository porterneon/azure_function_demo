using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using AzureFunctionDemo.Dal.Interfaces;
using AzureFunctionDemo.Dal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctionDemo
{
    public class PostUserProfile
    {
        private readonly IUserProfileService _service;

        public PostUserProfile(IUserProfileService service)
        {
            _service = service;
        }

        [FunctionName("UserProfile")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<List<UserProfile>>(requestBody);

                if (data == null)
                    return new BadRequestObjectResult("Request body is required.");

                var validRequest = true;
                var validationResults = new List<ValidationResult>();

                foreach (var model in data)
                {
                    var itemValidationResults = new List<ValidationResult>();
                    var valid = Validator.TryValidateObject(model, new ValidationContext(model, null, null),
                        itemValidationResults, true);

                    validationResults.AddRange(itemValidationResults);

                    if (validRequest)
                        validRequest = valid;
                }

                if (!validRequest)
                    return new BadRequestObjectResult(validationResults);

                await UpsertAsync(data);

                return new OkObjectResult("Ok");
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                var code = 500;

                if (e is JsonReaderException ||
                    e is FormatException ||
                    e is RuntimeBinderException)
                    code = 400;

                return new ObjectResult(JsonConvert.SerializeObject(e.Message))
                {
                    StatusCode = code
                };
            }
        }

        public async Task UpsertAsync(List<UserProfile> models)
        {
            foreach (var model in models)
            {
                if (string.IsNullOrEmpty(model.GlobalId)) continue;
                var entity = await _service.GetAsync(model.GlobalId);

                if (entity != null)
                {
                    await _service.UpdateAsync(entity, model);
                }
                else
                {
                    await _service.InsertAsync(model);
                }
            }
        }
    }
}
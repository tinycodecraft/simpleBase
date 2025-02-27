using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace uploadBase.Web.Helpers
{

    public class CustomError
    {
        /// <summary>
        /// The error code
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// A message from and to the Developer
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class CustomValidationProblemDetails : ValidationProblemDetails
    {
        public CustomValidationProblemDetails()
        {
        }

        public CustomValidationProblemDetails(IEnumerable<CustomError> errors)
        {
            Errors = errors;
        }

        public CustomValidationProblemDetails(ModelStateDictionary modelState)
        {
            Errors = ConvertModelStateErrorsToValidationErrors(modelState);
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public new string? Detail { get; set; }

        [JsonPropertyName("errors")]
        public new IEnumerable<CustomError> Errors { get; } = new List<CustomError>();

        private List<CustomError> ConvertModelStateErrorsToValidationErrors(ModelStateDictionary modelStateDictionary)
        {
            List<CustomError> validationErrors = new();

            foreach (var keyModelStatePair in modelStateDictionary)
            {
                var errors = keyModelStatePair.Value.Errors;
                switch (errors.Count)
                {
                    case 0:
                        continue;

                    case 1:
                        validationErrors.Add(item: new CustomError { Code = string.Empty, Message = errors[0].ErrorMessage });
                        break;

                    default:
                        var errorMessage = string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage));
                        validationErrors.Add(new CustomError { Message = errorMessage });
                        break;
                }
            }

            return validationErrors;
        }
    }
}

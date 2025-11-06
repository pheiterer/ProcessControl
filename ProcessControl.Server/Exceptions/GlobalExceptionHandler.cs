using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProcessControl.Application.Exceptions;

namespace ProcessControl.API.Exceptions
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case NotFoundException e:
                    problemDetails.Title = "Recurso não encontrado";
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Detail = e.Message;
                    break;

                case DuplicateEntryException e:
                    problemDetails.Title = "Conflito de dados";
                    problemDetails.Status = (int)HttpStatusCode.Conflict;
                    problemDetails.Detail = e.Message;
                    break;

                case ForeignKeyViolationException e:
                    problemDetails.Title = "Referência de dados inválida";
                    problemDetails.Status = (int)HttpStatusCode.Conflict;
                    problemDetails.Detail = e.Message;
                    break;

                case InvalidOperationException e:
                    problemDetails.Title = "Operação inválida";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Detail = e.Message;
                    break;

                case ValidationException e:
                    problemDetails.Title = "Erro de validação";
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Extensions["errors"] = e.Errors.Select(err => new { err.PropertyName, err.ErrorMessage });
                    break;

                default:
                    problemDetails.Title = "Erro interno do servidor";
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

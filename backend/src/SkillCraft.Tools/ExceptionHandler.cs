﻿using FluentValidation;
using Logitar;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Logging;

namespace SkillCraft.Tools;

internal class ExceptionHandler : IExceptionHandler
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static ExceptionHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private readonly ProblemDetailsFactory _problemDetailsFactory;
  private readonly IProblemDetailsService _problemDetailsService;

  public ExceptionHandler(ProblemDetailsFactory problemDetailsFactory, IProblemDetailsService problemDetailsService)
  {
    _problemDetailsFactory = problemDetailsFactory;
    _problemDetailsService = problemDetailsService;
  }

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    int? statusCode = GetStatusCode(exception);
    if (statusCode == null)
    {
      return false;
    }

    Error error = GetError(exception);
    ProblemDetails problemDetails = _problemDetailsFactory.CreateProblemDetails(
      httpContext,
      statusCode,
      title: FormatToTitle(error.Code),
      type: null,
      detail: error.Message,
      instance: httpContext.Request.GetDisplayUrl());
    problemDetails.Extensions.TryAdd("error", error);
    if (exception is ValidationException validation)
    {
      problemDetails.Extensions.TryAdd("errors", validation.Errors);
    }

    httpContext.Response.StatusCode = statusCode.Value;
    ProblemDetailsContext context = new()
    {
      HttpContext = httpContext,
      ProblemDetails = problemDetails,
      Exception = exception
    };
    bool result = await _problemDetailsService.TryWriteAsync(context);
    if (result)
    {
      ILoggingService loggingService = httpContext.RequestServices.GetRequiredService<ILoggingService>();
      loggingService.Report(exception);
    }
    return result;
  }

  private static string FormatToTitle(string code)
  {
    List<string> words = new(capacity: code.Length);

    StringBuilder word = new();
    for (int i = 0; i < code.Length; i++)
    {
      char? previous = (i > 0) ? code[i - 1] : null;
      char current = code[i];
      char? next = (i < code.Length - 1) ? code[i + 1] : null;

      if (char.IsUpper(current) && ((previous.HasValue && char.IsLower(previous.Value)) || (next.HasValue && char.IsLower(next.Value))))
      {
        if (word.Length > 0)
        {
          words.Add(word.ToString());
          word.Clear();
        }
      }

      word.Append(current);
    }
    if (word.Length > 0)
    {
      words.Add(word.ToString());
    }

    return string.Join(' ', words);
  }

  private static Error GetError(Exception exception)
  {
    if (exception is ErrorException errorException)
    {
      return errorException.Error;
    }
    if (exception is ValidationException)
    {
      return new Error("Validation", "Validation failed.");
    }

    Error error = new(exception.GetErrorCode(), exception.Message.Remove("\r").Split('\n').First());
    foreach (DictionaryEntry item in exception.Data)
    {
      try
      {
        string? key = item.Key is string keyString ? keyString : JsonSerializer.Serialize(item.Key, item.Key.GetType(), _serializerOptions);
        error.Data[key] = item.Value;
      }
      catch (Exception)
      {
      }
    }
    return error;
  }

  private static int? GetStatusCode(Exception exception)
  {
    if (exception is BadRequestException || exception is ValidationException)
    {
      return StatusCodes.Status400BadRequest;
    }
    if (exception is NotFoundException)
    {
      return StatusCodes.Status404NotFound;
    }
    if (exception is ConflictException)
    {
      return StatusCodes.Status409Conflict;
    }

    return null;
  }
}

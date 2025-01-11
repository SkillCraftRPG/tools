﻿namespace SkillCraft.Tools.Core.Errors;

public abstract class ErrorException : Exception
{
  public abstract Error Error { get; }

  protected ErrorException(string message, Exception? innerException = null) : base(message, innerException)
  {
  }
}

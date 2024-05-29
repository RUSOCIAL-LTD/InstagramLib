using System;

namespace InstagramLib.Exceptions;
public class AndroidIDNotSet: Exception
{
    public AndroidIDNotSet() : base("Android ID needed")
    {
    }
}

public abstract class OpenIssueException : Exception
{
    protected OpenIssueException(string error, Exception exception = null) : base($"${error}. Contact RUSocial to report a bug / Open a Issue.{exception?.Message}", exception)
    {
    }
}

public class SignerException : OpenIssueException
{
    public SignerException(string error): base(error)
    {
    }
}

public class FailedToInitClient : Exception
{
    public FailedToInitClient() : base("Init Client Failed Retry")
    {
    }
}

public class DeserializationException : OpenIssueException
{
    public DeserializationException(string typeName) : base($"Unable to deserialize data into type \"{typeName}\"")
    {
    }
}

public class SerializationException : OpenIssueException
{
    public SerializationException(string typeName, Exception innerException) : base($"Unable to deserialize data into type \"{typeName}\"", innerException)
    {
    }
}

public class FailedToPredictGenderException : OpenIssueException
{
    public FailedToPredictGenderException() : base($"Failed to predict gender")
    {
    }
}

public class EmptyIEnumerableException : OpenIssueException
{
    public EmptyIEnumerableException() : base($"The code was expecting elements in side a collection but it was empty")
    {
    }
}

public class DeadProxyException : Exception
{
    public DeadProxyException() : base("Dead Proxy")
    {
    }
}

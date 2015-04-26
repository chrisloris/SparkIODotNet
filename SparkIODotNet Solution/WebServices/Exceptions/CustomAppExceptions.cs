using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkIO.WebServices.Exceptions
{
    public class InvalidVariableOrFunctionException : ApplicationException
    { public InvalidVariableOrFunctionException() : base() { } }
    
    public class UsernameOrPasswordIncorrectException : ApplicationException
    { public UsernameOrPasswordIncorrectException() : base() { } }
    
    public class NotAuthorizedForThisCoreException : ApplicationException
    { public NotAuthorizedForThisCoreException() : base() { } }
    
    public class CoreNotConnectedToCloudException : ApplicationException
    { public CoreNotConnectedToCloudException() : base() { } }
    
    public class SparkCloudConnectionTimeoutException : ApplicationException
    { public SparkCloudConnectionTimeoutException() : base() { } }
    
    public class SparkCloudNotAvailableException : ApplicationException
    { public SparkCloudNotAvailableException() : base() { } }
    
    public class UnknownNetworkConnectionErrorException : ApplicationException
    { public UnknownNetworkConnectionErrorException() : base() { } }
}

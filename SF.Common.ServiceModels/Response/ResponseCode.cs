using System.ComponentModel;

namespace SF.Common.ServiceModels.Response
{
    public enum ResponseCode
    {
        [Description("UNDEFINED")]
        Undefined = 0,

        [Description("SUCCESS")]
        Success = 1,

        [Description("VALIDATION_ERROR")]
        ValidationError = 2,

        [Description("NOT_FOUND")]
        NotFound = 3,

        [Description("SERVER_ERROR")]
        ServerError = 4
    }
}
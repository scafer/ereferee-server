using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ereferee.Models
{
    public class Result
    {
        public int errorCode { get; set; }
        public string errorMessage { get; set; }

        private Result(int errorCode, string errorMessage)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }

        public static Result Get(int error, string message)
        {
            return new Result(error, message);
        }
    }

    public enum ErrorCode
    {
        OK = 0,
        NOK = 1
    }

    public enum ErrorMessage
    {
        USER_EXISTS = "",
        EMAIL_EXISTS
    }
}

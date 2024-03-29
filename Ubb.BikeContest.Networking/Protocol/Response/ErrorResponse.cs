using System;

namespace Ubb.BikeContest.Networking.Protocol.Response
{
    [Serializable]
    public class ErrorResponse : IResponse
    {
        private readonly string message;

        public ErrorResponse(string message)
        {
            this.message = message;
        }

        public virtual string Message
        {
            get
            {
                return message;
            }
        }
    }

}
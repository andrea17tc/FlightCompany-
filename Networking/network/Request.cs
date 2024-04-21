using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.network
{
    [Serializable]
    public class Request
    {
        private RequestType type;
        private Object data;

        private Request() { }
        public RequestType Type()
        {
            return type;
        }

        public Object Data()
        {
            return data;
        }

        public string toString()
        {
            return "Request{" +
                    "type='" + type + '\'' +
                    ", data='" + data + '\'' +
                    '}';
        }

        public class Builder
        {
            private Request request = new Request();

            public Builder type(RequestType type)
            {
                request.type = type;
                return this;
            }

            public Builder data(Object data)
            {
                request.data = data;
                return this;
            }

            public Request build()
            {
                return request;
            }

        }

        private void Data(Object data)
        {
            this.data = data;
        }

        private void Type(RequestType type)
        {
            this.type = type;
        }
    }

}

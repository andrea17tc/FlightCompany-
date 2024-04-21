using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.network
{
    [Serializable]
    public class Response
    {
        private ResponseType type;
        private Object data;

        private Response() { }

        public ResponseType Type()
        {
            return type;
        }

        public Object Data()
        {
            return data;
        }

        private void Type(ResponseType type)
        {
            this.type = type;
        }

        private void Data(Object data)
        {
            this.data = data;
        }
        public string toString()
        {
            return "Response{" +
                    "type='" + type + '\'' +
                    ", data='" + data + '\'' +
                    '}';
        }


        public class Builder
        {
            private Response response = new Response();

            public Builder Type(ResponseType type)
            {
                response.Type(type);
                return this;
            }

            public Builder Data(Object data)
            {
                response.Data(data);
                return this;
            }

            public Response Build()
            {
                return response;
            }
        }
    }
}

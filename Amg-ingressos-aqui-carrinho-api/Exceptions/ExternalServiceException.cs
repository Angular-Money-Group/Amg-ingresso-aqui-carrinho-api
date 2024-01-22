using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Exceptions
{
    public class ExternalServiceException: Exception
    {

        public ExternalServiceException()
        {
        }

        public ExternalServiceException(string message)
            : base(message)
        {
        }

        public ExternalServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
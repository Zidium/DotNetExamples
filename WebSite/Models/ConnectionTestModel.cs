using System;

namespace Zidium.Examples.Models
{
    public class ConnectionTestModel
    {
        public bool ZidiumComponentIsFake { get; set; }

        public Guid ComponentId { get; set; }

        public bool IsResponseSuccess { get; set; }

        public int? ResponseCode { get; set; }

        public string ResponseErrorMessage { get; set; }
    }
}
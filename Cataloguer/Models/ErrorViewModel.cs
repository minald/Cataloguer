using System;

namespace Cataloguer.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public ErrorViewModel(string requestId) => RequestId = requestId;

        public bool ShowRequestId => !String.IsNullOrEmpty(RequestId);
    }
}

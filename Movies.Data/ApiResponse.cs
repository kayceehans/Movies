using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Data
{
    public class ApiResponse<T>
    {
        public T payload { get; set; } = default(T);
        public bool status { get; set; }
        public string message { get; set; }
        public ResponseCodesEnum responseCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Security;

namespace CustomHTTPRequestNS
{
    /// <summary>
    /// 
    /// </summary>
    public class ParallelRequest : CustomHTTPRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public ParallelRequest()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            UserAgent = "Agent X";

            XDeveloper = "";
            this.Timeout = 120 * 1000;
            CharEncode = Encoding.Default;
        }
    }
}
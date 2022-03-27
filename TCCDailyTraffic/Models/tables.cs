using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TCCDailyTraffic.Models
{
    public class tables
    {
        public DataTable dtblFirst { get; set; }
        public DataTable dtblSecond { get; set; }
        public DataTable dtblThird { get; set; }

        public DataSet dtsSecond { get; set; }
    }
}
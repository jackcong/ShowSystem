using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Col
{
    public class colModel
    {
        public string columnName { get; set; }
        public string columnValue { get; set; }
        public string columnOperator { get; set; }
        public string isOutParam { get; set; }
        public string extraData { get; set; }
    }
    public class colInfoModel
    {
        public string columnName { get; set; }
        public string disName_en { get; set; }
        public string disType { get; set; }
        public string isHidden { get; set; }
        public int sortOrder { get; set; }
        public string extraData { get; set; }
    }
}

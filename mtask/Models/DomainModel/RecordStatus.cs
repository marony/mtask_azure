using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mtask.Models.DomainModel
{
    public enum RecordStatus : byte
    {
        Deactive = 0,
        Active,
        Deleted = 9
    }
}
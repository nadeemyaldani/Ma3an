using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Models
{
    public enum UserType
    {
        Admin = 0,
        [Description("مستخدم")]
        Public = 3,
        [Description("مشرف النظام")]
        Approver = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Interface
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        string FullName { get; }
        string RoleId { get; }
    }
}

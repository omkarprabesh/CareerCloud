using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    class SecurityLoginsLogLogic: BaseLogic<SecurityLoginsLogPoco>
    {
        public SecurityLoginsLogLogic
           (IDataRepository<SecurityLoginsLogPoco> repository) : base(repository)
        { }
    }
}

using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.SecurityLogin;

namespace CareerCloud.gRPC.Services
{
    public class SecurityLoginService: SecurityLoginBase
    {
            private readonly SecurityLoginLogic _logic;

            public SecurityLoginService()
            {
                _logic = new SecurityLoginLogic(new EFGenericRepository<SecurityLoginPoco>());
            }



            private SecurityLoginReply FromPoco(SecurityLoginPoco poco)
            {
                return new SecurityLoginReply()
                {
                    Id = poco.Id.ToString(),
                    Login = poco.Login,
                    Password = poco.Password,
                    Created = poco.Created.ToTimestamp(),
                    PasswordUpdate= poco.PasswordUpdate.ToTimeStamp(),
                    AgreementAccepted = poco.AgreementAccepted.ToTimeStamp(),
                    IsLocked= poco.IsLocked,
                    IsInactive= poco.IsInactive,
                    EmailAddress= poco.EmailAddress,
                    PhoneNumber= poco.PhoneNumber,
                    FullName= poco.FullName,
                    ForceChangePassword= poco.ForceChangePassword,
                    PreferredLanguage= poco.PrefferredLanguage,

                    TimeStamp = ByteString.CopyFrom(poco.TimeStamp)
                };

            }

            private List<SecurityLoginPoco> ToPoco(SecurityLogins secLogs)
            {
                List<SecurityLoginPoco> pocos = new List<SecurityLoginPoco>();
                foreach (var reply in secLogs.SecLogs)
                {
                    SecurityLoginPoco poco = new SecurityLoginPoco();

                    poco.Id = Guid.Parse(reply.Id);
                    poco.Login = reply.Login;
                    poco.Password = reply.Password;
                    poco.Created= reply.Created.ToDateTime();
                    poco.PasswordUpdate = reply.PasswordUpdate.ToDateTime();
                poco.AgreementAccepted = reply.AgreementAccepted.ToDateTime();
                poco.IsLocked = reply.IsLocked;
                poco.IsInactive = reply.IsInactive;
                poco.EmailAddress = reply.EmailAddress;
                poco.PhoneNumber = reply.PhoneNumber;
                poco.FullName = reply.FullName;
                poco.ForceChangePassword = reply.ForceChangePassword;
                poco.PrefferredLanguage = reply.PreferredLanguage;

                    pocos.Add(poco);

                }
                return pocos;
            }


            public override Task<SecurityLoginReply> GetSecurityLogin(Protos.SLIdRequest request, ServerCallContext context)
            {
                SecurityLoginPoco poco = _logic.Get(Guid.Parse(request.Id));
                return Task.FromResult(FromPoco(poco));
            }

            public override Task<SecurityLogins> GetSecurityLogins(Empty request, ServerCallContext context)
            {
                List<SecurityLoginPoco> pocos = _logic.GetAll();
                SecurityLogins seclogs = new SecurityLogins();
                foreach (var poco in pocos)
                {
                    SecurityLoginReply reply = FromPoco(poco);
                    seclogs.SecLogs.Add(reply);
                }
                return Task.FromResult(seclogs);
            }

            public override Task<Empty> AddSecurityLogin(SecurityLogins request, ServerCallContext context)
            {
                var topoco = ToPoco(request).ToArray();
                _logic.Add(topoco);
                return Task.FromResult<Empty>(null);
            }

            public override Task<Empty> DeleteSecurityLogin(SecurityLogins request, ServerCallContext context)
            {
                var topoco = ToPoco(request).ToArray();
                _logic.Delete(topoco);
                return Task.FromResult<Empty>(null);

            }

            public override Task<Empty> UpdateSecurityLogin(SecurityLogins request, ServerCallContext context)
            {
                var topoco = ToPoco(request).ToArray();
                _logic.Update(topoco);
                return Task.FromResult<Empty>(null);
            }
        }
}

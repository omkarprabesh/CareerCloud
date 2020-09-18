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
using static CareerCloud.gRPC.Protos.ApplicantProfile;

namespace CareerCloud.gRPC.Services
{
    public class ApplicantProfileService: ApplicantProfileBase

    {
        private const decimal nanoFactor = 1_000_000_000;
        private decimalValue TodecimalValue(decimal? dec)
        {
            decimalValue value = new decimalValue();
            if (dec == null)
                return null;
            else
            {
                value.Units = (Int64)dec;
                value.Nanos = (Int32)(dec - value.Units * nanoFactor);
            }
            return value;
        }

        private decimal ToDecimal(decimalValue decval)
        {
            decimal value = decval.Units + decval.Nanos/ nanoFactor;
            return value;
        }
        private readonly ApplicantProfileLogic _logic;

        public ApplicantProfileService()
        {
            _logic = new ApplicantProfileLogic(new EFGenericRepository<ApplicantProfilePoco>());
        }



        private ApplicantProfileReply FromPoco(ApplicantProfilePoco poco)
        {
            return new ApplicantProfileReply()
            {
                Id = poco.Id.ToString(),
                Login = poco.Login.ToString(),
                CurrentSalary = TodecimalValue(poco.CurrentSalary),
                CurrentRate= TodecimalValue(poco.CurrentRate),
               Currency = poco.Currency,
            Country = poco.Country,
            Province = poco.Province,
            Street = poco.Street,
            City = poco.City,
            PostalCode = poco.PostalCode,
                TimeStamp = ByteString.CopyFrom(poco.TimeStamp)


            };

        }

        private List<ApplicantProfilePoco> ToPoco(ApplicantProfiles appPro)
        {
            List<ApplicantProfilePoco> pocos = new List<ApplicantProfilePoco>();
            foreach (var reply in appPro.AppProfs)
            {
                ApplicantProfilePoco poco = new ApplicantProfilePoco();
                {
                    poco.Id = Guid.Parse(reply.Id);
                    poco.Login = Guid.Parse(reply.Login);
                    poco.CurrentSalary = ToDecimal(reply.CurrentSalary);
                    poco.CurrentRate = ToDecimal(reply.CurrentRate);
                    poco.Currency = reply.Currency;
                    poco.Country = reply.Country;
                    poco.Province = reply.Province;
                    poco.Street = reply.Street;
                    poco.City = reply.City;
                    poco.PostalCode = reply.PostalCode;


                    pocos.Add(poco);

                }           
            }
            return pocos;
        }


        public override Task<ApplicantProfileReply> GetApplicantProfile(Protos.IdRequest2 request, ServerCallContext context)
        {
            ApplicantProfilePoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<ApplicantProfiles> GetApplicantProfiles(Empty request, ServerCallContext context)
        {
            List<ApplicantProfilePoco> pocos = _logic.GetAll();
            ApplicantProfiles apppro = new ApplicantProfiles();
            foreach (var poco in pocos)
            {
                ApplicantProfileReply reply = FromPoco(poco);
                apppro.AppProfs.Add(reply);
            }
            return Task.FromResult(apppro);
        }

        public override Task<Empty> AddApplicantProfile(ApplicantProfiles request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteApplicantProfile(ApplicantProfiles request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateApplicantProfile(ApplicantProfiles request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}

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
using static CareerCloud.gRPC.Protos.CompanyJob;

namespace CareerCloud.gRPC.Services
{
    public class CompanyJobService: CompanyJobBase
    {
        private readonly CompanyJobLogic _logic;

        public CompanyJobService()
        {
            _logic = new CompanyJobLogic(new EFGenericRepository<CompanyJobPoco>());
        }



        private CompanyJobReply FromPoco(CompanyJobPoco poco)
        {
            return new CompanyJobReply()
            {
                Id = poco.Id.ToString(),
                Company = poco.Company.ToString(),
                ProfileCreated = poco.ProfileCreated.ToTimestamp(),
                IsInActive = poco.IsInactive,
                IsCompanyHidden = poco.IsCompanyHidden,

                TimeStamp = ByteString.CopyFrom(poco.TimeStamp)
            };

        }

        private List<CompanyJobPoco> ToPoco(CompanyJobs comJobs)
        {
            List<CompanyJobPoco> pocos = new List<CompanyJobPoco>();
            foreach (var reply in comJobs.ComJobs)
            {
                CompanyJobPoco poco = new CompanyJobPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Company = Guid.Parse(reply.Company);
                poco.ProfileCreated = reply.ProfileCreated.ToDateTime();
                poco.IsInactive = reply.IsInActive;
                poco.IsCompanyHidden = reply.IsCompanyHidden;

                pocos.Add(poco);

            }
            return pocos;
        }


        public override Task<CompanyJobReply> GetCompanyJob(Protos.CJobIdRequest request, ServerCallContext context)
        {
            CompanyJobPoco poco = _logic.Get(Guid.Parse(request.Id));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<CompanyJobs> GetCompanyJobs(Empty request, ServerCallContext context)
        {
            List<CompanyJobPoco> pocos = _logic.GetAll();
            CompanyJobs comJobs = new CompanyJobs();
            foreach (var poco in pocos)
            {
                CompanyJobReply reply = FromPoco(poco);
                comJobs.ComJobs.Add(reply);
            }
            return Task.FromResult(comJobs);
        }

        public override Task<Empty> AddCompanyJob(CompanyJobs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteCompanyJob(CompanyJobs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateCompanyJob(CompanyJobs request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
}

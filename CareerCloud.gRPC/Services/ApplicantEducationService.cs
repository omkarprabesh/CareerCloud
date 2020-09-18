using CareerCloud.BusinessLogicLayer;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.gRPC.Protos;
using CareerCloud.Pocos;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CareerCloud.gRPC.Protos.ApplicantEducation;

namespace CareerCloud.gRPC.Services
{
    public static class ForTimeStampConversion
    {
        public static Timestamp ToTimeStamp(this DateTime? datetime) => datetime == null ? null : Timestamp.FromDateTime((DateTime)datetime);
    }
    public class ApplicantEducationService : ApplicantEducationBase
    {
        private readonly ApplicantEducationLogic _logic;

        public ApplicantEducationService()
        {
            _logic = new ApplicantEducationLogic(new EFGenericRepository<ApplicantEducationPoco>());
        }

        private ApplicantEducationReply FromPoco(ApplicantEducationPoco poco)
        {
            return new ApplicantEducationReply()
            {
                Id = poco.Id.ToString(),
                Applicant = poco.Applicant.ToString(),
                Major = poco.Major,
                CertificateDiploma = poco.CertificateDiploma,
                StartDate = poco.StartDate.ToTimeStamp(),
                CompletionDate = poco.CompletionDate.ToTimeStamp(),
                CompletionPercent = poco.CompletionPercent == null ? 0 : (byte)poco.CompletionPercent,


               Timestamp = ByteString.CopyFrom(poco.TimeStamp)
            };
            
        }

        private List<ApplicantEducationPoco> ToPoco(ApplicantEducations appEdu)
        {
            List<ApplicantEducationPoco> pocos = new List<ApplicantEducationPoco>();
            foreach (var reply in appEdu.AppEdus)
            {
                ApplicantEducationPoco poco = new ApplicantEducationPoco();

                poco.Id = Guid.Parse(reply.Id);
                poco.Applicant = Guid.Parse(reply.Applicant);
                poco.Major = reply.Major;
                poco.CertificateDiploma = reply.CertificateDiploma;
                poco.StartDate = reply.StartDate.ToDateTime();
                poco.CompletionDate = reply.CompletionDate.ToDateTime();
                poco.CompletionPercent = (byte?)reply.CompletionPercent;
                pocos.Add(poco);
    
            }
            return pocos;
        }

        public override Task<ApplicantEducationReply> GetApplicantEducation(IdRequest request, ServerCallContext context)
        {
            ApplicantEducationPoco poco = _logic.Get((Guid.Parse(request.Id)));
            return Task.FromResult(FromPoco(poco));
        }

        public override Task<ApplicantEducations> GetApplicantEducations(Empty request, ServerCallContext context)
        {
            List<ApplicantEducationPoco> pocos = _logic.GetAll();
            ApplicantEducations edus = new ApplicantEducations();
            foreach (var poco in pocos)
            {
                ApplicantEducationReply reply = FromPoco(poco);
                edus.AppEdus.Add(reply);
            }
            return Task.FromResult(edus);
        }

        public override Task<Empty> AddApplicantEducations(ApplicantEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Add(topoco);
            return Task.FromResult<Empty>(null);
        }

        public override Task<Empty> DeleteApplicantEducation(ApplicantEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Delete(topoco);
            return Task.FromResult<Empty>(null);

        }

        public override Task<Empty> UpdateApplicantEducation(ApplicantEducations request, ServerCallContext context)
        {
            var topoco = ToPoco(request).ToArray();
            _logic.Update(topoco);
            return Task.FromResult<Empty>(null);
        }
    }
   
}

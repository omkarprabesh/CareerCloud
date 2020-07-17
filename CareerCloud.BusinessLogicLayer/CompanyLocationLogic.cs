using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyLocationLogic : BaseLogic<CompanyLocationPoco>
    {

        public CompanyLocationLogic
            (IDataRepository<CompanyLocationPoco> repository) : base(repository)
        { }

        public override void Add(CompanyLocationPoco[] pocos)
        {
            Verify(pocos);
            base.Add(pocos);
        }
        public override void Update(CompanyLocationPoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }

        protected override void Verify(CompanyLocationPoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();

            foreach (CompanyLocationPoco poco in pocos)
            {
                if ((poco.CountryCode ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(500, "Cannot be empty"));
                }
                if ((poco.Province ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(501, "Cannot be empty"));
                }

                if ((poco.Street ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(502, "Cannot be empty"));
                }

                if ((poco.City ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(503, "Cannot be empty"));
                }

                if ((poco.PostalCode?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(504, "Cannot be empty"));
                }
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}
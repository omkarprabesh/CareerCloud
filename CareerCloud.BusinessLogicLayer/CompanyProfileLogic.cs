using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyProfileLogic : BaseLogic<CompanyProfilePoco>
    {

        public CompanyProfileLogic
            (IDataRepository<CompanyProfilePoco> repository) : base(repository)
        { }

        public override void Add(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);
            base.Add(pocos);
        }
        public override void Update(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }

        protected override void Verify(CompanyProfilePoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();

            foreach (CompanyProfilePoco poco in pocos)
            {
                if ((poco.CompanyWebsite ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(600, "Not a valid extension"));
                }
                else if ((!poco.CompanyWebsite.EndsWith(".ca")) || (!poco.CompanyWebsite.EndsWith(".com"))
                    || (!poco.CompanyWebsite.EndsWith(".biz")))
                    
                {
                    exceptions.Add(new ValidationException(600, "Not a valid extension"));
                }

                if ((poco.ContactPhone ?? "1") == "1")
                {
                    exceptions.Add(new ValidationException(601, "Not a valid number"));
                }
                else
                {
                    char[] charNumbers = poco.ContactPhone.ToCharArray();
                    string validCharacters = "0123456789-";

                    for (int i = 0; i < charNumbers.Length; i++)
                    {
                        if (!validCharacters.Contains(charNumbers[i]))
                        {
                            exceptions.Add(new ValidationException(601, "Not a valid number"));
                        }
                    }


                        if (poco.ContactPhone.Length != 12
                             || (poco.ContactPhone.IndexOf('-') != 3)
                             || (poco.ContactPhone.LastIndexOf('-') != 7)
                             || ((poco.ContactPhone.Substring(4)).IndexOf('-') != 3))
                        {
                            exceptions.Add(new ValidationException(601, "Not a valid number"));
                        }
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}
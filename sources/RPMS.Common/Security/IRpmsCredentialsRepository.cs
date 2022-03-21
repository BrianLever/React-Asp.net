using System;
using System.Collections.Generic;

namespace RPMS.Common.Security
{
    public interface IRpmsCredentialsRepository
    {
        List<RpmsCredentials> GetAll();
        RpmsCredentials Get();

        void Add(RpmsCredentials openTextCredentials);

        bool Delete(Guid id);
    }
}
using System;

namespace ereferee.Services
{
    public class BaseService : IDisposable
    {
        ~BaseService()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
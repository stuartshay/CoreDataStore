using AutoMapper;

namespace CoreDataStore.Service.Mappings
{
    public static class AutoMapperConfiguration
    {
        private static bool _isMappingInitialized;

        private static readonly object ThisLock = new object();

        public static void Configure()
        {
            lock (ThisLock)
            {
                if (!_isMappingInitialized)
                {
                    //Mapper.Initialize(x =>
                    //{
                    //    x.AddProfile<LpcReportMapping>();
                    //    x.AddProfile<LandmarkMapping>();
                    //    x.AddProfile<PlutoMapping>();
                    //});
                }

                _isMappingInitialized = true;
                //Mapper.AssertConfigurationIsValid();
            }
        }
    }
}

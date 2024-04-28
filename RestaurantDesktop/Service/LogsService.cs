using RestaurantDesktop.Interface;
using RestSharp;

namespace RestaurantDesktop.Service
{
    public class LogsService : ILogsService
    {
        private readonly ILogsRepository _logsRepository;
        public LogsService(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

    }
}

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

        public async Task<RestResponse> GetLoginLogs(string userToken)
        {
            return await _logsRepository.GetLoginLogs(userToken);
        }
    }
}

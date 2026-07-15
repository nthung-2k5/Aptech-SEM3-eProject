namespace GiveAID.Utils;

public class QueuedHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueuedHostedService> _logger;

    public QueuedHostedService(IBackgroundTaskQueue taskQueue, IServiceProvider serviceProvider, ILogger<QueuedHostedService> logger)
    {
        _taskQueue = taskQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Lấy công việc ra từ Queue
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                // Quan trọng: Tạo một Scope mới để dùng được các Service như IEmailService, DbContext...
                using var scope = _serviceProvider.CreateScope();
                
                // Thực thi công việc (Running state)
                await workItem(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
        
        _logger.LogInformation("Background Service is stopping."); // Terminated state
    }
}
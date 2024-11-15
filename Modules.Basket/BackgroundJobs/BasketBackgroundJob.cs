using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Modules.Basket.BackgroundJobs;

public class BasketBackgroundJob : IJob
{
    private readonly ILogger<BasketBackgroundJob> _logger;

    public BasketBackgroundJob(ILogger<BasketBackgroundJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"Basket Background Job Handled - {DateTime.Now}");
        return Task.CompletedTask;
    }
}

public class BasketBackgroundJobOption : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(BasketBackgroundJob));

        options
            .AddJob<BasketBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithCronSchedule("0 0/1 * * * ?"));
    }
}
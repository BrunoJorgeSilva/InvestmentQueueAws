using InvesmentConsumer.Worker;
using InvestmentConsumer.Application.Interfaces;
using InvestmentConsumer.Application.Services;
using InvestmentConsumer.Infrastructure.External;
using Amazon.SQS;
using InvestmentConsumer.Infrastructure.QueueIntegration;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IInvesmentRepository>(provider =>
    new InvestmentRepository(
        builder.Configuration.GetConnectionString("OracleInvestmentConnection"),
        provider.GetRequiredService<ILogger<InvestmentRepository>>()
    ));

builder.Services.AddScoped<IInvestmentService, InvestmentService>();
builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddSingleton<IQueueIntegration, QueueIntegration>();


builder.Services.AddOptions();

builder.Services.AddSingleton<IAmazonSQS>(sp =>
    new AmazonSQSClient(
        builder.Configuration.GetValue<string>("AWS:AccessKeyId"),
        builder.Configuration.GetValue<string>("AWS:SecretAccessKey"),
        Amazon.RegionEndpoint.USEast1 
    ));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using NLog;
using NLog.Config;
using PdfConverter.Properties;
using Xceed.Words.NET;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Initializing Nlog with nlog-config.xml.
var isRunningOnLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
var rootLightConversionDirectory = isRunningOnLinux ? "/light-conversion" : Path.Combine("C:", "light-conversion");
var logsFolder = Path.Combine(rootLightConversionDirectory, "logs", "chiron");

// NLog doesn't like backslashes.
logsFolder = logsFolder.Replace("\\", "/");

var nlogConfig = Resources.nlog_config.Replace("!LogsFolderTag!", logsFolder);
XmlLoggingConfiguration parsedConfiguration = null;
try {
    using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(nlogConfig));
    using var xmlTextReader = new XmlTextReader(memoryStream);
    parsedConfiguration = new XmlLoggingConfiguration(xmlTextReader);
} catch (Exception ex) {
    Console.WriteLine(@$"Parsing nlog configuration failed because of exception {ex}");
    Console.WriteLine(ex.StackTrace);
}
LogManager.Configuration = parsedConfiguration;

var log = LogManager.GetLogger("PdfConvert.Program");
log.Info("PdfConvert app is starting up.");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("XceedDocxLicence"))) {
    log.Error("'XceedDocxLicence' environment variable is missing or empty. App is shutting down.");
    Environment.Exit(-1);
} else {
    Licenser.LicenseKey = Environment.GetEnvironmentVariable("XceedDocxLicence");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
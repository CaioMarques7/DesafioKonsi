using API.DTO.Requisicao;
using API.Validadores;
using FluentValidation;
using InfraEstrutura.Extensoes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace API
{
    /// <summary>
    /// Classe de entrada da aplicação.
    /// </summary>
    public partial class Program
    {
        public const string NOME_AMBIENTE_TESTES = "Testes";

        /// <summary>
        /// Inicia uma nova instância de <see cref="Program"/>.
        /// </summary>
        protected Program() { }

        /// <summary>
        /// Inicializa a construção da API.
        /// </summary>
        /// <param name="args">Parâmetros para inicialização.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddCors();

            builder.Services.AddMemoryCache();

            builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<GzipCompressionProvider>();
                });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:sszzz";
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IValidator<SolicitacaoDeConsultaDeMatricula>, ValidadorDeSolicitacaoDeConsultaDeMatricula>();
            builder.Services.Configure<ExtratoClube.Configuracoes>(options => builder.Configuration.GetSection("ExtratoClube").Bind(options, _ => _.BindNonPublicProperties = true));

            builder.Services.RegistrarDependenciasDeInfraEstrutura(builder.Environment.IsEnvironment(NOME_AMBIENTE_TESTES));

            ConfiguracaoDoSwagger(builder.Services);

            var app = builder.Build();

            app.UseResponseCompression();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio Konsi"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }

        private static void ConfiguracaoDoSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.CustomSchemaIds(x => x.FullName);

                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "3.0.3",
                    Title = "Desafio Konsi",
                    Description = "API para consulta de matrículas"
                });

                s.DescribeAllParametersInCamelCase();

                s.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                s.DocInclusionPredicate((version, api) => true);

                s.TagActionsBy(api => new[] { api.GroupName });

            }).AddSwaggerGenNewtonsoftSupport();
        }
    }
}
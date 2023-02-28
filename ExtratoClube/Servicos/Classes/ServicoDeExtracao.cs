using ExtratoClube.DTO.Resposta;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtratoClube.Servicos.Classes
{
    internal sealed class ServicoDeExtracao : IServicoDeExtracao
    {
        private bool _recursoLiberado;

        private readonly IMemoryCache _cache;
        private readonly Configuracoes _configuracoes;
        private readonly HttpClient _httpClient;

        private string _login;
        private string _senha;

        public ServicoDeExtracao(IMemoryCache cache, IOptions<Configuracoes> configuracoes, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _configuracoes = configuracoes.Value;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_configuracoes.Hostname);
        }

        /// <inheritdoc />
        public IServicoDeExtracao PreencherLogin(string login)
        {
            _login = login;

            return this;
        }

        /// <inheritdoc />
        public IServicoDeExtracao PreencherSenha(string senha)
        {
            _senha = senha;

            return this;
        }

        /// <inheritdoc />
        public async Task<RespostaDeConsulta> ConsultarBeneficios(string numeroDoCpf, CancellationToken cancellationToken)
        {
            var tokenDeAutenticacao = await Autenticar(cancellationToken).ConfigureAwait(false);

            using (var requisicao = new HttpRequestMessage(HttpMethod.Get, string.Format(_configuracoes.EndpointConsulta, numeroDoCpf)))
            {
                requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDeAutenticacao);

                var resposta = await _httpClient.SendAsync(requisicao, cancellationToken).ConfigureAwait(false);
                resposta.EnsureSuccessStatusCode();

                var json = await resposta.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                return JsonConvert.DeserializeObject<RespostaDeConsulta>(json);
            }
        }

        private async Task<string> Autenticar(CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(_login, out string tokenDeAutenticacao))
            {
                using (var requisicao = new HttpRequestMessage(HttpMethod.Post, _configuracoes.EndpointLogin))
                {
                    requisicao.Content = new StringContent(JsonConvert.SerializeObject(new { login = _login, senha = _senha }), Encoding.UTF8, MediaTypeNames.Application.Json);

                    var resposta = await _httpClient.SendAsync(requisicao, cancellationToken).ConfigureAwait(false);
                    resposta.EnsureSuccessStatusCode();

                    tokenDeAutenticacao = resposta.Headers.GetValues("Authorization").First();
                    tokenDeAutenticacao = tokenDeAutenticacao[tokenDeAutenticacao.IndexOf(" ")..].Trim();
                }

                _cache.Set(_login, tokenDeAutenticacao, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(70)
                });
            }

            return tokenDeAutenticacao;
        }

        public void Dispose()
        {
            if (!_recursoLiberado)
            {
                _recursoLiberado = true;

                _httpClient.Dispose();
            }
        }
    }
}

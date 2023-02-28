namespace ExtratoClube
{
    /// <summary>
    /// Modelo de dados para configurações de acesso ao Extrato Clube
    /// </summary>
    public sealed class Configuracoes
    {
        /// <summary>
        /// Hostname do site
        /// </summary>
        public string Hostname { get; private set; }

        /// <summary>
        /// Endpoint para login
        /// </summary>
        public string EndpointLogin { get; private set; }

        /// <summary>
        /// Endpoint para consulta de matrícula
        /// </summary>
        public string EndpointConsulta { get; private set; }
    }
}
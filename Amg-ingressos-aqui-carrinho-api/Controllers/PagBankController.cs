using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/PagBank")]
    [Produces("application/json")]
    public class PagBankController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IPagbankService _pagbankService;

        public PagBankController(
            ILogger<TransactionController> logger,
            IPagbankService transactionService)
        {
            _logger = logger;
            _pagbankService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("publicKey")]
        public async Task<IActionResult> GeneratePublicKey()
        {
            var result = await _pagbankService.GeneratePublicKey();
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }
        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("session3ds")]
        public async Task<IActionResult> GenerateSession()
        {
            var result = await _pagbankService.GenerateSession();
            if (result.Message != null && result.Message.Any())
            {
                _logger.LogInformation(result.Message);
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
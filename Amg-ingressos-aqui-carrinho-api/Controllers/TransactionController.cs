using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transactionos")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionPaymentService _transactionPaymentService;

        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionService transactionService,
            ITransactionPaymentService transactionPaymentService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _transactionPaymentService = transactionPaymentService;
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("createTransaction")]
        public async Task<IActionResult> SaveTransactionAsync(Transaction transaction)
        {
            try
            {
                var result = await _transactionService.SaveAsync(transaction);
                if (result.Message!= null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="payment">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("PayTransaction")]
        public async Task<IActionResult> PayTransactionAsync(TransactionPayment payment)
        {
            try
            {
                var resultSave = await _transactionPaymentService.SaveAsync(payment);
                if (resultSave.Message.Any())
                {
                    _logger.LogInformation(resultSave.Message);
                    return NoContent();
                }

                var resultPayment = await _transactionPaymentService.Payment(payment);
                if (resultPayment.Message.Any())
                {
                    _logger.LogInformation(resultPayment.Message);
                    return NoContent();
                }

                return Ok("Transação Efetivada");
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }
    }
}
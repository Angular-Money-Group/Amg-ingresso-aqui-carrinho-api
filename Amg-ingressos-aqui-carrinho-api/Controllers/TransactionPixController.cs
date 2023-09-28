using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transaction")]
    [Produces("application/json")]
    public class TransactionPixController : ControllerBase
    {
        private readonly ILogger<TransactionPixController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionPixController(
            ILogger<TransactionPixController> logger,
            ITransactionService transactionService
        )
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idUser">id do usuário</param>
        /// <param name="idEvent">id do evento</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("verifyPixPayment/{transactionId}")]
        public async Task<IActionResult> GetStatusPixPaymentAsync([FromRoute] string transactionId)
        {
            try
            {
                var transactionDb = (
                    _transactionService.GetByIdAsync(transactionId).Result.Data
                    as List<GetTransactionEventData>
                ).FirstOrDefault();

                if (
                    transactionDb.Stage != StageTransactionEnum.PaymentTransaction
                    && transactionDb.Status != StatusPaymentEnum.Pending
                )
                    return NotFound("Estágio fora do padrão");

                var result = await _transactionService.GetStatusPixPaymentAsync(
                    transactionDb.PaymentPix.Payment.PaymentId
                );

                var transactionStatus = result.Data as PaymentTransactionGetStatus;

                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                if (transactionStatus.Payment.Status == Enum.StatusCallbackCielo.PaymentConfirmed)
                {
                    var transaction = transactionDb.GeTransactionToPixTransaction();

                    transaction.Status = StatusPaymentEnum.Aproved;
                    transaction.Stage = StageTransactionEnum.Finished;

                    await _transactionService.UpdateAsync(transaction);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByPersonTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByPersonTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPaymentPixData/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPaymentPixDataAsync(
            [FromRoute] string idTransaction,
            [FromBody] PaymentMethodPix transactionDto
        )
        {
            try
            {
                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");

                var transaction = transactionDto.StagePaymentPixDataDtoToTransaction();
                transaction.Id = idTransaction;
                var transactionDb = (
                    _transactionService.GetByIdAsync(transaction.Id).Result.Data
                    as List<GetTransactionEventData>
                ).FirstOrDefault();

                if (
                    transactionDb.Stage != StageTransactionEnum.TicketsData
                    && transactionDb.Status != StatusPaymentEnum.InProgress
                )
                    return NotFound("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;
                transaction.Discount = transactionDb.Discount;
                transaction.Tax = transactionDb.Tax;

                var result = await _transactionService.UpdateAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.updateTransactionMessage);
            }
        }

        // /// <summary>
        // /// Grava Transação
        // /// </summary>
        // /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        // /// <returns>200 Transação atualizada</returns>
        // /// <returns>500 Erro inesperado</returns>
        // /// <returns>404 Erro tratado</returns>
        // [HttpPut]
        // [Route("refundPaymentPixAsync/{idTransaction}")]
        // public async Task<IActionResult> RefundPaymentPixAsync(
        //     [FromRoute] string idTransaction,
        //     [FromBody] long amount
        // )
        // {
        //     try
        //     {
        //         if (idTransaction == string.Empty)
        //             return NotFound("Id Transa  ção é Obrigatório");
        //         var transactionDb = (
        //             _transactionService.RefundPaymentPixAsync(idTransaction, amount).Result.Data
        //             as List<GetTransaction>
        //         ).FirstOrDefault();

        //         if (transactionDb.Stage != StageTransactionEnum.PaymentData)
        //             return NotFound("Estágio fora do padrão");

        //         var transaction = transactionDb.GetTransactionPixToTransactionPix();

        //         var resultPixQrCode = await _transactionService.GeneratePixQRcode(transaction);

        //         transaction.Stage = StageTransactionEnum.PaymentTransaction;

        //         var result = await _transactionService.UpdateAsync(transaction);
        //         if (result.Message != null && result.Message.Any())
        //         {
        //             _logger.LogInformation(result.Message);
        //             return NotFound(result.Message);
        //         }

        //         return Ok(result.Data);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
        //         return StatusCode(500, MessageLogErrors.updateTransactionMessage);
        //     }
        // }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("generatePixQRcodeTransaction/{idTransaction}")]
        public async Task<IActionResult> UpdateTransactionPaymentPixDataAsync(
            [FromRoute] string idTransaction
        )
        {
            try
            {
                if (idTransaction == string.Empty)
                    return NotFound("Id Transação é Obrigatório");
                var transactionDb = (
                    _transactionService.GetByIdAsync(idTransaction).Result.Data
                    as List<GetTransactionEventData>
                ).FirstOrDefault();

                if (transactionDb.Stage != StageTransactionEnum.PaymentData)
                    return NotFound("Estágio fora do padrão");

                var transaction = transactionDb.GetTransactionPixToTransactionPix();

                var resultPixQrCode = await _transactionService.GeneratePixQRcode(transaction);

                return Ok(resultPixQrCode.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.updateTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.updateTransactionMessage);
            }
        }
    }
}

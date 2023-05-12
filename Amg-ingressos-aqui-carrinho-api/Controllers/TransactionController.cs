using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;
using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Controllers
{
    [Route("v1/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idTransaction">id da transacao</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdTransactionAsync(string id)
        {
            try
            {
                var result = await _transactionService.GetByIdAsync(id);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByIdTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByIdTransactionMessage);
            }
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idPerson">id do usuário</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("person/{idPerson}")]
        public async Task<IActionResult> GetByPersonAsync([FromRoute]string idPerson)
        {
            try
            {
                var result = await _transactionService.GetByPersonAsync(idPerson);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
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
        /// <param name="transaction">Dados para criar Transacao</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPost]
        [Route("confirm")]
        public async Task<IActionResult> SaveTransactionAsync([FromBody]TransactionDto transaction)
        {
            try
            {
                var result = await _transactionService.SaveAsync(transaction);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
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
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPersonData")]
        public async Task<IActionResult> UpdateTransactionPersonDataAsync([FromRoute]string idTransaction)
        {
            try
            {
                var transactionDb = (_transactionService
                    .GetByIdAsync(idTransaction).Result.Data as List<GetTransaction>).FirstOrDefault();
                var transaction = new Transaction(){
                    Id = idTransaction,
                    IdPerson= transactionDb.IdPerson,
                    Stage = Enum.StageTransactionEnum.PersonData
                };
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

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmTicketsData")]
        public async Task<IActionResult> UpdateTransactionTicketsDataAsync([FromBody]StageTicketDataDto transactionDto)
        {
            try
            {
                var transaction = transactionDto.StageTicketDataDtoToTransaction();
                var transactionDb = (_transactionService
                    .GetByIdAsync(transaction.Id).Result.Data as List<GetTransaction>).FirstOrDefault();

                transaction.IdPerson = transactionDb.IdPerson;
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

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Dados para atualizar transacao no stage de person DAta</param>
        /// <returns>200 Transação atualizada</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpPut]
        [Route("confirmPaymentData")]
        public async Task<IActionResult> UpdateTransactionPaymentDataAsync([FromBody]StagePaymentDataDto transactionDto)
        {
            try
            {

                var transaction = transactionDto.StagePaymentDataDtoToTransaction();
                var transactionDb = (_transactionService
                    .GetByIdAsync(transaction.Id).Result.Data as List<GetTransaction>).FirstOrDefault();

                if(transactionDb.Stage != StageTransactionEnum.PaymentData)
                    return NotFound("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
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

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transactionDto">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("paymentTransaction")]
        public async Task<IActionResult> PaymentTransactionAsync([FromRoute]string id)
        {
            try
            {
                var result = await _transactionService.GetByIdAsync(id);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }
                var transactionDb = (result.Data as Transaction);
                

                var resultPayment = await _transactionService.Payment(transactionDb);
                if (resultPayment.Message != null && resultPayment.Message.Any())
                {
                    _logger.LogInformation(resultPayment.Message);
                    return NotFound(resultPayment.Message);
                }

                return Ok("Transação Efetivada");
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.paymentTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.paymentTransactionMessage);
            }
        }

        /// <summary>
        /// Grava Transação
        /// </summary>
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("finishedTransaction")]
        public async Task<IActionResult> FinishedTransactionAsync(string idTransaction)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(idTransaction);
                var resultQrcode = await _transactionService.FinishedTransactionAsync(transaction.Data as Transaction);
                if (resultQrcode.Message.Any())
                {
                    _logger.LogInformation(resultQrcode.Message);
                    return NotFound(resultQrcode.Message);
                }

                return Ok(resultQrcode.Data);
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
        /// <param name="transaction">Corpo Transação a ser Gravado</param>
        /// <returns>200 Transação criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("payment")]
        public async Task<IActionResult> PaymentAsync()
        {
            try
            {
                _transactionService.Payment(new Transaction());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.saveTransactionMessage);
            }
        }
    }
}
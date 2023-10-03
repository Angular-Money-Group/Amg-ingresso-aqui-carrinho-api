using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Repository.Querys
{
    public static class QuerysMongo
    {
        public const string GetTransactionQuery = @"{
                                $lookup: {
                                    from: 'transactionIten',
                                    'let': { transactionId : { '$toString': '$_id' }},
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [
                                                        { '$toString': '$IdTransaction' },
                                                        '$$transactionId'
                                                    ]
                                                }
                                            }
                                        }
                                    ],
                                    as: 'TransactionIten'
                                }
                            }";

        public const string GetTransactionTicketQuery = @"{
                        $lookup: {
                            from: 'transactionIten',
                            'let': { transactionId: '$_id' },
                            pipeline: [
                                {
                                    $match: {
                                        $expr: {
                                            $eq: [
                                                '$IdTransaction',
                                                '$$transactionId'
                                            ]
                                        }
                                    }
                                },
                                {
                                    $lookup: {
                                        from: 'tickets',
                                        'let': { ticketId: '$IdTicket' },
                                        pipeline: [
                                            {
                                                $match: {
                                                    $expr: {
                                                        $eq: [
                                                            '$_id',
                                                            '$$ticketId'
                                                        ]
                                                    }
                                                }
                                            }
                                        ],
                                        as: 'ticket'
                                    }
                                },
                            ],
                            as: 'TransactionIten'
                        }
                    }";

    }
}
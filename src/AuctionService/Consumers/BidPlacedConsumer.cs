using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _dbContext;

        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("---> Consuming bid placed.");

            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if (!ShouldUpdateCurrentHighBid(auction, context.Message))
            {
                return;
            }

            auction.CurrentHighBid = context.Message.Amount;

            await _dbContext.SaveChangesAsync();
        }

        private bool ShouldUpdateCurrentHighBid(Auction auction, BidPlaced message)
        {
            return auction.CurrentHighBid == null
                || message.BidStatus.Contains("Accepted")
                && message.Amount > auction.CurrentHighBid;
        }
    }
}
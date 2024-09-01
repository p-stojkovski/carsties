using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("----> Consuming bid placed: " + context.Message.Id);

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        if (!ShouldUpdateCurrentHighBid(auction, context.Message))
        {
            return;
        }

        auction.CurrentHighBid = context.Message.Amount;

        await auction.SaveAsync();
    }

    private bool ShouldUpdateCurrentHighBid(Item auction, BidPlaced message)
    {
        return message.BidStatus.Contains("Accepted")
            && message.Amount > auction.CurrentHighBid;
    }
}

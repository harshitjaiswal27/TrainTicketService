using TrainTicketService;
using TrainTicketService.Services;

public class TrainTicketTests
{
    [Fact]
    public async void PurchaseTicket_ShouldReturnTicketWithSeat()
    {
        var service = new TicketService();
        var request = new Ticket
        {
            From = "London",
            To = "France",
            User = new User { FirstName = "Harshit", LastName = "Jaiswal", Email = "harshit.jaiswal@gmail.com" },
            PricePaid = 20,
        };

        var response = await service.PurchaseTicket(request, null);

        Assert.NotNull(response);
        Assert.NotNull(response.Section);
    }

    [Fact]
    public async void GetReceipt_ShouldReturnTicket()
    {
        var service = new TicketService();
        var purchaseRequest = new Ticket
        {
            From = "London",
            To = "France",
            User = new User { FirstName = "Harshit", LastName = "Jaiswal", Email = "harshit.jaiswal@gmail.com" },
            PricePaid = 20,
        };
        var request = new Request{
            Email = "harshit.jaiswal@gmail.com"
        };

        await service.PurchaseTicket(purchaseRequest, null);

        var receipt = await service.GetReceipt(request, null);

        Assert.NotNull(receipt);
    }

    [Fact]
    public async Task RemoveUser_RemovesUserSuccessfully()
    {
        var service = new TicketService(); 
        var existingTicket = new Ticket { User = new User { Email = "harshit.jaiswal@gmail.com" }, Section = "A" };
        await service.PurchaseTicket(existingTicket,null);
        var request = new Request { Email = "harshit.jaiswal@gmail.com" }; 

        var removedUser = await service.RemoveUser(request, null);

        Assert.NotNull(removedUser);
        Assert.Equal("harshit.jaiswal@gmail.com", removedUser.User.Email); 
    }

    [Fact]
    public async Task ModifySeat_ModifiesSeatSuccessfully()
    {
        var service = new TicketService(); 
        var existingTicket = new Ticket { User = new User { Email = "harshit.jaiswal@gmail.com" }, Section = "A" };
        await service.PurchaseTicket(existingTicket,null);
        var request = new Ticket { User = new User { Email = "harshit.jaiswal@gmail.com" }, Section = "B" }; 

        var modifiedSeat = await service.ModifySeat(request, null);

        Assert.NotNull(modifiedSeat);
        Assert.Equal("B", modifiedSeat.Section);
    }

}

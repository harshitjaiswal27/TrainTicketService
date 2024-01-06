using Grpc.Core;
using TrainTicketService;

namespace TrainTicketService.Services;

public class TicketService : TrainTicket.TrainTicketBase
{
    private static readonly List<Ticket> Tickets = new List<Ticket>();
    
    public override Task<Ticket> PurchaseTicket(Ticket request, ServerCallContext context)
    {
        request.Section = AssignSeat();
        Tickets.Add(request);
        return Task.FromResult(request);
    }

    public override Task<Ticket> GetReceipt(Request request, ServerCallContext context)
    {
        var ticket = Tickets.FirstOrDefault(t => t.User.Email == request.Email);
        if (ticket == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Ticket not found."));
        return Task.FromResult(ticket);
    }

    public override async Task GetUsersBySection(SectionRequest sectionRequest, IServerStreamWriter<Ticket> responseStream, ServerCallContext context)
    {
        var usersInSection = Tickets.Where(t => t.Section == sectionRequest.Section).ToList();
        foreach (var user in usersInSection)
        {
            await responseStream.WriteAsync(user);
        }
    }

    public override Task<Ticket> RemoveUser(Request request, ServerCallContext context)
    {
        var ticket = Tickets.FirstOrDefault(t => t.User.Email == request.Email);
        if (ticket != null)
            Tickets.Remove(ticket);
        return Task.FromResult(ticket);
    }

    public override Task<Ticket> ModifySeat(Ticket request, ServerCallContext context)
    {
        var existingTicket = Tickets.FirstOrDefault(t => t.User.Email == request.User.Email);
        if (existingTicket != null)
            existingTicket.Section = request.Section;
        return Task.FromResult(existingTicket);
    }

    private string AssignSeat()
    {
        return Tickets.Count(t => t.Section == "A") < Tickets.Count(t => t.Section == "B") ? "A" : "B";
    }
}

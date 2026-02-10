using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

public class SystemStateService
{
    private readonly SystemStateRepository _repository;
    private readonly IGiftRepository _giftRepository;
    private readonly TrickyTrayDbContext _context;

    public SystemStateService(SystemStateRepository repository, IGiftRepository giftRepository, TrickyTrayDbContext context)
    {
        _repository = repository;
        _giftRepository = giftRepository;
        _context = context;
    }

    public SystemState GetState() => _repository.Get();

    public void StartSale()
    {
        var state = GetState();
        if (state.Status != SystemState.SaleStatus.Draft)
            throw new InvalidOperationException("המכירה כבר התחילה.");

        state.Status = SystemState.SaleStatus.Active;
        state.StartTime = DateTime.Now;
        _repository.Update(state);
    }

    public async Task<List<RaffleReportDto>> FinishSaleAsync()
    {
        var state = GetState();
        if (state.Status != SystemState.SaleStatus.Active)
            throw new InvalidOperationException("המכירה אינה פעילה.");

        // 1. עדכון סטטוס המערכת
        state.Status = SystemState.SaleStatus.Finished;
        state.EndTime = DateTime.Now;
        _repository.Update(state);

        // 2. לוגיקת הגרלה
        var report = new List<RaffleReportDto>();
        var gifts = await _giftRepository.GetAllAsync();
        var random = new Random();

        foreach (var gift in gifts)
        {
            // שליפת כל ה"כרטיסים" (שורות ב-OrderGift) עבור מתנה זו
            var tickets = await _context.OrderGift
                .Include(og => og.Order.Buyer) // כדי לקבל את שם הזוכה
                .Where(og => og.GiftId == gift.Id)
                .ToListAsync();

            if (tickets.Any())
            {
                // הגרלת הכרטיס המנצח
                var winningTicket = tickets[random.Next(tickets.Count)];

                // עדכון השדה IsWinner ל-true
                winningTicket.IsWinner = true;

                report.Add(new RaffleReportDto
                {
                    GiftName = gift.Name,
                    WinnerName = winningTicket.Order.Buyer.Name,
                    WinnerEmail = winningTicket.Order.Buyer.Email
                });
            }
        }

        // שמירת כל השינויים (הזכיות) בבת אחת
        await _context.SaveChangesAsync();

        return report;
    }

    public void Reset()
    {
        var state = GetState();
        state.Status = SystemState.SaleStatus.Draft;
        state.StartTime = null;
        state.EndTime = null;
        _repository.Update(state);
    }
}
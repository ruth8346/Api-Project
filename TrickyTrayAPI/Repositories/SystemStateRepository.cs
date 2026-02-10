using System;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;

public class SystemStateRepository
{
    private readonly TrickyTrayDbContext _context;

    public SystemStateRepository(TrickyTrayDbContext context)
    {
        _context = context;
    }

    public SystemState Get()
    {
        return _context.SystemState.First();
    }

    public void Update(SystemState state)
    {
        _context.SystemState.Update(state);
        _context.SaveChanges();
    }
}

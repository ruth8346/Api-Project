using TrickyTrayAPI.Models;

public class SystemStateService
{
    private readonly SystemStateRepository _repository;

    public SystemStateService(SystemStateRepository repository)
    {
        _repository = repository;
    }

    public SystemState GetState()
    {
        return _repository.Get();
    }

    public void StartSale()
    {
        var state = GetState();

        if (state.Status != SystemState.SaleStatus.Draft)
            throw new InvalidOperationException("Sale already started");

        state.Status = SystemState.SaleStatus.Active;
        state.StartTime = DateTime.Now;

        _repository.Update(state);
    }

    public void FinishSale()
    {
        var state = GetState();

        if (state.Status != SystemState.SaleStatus.Active)
            throw new InvalidOperationException("Sale is not active");

        state.Status = SystemState.SaleStatus.Finished;
        state.EndTime = DateTime.Now;

        _repository.Update(state);
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
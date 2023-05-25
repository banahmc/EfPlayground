namespace Model.Abstractions.Results;

public readonly struct Result<TGoodState, TBadState>
{
    private readonly TGoodState? _goodState;
    private readonly TBadState? _badState;

    private Result(TGoodState goodState)
    {
        IsSuccess = true;
        _goodState = goodState;
        _badState = default;
    }

    private Result(TBadState badState)
    {
        IsSuccess = false;
        _goodState = default;
        _badState = badState;
    }

    public bool IsSuccess { get; }

    public TResult Match<TResult>(
        Func<TGoodState, TResult> goodStateHandler,
        Func<TBadState, TResult> badStateHandler) => IsSuccess ? goodStateHandler(_goodState!) : badStateHandler(_badState!);

    public static implicit operator Result<TGoodState, TBadState>(TGoodState goodState) => new(goodState);
    public static implicit operator Result<TGoodState, TBadState>(TBadState badState) => new(badState);
}

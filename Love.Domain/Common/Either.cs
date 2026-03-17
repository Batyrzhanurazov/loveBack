namespace Love.Domain.Common;

public sealed class Either<TL, TR>
{
    public readonly TL Left = default!;
    public readonly TR Right = default!;
    public readonly bool IsLeft;

    private Either(TL left) 
    {
        Left = left;
        IsLeft = true; 
    }

    private Either(TR right)
    { 
        Right = right;
        IsLeft = false; 
    }

    public static implicit operator Either<TL, TR>(TL left) => new(left);
    public static implicit operator Either<TL, TR>(TR right) => new(right);
}